using System;
using System.Linq;

using WoWSharp.Logics.Combats;
using WoWSharp.WoW.Impl.Objects;

namespace WoWSharp.DefaultRoutines.Mage
{
    public class Mage : CombatRoutine
    {
        #region Spells

        /* Generals */
        private static readonly WoW.Spell FrostBolt             = new WoW.Spell(116);
        private static readonly WoW.Spell FireBlast             = new WoW.Spell(108853);
        private static readonly WoW.Spell Invisibility          = new WoW.Spell(66);
        private static readonly WoW.Spell MirrorImage           = new WoW.Spell(55342);
        private static readonly WoW.Spell RuneOfPower           = new WoW.Spell(116011);
        private static readonly WoW.Spell TimeWarp              = new WoW.Spell(80353);

        /* Arcane Spec */
        private static readonly WoW.Spell ArcaneBlast           = new WoW.Spell(30451);
        private static readonly WoW.Spell ArcaneMissiles        = new WoW.Spell(5143);

        /* Fire Spec */
        private static readonly WoW.Spell Fireball              = new WoW.Spell(133);
        private static readonly WoW.Spell Pyroblast             = new WoW.Spell(11366);
        private static readonly WoW.Spell Combustion            = new WoW.Spell(190319);

        /* Frost Spec */
        private static readonly WoW.Spell FrozenOrb             = new WoW.Spell(84714);
        private static readonly WoW.Spell IcyVeins              = new WoW.Spell(12472);
        private static readonly WoW.Spell FrozenTouch           = new WoW.Spell(205030);
        private static readonly WoW.Spell IceLance              = new WoW.Spell(30455);
        private static readonly WoW.Spell SummonWaterElemental  = new WoW.Spell(31687);
        private static readonly WoW.Spell Flurry                = new WoW.Spell(44614);

        #endregion

        #region Buffs

        /* Arcane Spec */
        private static readonly WoW.Spell ArcaneMissilesProc    = new WoW.Spell(79683);

        /* Fire Spec */
        private static readonly WoW.Spell HotStreak             = new WoW.Spell(48108);

        /* Frost Spec */
        private static readonly WoW.Spell FingersOfFrost        = new WoW.Spell(44544);
        private static readonly WoW.Spell BrainFreeze           = new WoW.Spell(190446);

        #endregion

        #region Helpers

        private static WowUnit GetWaterElementalUnit(WowUnit p_Owner)
        {
            var l_Results = WoW.ObjectManager.GetObjects<WowUnit>().Where(x => x.CreatedByGuid == p_Owner.Guid && x.SummonedByGuid == p_Owner.Guid);

            return l_Results.FirstOrDefault();
        }

        #endregion

        public override string Name { get { return "Mage - Default"; } }

        public override double CombatRange { get { return 30.0; } }

        public override double PullRange { get { return 30.0; } }

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

            var l_TargetCombatRange = l_ActivePlayer.Position.Distance3D(p_Target.Position);
            var l_PlayerSpecialization = l_ActivePlayer.Specialization;
            var l_TargetLineOfSight = p_Target.IsLineOfSight();
            var l_PlayerCastingSpell = l_ActivePlayer.CastingSpell;

            if (l_PlayerSpecialization == WowPlayer.Specializations.MageFrost &&
                l_ActivePlayer.HasAura(BrainFreeze.SpellId) && 
                Flurry.CanCast(p_Target) && 
                Flurry.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                Flurry.Cast(p_Target);
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.MageFire &&
                l_ActivePlayer.HasAura(HotStreak.SpellId) && 
                Pyroblast.CanCast(p_Target) && 
                Pyroblast.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                Pyroblast.Cast(p_Target);
                return;
            }

            if (l_PlayerCastingSpell != null)
            {
                // Nothing more to do while casting ...
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.MageArcane &&
                l_ActivePlayer.HasAura(ArcaneMissilesProc.SpellId) &&
                ArcaneMissiles.CanCast(p_Target) &&
                ArcaneMissiles.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                ArcaneMissiles.Cast(p_Target);
                return;
            }

            if (p_UseBigCooldowns &&
                l_ActivePlayer.IsInCombat&&
                Invisibility.CanCast() &&
                l_ActivePlayer.HealthPercent <= 100)
            {
                Invisibility.Cast();
                return;
            }

            if (p_UseBigCooldowns &&
                l_ActivePlayer.IsInCombat&&
                MirrorImage.CanCast())
            {
                MirrorImage.Cast();
                return;
            }

            if (p_UseBigCooldowns &&
                l_ActivePlayer.IsInCombat &&
                RuneOfPower.CanCast())
            {
                RuneOfPower.Cast();
                return;
            }

            if (p_UseBigCooldowns &&
                l_ActivePlayer.IsInCombat &&
                TimeWarp.CanCast())
            {
                TimeWarp.Cast();
                return;
            }

            if (p_UseBigCooldowns &&
                l_PlayerSpecialization == WowPlayer.Specializations.MageFrost &&
                l_ActivePlayer.IsInCombat&&
                l_ActivePlayer.IsFacingHeading(p_Target.Position) &&
                l_TargetCombatRange < 40.0f &&
                FrozenOrb.CanCast(p_Target))
            {
                FrozenOrb.Cast(p_Target);
                return;
            }

            if (p_UseBigCooldowns &&
                l_PlayerSpecialization == WowPlayer.Specializations.MageFire &&
                l_ActivePlayer.IsInCombat&&
                Combustion.CanCast())
            {
                Combustion.Cast();
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.MageFrost &&
                l_ActivePlayer.IsInCombat&&
                p_UseBigCooldowns &&
                IcyVeins.CanCast())
            {
                IcyVeins.Cast();
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.MageFrost &&
                l_ActivePlayer.IsInCombat&&
                p_UseBigCooldowns &&
                FrozenTouch.CanCast())
            {
                FrozenTouch.Cast();
                return;
            }
            
            if (l_PlayerSpecialization == WowPlayer.Specializations.MageFrost && 
                l_ActivePlayer.HasAura(FingersOfFrost.SpellId) &&
                IceLance.IsKnown &&
                IceLance.CanCast(p_Target) && 
                IceLance.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                Console.WriteLine("[Mage] Fingers of Frost found, cast Ice Lance !");
                IceLance.Cast(p_Target);
                return;
            }
            
            if ((l_PlayerSpecialization == WowPlayer.Specializations.None || l_PlayerSpecialization == WowPlayer.Specializations.MageFire) &&
                FireBlast.IsKnown &&
                FireBlast.CanCast(p_Target) && 
                FireBlast.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                FireBlast.Cast(p_Target);
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.MageFire &&
                Fireball.CanCast(p_Target) &&
                Fireball.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                Fireball.Cast(p_Target);
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.MageArcane &&
                ArcaneBlast.CanCast(p_Target) &&
                ArcaneBlast.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                ArcaneBlast.Cast(p_Target);
                return;
            }

            if (FrostBolt.CanCast(p_Target) && 
                FrostBolt.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                FrostBolt.Cast(p_Target);
                return;
            }
        }

        public override void OnPulse()
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;

            if (l_ActivePlayer == null)
                return;

            if (l_ActivePlayer.Specialization == WowPlayer.Specializations.MageFrost)
            {
                var l_WaterElemental = GetWaterElementalUnit(l_ActivePlayer);
                if (SummonWaterElemental.IsKnown &&
                    (l_WaterElemental == null || l_WaterElemental.IsDead) &&
                    SummonWaterElemental.CanCast())
                {
                    SummonWaterElemental.Cast();
                    return;
                }
            }
        }

    }
}
