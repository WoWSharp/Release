using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWSharp.UIControls;
using WoWSharp.WoW;
using WoWSharp.WoW.Impl.Frames;
using WoWSharp.WoW.Impl.Lua;
using WoWSharp.WoW.Impl.Objects;

namespace RotationEngine
{
    public class MainWindow : Window
    {
        private DropDownMenu DropDownMenuCombatRoutines;
        private CheckButton CheckButtonRotator;
        private CheckButton CheckButtonAttackOutOfCombatUnits;
        private CheckButton CheckButtonBigCooldowns;

        public MainWindow(LuaTable p_Object, bool p_SelfCreated)
            : base(p_Object, p_SelfCreated)
        {
            if (p_SelfCreated)
            {
                this.SetPoint(AnchorPoint.Center);
                this.Width = 250;
                this.Height = 300;
                this.Title = "Rotation Engine";

                this.DropDownMenuCombatRoutines = this.CreateChildFrame<DropDownMenu>();
                this.DropDownMenuCombatRoutines.SetPoint(AnchorPoint.Top, this, AnchorPoint.Top, 0, -30);
                this.DropDownMenuCombatRoutines.Width = this.Width - 50; ;
                this.DropDownMenuCombatRoutines.OnLoad += DropDownMenuCombatRoutines_OnLoad;

                this.CheckButtonRotator = this.CreateChildFrame<CheckButton>();
                this.CheckButtonRotator.SetPoint(AnchorPoint.TopLeft, this.DropDownMenuCombatRoutines, AnchorPoint.TopLeft, 15, -30);
                this.CheckButtonRotator.Text = "Enable rotation";
                this.CheckButtonRotator.OnClick += (object p_Sender, SimpleButton.OnClickEventArgs p_Event) =>
                {
                    UserSettings.Instance.Enabled = !UserSettings.Instance.Enabled;
                };

                this.CheckButtonAttackOutOfCombatUnits = this.CreateChildFrame<CheckButton>();
                this.CheckButtonAttackOutOfCombatUnits.SetPoint(AnchorPoint.TopLeft, this.CheckButtonRotator, AnchorPoint.TopLeft, 0, -30);
                this.CheckButtonAttackOutOfCombatUnits.Text = "Attack out of combat units";
                this.CheckButtonAttackOutOfCombatUnits.OnClick += (object p_Sender, SimpleButton.OnClickEventArgs p_Event) => 
                {
                    UserSettings.Instance.AttackOutOfCombatUnits = !UserSettings.Instance.AttackOutOfCombatUnits;
                };

                this.CheckButtonBigCooldowns = this.CreateChildFrame<CheckButton>();
                this.CheckButtonBigCooldowns.SetPoint(AnchorPoint.TopLeft, this.CheckButtonAttackOutOfCombatUnits, AnchorPoint.TopLeft, 0, -30);
                this.CheckButtonBigCooldowns.Text = "Use cooldows on boss only";
                this.CheckButtonBigCooldowns.OnClick += (object p_Sender, SimpleButton.OnClickEventArgs p_Event) =>
                {
                    UserSettings.Instance.BigCooldownsBossOnly = !UserSettings.Instance.BigCooldownsBossOnly;
                };

                this.OnUpdate += MainWindow_OnUpdate;
                this.OnHide += MainWindow_OnHide;
                this.OnShow += MainWindow_OnShow;
                this.Visible = UserSettings.Instance.MainWindowVisibility;
                SetCombatRoutine(WoWSharp.Plugins.LoadedRoutines.FirstOrDefault(x => x.Name == UserSettings.Instance.LastCombatRotation));
            }
        }

        private void MainWindow_OnShow(object sender, OnShowEventArgs e)
        {
            UserSettings.Instance.MainWindowVisibility = true;
        }

        private void MainWindow_OnHide(object sender, OnHideEventArgs e)
        {
            UserSettings.Instance.MainWindowVisibility = false;
        }

        private void MainWindow_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            this.CheckButtonRotator.Enabled = Rotator.ActiveRoutine != null;

            this.CheckButtonRotator.Checked = UserSettings.Instance.Enabled;
            this.CheckButtonAttackOutOfCombatUnits.Checked = UserSettings.Instance.AttackOutOfCombatUnits;
            this.CheckButtonBigCooldowns.Checked = UserSettings.Instance.BigCooldownsBossOnly;
        }

        private void DropDownMenuCombatRoutines_OnLoad(object sender, DropDownMenu.OnLoadEventArgs e)
        {
            foreach (var l_Routine in WoWSharp.Plugins.LoadedRoutines)
            {
                this.DropDownMenuCombatRoutines.AddButton(new DropDownMenu.DropDownButtonInfo()
                {
                    Text = l_Routine.Name,
                    OnClick = () => 
                    {
                        SetCombatRoutine(l_Routine);
                    }
                });
            }
        }

        private void SetCombatRoutine(WoWSharp.Logics.Combats.CombatRoutine p_Routine)
        {
            if (p_Routine != null)
            {
                Rotator.ActiveRoutine = p_Routine;
                UserSettings.Instance.LastCombatRotation = p_Routine.Name;
                this.DropDownMenuCombatRoutines.Text = p_Routine.Name;
            }
            else
            {
                Rotator.ActiveRoutine = null;
                this.DropDownMenuCombatRoutines.Text = "Select combat routine ...";
            }
        }
    }
}
