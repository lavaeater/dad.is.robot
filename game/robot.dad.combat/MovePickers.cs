using System;
using System.Collections.Generic;
using System.Linq;

namespace robot.dad.combat
{
    public static class MovePickers
    {
        public static void RandomPicker(Combattant picker, IEnumerable<Combattant> possibleTargets,
            List<CombatMove> possibleMoves)
        {
            picker.CurrentMove = possibleMoves[DiceRoller.RollDice(0, possibleMoves.Count - 1)];
            if (picker.CurrentMove.MoveType == CombatMoveType.Healing)
            {
                var pts = possibleTargets.Where(pt => pt.Team == picker.Team).ToList();
                picker.CurrentTarget = pts[DiceRoller.RollDice(0, pts.Count - 1)];
            }
            else
            {
                var pts = possibleTargets.Where(pt => pt.Team != picker.Team).ToList();
                picker.CurrentTarget = pts[DiceRoller.RollDice(0, pts.Count - 1)];
            }
        }

        public static void RandomReversePicker(Combattant picker, IEnumerable<Combattant> possibleTargets,
    List<CombatMove> possibleMoves)
        {
            picker.CurrentMove = possibleMoves[DiceRoller.RollDice(0, possibleMoves.Count - 1)];
            if (picker.CurrentMove.MoveType == CombatMoveType.Healing)
            {
                var pts = possibleTargets.Where(pt => pt.Team != picker.Team).ToList();
                picker.CurrentTarget = pts[DiceRoller.RollDice(0, pts.Count - 1)];
            }
            else
            {
                var pts = possibleTargets.Where(pt => pt.Team == picker.Team).ToList();
                picker.CurrentTarget = pts[DiceRoller.RollDice(0, pts.Count - 1)];
            }
        }

        public static void ManualPicker(Combattant picker, IEnumerable<Combattant> possibleTargets, List<CombatMove> possibleMoves)
        {
            //1. List targets and make player choose one!
            var pts = possibleTargets.Where(pt => pt.Team != picker.Team).ToList();
            int index = 1;
            Console.WriteLine("Välj vem du vill attackera genom att mata in siffran");
            foreach (var possibleTarget in pts)
            {
                Console.WriteLine($"{index}. {possibleTarget.Name}");
                index++;
            }
            var choice = Console.ReadKey();
            int targetIndex = 0;
            if (char.IsDigit(choice.KeyChar))
            {
                targetIndex = int.Parse(choice.KeyChar.ToString()) - 1;
            }
            picker.CurrentTarget = pts[targetIndex];

            //2. Choose attack
            index = 1;
            Console.WriteLine("Välj attack");
            foreach (var possibleMove in possibleMoves)
            {
                Console.WriteLine($"{index}. {possibleMove.Name}");
                index++;
            }
            choice = Console.ReadKey();
            targetIndex = 0;
            if (char.IsDigit(choice.KeyChar))
            {
                targetIndex = int.Parse(choice.KeyChar.ToString()) - 1;
            }
            picker.CurrentMove = possibleMoves[targetIndex];
        }
    }
}