using System.Collections.Generic;
using System.Linq;
using robot.dad.combat;
using robot.dad.combat.MoveResolvers;
using robot.dad.common;

namespace robot.dad.game
{
    public class CombatDemo
    {
        public static List<ICombattant> GetProtagonists()
        {
            return new List<ICombattant>
            {
                new Human("Tommie", "nygren", MovePickers.GetRandomPicker()),
                //new Human("Lisa", "nygren", MovePickers.RandomPicker),
                //new Human("Freja", "nygren", MovePickers.RandomPicker),
                new Human("Anja", "nygren", MovePickers.GetRandomPicker(), new List<ICombatMove>()
                {
                    new CombatMove("Läka sår", CombatMoveType.Healing, 10, 5, 15, "helar", Resolvers.HealingResolver)
                })
            };
        }
        public static readonly List<Combattant> Protagonists = new List<Combattant>
        {
            new Human("Tommie", "nygren", MovePickers.GetRandomPicker()),
            //new Human("Lisa", "nygren", MovePickers.RandomPicker),
            //new Human("Freja", "nygren", MovePickers.RandomPicker),
            new Human("Anja", "nygren", MovePickers.GetRandomPicker(), new List<ICombatMove>()
            {
                new CombatMove("Läka sår", CombatMoveType.Healing, 10, 5, 15, "helar", Resolvers.HealingResolver)
            })
        };

        public static readonly List<ICombattant> Antagonists = new List<ICombattant>
        {
            new Monster("Snarfor", 30, 90, 10, 5, 10, "gargelbarg", new List<ICombatMove>()
            {
                new CombatMove("Vattenförmåga", CombatMoveType.Attack, 0, 5, 10, "sprutar vatten på", Resolvers.AttackResolver)
            }, MovePickers.GetRandomPicker()),
            //new Monster("Gargelbarg", 200, 60, 0, 5, 10, "gargelbarg", new List<CombatMove>()
            //{
            //    new CombatMove("GargelBett", CombatMoveType.Attack, 15, 12, 25, "gargelbiter", Resolvers.AttackResolver)
            //}, MovePickers.GetRandomPicker()),
            new Monster("Fyrkantsmonster", 100, 80, 30, 10, 10, "gargelbarg", new List<ICombatMove>()
            {
                new CombatMove("Hypno", CombatMoveType.Special, 0, 2, 6, "hypnotiserar", Resolvers.HypnosisResolver)
            }, MovePickers.GetRandomPicker())
        };

        public static IEnumerable<Combattant> GetAntagonists(int numberOfMonsters)
        {
            int i = 0;
            while (i < numberOfMonsters)
            {
                i++;
                yield return GetRandomAntagonist();
            }
        }

        private static IEnumerable<ICombatMove> GetScavengerMoves(int numberOfMoves)
        {
            int i = 0;
            while (i < numberOfMoves)
            {
                i++;
                yield return GetScavengerMove();
            }
        }

        private static CombatMove GetScavengerMove()
        {
            return new CombatMove(GetScavengerMoveName(), CombatMoveType.Attack, DiceRoller.RollDice(-5, 5),
                DiceRoller.RollDice(5, 15), DiceRoller.RollDice(20, 40), "skjuter", Resolvers.AttackResolver);
        }

        public static string GetScavengerMoveName()
        {
            int index = DiceRoller.RollDice(0, ScavengerMoveNames.Count - 1);
            return ScavengerMoveNames[index];
        }

        public static string GetScavengerName()
        {
            int index = DiceRoller.RollDice(0, ScavengerNames.Count - 1);
            return ScavengerNames[index];
        }

        public static List<string> ScavengerMoveNames => new List<string>
        {
            "Skrotbössa",
            "Femskjutare",
            "Minimörsare",
            "Slaktarn",
            "Splitterbrakaren"
        };

        private static Monster GetRandomAntagonist()
        {
            bool scavengerOrNot = DiceRoller.RollDice(0, 1) == 1;
            int nameIndex = DiceRoller.RollDice(0, scavengerOrNot ? ScavengerNames.Count -1 : MonsterNames.Count -1);
            string name = scavengerOrNot ? ScavengerNames[nameIndex] : MonsterNames[nameIndex];
            var monster = new Monster(name, DiceRoller.RollDice(20, scavengerOrNot ? 50:100), DiceRoller.RollDice(20, scavengerOrNot ? 100 : 50), DiceRoller.RollDice(5, scavengerOrNot ? 50 : 25), DiceRoller.RollDice(5, scavengerOrNot ? 15 : 25), DiceRoller.RollDice(3, scavengerOrNot ? 40 : 20), "enemy", scavengerOrNot ? GetScavengerMoves(DiceRoller.RollDice(1,3)).ToList(): GetMonsterMoves(DiceRoller.RollDice(1,2)).ToList(), MovePickers.GetRandomPicker());
            return monster;
        }

        private static IEnumerable<ICombatMove> GetMonsterMoves(int numberOfMoves)
        {
            int i = 0;
            while (i < numberOfMoves)
            {
                i++;
                yield return GetMonsterMove();
            }
        }

        private static CombatMove GetMonsterMove()
        {
            int test = DiceRoller.RollDice(1, 100);
            if (test > 90)
            {
                return new CombatMove("Hypnotisera", CombatMoveType.Special, -5,3, 5,"hypnotiserar", Resolvers.HypnosisResolver);
            }
            return new CombatMove(MonsterMoveNames[DiceRoller.RollDice(0, MonsterMoveNames.Count - 1)],
                CombatMoveType.Attack, DiceRoller.RollDice(-5, 5), DiceRoller.RollDice(20, 30),
                DiceRoller.RollDice(35, 65), "attackerar", Resolvers.AttackResolver);
        }

        public static List<string> MonsterMoveNames => new List<string>
        {
            "Tugga",
            "Slita",
            "Riva",
            "Stånga",
            "Mosa"
        };


        private static List<string> ScavengerNames => new List<string>
        {
            "Kraklmack",
            "Zorgmeister",
            "Harkonnen",
            "The Devil",
            "Mr. Teague",
            "Herr Nilsson",
            "Suzie",
            "Wilson",
            "Max",
            "Ronda",
        };

        private static List<string> MonsterNames => new List<string>
        {
            "Kraken",
            "Leviathan",
            "Lobsterface",
            "Gungan",
            "Snargelbark",
            "A Giant Bug",
            "The Nameless",
        };

        public void StartGame()
        {
            //var ce = new CombatEngine(Protagonists, Antagonists);
            //ce.StartCombat();
            //Console.ReadKey();
        }
    }
}