namespace robot.dad.quest
{
    public interface IQuestChoice
    {
        string Title { get; }
        IQuestEvent QuestEvent { get; }
    }
}