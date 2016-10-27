using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using Appccelerate.StateMachine;

namespace robot.dad.combat
{
    public class CombatEngine
    {
        public List<Combattant> Participants { get; set; } = new List<Combattant>();
        public List<Combattant> ParticipantsThatFled { get; set; } = new List<Combattant>();
        public List<Combattant> ParticipantsThatDied { get; set; } = new List<Combattant>();
        public IEnumerable<Combattant> ParticipantsThatCanFight => Participants.Except(ParticipantsThatDied).Except(ParticipantsThatFled);
        public PassiveStateMachine<States, Events> StateMachine { get; set; }
        public int Round { get; set; }

        public CombatEngine(List<Combattant> participants) : this()
        {
            Participants.AddRange(participants);
        }
        public CombatEngine()
        {
            StateMachine = new PassiveStateMachine<States, Events>();
            StateMachine
                .In(States.BeforeCombat)
                .ExecuteOnExit(FixParticipants)
                .On(Events.Start)
                .Goto(States.PlayerPicking);

            StateMachine
                .In(States.PlayerPicking)
                .ExecuteOnEntry(PickMove)
                .On(Events.PlayerPicked)
                .If(() => AllPlayersHavePicked).Goto(States.ResolveCombat)
                .If(() => !AllPlayersHavePicked).Goto(States.PlayerPicking);

            StateMachine
                .In(States.ResolveCombat)
                .ExecuteOnEntry(ResolveCombatRound)
                .ExecuteOnExit(ResetPicks)
                .ExecuteOnExit(FixParticipants)
                .On(Events.CombatRoundResolved)
                .Goto(States.PlayerPicking);

            StateMachine
                .In(States.ResolveCombat)
                .On(Events.CombatOver)
                .Goto(States.CombatOver);

            StateMachine
                .In(States.CombatOver)
                .ExecuteOnEntry(CombatOver);
            StateMachine.Initialize(States.BeforeCombat);
        }

        private void ResetPicks()
        {
            Participants.ForEach(p => p.ClearMove());
        }

        public void StartCombat()
        {
            Console.WriteLine("Starta fajten - tryck på en knapp");
            Console.ReadKey();
            StateMachine.Start();
            StateMachine.Fire(Events.Start);
        }

        private void CombatOver()
        {
            Console.WriteLine("Striden över");
        }

        public void PickMove()
        {
            var playerToPickFor = AliveParticipants.FirstOrDefault(p => !p.HasPicked);
            playerToPickFor?.PickMove(AliveParticipants);
            StateMachine.Fire(Events.PlayerPicked);
        }

        private void FixParticipants()
        {
            AliveParticipants = ParticipantsThatCanFight.ToList();
        }

        public void ResolveCombatRound()
        {
            Round++;
            Console.WriteLine($"Runda {Round}!");

            //Anyone trying to get away?
            var runningParticipants = AliveParticipants.FindAll(p => p.CurrentMove.MoveType == CombatMoveType.Runaway);
            foreach (var runningParticipant in runningParticipants)
            {
                int targetValue = runningParticipant.DefenseSkill + runningParticipant.CurrentMove.Modifier;
                int runawayRoll = DiceRoller.RollHundredSided();
                Console.Write($"{runningParticipant.Name} vill fly och måste slå {targetValue} och slog {runawayRoll}");
                if (runawayRoll <= targetValue)
                {
                    ParticipantsThatFled.Add(runningParticipant);
                    //No longer in the fight - but can still be attacked this round!
                    Console.WriteLine($" - och lyckades {runningParticipant.CurrentMove.Verbified}");
                }
                else
                {
                    Console.WriteLine($" - och lyckades inte");
                }
            }

            var specialParticipants = AliveParticipants.FindAll(p => p.CurrentMove.MoveType == CombatMoveType.Special);
            foreach (var specialParticipant in specialParticipants)
            {
                int targetValue = specialParticipant.AttackSkill +
                    specialParticipant.CurrentMove.Modifier -
                    specialParticipant.CurrentTarget.DefenseSkill;

                int specialRoll = DiceRoller.RollHundredSided();
                Console.Write($"{specialParticipant.Name} vill {specialParticipant.CurrentMove.Verbified} {specialParticipant.CurrentTarget.Name} och måste slå {targetValue} och slog {specialRoll}");

                if (specialRoll <= targetValue)
                {
                    Console.WriteLine(
                        $" - och lyckades!");
                }
                else
                {
                    Console.WriteLine(
                        $"- och misslyckades");
                }
            }

            //Defensive moves just means that one does not attack, you just get an extra defensive bonus this round
            var attackingParticipants = AliveParticipants.FindAll(p => p.CurrentMove.MoveType == CombatMoveType.Attack);
            foreach (var attackingParticipant in attackingParticipants)
            {
                int targetValue = attackingParticipant.AttackSkill +
                                  attackingParticipant.CurrentMove.Modifier -
                                  attackingParticipant.CurrentTarget.DefenseSkill;
                if (attackingParticipant.CurrentTarget.CurrentMove.MoveType == CombatMoveType.Defend || attackingParticipant.CurrentTarget.CurrentMove.MoveType == CombatMoveType.Runaway)
                {
                    targetValue += attackingParticipant.CurrentTarget.CurrentMove.Modifier;
                }
                int attackRoll = DiceRoller.RollHundredSided();
                Console.Write($"{attackingParticipant.Name} vill {attackingParticipant.CurrentMove.Verbified} {attackingParticipant.CurrentTarget.Name} och måste slå {targetValue} och slog {attackRoll}");

                if (attackRoll <= targetValue)
                {
                    int attackDamage = DiceRoller.RollDice(attackingParticipant.CurrentMove.MinDamage,
                        attackingParticipant.CurrentMove.MaxDamage);
                    Console.WriteLine($" och gjorde {attackingParticipant.CurrentTarget.ApplyDamage(attackDamage)} i skada");
                    Console.WriteLine($"{attackingParticipant.CurrentTarget.Name} har {attackingParticipant.CurrentTarget.Health} hälsa kvar");
                }
                else
                {
                    Console.WriteLine($" - men missade!");
                }
            }

            Console.WriteLine("Rundan över, tryck för nästa!");
            Console.ReadKey();

            //This will be a doozy
            StateMachine.Fire(CheckIfCombatIsOver() ? Events.CombatOver : Events.CombatRoundResolved);
        }

        public List<Combattant> AliveParticipants { get; set; }

        private bool CheckIfCombatIsOver()
        {

            ParticipantsThatDied = Participants.FindAll(p => p.Dead);
            //Fight is over if all participants on either team is dead!
            var group = Participants.GroupBy(p => p.Team);
            var groupingdead = group.Select(grouping => grouping.All(g => g.Dead));
            return groupingdead.Any(b => b);
        }

        public bool AllPlayersHavePicked => Participants.TrueForAll(p => p.HasPicked);
    }

    public static class DiceRoller
    {
        private static readonly Random Rnd = new Random();

        public static int RollDice(int minVal, int maxVal)
        {
            return Rnd.Next(minVal, maxVal + 1);
        }

        public static int RollHundredSided()
        {
            return RollDice(1, 100);
        }
    }

    public class Combattant
    {
        public Combattant(string name, int health, int attackSkill, int defenseSkill, int armor, string team, List<CombatMove> combatMoves, Action<Combattant, List<Combattant>, List<CombatMove>> movePicker)
        {
            Name = name;
            Health = health;
            AttackSkill = attackSkill;
            DefenseSkill = defenseSkill;
            Armor = armor;
            Team = team;
            CombatMoves = combatMoves;
            MovePicker = movePicker;
        }

        public string Team { get; set; }
        public List<CombatMove> CombatMoves { get; set; }
        public Action<Combattant, List<Combattant>, List<CombatMove>> MovePicker { get; private set; }

        public int ApplyDamage(int damage)
        {
            int actualDamage = damage - Armor;
            actualDamage = actualDamage < 0 ? 0 : actualDamage;
            Health -= actualDamage;
            if (Health < 1)
            {
                Console.WriteLine($"{Name} dog!");
            }
            return actualDamage;
        }

        public CombatMove CurrentMove { get; set; }
        //Choose a target as well!
        public void PickMove(List<Combattant> otherTeam)
        {
            MovePicker?.Invoke(this, otherTeam, CombatMoves);
        }

        public Combattant CurrentTarget { get; set; }
        public bool Dead => Health <= 0;

        public void ClearMove()
        {
            CurrentTarget = null;
            CurrentMove = null;
        }

        public bool HasPicked => CurrentMove != null && CurrentTarget != null;
        public int Health { get; set; }
        public int DefenseSkill { get; set; }
        public int Armor { get; set; }
        public int AttackSkill { get; set; }

        public bool Npc { get; set; }
        public string Name { get; set; }
    }

    public class CombatMove
    {
        public CombatMove(string name, CombatMoveType moveType, int modifier, int minDamage, int maxDamage, string verbified)
        {
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Verbified = verbified;
        }

        public CombatMove(string name, CombatMoveType moveType, int modifier, string verbified)
        {
            if (moveType == CombatMoveType.Attack)
            {
                throw new InvalidOperationException("Attack move types MUST have min and max damage");
            }
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            Verbified = verbified;
        }


        public string Name { get; set; }
        public CombatMoveType MoveType { get; set; }
        public int Modifier { get; set; }
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }
        public static List<CombatMove> CombatMoves => new List<CombatMove>()
        {
            new CombatMove("Slag", CombatMoveType.Attack, 10, 6, 12, "slå"),
            new CombatMove("Spark", CombatMoveType.Attack, -5, 10, 16, "sparka"),
            new CombatMove("Undvik", CombatMoveType.Defend, 20, "undvika"),
            new CombatMove("Fly", CombatMoveType.Runaway, -25, "fly")
        };

        public string Verbified { get; set; }
    }

    public enum CombatMoveType
    {
        Attack,
        Defend,
        Runaway,
        Special
    }

    public enum States
    {
        BeforeCombat,
        PlayerPicking,
        ResolveCombat,
        CombatOver
    }

    public enum Events
    {
        Start,
        PlayerPicked,
        CombatRoundResolved,
        CombatOver
    }

    public static class MovePickers
    {
        public static void RandomPicker(Combattant picker, List<Combattant> possibleTargets,
            List<CombatMove> possibleMoves)
        {
            var pts = possibleTargets.Where(pt => pt.Team != picker.Team).ToList();
            picker.CurrentTarget = pts[DiceRoller.RollDice(0, pts.Count - 1)];
            picker.CurrentMove = possibleMoves[DiceRoller.RollDice(0, possibleMoves.Count - 1)];
        }

        public static void ManualPicker(Combattant picker, List<Combattant> possibleTargets, List<CombatMove> possibleMoves)
        {
            //1. List targets and make player choose one!
            var pts = possibleTargets.Where(pt => pt.Team != picker.Team).ToList();
            int index = 1;
            Console.WriteLine("Välj vem du vill attackera genom att mata in siffran");
            foreach (var possibleTarget in pts)
            {
                Console.WriteLine($"{index}. {possibleTarget.Name}");
                index++;
            }
            var choice = Console.ReadKey();
            int targetIndex = 0;
            if (char.IsDigit(choice.KeyChar))
            {
                targetIndex = int.Parse(choice.KeyChar.ToString()) - 1;
            }
            picker.CurrentTarget = pts[targetIndex];

            //2. Choose attack
            index = 1;
            Console.WriteLine("Välj attack");
            foreach (var possibleMove in possibleMoves)
            {
                Console.WriteLine($"{index}. {possibleMove.Name}");
                index++;
            }
            choice = Console.ReadKey();
            targetIndex = 0;
            if (char.IsDigit(choice.KeyChar))
            {
                targetIndex = int.Parse(choice.KeyChar.ToString()) - 1;
            }
            picker.CurrentMove = possibleMoves[targetIndex];
        }
    }
}
