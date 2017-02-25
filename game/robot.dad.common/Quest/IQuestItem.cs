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

    public class QuestItemBase : IQuestItem
    {
        public string QuestId { get; }
        public QuestType Type { get; }
        public QuestItemState State { get; }
        public string CutSceneText { get; }

        public virtual IQuestItem NextStep => QuestEngine.GetNextStep(this);

        public bool CanStart { get; }
        public void Accept()
        {
            throw new System.NotImplementedException();
        }

        public void Finish()
        {
            throw new System.NotImplementedException();
        }
    }
}