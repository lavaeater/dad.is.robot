namespace robot.dad.quest
{
    public class QuestChoice : IQuestChoice
    {
        public QuestChoice(string title, IQuestEvent questEventType)
        {
            Title = title;
            QuestEvent = questEventType;
        }

        public string Title { get; }
        public IQuestEvent QuestEvent { get; }
    }
}