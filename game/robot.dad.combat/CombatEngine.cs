using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appccelerate.StateMachine;

namespace dad.@is.robot.combat
{
    public class CombatEngine
    {
        public CombatEngine()
        {
            CombatStateMachine = CombatStateMachine.Create();
        }

        public CombatStateMachine CombatStateMachine { get; set; }
    }

    public class CombatStateMachine
    {
        public PassiveStateMachine<States, Events> StateMachine { get; set; }
        public static CombatStateMachine Create()
        {
            var cms = new CombatStateMachine();
            cms.StateMachine = new PassiveStateMachine<States, Events>();
            cms.StateMachine
                .In(States.BeforeCombat)
                .On(Events.Start)
                .Goto(States.ProtagonistPicking);

            cms.StateMachine
                .In(States.ProtagonistPicking)
                .On(Events.ProtagonistPicked)
                .Goto(States.AntagonistPicking);

            cms.StateMachine
                .In(States.AntagonistPicking)
                .On(Events.AntagonistPicked)
                .Goto(States.ResolveCombat);
            cms.StateMachine
                .In(States.ResolveCombat)
                .On(Events.CombatRoundResolved)
                .If()
            {

            }
            return cms;
        }

        private static bool Guard()
        {
            throw new NotImplementedException();
        }
    }

    public enum States
    {
        BeforeCombat,
        ProtagonistPicking,
        AntagonistPicking,
        ResolveCombat,
        FightOver
    }

    public enum Events
    {
        Start,
        ProtagonistPicked,
        AntagonistPicked,
        CombatRoundResolved,
        CombatOver
    }
}
