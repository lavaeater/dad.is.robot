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
                .ExecuteOnExit(SetNextCombattant)
                .On(Events.Start)
                .Goto(States.PlayerPicking);

            StateMachine
                .In(States.PlayerPicking)
                .ExecuteOnEntry(PickMove)
                .On(Events.PlayerPicked)
                .Goto(States.ResolveMove);
                //.In(States.PlayerPicking)
                //.ExecuteOnEntry(PickMove)
                //.On(Events.PlayerPicked)
                //.If(() => AllPlayersHavePicked).Goto(States.ResolveMove)
                //.If(() => !AllPlayersHavePicked).Goto(States.PlayerPicking);

            StateMachine
                .In(States.ResolveMove)
                .ExecuteOnEntry(ResolveMove)
                .ExecuteOnExit(ApplyCombatEffects)
                .ExecuteOnExit(SetNextCombattant)
                //.ExecuteOnExit(SetAliveParticipantsForRound)
                .On(Events.MoveResolved)
                .Goto(States.PlayerPicking);

            StateMachine
                .In(States.ResolveMove)
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
            foreach (var target in AliveByInitiative.Where(t => t.CombatEffects.Any()))
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

        private int _currentIndex = -1;
        private void SetNextCombattant()
        {
            _currentIndex++;

            //TODO: Watch out, one player or two might be jumped if someone before or after dies and the number of items in AliveByInitiative changes.
            if (_currentIndex > AliveByInitiative.Count - 1)
            {
                _currentIndex = 0;
                Round++; //Normally, if we have gone full circle, it is a "round", useful for combat effects... for now. This is shit. ;-)
            }

            CurrentCombattant = AliveByInitiative[_currentIndex];
            CurrentCombattant.ClearMove();
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

        /// <summary>
        /// Lets the next alive Participant with the highest initiative pick a move
        /// </summary>
        public void PickMove()
        {
            PrintCombatBoard();
//            CurrentCombattant = AliveByInitiative.First(p => !p.HasPicked);
            CurrentCombattant.PickMove(AliveByInitiative);

            StateMachine.Fire(Events.PlayerPicked);
        }

        public Combattant CurrentCombattant { get; set; }

        public void ResolveMove()
        {
            Console.WriteLine($"Runda {Round}!");

            CurrentCombattant.ResolveMove();

            //This will be a doozy
            StateMachine.Fire(CheckIfCombatIsOver() ? Events.CombatOver : Events.MoveResolved);
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

        public List<Combattant> AliveByInitiative => Participants.Where(c => !c.Dead).OrderByDescending(c => c.Initiative).ToList();

        private bool CheckIfCombatIsOver()
        {
            //Fight is over if all participants on either team is dead!
            return Protagonists.TrueForAll(c => c.Dead) || Antagonists.TrueForAll(c => c.Dead);
        }
    }
}
