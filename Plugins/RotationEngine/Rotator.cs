using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotationEngine
{
    public static class Rotator
    {
        public static WoWSharp.Logics.Combats.CombatRoutine ActiveRoutine = null;

        public static void OnPulse(object p_Sender, WoWSharp.WoW.Pulsator.OnPulseEventArgs p_EventArgs)
        {
            var l_ActivePlayer = WoWSharp.WoW.ObjectManager.ActivePlayer;

            if (l_ActivePlayer != null && UserSettings.Instance.Enabled && ActiveRoutine != null)
            {
                var l_Target = l_ActivePlayer.Target;
                
                if (l_Target != null && 
                    (UserSettings.Instance.AttackOutOfCombatUnits || l_Target.IsInCombat) &&
                    l_Target.CanAttack && 
                    l_Target.CanAttackNow && 
                    !l_Target.IsDead)
                {
                    ActiveRoutine.OnCombat(l_Target, !UserSettings.Instance.BigCooldownsBossOnly || l_Target.IsBoss);
                }

                ActiveRoutine.OnPulse();
            }
        }

    }
}
