using System;
using System.Collections.Generic;
using System.Linq;
using robot.dad.combat;
using robot.dad.common;
using robot.dad.game.Scenes;

namespace robot.dad.game.Entities
{
    public class GraphicalPicker : MovePickerBase
    {
        public CombatScene Scene { get; set; }

        public GraphicalPicker(Action donePicking, CombatScene scene) : base(donePicking)
        {
            Scene = scene;
        }

        public CombattantCard CurrentCard { get; set; }

        public override void PickMove(ICombattant attacker, IEnumerable<ICombattant> possibleTargets)
        {
            //1. Find card
            CurrentCard = Scene.GetEntities<CombattantCard>().Single(cc => cc.Combattant == attacker);

            //2. Put it in "picking mode"
            CurrentCard.SetInPickMoveMode(AMoveWasPicked);

            //3. Wait for input... like a click on something? Read on ze internet

        }

        public void AMoveWasPicked(ICombatMove pickedMove)
        {
            CurrentCard.Combattant.CurrentMove = pickedMove;
            CurrentCard.StopPicking();
            foreach (var card in Scene.CombattantCards.Except(new []{ CurrentCard}))
            {
                if(card.Combattant.Status == CombatStatus.Active)
                    card.MakePickable(ATargetWasPicked);
            }
        }

        public void ATargetWasPicked(Combattant target)
        {
            foreach (var card in Scene.CombattantCards.Except(new[] { CurrentCard }))
            {
                card.StopBeingPickable();
            }
            CurrentCard.Combattant.CurrentTarget = target;
            DonePicking();
        }
    }
}