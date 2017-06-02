using NUnit.Framework;
using robot.dad.quest;

namespace robot.dad.tests
{
    [TestFixture]
    class QuestTests
    {
        [Test]
        public void What()
        {
            var quest = new QuestContext("Test","Test","Wurgl","Bargl", new QuestingState(), Update);

            var conf = StateMachine<

        }

        public void Update()
        {
            //This method will be called when the state is updated or something
        }
    }
}
