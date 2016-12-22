using System;
using System.Linq;

using WoWSharp.Logics.Combats;
using WoWSharp.WoW.Impl.Objects;

namespace WoWSharp.DefaultRoutines.Mage
{
    public class Mage : CombatRoutine
    {
        #region Spells

        private static readonly WoW.Spell FrostBolt             = new WoW.Spell(116);
        private static readonly WoW.Spell IceLance              = new WoW.Spell(30455);
        private static readonly WoW.Spell FireBlast             = new WoW.Spell(108853);
        private static readonly WoW.Spell Flurry                = new WoW.Spell(44614);
        private static readonly WoW.Spell SummonWaterElemental  = new WoW.Spell(31687);
        private static readonly WoW.Spell FrozenOrb             = new WoW.Spell(84714);
        private static readonly WoW.Spell IcyVeins              = new WoW.Spell(12472);
        private static readonly WoW.Spell FrozenTouch           = new WoW.Spell(205030);
        private static readonly WoW.Spell Invisibility          = new WoW.Spell(66);
        private static readonly WoW.Spell MirrorImage           = new WoW.Spell(55342);
        private static readonly WoW.Spell RuneOfPower           = new WoW.Spell(116011);

        /* Fire Spec */
        private static readonly WoW.Spell Fireball             = new WoW.Spell(133);
        private static readonly WoW.Spell Pyroblast            = new WoW.Spell(11366);
        private static readonly WoW.Spell Combustion           = new WoW.Spell(190319);

        #endregion

        #region Buffs

        private static readonly WoW.Spell FingersOfFrost        = new WoW.Spell(44544);
        private static readonly WoW.Spell BrainFreeze           = new WoW.Spell(190446);

        /* Fire Spec */
        private static readonly WoW.Spell HotStreak             = new WoW.Spell(48108);

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
        /// yiotitiuotyiuoy_çoho
        /// </summary>
        /// <param name="p_Target"></param>
        public override void OnCombat(WowUnit p_Target)
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;

            if (l_ActivePlayer == null)
                return;

            if (l_ActivePlayer.HasAura(BrainFreeze.SpellId) && Flurry.CanCast(p_Target) && Flurry.IsInRange(p_Target))
            {
                Flurry.Cast(p_Target);
                return;
            }

            if (l_ActivePlayer.HasAura(HotStreak.SpellId) && Pyroblast.CanCast(p_Target) && Pyroblast.IsInRange(p_Target))
            {
                Pyroblast.Cast(p_Target);
                return;
            }

            var l_PlayerCastingSpell = l_ActivePlayer.CastingSpell;

            if (l_PlayerCastingSpell != null)
            {
                // Nothing more to do while casting ...
                return;
            }

            if (Invisibility.CanCast() && l_ActivePlayer.HealthPercent <= 100 /* Check if boss when in party */)
            {
                Invisibility.Cast();
                return;
            }
            
            if (FrozenOrb.CanCast(p_Target) /* Check range and if boss when in party */)
            {
                // Face target
                FrozenOrb.Cast(p_Target);
                return;
            }

            if (MirrorImage.CanCast() /* Check if boss when in party */)
            {
                MirrorImage.Cast();
                return;
            }

            if (RuneOfPower.CanCast() /* Check if boss when in party */)
            {
                RuneOfPower.Cast();
                return;
            }

            if (Combustion.CanCast() /* Check if boss when in party */)
            {
                Combustion.Cast();
                return;
            }

            if (IcyVeins.CanCast() /* Check if boss when in party */)
            {
                IcyVeins.Cast();
                return;
            }

            if (FrozenTouch.CanCast() /* Check if boss when in party */)
            {
                FrozenTouch.Cast();
                return;
            }

            if (IceLance.IsKnown)
            {
                if (l_ActivePlayer.HasAura(FingersOfFrost.SpellId) && IceLance.CanCast(p_Target) && IceLance.IsInRange(p_Target))
                {
                    Console.WriteLine("[Mage] Fingers of Frost found, cast Ice Lance !");
                    IceLance.Cast(p_Target);
                    return;
                }
            }
            else if (FireBlast.IsKnown)
            {
                if (FireBlast.CanCast(p_Target) && FireBlast.IsInRange(p_Target))
                {
                    FireBlast.Cast(p_Target);
                    return;
                }
            }

            if (FrostBolt.CanCast(p_Target) && FrostBolt.IsInRange(p_Target))
            {
                FrostBolt.Cast(p_Target);
                return;
            }

            if (Fireball.CanCast(p_Target) && Fireball.IsInRange(p_Target))
            {
                Fireball.Cast(p_Target);
                return;
            }
        }

        public override void OnPulse()
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;

            if (l_ActivePlayer == null)
                return;

            var l_WaterElemental = GetWaterElementalUnit(l_ActivePlayer);

            if (SummonWaterElemental.IsKnown && (l_WaterElemental == null || l_WaterElemental.IsDead) && SummonWaterElemental.CanCast())
            {
                SummonWaterElemental.Cast();
                return;
            }

            var l_PlayerCastingSpell = l_ActivePlayer.CastingSpell;

            if (l_PlayerCastingSpell != null)
            {
                // Nothing more to do while casting ...
                return;
            }
        }

    }
}
