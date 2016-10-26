using System;
using System.Collections.Generic;
using System.Linq;
using Appccelerate.StateMachine;

namespace robot.dad.combat
{
    public class CombatEngine
    {
        public List<Combattant> Participants => new List<Combattant>();
        public PassiveStateMachine<States, Events> StateMachine { get; set; }
        public int Round { get; set; }

        public CombatEngine(List<Combattant> participants) :this()
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

        public void StartCombat()
        {
            Console.WriteLine("Start the fight");
            StateMachine.Start();
            StateMachine.Fire(Events.Start);
        }

        private void CombatOver()
        {
            Console.WriteLine("Combat over!!");
        }

        public void PickMove()
        {
            var playerToPickFor = Participants.First(p => !p.HasPicked);
            playerToPickFor.PickMove();
            StateMachine.Fire(Events.PlayerPicked);
        }

        public void ResolveCombatRound()
        {
            Round++;
            Console.WriteLine($"Round {Round}!");
            //This will be a doozy
            StateMachine.Fire(Events.CombatRoundResolved);            
        }

        public bool AllPlayersHavePicked => Participants.TrueForAll(p => p.HasPicked);
    }

    public class Combattant
    {
        public void PickMove()
        {
            HasPicked = true;
        }

        public bool HasPicked { get; set; }
        public bool Npc { get; set; }
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
