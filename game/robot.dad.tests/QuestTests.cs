using robot.dad.quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace robot.dad.tests
{
    class QuestTests
    {
        [Theory]
        public void What()
        {
            var quest = new QuestContext("","","","", new QuestingState(), Update);
        }

        public void Update()
        {
            
        }
    }
}
