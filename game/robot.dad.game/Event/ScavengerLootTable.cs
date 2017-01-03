using rds;
using robot.dad.common;

namespace robot.dad.game.Event
{
    public sealed class ScavengerLootTable : ThingTable
    {
        public ScavengerLootTable(int numberOfScavengers)
        {
            Count = numberOfScavengers;
            AddEntry(new StoryItem(StoryItem.SomeText),100, true, true, true);
            AddEntry(new WeaponsTable(1), 300);
            AddEntry(new ArmorTable(1), 200);
            AddEntry(new BasicItem("Elektroniska komponenter", "Delar av datorer och annan utrustning från den gamla tiden"), 25);
            AddEntry(new CountableItem("Food", "Mat",10, 100));
            AddEntry(new CountableItem("Money", "Mynt",10, 100));
            Count = 3*numberOfScavengers;
        }
    }

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
    public class StoryItem : Thing, IQuestItem
    {
        public StoryItem(string cutSceneText)
        {
            CutSceneText = cutSceneText;
        }
        public string CutSceneText { get; }

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

    public interface IQuestItem
    {
        string CutSceneText { get; }
    }
}