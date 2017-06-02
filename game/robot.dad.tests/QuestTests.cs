using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using robot.dad.quest;
using Stateful;

namespace robot.dad.tests
{
    /*
     * class QuestNotStarted: IQuestState
class Questing: IQuestState
class QuestNeedsNewStep: IQuestState
class QuestIsFailed: IQuestState
class QuestIsDone: IQuestState

interface IQuestEvent
class StepSelected:IQuestEvent
class StepFailed:IQuestEvent
class StepSucceeded:IQuestEvent
class QuestFailed:IQuestEvent
class QuestSucceeded:IQuestEvent

interface IQuest
class Quest: IQuest

class QuestEngine {
    val configurator = StateMachine.createConfigurator<IQuest, IQuestState, IQuestEvent>()
    init {
        configurator
                .state(QuestNotStarted::class)
                .event(StepSelected::class)
                .goto { Questing() }
        configurator
                .state(Questing::class)
                .event(StepSucceeded::class)
                .goto { QuestNeedsNewStep()  }
        configurator
                .state(Questing::class)
                .event(StepFailed::class)
                .goto { QuestNeedsNewStep()  }
        configurator
                .state(QuestNeedsNewStep::class)
                .event(QuestFailed::class)
                .goto{ QuestIsFailed() }
        configurator
                .state(QuestNeedsNewStep::class)
                .event(QuestSucceeded::class)
                .goto { QuestIsDone() }
    }

    fun start(quest: IQuest): IExecutor<IQuest, IQuestState, IQuestEvent> {
        return configurator.createExecutor(quest, QuestNotStarted())
    }
     */
    [TestFixture]
    class QuestTests
    {
        [Test]
        public void What()
        {
            var engine = new QuestEngine(Update);

            var choice = engine.Choices.First();

            engine.AChoiceWasMade(choice);

            Assert.That(engine.Choices, Is.Empty);



        }

        public void Update()
        {
            //This method will be called when the state is updated or something
            string yihaa = "Yihaa";
        }
    }

    class QuestEngine
    {
        public Action Updated { get; }
        private readonly IList<IQuestChoice> _choices = new List<IQuestChoice>();

        public IEnumerable<IQuestChoice> Choices => _choices;


        /*
         * Is this the engine? Yes, this contains a public context, from which
         * the ui will be built
         * It also contains a statemachine, and perhaps methods to move to make choices 
         * and stuff?
         */
        public IQuest Quest { get; set; }
        
        public StateMachine<IQuest, IQuestState,IQuestEvent>.IConfigurator Configurator { get; }
        public StateMachine<IQuest, IQuestState,IQuestEvent>.IExecutor Executor { get; }


        public QuestEngine(Action updated)
        {
            Updated = updated;
            var currentStep = new QuestStep("Färdas till öster", "WYryryryry");
            Quest = new QuestContext("Test", "Test", new StateQuestNotStarted(), new List<IQuestStep>()
            {
                currentStep,
                new QuestStep("HUUUUR SKA DET HÄR FUNGERA?", "UWDWDJWDJ")
            }, null);
            Configurator = StateMachine<IQuest, IQuestState, IQuestEvent>.NewConfigurator();
            ConfigureStateMachine();
            Executor = StateMachine<IQuest, IQuestState, IQuestEvent>.NewExecutor(Configurator, Quest,
                new StateQuestNotStarted());
            UpdateChoices();
        }
        
        private void ConfigureStateMachine()
        {
            Configurator.In<StateQuestNotStarted>()
                .OnEnter(context => Update())
                .On<EventStepSelected>()
                .Goto(context => new StateQuesting());

            Configurator.In<StateQuesting>()
                .OnEnter(context => Update());

        }

        public void AChoiceWasMade(IQuestChoice choice)
        {
            if(choice.QuestEvent != null)
                Executor.Fire(choice.QuestEvent);
        }

        private void Update()
        {
            //Set som props and then call updated
            UpdateChoices();


            Updated.Invoke();
        }

        private void UpdateChoices()
        {
            if (Executor?.State == null) return;
            _choices.Clear();
            if (Executor.State is StateQuestNotStarted)
            {
                //What choices are available at this moment?
                _choices.Add(new QuestChoice("Påbörja denna quest",
                    new EventStepSelected(Quest.Steps.First.Value)));
            }
            if (Executor.State is StateQuesting)
            {
                //No choices, no nothing, something external has to happen! 
            }
        }
    }

    internal class EventStepSelected : IQuestEvent
    {
        public EventStepSelected(IQuestStep step)
        {
            Step = step;
        }
        public IQuestStep Step { get; }
    }

    internal class StateQuestNotStarted : IQuestState
    {
        public string Title => "NotStarted";
    }
}
