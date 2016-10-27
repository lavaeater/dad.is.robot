using System;
using System.Collections.Generic;
using System.Linq;
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
            var playerToPickFor = Participants.First(p => !p.HasPicked);
            playerToPickFor.PickMove(GetRandomTargetNotSelf(playerToPickFor));
            StateMachine.Fire(Events.PlayerPicked);
        }

        private Combattant GetRandomTargetNotSelf(Combattant attacker)
        {

            var possibleTargets = Participants
                .Except(ParticipantsThatDied)
                .Except(ParticipantsThatDied)
                .Where(p => p.Team != attacker.Team).ToList();
            int diceRoll = DiceRoller.RollDice(0, possibleTargets.Count - 1);
            return possibleTargets[diceRoll];
        }

        public void ResolveCombatRound()
        {
            //Who is still alive?
            var aliveParticipants = ParticipantsThatCanFight.ToList();

            Round++;
            Console.WriteLine($"Runda {Round}!");

            //Anyone trying to get away?
            var runningParticipants = aliveParticipants.FindAll(p => p.CurrentMove.MoveType == CombatMoveType.Runaway);
            foreach (var runningParticipant in runningParticipants)
            {
                int runawayRoll = DiceRoller.RollDice(0, 10);
                if (runawayRoll + runningParticipant.DefenseModifier > 0)
                {
                    ParticipantsThatFled.Add(runningParticipant); //No longer in the fight - but can still be attacked this round!
                    Console.WriteLine($"{runningParticipant.Name} {runningParticipant.CurrentMove.Verbified}");
                }
            }

            //Defensive moves just means that one does not attack, you just get an extra defensive bonus this round
            var attackingParticipants = aliveParticipants.FindAll(p => p.CurrentMove.MoveType == CombatMoveType.Attack);
            foreach (var attackingParticipant in attackingParticipants)
            {
                int attackRoll = DiceRoller.RollDice(0, 10);
                int attackValue = attackRoll + attackingParticipant.AttackModifier +
                                  attackingParticipant.CurrentMove.Modifier;
                attackValue -= attackingParticipant.CurrentTarget.DefenseModifier;
                if (attackingParticipant.CurrentTarget.CurrentMove.MoveType == CombatMoveType.Defend)
                {
                    attackValue -= attackingParticipant.CurrentTarget.CurrentMove.Modifier;
                }
                if (attackingParticipant.CurrentTarget.CurrentMove.MoveType == CombatMoveType.Runaway)
                {
                    attackValue -= attackingParticipant.CurrentTarget.CurrentMove.Modifier;
                }
                if (attackValue > 0)
                {
                    Console.Write($"{attackingParticipant.Name} {attackingParticipant.CurrentMove.Verbified} {attackingParticipant.CurrentTarget.Name} ");
                    int attackDamage = DiceRoller.RollDice(attackingParticipant.CurrentMove.MinDamage,
                        attackingParticipant.CurrentMove.MaxDamage);
                    Console.WriteLine($"och gjorde {attackingParticipant.CurrentTarget.ApplyDamage(attackDamage)}");
                    Console.WriteLine($"{attackingParticipant.CurrentTarget.Name} har {attackingParticipant.CurrentTarget.Health} hälsa kvar");
                }
                else
                {
                    Console.WriteLine($"{attackingParticipant.Name} attacked {attackingParticipant.CurrentTarget.Name} but missed!");
                }
            }

            Console.WriteLine("Rundan över, tryck för nästa!");
            Console.ReadKey();

            //This will be a doozy
            StateMachine.Fire(CheckIfCombatIsOver() ? Events.CombatOver : Events.CombatRoundResolved);
        }

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
            return Rnd.Next(minVal, maxVal);
        }
    }

    public class Combattant
    {
        public Combattant(string name, int health, int attackModifier, int defenseModifier, int armor, string team, List<CombatMove> combatMoves)
        {
            Name = name;
            Health = health;
            AttackModifier = attackModifier;
            DefenseModifier = defenseModifier;
            Armor = armor;
            Team = team;
            CombatMoves = combatMoves;
        }

        public string Team { get; set; }
        public List<CombatMove> CombatMoves { get; set; }

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
        public virtual void PickMove(Combattant target)
        {
            int moveIndex = DiceRoller.RollDice(0, CombatMoves.Count - 1);
            CurrentMove = CombatMoves[moveIndex];
            CurrentTarget = target;
            Console.WriteLine($"{Name} valde att attackera {CurrentTarget.Name} med {CurrentMove.Name}");
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
        public int DefenseModifier { get; set; }
        public int Armor { get; set; }
        public int AttackModifier { get; set; }

        public bool Npc { get; set; }
        public string Name { get; set; }
    }

    public class CombatMove
    {
        private string v;

        public CombatMove(string name, CombatMoveType moveType, int modifier, int minDamage, int maxDamage, string verbified)
        {
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Verbified = verbified;
        }

        private CombatMove(string name, CombatMoveType moveType, int modifier, string verbified)
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
            new CombatMove("Slag", CombatMoveType.Attack, 2, 6, 12, "slog"),
            new CombatMove("Spark", CombatMoveType.Attack, -1, 10, 16, "sparkade"),
            new CombatMove("Undvik", CombatMoveType.Defend, 4, "undvek"),
            new CombatMove("Fly", CombatMoveType.Runaway, -5, "flydde")
        };

        public string Verbified { get; set; }
    }

    public enum CombatMoveType
    {
        Attack,
        Defend,
        Runaway
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
}
