using System.Collections.Generic;

namespace robot.dad.quest
{
    public interface IQuest
    {
        string Title { get; }
        string Description { get; }
        string CurrentStepTitle { get; }
        string CurrentStepDescription { get; }
        IQuestState CurrentState { get; }

        IEnumerable<IQuestChoice> Choices { get; }
        //Some mechanism to enumerate possible choices and what data those choices need to contain.
        //May need changes of state machine implementation
    }
}