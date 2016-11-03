using System;
using System.Collections.Generic;

namespace robot.dad.combat
{
    public class CombatMove
    {
        private readonly IApplyMoveEffects _effectApplier;

        public CombatMove(string name, CombatMoveType moveType, int modifier, int minDamage, int maxDamage, string verbified, IApplyMoveEffects effectApplier)
        {
            _effectApplier = effectApplier;
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Verbified = verbified;
        }

        public CombatMove(string name, CombatMoveType moveType, int modifier, string verbified, Action<CombatMove, Combattant, Combattant> moveApplier)
        {
            if (moveType == CombatMoveType.Attack)
            {
                throw new InvalidOperationException("Attack move types MUST have min and max damage");
            }
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            Verbified = verbified;
            ApplyMove = moveApplier;
        }

        public void Apply(Combattant attacker, Combattant target)
        {
            ApplyMove.Invoke(this, attacker, target);
        }

        public string Name { get; set; }
        public CombatMoveType MoveType { get; set; }
        public int Modifier { get; set; }
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }
        public string Verbified { get; set; }
        public Action<CombatMove, Combattant, Combattant> ApplyMove { get; set; }
    }

    public interface IResolveMove
    {
        void ResolveMove(CombatMove move, Combattant attacker, Combattant target);
    }

    public interface IApplyMoveEffects
    {
        string EffectName { get; set; }
        int Min { get; set; }
        int Max { get; set; }
        int LastRound { get; set; }
        EffectType EffectType { get; set; }
        void ApplyEffects(Combattant target);
        void EffectsEnded(Combattant target);
    }

    public abstract class ResolveMoveBase : IResolveMove
    {
        public abstract void ResolveMove(CombatMove move, Combattant attacker, Combattant target);
    }

    public abstract class ApplyMoveEffectsBase: IApplyMoveEffects
    {
        protected ApplyMoveEffectsBase(int min, int max, int lastRound, EffectType effectType)
        {
            Min = min;
            Max = max;
            LastRound = lastRound;
            EffectType = effectType;
        }

        public string EffectName { get; set; }
        public string AffectedProperty { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int LastRound { get; set; }
        public EffectType EffectType { get; set; }
        public abstract void ApplyEffects(Combattant target);

        public virtual void EffectsEnded(Combattant target)
        {
            throw new NotImplementedException("Recurring effects need a method to remove the effects!");
        }
    }

    public class NormalDamageEffectApplier : ApplyMoveEffectsBase 
    {
        public NormalDamageEffectApplier(int min, int max) : base(min, max, 0, EffectType.Immediate)
        {
        }

        public override void ApplyEffects(Combattant target)
        {
            int damageRoll = DiceRoller.RollDice(Min, Max);
            Console.WriteLine($", träffar och gör {target.ApplyDamage(damageRoll)} i skada!");
            Console.WriteLine($"{target.Name} har {target.CurrentHealth} kvar i hälsa.");
        }
    }

    public class HealingEffectApplier : ApplyMoveEffectsBase
    {
        public HealingEffectApplier(int min, int max) : base(min, max, 0, EffectType.Immediate)
        {
        }

        public override void ApplyEffects(Combattant target)
        {
            int damageRoll = DiceRoller.RollDice(Min, Max);
            target.CurrentHealth += damageRoll;
            if (target.CurrentHealth > target.Health) target.CurrentHealth = target.Health;
            Console.WriteLine($", lyckas och helar {damageRoll} hälsa!");
            Console.WriteLine($"{target.Name} har {target.CurrentHealth} kvar i hälsa.");
        }
    }

    public abstract class RecurringEffectApplierBase : ApplyMoveEffectsBase
    {
        protected RecurringEffectApplierBase(int min, int max, int lastRound) : base(min, max, lastRound, EffectType.Recurring)
        {
        }
    }

    public class HypnosisEffectApplier: RecurringEffectApplierBase
    {
        private bool _hasBeenApplied = false;
        public HypnosisEffectApplier(int lastRound) : base(0, 0, lastRound)
        {
        }

        public override void ApplyEffects(Combattant target)
        {
            //Switches movepickers!
            Console.WriteLine($"{target.Name} är hypnotiserad!");
            if (!_hasBeenApplied)
            {
                OriginalPicker = target.MovePicker;
                target.MovePicker = MovePickers.RandomReversePicker;
                _hasBeenApplied = true;
            }
        }

        public Action<Combattant, List<Combattant>, List<CombatMove>> OriginalPicker { get; set; }

        public override void EffectsEnded(Combattant target)
        {
            target.MovePicker = OriginalPicker;
        }
    }

    public enum EffectType
    {
        Immediate,
        Recurring
    }

    public class ResolveAttackMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
        {
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            if (target.CurrentMove.MoveType == CombatMoveType.Defend ||
                target.CurrentMove.MoveType == CombatMoveType.Runaway)
            {
                targetValue += target.CurrentMove.Modifier;
            }
            int perfectRollValue = targetValue / 10;

            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                Console.Write($"slår {diceRoll}");
                IApplyMoveEffects applier = diceRoll <= perfectRollValue ? new NormalDamageEffectApplier(move.MaxDamage, move.MaxDamage) : new NormalDamageEffectApplier(move.MinDamage, move.MaxDamage);
                target.CombatEffects.Add(applier);
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        }
    }

    public class ResolveHealingMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
        {
            int targetValue = attacker.AttackSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                Console.Write($"slår {diceRoll}");
                var applier = diceRoll <= 5 ? new HealingEffectApplier(move.MaxDamage, move.MaxDamage) : new HealingEffectApplier(move.MinDamage, move.MaxDamage);
                target.CombatEffects.Add(applier);
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        }
    }

    public class ResolveRunawayMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
        {
            int targetValue = attacker.DefenseSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //vad gör perfekt slag och hur?
                Console.WriteLine($"och slår {diceRoll} och {move.Verbified}!");
                attacker.Runaway();
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        }
    }

    public class ResolveHypnosisMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
        {
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                Console.Write($"och slår {diceRoll}");
                int perfectRoll = targetValue/10;
                var applier = new HypnosisEffectApplier(diceRoll <= perfectRoll ? move.MaxDamage : DiceRoller.RollDice(move.MinDamage, move.MaxDamage));
                target.CombatEffects.Add(applier);
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        }
    }

    public static class CombatMoveAppliers
    {
        public static Action<CombatMove, Combattant, Combattant> DamageApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            if (target.CurrentMove.MoveType == CombatMoveType.Defend ||
                target.CurrentMove.MoveType == CombatMoveType.Runaway)
            {
                targetValue += target.CurrentMove.Modifier;
            }
            int perfectRollValue = targetValue/10;

            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                int damageRoll = diceRoll <= perfectRollValue ? move.MaxDamage : DiceRoller.RollDice(move.MinDamage, move.MaxDamage);
                Console.WriteLine($"och slår {diceRoll}, träffar och gör {target.ApplyDamage(damageRoll)} i skada!");
                Console.WriteLine($"{target.Name} har {target.CurrentHealth} kvar i hälsa.");
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> HealingApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.AttackSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {               
                //1 == perfekt slag!
                int damageRoll = diceRoll <= 5 ? move.MaxDamage : DiceRoller.RollDice(move.MinDamage, move.MaxDamage);
                target.CurrentHealth += damageRoll;
                if (target.CurrentHealth > target.Health) target.CurrentHealth = target.Health;
                Console.WriteLine($"och slår {diceRoll}, lyckas och helar {damageRoll} hälsa!");
                Console.WriteLine($"{target.Name} har {target.CurrentHealth} kvar i hälsa.");
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> HypnosisApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                int damageRoll = diceRoll <= 5 ? move.MaxDamage : DiceRoller.RollDice(move.MinDamage, move.MaxDamage);
                Console.WriteLine($"och slår {diceRoll}, träffar och {move.Verbified} {target.Name}!");
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> RunawayApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.DefenseSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //vad gör perfekt slag och hur?
                Console.WriteLine($"och slår {diceRoll} och {move.Verbified}!");
                attacker.Runaway();
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> DefendApplier = (move, attacker, target) =>
        {
            Console.WriteLine($"{attacker.Name} {move.Verbified}");
        };
    }

}