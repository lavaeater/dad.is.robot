namespace robot.dad.common.Quest
{
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