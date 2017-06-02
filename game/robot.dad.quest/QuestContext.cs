using System;
using System.Collections.Generic;

namespace robot.dad.quest
{
    public class QuestContext : IQuest
    {
        public string Title { get; }
        public string Description { get; }
        public IQuestState CurrentState { get; }
        public IQuestStep CurrentStep { get; }
        public LinkedList<IQuestStep> Steps { get; } = new LinkedList<IQuestStep>();
        public string CurrentStepTitle => CurrentStep.Title;
        public string CurrentStepDescription => CurrentStep.Description;


        public QuestContext(string title, string description, IQuestState currentState, IEnumerable<IQuestStep> steps, IQuestStep currentStep)
        {
            Title = title;
            Description = description;
            CurrentStep = currentStep;
            CurrentState = currentState;
            foreach (var questStep in steps)
            {
                Steps.AddLast(questStep);
            }
        }
    }

    public interface IQuestStep
    {
        string Title { get; }
        string Description { get; }
    }

    public class QuestStep : IQuestStep
    {
        public QuestStep(string title, string description)
        {
            Title = title;
            Description = description;
        }
        public string Title { get; }
        public string Description { get; }
    }
}