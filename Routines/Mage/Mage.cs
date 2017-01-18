using System;
using System.Collections.Generic;
using System.Linq;

using WoWSharp.Logics.Combats;
using WoWSharp.Logics.Combats.Rotations;
using WoWSharp.WoW;
using WoWSharp.WoW.Impl.Objects;

namespace WoWSharp.DefaultRoutines.Mage
{
    public class Mage : CombatRoutine
    {
        public enum Spells : int
        {
            Invisibility            = 66,
            ConeOfCold              = 120,
            Fireball                = 133,
            FrostBolt               = 116,
            ArcaneMissiles          = 5143,
            Pyroblast               = 11366,
            ArcanePower             = 12042,
            Evocation               = 12051,
            IcyVeins                = 12472,
            ArcaneBlast             = 30451,
            IceLance                = 30455,
            SummonWaterElemental    = 31687,
            ArcaneBarrage           = 44425,
            Flurry                  = 44614,
            MirrorImage             = 55342,
            FrozenOrb               = 84714,
            FireBlast               = 108853,
            RuneOfPower             = 116011,
            RingOfFrost             = 113724,
            NetherTempest           = 114923,
            CometStorm              = 153595,
            ArcaneOrb               = 153626,
            IceNova                 = 157980,
            Supernova               = 157997,
            Combustion              = 190319,
            Blizzard                = 190356,
            GlacialSpike            = 199786,
            RayOfFrost              = 205021,
            ArcaneFamiliar          = 205022,
            FrozenTouch             = 205030,
            ChargedUp               = 205032,
            TimeWarp                = 6680353
        }

        public enum Auras : int
        {
            FingersOfFrost          = 44544,
            HotStreak               = 48108,
            ArcaneMissiles          = 79683,
            NetherTempest           = 114923,
            BrainFreeze             = 190446,
            ArcaneFamiliar          = 210126
        }

        public override string Name { get { return "Mage - Default"; } }

        public override double CombatRange { get { return 30.0; } }

        public override double PullRange { get { return 30.0; } }

        public override void OnStart()
        {
            RotationSpell l_Spell;
            m_RotationBook = new RotationBook();

            if (WoW.ObjectManager.ActivePlayer.Specialization == WowPlayer.Specializations.MageFire)
            {
                Console.WriteLine("[Mage] Loading fire rotation book ...");

                // Pyroblast when Hot Streak proc
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Pyroblast);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && p_Unit.HasAura((int)Auras.HotStreak));

                // Fireball
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Fireball);
                l_Spell.Match               = MatchType.MatchAll;
            }
            else if (WoW.ObjectManager.ActivePlayer.Specialization == WowPlayer.Specializations.MageArcane)
            {
                Console.WriteLine("[Mage] Loading arcane rotation book ...");
                #region Arcane

                // Arcane Familiar
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ArcaneFamiliar);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.CancelOtherCast     = true;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && !p_Unit.HasAura((int)Auras.ArcaneFamiliar));

                // Invisibility if HP < 30%
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Invisibility);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.CancelOtherCast     = true;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && p_Unit.HealthPercent < 30);

                // Evocation if Mana <= 20%
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Evocation);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && p_Unit.GetPowerPercent(WowUnit.PowerType.Mana) <= 20);

                // Arcane Power
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ArcanePower);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;

                // Time Warp
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.TimeWarp);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;

                // Mirror Image
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.MirrorImage);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && p_Unit.HealthPercent > 20);
                
                // Rune of Power
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.RuneOfPower);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && p_Unit.Position.Distance3D(ObjectManager.ActivePlayer.Position) - p_Unit.CombatReach < 45);

                // Arcane Missiles on proc
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ArcaneMissiles);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer,    p_Unit => p_Unit != null && p_Unit.HasAura((int)Auras.ArcaneMissiles));

                // Nether Tempest
                l_Spell = m_RotationBook.AddSpell("Combat", (int)Spells.NetherTempest);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit,   p_Unit => p_Unit != null && !p_Unit.HasAura((int)Auras.NetherTempest));

                // Charged Up
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ChargedUp);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && p_Unit.GetPower(WowUnit.PowerType.ArcaneCharges) == 0);
                
                // Arcane Barrage on 4 charges
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ArcaneBarrage);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && p_Unit.GetPower(WowUnit.PowerType.ArcaneCharges) >= 4);
                
                // Arcane Orb when more than 2 adds (including current target) are in front of player
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ArcaneOrb);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x => 
                    !x.IsDead &&
                    p_Unit.IsFacingHeading(x.Position, 0.8f) &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 40.0f &&
                    x.CanAttack) > 2);

                // Ring of Frost when more than 2 adds (including current target)
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.RingOfFrost);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsAoE               = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x =>
                    !x.IsDead &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 10.0f &&
                    x.CanAttack) > 2);

                // Supernova when more than 2 adds (including current target)
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Supernova);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x =>
                    !x.IsDead &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 8.0f &&
                    x.CanAttack) > 2);

                // Arcane Blast
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ArcaneBlast);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && p_Unit.GetPower(WowUnit.PowerType.ArcaneCharges) < 4);

                #endregion
            }
            else if (WoW.ObjectManager.ActivePlayer.Specialization == WowPlayer.Specializations.MageFrost)
            {
                Console.WriteLine("[Mage] Loading frost rotation book ...");
                #region Frost

                // Invisibility if HP < 30%
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Invisibility);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.CancelOtherCast     = true;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null && p_Unit.HealthPercent < 30);

                // Icy Veins
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.IcyVeins);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;

                // Time Warp
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.TimeWarp);
                l_Spell.CastOn              = EvaluedUnitType.SelfPlayer;
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;

                // Mirror Image
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.MirrorImage);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && p_Unit.HealthPercent > 20);

                // Rune of Power
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.RuneOfPower);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsBigCooldown       = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && p_Unit.Position.Distance3D(ObjectManager.ActivePlayer.Position) - p_Unit.CombatReach < 45);

                // Flurry when Brain Freeze proc
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Flurry);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer,    p_Unit => p_Unit != null && p_Unit.HasAura((int)Auras.BrainFreeze));
                //l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit,   p_Unit => ObjectManager.ActivePlayer.IsFacingHeading(p_Unit.Position));

                // Ice Lance when Fingers of Frost proc
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.IceLance);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer,    p_Unit => p_Unit != null && p_Unit.HasAura((int)Auras.FingersOfFrost));
                //l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit,   p_Unit => ObjectManager.ActivePlayer.IsFacingHeading(p_Unit.Position));

                // Cone of Cold when more than 2 adds (including current target) are in front of player
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.ConeOfCold);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.SelfPlayer, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x => 
                    !x.IsDead &&
                    p_Unit.IsFacingHeading(x.Position) &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 10.0f &&
                    x.CanAttack) > 2);
                
                // Frozen Orb when more than 2 adds (including current target)
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.FrozenOrb);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x =>
                    !x.IsDead &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 8.0f &&
                    x.CanAttack) > 2);

                // Comet Storm when more than 2 adds (including current target)
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.CometStorm);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsAoE               = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x =>
                    !x.IsDead &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 5.0f &&
                    x.CanAttack) > 2);

                // Blizzard when more than 2 adds (including current target)
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.Blizzard);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsAoE               = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x =>
                    !x.IsDead &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 8.0f &&
                    x.CanAttack) > 2);

                // Ice Nova when more than 2 adds (including current target)
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.IceNova);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x =>
                    !x.IsDead &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 8.0f &&
                    x.CanAttack) > 2);

                // Ring of Frost when more than 2 adds (including current target)
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.RingOfFrost);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.IsAoE               = true;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null &&
                    ObjectManager.GetObjects<WowUnit>().Count(x =>
                    !x.IsDead &&
                    x.Position.Distance3D(p_Unit.Position) - x.CombatReach < 10.0f &&
                    x.CanAttack) > 2);

                // Glacial Spike
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.GlacialSpike);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && p_Unit.HealthPercent > 20);
                //l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && ObjectManager.ActivePlayer.IsFacingHeading(p_Unit.Position));

                // Ray of Frost
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.RayOfFrost);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && p_Unit.HealthPercent > 20);
                //l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => p_Unit != null && ObjectManager.ActivePlayer.IsFacingHeading(p_Unit.Position));

                // Frostbolt
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.FrostBolt);
                l_Spell.Match               = MatchType.MatchAll;
                //l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => ObjectManager.ActivePlayer.IsFacingHeading(p_Unit.Position));

                #endregion
            }
            else
            {
                Console.WriteLine("[Mage] Loading no spec rotation book");
                #region NoSpec

                // Frostbolt
                l_Spell                     = m_RotationBook.AddSpell("Combat", (int)Spells.FrostBolt);
                l_Spell.Match               = MatchType.MatchAll;
                l_Spell.AddEvaluator(EvaluedUnitType.EvaluedUnit, p_Unit => ObjectManager.ActivePlayer.IsFacingHeading(p_Unit.Position));

                #endregion
            }
        }
        
        private RotationBook m_RotationBook = new RotationBook();

        /// <summary>
        /// Called when combating an unit
        /// </summary>
        /// <param name="p_Target"></param>
        /// <param name="p_UseBigCooldowns"></param>
        public override void OnCombat(WowUnit p_Target, bool p_UseBigCooldowns)
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;

            if (l_ActivePlayer == null)
                return;

            if (m_RotationBook.RunRotation("Combat" ,p_Target, p_UseBigCooldowns))
                return;
        }

        public override void OnPulse()
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;

            if (l_ActivePlayer == null)
                return;
        }

    }
}
