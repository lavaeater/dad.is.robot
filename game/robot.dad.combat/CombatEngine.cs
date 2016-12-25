using System;
using System.Collections.Generic;
using System.Linq;
using Appccelerate.StateMachine;
using robot.dad.common;

namespace robot.dad.combat
{
    public class CombatEngine
    {
        public Action ProtagonistsWin { get; set; }
        public Action AntagonistsWin { get; set; }
        public IEnumerable<Combattant> Participants => Protagonists.Union(Antagonists);
        public List<Combattant> Protagonists { get; set; } = new List<Combattant>();
        public List<Combattant> Antagonists { get; set; } = new List<Combattant>();
        public Combattant CurrentCombattant { get; set; }
        public List<Combattant> ParticipantsThatFled { get; set; } = new List<Combattant>();
        public IEnumerable<Combattant> ParticipantsThatCanFight => Participants.Where(p => p.Status == CombatStatus.Active);
        public static PassiveStateMachine<States, Events> StateMachine { get; set; }
        public static int Round { get; set; }
        public Action<ICombattant, ICombattant> MoveSucceeded { get; set; }
        public Action<Combattant> MoveFailed { get; set; }
        public Action<ICombattant, ICombatMove> SomeoneIsDoingSomething { get; set; }
        public Action<ICombattant> SomeoneDied { get; set; }
        public bool CurrentMoveWasSuccessful { get; set; }
        public List<Combattant> AliveByInitiative => Participants.Where(c => !c.Dead).OrderByDescending(c => c.Initiative).ToList();
        public Action<ICombattant, int> SomeoneTookDamage { get; set; }

        public CombatEngine(List<Combattant> protagonists, List<Combattant> antagonists, Action protagonistsWin, Action antagonistsWin) : this()
        {
            ProtagonistsWin = protagonistsWin;
            AntagonistsWin = antagonistsWin;
            Protagonists.AddRange(protagonists);
            Antagonists.AddRange(antagonists);
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
                .ExecuteOnEntry(SetNextCombattant)
                .ExecuteOnEntry(PickMove)
                .On(Events.PlayerPicked)
                .Goto(States.ResolveMove);

            StateMachine
                .In(States.ResolveMove)
                .ExecuteOnEntry(ResolveMove)
                .On(Events.MoveSuccesful)
                .Goto(States.SuccessfulMove)
                .On(Events.MoveFailed)
                .Goto(States.FailedMove);

            StateMachine
                .In(States.FailedMove)
                .ExecuteOnEntry(ResolveFailure)
                .On(Events.FailureResolved)
                .Goto(States.ApplyCombatEffects);
            StateMachine
                .In(States.SuccessfulMove)
                .ExecuteOnEntry(ResolveSuccess)
                .On(Events.SucccesResolved)
                .Goto(States.ApplyCombatEffects);

            StateMachine.In(States.ApplyCombatEffects)
                .ExecuteOnEntry(ApplyCombatEffects)
                .On(Events.EffectsApplied)
                .Goto(States.PlayerPicking)
                .On(Events.CombatOver)
                .Goto(States.CombatOver);

            StateMachine
                .In(States.CombatOver)
                .ExecuteOnEntry(CombatOver);
            StateMachine.Initialize(States.BeforeCombat);
        }

        private void ResolveSuccess()
        {
            MoveSucceeded?.Invoke(CurrentCombattant, CurrentCombattant.CurrentTarget);
            StateMachine.Fire(Events.SucccesResolved);
        }

        private void ResolveFailure()
        {
            MoveFailed?.Invoke(CurrentCombattant);
            StateMachine.Fire(Events.FailureResolved);
        }

        private void ApplyCombatEffects()
        {
            //What kind of indication to the game engine can be done here? Effects should be rendered by the card automatically, but damage being inflicted?
            foreach (var target in AliveByInitiative.Where(t => t.CombatEffects.Any()))
            {
                foreach (var effect in target.CombatEffects)
                {
                    effect.ApplyEffects(target);
                }
                var doneEffects = target.CombatEffects.Where(
                    e =>
                        (e.LastRound < Round))
                    .ToList();

                doneEffects.ForEach(e => e.EffectsEnded(target));
            }

            //Fixa dödsmeddelande!
            StateMachine.Fire(CheckIfCombatIsOver() ? Events.CombatOver : Events.EffectsApplied);
        }

        private int _currentIndex = -1;
        private void SetNextCombattant()
        {
            SetupCombattantActions();
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

        private void SetupCombattantActions()
        {
            if (SomeoneDied != null)
            {
                foreach (var combattant in Participants)
                {
                    combattant.IJustDied = SomeoneDied;
                }
            }

            if (SomeoneTookDamage != null)
            {
                foreach (var combattant in Participants)
                {
                    combattant.TookDamage = SomeoneTookDamage;
                }
            }
        }

        public void StartCombat()
        {
            StateMachine.Start();
            StateMachine.Fire(Events.Start);
        }

        private void CombatOver()
        {
            ProtagonistsWin();
        }

        /// <summary>
        /// Lets the next alive Participant with the highest initiative pick a move
        /// </summary>
        public void PickMove()
        {
            CurrentCombattant.PickMove(AliveByInitiative, Picked);
        }

        public static void Picked()
        {
            StateMachine.Fire(Events.PlayerPicked);
        }

        public void ResolveMove()
        {
            SomeoneIsDoingSomething?.Invoke(CurrentCombattant, CurrentCombattant.CurrentMove);

            StateMachine.Fire(CurrentCombattant.ResolveMove() ? Events.MoveSuccesful : Events.MoveFailed);
        }

        private bool CheckIfCombatIsOver()
        {
            //Fight is over if all participants on either team is dead!
            return Protagonists.TrueForAll(c => c.Dead || c.Status == CombatStatus.Fled) || Antagonists.TrueForAll(c => c.Dead || c.Status == CombatStatus.Fled);
        }
    }
}
