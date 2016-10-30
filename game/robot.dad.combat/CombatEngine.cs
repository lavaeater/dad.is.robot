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
        public IEnumerable<Combattant> ParticipantsThatCanFight => Participants.Where(p => p.Status == CombatStatus.Active);
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
                .ExecuteOnExit(SetAliveParticipantsForRound)
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
                .ExecuteOnExit(SetAliveParticipantsForRound)
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
            if (playerToPickFor == null)
            {
                string wut = "Wurt?";
            }
            playerToPickFor?.PickMove(AliveParticipants);

            StateMachine.Fire(Events.PlayerPicked);
        }

        private void SetAliveParticipantsForRound()
        {
            AliveParticipants = ParticipantsThatCanFight.ToList();
        }

        public void ResolveCombatRound()
        {
            Round++;
            Console.WriteLine($"Runda {Round}!");

            ResolveRunaways();

            ResolveSpecialAttacks();

            ResolveRegularAttacks();

            Console.WriteLine("Rundan över!");
            Thread.Sleep(3000);
            Console.Clear();

            //This will be a doozy
            StateMachine.Fire(CheckIfCombatIsOver() ? Events.CombatOver : Events.CombatRoundResolved);
        }

        private void ResolveRegularAttacks()
        {
            //Defensive moves just means that one does not attack, you just get an extra defensive bonus this round
            var attackingParticipants = AliveParticipants.FindAll(p => p.CurrentMove.MoveType == CombatMoveType.Attack);
            foreach (var attackingParticipant in attackingParticipants)
            {
                int targetValue = attackingParticipant.AttackSkill +
                                  attackingParticipant.CurrentMove.Modifier -
                                  attackingParticipant.CurrentTarget.DefenseSkill;
                if (attackingParticipant.CurrentTarget.CurrentMove.MoveType == CombatMoveType.Defend ||
                    attackingParticipant.CurrentTarget.CurrentMove.MoveType == CombatMoveType.Runaway)
                {
                    targetValue += attackingParticipant.CurrentTarget.CurrentMove.Modifier;
                }
                int attackRoll = DiceRoller.RollHundredSided();
                Console.Write(
                    $"{attackingParticipant.Name} vill {attackingParticipant.CurrentMove.Verbified} {attackingParticipant.CurrentTarget.Name} och måste slå {targetValue} och slog {attackRoll}");

                if (attackRoll <= targetValue)
                {
                    int attackDamage = DiceRoller.RollDice(attackingParticipant.CurrentMove.MinDamage,
                        attackingParticipant.CurrentMove.MaxDamage);
                    Console.WriteLine($" och gjorde {attackingParticipant.CurrentTarget.ApplyDamage(attackDamage)} i skada");
                    Console.WriteLine(
                        $"{attackingParticipant.CurrentTarget.Name} har {attackingParticipant.CurrentTarget.Health} hälsa kvar");
                }
                else
                {
                    Console.WriteLine($" - men missade!");
                }
            }
        }

        private void ResolveSpecialAttacks()
        {
            var specialParticipants = AliveParticipants.FindAll(p => p.CurrentMove.MoveType == CombatMoveType.Special);
            foreach (var specialParticipant in specialParticipants)
            {
                int targetValue = specialParticipant.AttackSkill +
                                  specialParticipant.CurrentMove.Modifier -
                                  specialParticipant.CurrentTarget.DefenseSkill;

                int specialRoll = DiceRoller.RollHundredSided();
                Console.Write(
                    $"{specialParticipant.Name} vill {specialParticipant.CurrentMove.Verbified} {specialParticipant.CurrentTarget.Name} och måste slå {targetValue} och slog {specialRoll}");

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
        }

        private void ResolveRunaways()
        {
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

        public bool AllPlayersHavePicked => AliveParticipants.TrueForAll(p => p.HasPicked);
    }
}
