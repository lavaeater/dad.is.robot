using System;
using System.Collections.Generic;

namespace robot.dad.quest
{
    public class QuestContext : IQuest
    {
        public string Title { get; }
        public string Description { get; }
        public string CurrentStepTitle { get; }
        public string CurrentStepDescription { get; }
        public IQuestState CurrentState { get; }
        private readonly List<IQuestChoice> _choices = new List<IQuestChoice>();
        public IEnumerable<IQuestChoice> Choices => _choices;

        public QuestContext(string title, string description, string currentStepTitle, string currentStepDescription, IQuestState currentState, Action updated)
        {
            Title = title;
            Description = description;
            CurrentStepTitle = currentStepTitle;
            CurrentStepDescription = currentStepDescription;
            CurrentState = currentState;
            _choices.Add(new QuestChoice("Överge denna quest.", new EventCancelQuest()));
            _choices.Add(new QuestChoice("OK", null));//Null som event gör att dialogen bara stängs, inget händer ju... 
        }
    }
}