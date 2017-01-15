using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWSharp.Logics.Combats;
using WoWSharp.WoW.Impl.Objects;

namespace WoWSharp.DefaultRoutines.DemonHunter
{
    public class DemonHunter : CombatRoutine
    {
        #region Spells

        /* Generals */
        private static readonly WoW.Spell ThrowGlaive           = new WoW.Spell(185123);
        private static readonly WoW.Spell ConsumeMagic          = new WoW.Spell(183752);
        private static readonly WoW.Spell Blur                  = new WoW.Spell(198589);

        /* Havoc Spec */
        private static readonly WoW.Spell Metamorphosis         = new WoW.Spell(191427);
        private static readonly WoW.Spell EyeBeam               = new WoW.Spell(198013);
        private static readonly WoW.Spell ChaosStrike           = new WoW.Spell(162794);
        private static readonly WoW.Spell Darkness              = new WoW.Spell(196718);

        #endregion

        #region Buffs

        #endregion

        #region Helpers

        #endregion

        public override string Name { get { return "DemonHunter - Default"; } }

        public override double CombatRange { get { return 3.0; } }

        public override double PullRange { get { return 10.0; } }

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

            var l_TargetDistance = l_ActivePlayer.Position.Distance3D(p_Target.Position);
            var l_PlayerSpecialization = l_ActivePlayer.Specialization;
            var l_TargetLineOfSight = p_Target.IsLineOfSight();
            var l_PlayerCastingSpell = l_ActivePlayer.CastingSpell;

            if (p_Target.IsCasting &&
                ConsumeMagic.CanCast(p_Target) &&
                ConsumeMagic.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                ConsumeMagic.Cast(p_Target.Position);
                return;
            }

            if (l_ActivePlayer.HealthPercent < 50 &&
                Blur.CanCast(p_Target))
            {
                Blur.Cast();
                return;
            }

            if (l_PlayerCastingSpell != null)
            {
                // Nothing more to do while casting ...
                return;
            }

            if (p_UseBigCooldowns &&
                l_PlayerSpecialization == WowPlayer.Specializations.DemonHunterHavoc &&
                Metamorphosis.CanCast(p_Target) &&
                l_TargetDistance < 40.0f &&
                l_TargetLineOfSight)
            {
                Metamorphosis.Cast(p_Target.Position);
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.DemonHunterHavoc &&
                l_ActivePlayer.IsInCombat &&
                l_TargetDistance - p_Target.CombatReach < 5.0f &&
                Darkness.CanCast())
            {
                Darkness.Cast();
                return;
            }

            if (l_PlayerSpecialization == WowPlayer.Specializations.DemonHunterHavoc &&
                l_ActivePlayer.IsFacingHeading(p_Target.Position) &&
                l_TargetDistance - p_Target.CombatReach < 20.0f &&
                EyeBeam.CanCast())
            {
                EyeBeam.Cast(p_Target);
                return;
            }

            if (ThrowGlaive.CanCast(p_Target) &&
                ThrowGlaive.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                ThrowGlaive.Cast(p_Target);
                return;
            }

            if (!EyeBeam.CanCast() &&
                ChaosStrike.CanCast(p_Target) &&
                ChaosStrike.IsInRange(p_Target) &&
                l_TargetLineOfSight)
            {
                ChaosStrike.Cast(p_Target);
                return;
            }
        }

        public override void OnPulse()
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;

            if (l_ActivePlayer == null)
                return;
        }

    }
}
