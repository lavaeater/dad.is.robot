using System;
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
        IQuestStep CurrentStep { get; }
        LinkedList<IQuestStep> Steps { get; }

    }
}