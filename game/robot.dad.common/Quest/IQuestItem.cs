namespace robot.dad.common.Quest
{
    public interface IQuestItem
    {
        string QuestId { get; } //Generated this somehow... from type or something? Connect them all?
        QuestType Type { get; }
        QuestItemState State { get; }
        string CutSceneText { get; }
        IQuestItem NextStep { get; }
        bool CanStart { get; }
        void Accept();
        void Finish();
    }
}