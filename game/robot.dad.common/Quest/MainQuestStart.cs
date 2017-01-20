using rds;

namespace robot.dad.common.Quest
{
    /// <summary>
    /// Represents either a unique story item (created by me with specific content)
    /// Or a generated quest of some sort. Yay!
    /// 
    /// In both cases, find a quest item should create an entry into a quest table 
    /// or collection of some sort, so that if the player finds more quest items they 
    /// can by some predetermined probability either be standalone or be add-ons
    /// to that existing quest.
    /// 
    /// For now, we will simply have the event engine detect this particular drop
    /// and send us to a cut scene. Fuck yeah!
    /// </summary>
    public class MainQuestStart : Thing, IQuestItem
    {
        public MainQuestStart()
        {
            CutSceneText = MainQuestStart.SomeText;
            QuestId = "MainQuest";
            Type = QuestType.Scripted;
            NextStep = null;
        }

        public string QuestId { get; }
        public QuestType Type { get; }
        public QuestItemState State { get; set; } = QuestItemState.NotStarted;
        public string CutSceneText { get; }
        public IQuestItem NextStep { get; }
        public bool CanStart => true;

        public void Accept()
        {
            State = QuestItemState.Started;
        }

        public void Finish()
        {
            State = QuestItemState.Finished;
        }

        public static string SomeText =
            @"En av plundrarna drar till sig din uppmärksamhet.
Han har något runt halsen du känner igen.
När du kommer närmare honom förstår du vad det är.
Det är din mors halsband!
Ilsket frågar du honom var din mor är men han vet inte.
Han köpte halsbandet av en plundrare norrut.
Han visar dig på en grov karta var.
En ledtråd!";
    }
}