﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
            PrintCombatBoard();
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

            AliveParticipants.ForEach(ap => ap.ApplyMove());

            Console.WriteLine("Rundan över!");
            Thread.Sleep(3000);
            Console.Clear();

            //This will be a doozy
            StateMachine.Fire(CheckIfCombatIsOver() ? Events.CombatOver : Events.CombatRoundResolved);
        }

        public void PrintCombatBoard()
        {
            //Assume stuff!
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

            ParticipantsThatDied = Participants.FindAll(p => p.Dead);
            //Fight is over if all participants on either team is dead!
            var group = Participants.GroupBy(p => p.Team);
            var groupingdead = group.Select(grouping => grouping.All(g => g.Dead));
            return groupingdead.Any(b => b);
        }

        public bool AllPlayersHavePicked => AliveParticipants.TrueForAll(p => p.HasPicked);
    }
}
