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
            AddEntry(new BasicItem("Elektroniska komponenter", "Delar av datorer och annan utrustning fr�n den gamla tiden"), 25);
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
            @"En av plundrarna drar till sig din uppm�rksamhet.
Han har n�got runt halsen du k�nner igen.
N�r du kommer n�rmare honom f�rst�r du vad det �r.
Det �r din mors halsband!
Ilsket fr�gar du honom var din mor �r men han vet inte.
Han k�pte halsbandet av en plundrare norrut.
Han visar dig p� en grov karta var.
En ledtr�d!";
    }

    public interface IQuestItem
    {
        string CutSceneText { get; }
    }

    public class QuestEngine
    {
        public IQuestItem GenerateQuest()
        {
            return new StoryItem(StoryItem.SomeText);
        }

        /*
         * What is a quest? How do we use loot tables?
         * 
         * Easy. The game will have two types of quests, 
         * Generated
         * Scripted
         * 
         * The generated type is then sub-divided into types like 
         * KillAMonster
         * 
         * FindLostPerson
         * 
         * FindArtefact
         * 
         * The scripted type can have their entirely own types 
         * of arc with custom code and everything, preferrably they should be loadable
         * from a dll, we'll see about that. Most importantly, they are UNIQUE in 
         * the space of quests. The other ones can occur multiple times
         * with variations.
         * 
         * So, our main focus is to have a Main Quest, scripted
         * and
         * ONE type of generated Quest, like KillScavengersInArea
         * 
         * So, a quest must keep track of wether or not it is fulfilled. So every
         * step on the way of a quest should be able to tell if it is fulfilled.
         * 
         * So, we need a new tileevent, settlement. They should sometimes give the player 
         * a quest to kill scavengers in the surrounding area - which would mean that this quest has 
         * two-three steps, like first a yes-no step (accepting the quest), leading to the second step,
         * a kill-counting step that keeps track of dead scavengers. The thing is, having this quest could
         * make it much likelier to bump into free-flying scavengers in the area surrounding
         * the settlement. So, we have a radius of hexes inside of which we will RAISE the 
         * chance of chance encounters (which should be invisible, by the way!) so that the area
         * actually is more dangerous. After reaching the kill count, we could indicate this, give the players
         * a chance to return to the quest start and finish it and get what all good quests should have:
         * REWARD
         * 
         * What is reward then? Loot, is the answer. This could be a special loot table that has some
         * chance, that actually might be increasing all the time, for other quests, main quest clues,
         * extra crew members, robot parts, crafting material, food, equipment - or area maps,
         * directions to bases, caves, scavengers, other settlements.
         * 
         * Finishing quests should give invisible rewards as well, like reputation. In this game
         * it isn't possible to do evil things (or should it be?), so this is more like reputation in 
         * Pirates... should there exist kingdoms like in Nausica�? Later.
         * 
         * A generated quest could, as a reward, give a quest-step for the main quest. Or other quests,
         * but let's focus on that quest.
         * 
         * The goal is to find the family and heal the wasteland as a family, again. To do this,
         * the girls have to explore the wasteland
         */
    }
}