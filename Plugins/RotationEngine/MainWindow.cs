using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWSharp.UIControls;
using WoWSharp.WoW;
using WoWSharp.WoW.Impl.Frames;
using WoWSharp.WoW.Impl.Lua;

namespace RotationEngine
{
    public class MainWindow : Window
    {
        private Button TestButton;
        private DropDownMenu DropDownMenuCombatRoutines;
        private CheckButton CheckButtonRotator;
        private CheckButton CheckButtonAttackOutOfCombatUnits;

        public MainWindow(LuaTable p_Object, bool p_SelfCreated)
            : base(p_Object, p_SelfCreated)
        {
            if (p_SelfCreated)
            {
                this.Visible = false;
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
                    Rotator.Settings.Enabled = !Rotator.Settings.Enabled;
                };

                this.CheckButtonAttackOutOfCombatUnits = this.CreateChildFrame<CheckButton>();
                this.CheckButtonAttackOutOfCombatUnits.SetPoint(AnchorPoint.TopLeft, this.CheckButtonRotator, AnchorPoint.TopLeft, 0, -30);
                this.CheckButtonAttackOutOfCombatUnits.Text = "Attack out of combat units";
                this.CheckButtonAttackOutOfCombatUnits.OnClick += (object p_Sender, SimpleButton.OnClickEventArgs p_Event) => 
                {
                    Rotator.Settings.AttackOutOfCombatUnits = !Rotator.Settings.AttackOutOfCombatUnits;
                };

                this.TestButton = this.CreateChildFrame<Button>();
                this.TestButton.SetPoint(AnchorPoint.Bottom, 0, 20);
                this.TestButton.Width = this.Width - 40;
                this.TestButton.Text = "Test button";
                this.TestButton.OnClick += TestButton_OnClick;

                this.OnUpdate += MainWindow_OnUpdate;
                SetCombatRoutine(WoWSharp.Plugins.LoadedRoutines.FirstOrDefault(x => x.Name == Rotator.Settings.LastCombatRotation));
            }
        }

        WoWSharp.Logics.Movements.IPlayerMover PlayerMover = new WoWSharp.Logics.Movements.SmoothTurnPlayerMover();
        private void MainWindow_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            PlayerMover.OnPulse();

            this.CheckButtonAttackOutOfCombatUnits.Enabled = Rotator.ActiveRoutine != null;
            this.CheckButtonAttackOutOfCombatUnits.Checked = Rotator.Settings.AttackOutOfCombatUnits;

            this.CheckButtonRotator.Checked = Rotator.Settings.Enabled;
            this.CheckButtonRotator.Enabled = Rotator.ActiveRoutine != null;
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
                this.DropDownMenuCombatRoutines.Text = p_Routine.Name;
            }
            else
            {
                Rotator.ActiveRoutine = null;
                this.DropDownMenuCombatRoutines.Text = "Select combat routine ...";
            }
        }

        private void TestButton_OnClick(object sender, SimpleButton.OnClickEventArgs e)
        {
            var l_Spell = new WoWSharp.WoW.Spell(190356);

            var l_Target = ObjectManager.ActivePlayer.Target;

            if (l_Target != null)
            {
                PlayerMover.StartMoving(l_Target.Position);
            }

            //l_Spell.Cast(ObjectManager.ActivePlayer.Target.Position);

        }
    }
}
