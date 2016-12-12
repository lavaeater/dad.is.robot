using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appccelerate.StateMachine;

namespace robot.dad.combat
{
    public class CombatEngine
    {
        public IEnumerable<Combattant> Participants => Protagonists.Union(Antagonists);
        public List<Combattant> Protagonists { get; set; } = new List<Combattant>();
        public List<Combattant> Antagonists { get; set; } = new List<Combattant>();


        public List<Combattant> ParticipantsThatFled { get; set; } = new List<Combattant>();
        public IEnumerable<Combattant> ParticipantsThatCanFight => Participants.Where(p => p.Status == CombatStatus.Active);
        public PassiveStateMachine<States, Events> StateMachine { get; set; }
        public static int Round { get; set; }

        public CombatEngine(List<Combattant> protagonists, List<Combattant> antagonists) : this()
        {
            Protagonists.AddRange(protagonists);
            Antagonists.AddRange(antagonists);
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
                .ExecuteOnEntry(ApplyCombatEffects)
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

        private void ApplyCombatEffects()
        {
            //Time delayed effects, hypnosis, fire, cold, poison, everything should be done here
            /*
             * Here we could like call some method on every object that takes rounds ticking into account?
             * Or how do we keep track of rounds and passing time when it comes to hypnosis?
             */

            //Also fear 
            foreach (var target in AliveParticipants.Where(t => t.CombatEffects.Any()))
            {
                foreach (var effect in target.CombatEffects)
                {
                    effect.ApplyEffects(target);
                }
                target.CombatEffects.Where(
                    e =>
                        (e.LastRound > Round))
                    .ToList()
                    .ForEach(e => e.EffectsEnded(target));
            }
        }

        private void ResetPicks()
        {
            foreach (var participant in Participants)
            {
                participant.ClearMove();
            }
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
            PrintCombatBoard();
            var playerToPickFor = AliveParticipants.First(p => !p.HasPicked);
            playerToPickFor.PickMove(AliveParticipants);

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

            AliveParticipants.ForEach(ap => ap.ResolveMove());

            Console.WriteLine("Rundan över!");
            Console.ReadKey();

            //This will be a doozy
            StateMachine.Fire(CheckIfCombatIsOver() ? Events.CombatOver : Events.CombatRoundResolved);
        }

        public void PrintCombatBoard()
        {
            //Assume stuff!
            Console.Clear();
            StringBuilder sb = new StringBuilder();
            var grouping = Participants.GroupBy(p => p.Team);
            int max = 0;
            foreach (var group in grouping)
            {
                if (group.Count() > max) max = group.Count();
            }
            for (int i = 0; i < max; i++)
            {
                int j = 0;
                foreach (var group in grouping)
                {
                    Console.SetCursorPosition(i * 20, 0);
                    var people = group.ToList();
                    if (i < group.Count())
                        sb.Append($"{people[i]}");
                    if (j%2 == 0)
                        sb.Append("\t\t\t");
                    if (j%2 != 0)
                        sb.AppendLine();

                    j++;
                }
            }
            Console.WriteLine(sb.ToString());

        }

        public List<Combattant> AliveParticipants { get; set; }

        private bool CheckIfCombatIsOver()
        {
            //Fight is over if all participants on either team is dead!
            return Protagonists.TrueForAll(c => c.Dead) || Antagonists.TrueForAll(c => c.Dead);
        }

        public bool AllPlayersHavePicked => AliveParticipants.TrueForAll(p => p.HasPicked);
    }
}
