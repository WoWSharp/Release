using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WoWSharp.Logics.Movements;
using WoWSharp.Logics.Navigation;
using WoWSharp.Maths;
using WoWSharp.UIControls;
using WoWSharp.WoW;
using WoWSharp.WoW.Graphics;
using WoWSharp.WoW.Impl.Frames;
using WoWSharp.WoW.Impl.Lua;

namespace WoWSharp.Morpher
{
    public sealed class MainWindow : Window, IDisposable
    {
        private Button ButtonCopyTarget;
        private DropDownMenu DropDownMenuLoadSettings;
        private SimpleFontString LabelDisplayId;
        private readonly TextBox TextBoxDisplayId;
        private Button ButtonMorphDisplayId;

        public MainWindow(LuaTable p_Object, bool p_SelfCreated)
            : base(p_Object, p_SelfCreated)
        {
            if (p_SelfCreated)
            {
                var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;

                this.Visible = false;
                this.SetPoint(AnchorPoint.Center);
                this.Width = 260;
                this.Height = 300;
                this.Title = "Morpher";

                DropDownMenuLoadSettings = this.CreateChildFrame<DropDownMenu>();
                DropDownMenuLoadSettings.SetPoint(AnchorPoint.Top, this, AnchorPoint.Top, 0, -30);
                DropDownMenuLoadSettings.Width = this.Width - 50; ;
                DropDownMenuLoadSettings.Text = "Load settings ...";
                DropDownMenuLoadSettings.OnLoad += DropDownMenuLoadSettings_OnLoad;

                LabelDisplayId = this.CreateFontString();
                LabelDisplayId.SetPoint(AnchorPoint.TopLeft, 12, -85);
                LabelDisplayId.Text = "Display id :";

                TextBoxDisplayId = this.CreateChildFrame<TextBox>();
                TextBoxDisplayId.SetPoint(AnchorPoint.TopLeft, 110, -80);
                TextBoxDisplayId.IsNumeric = true;
                TextBoxDisplayId.Text = l_ActivePlayer != null ? l_ActivePlayer.DisplayId.ToString() : string.Empty;
                TextBoxDisplayId.Width = 50;
                TextBoxDisplayId.Height = 26;

                ButtonMorphDisplayId = this.CreateChildFrame<Button>();
                ButtonMorphDisplayId.SetPoint(AnchorPoint.TopLeft, 180, -80);
                ButtonMorphDisplayId.Width = 60;
                ButtonMorphDisplayId.Height = 26;
                ButtonMorphDisplayId.Text = "Morph";
                ButtonMorphDisplayId.OnClick += ButtonMorphDisplayId_OnClick;

                ButtonCopyTarget = this.CreateChildFrame<Button>();
                ButtonCopyTarget.SetPoint(AnchorPoint.Bottom, 0, 20);
                ButtonCopyTarget.Width = this.Width - 40;
                ButtonCopyTarget.Text = "Copy target data";
                ButtonCopyTarget.OnClick += ButtonCopyTarget_OnClick;

                this.OnUpdate += MainWindow_OnUpdate;

            }
        }

        private void ButtonCopyTarget_OnClick(object sender, SimpleButton.OnClickEventArgs e)
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;
            WoW.Impl.Objects.WowUnit l_Target;
            if (l_ActivePlayer != null && (l_Target = l_ActivePlayer.Target) != null)
            {
                TextBoxDisplayId.Text = l_Target.DisplayId.ToString();
            }
        }

        private void ButtonMorphDisplayId_OnClick(object sender, SimpleButton.OnClickEventArgs e)
        {
            var l_ActivePlayer = WoW.ObjectManager.ActivePlayer;
            int l_DisplayId;

            if (l_ActivePlayer != null && int.TryParse(TextBoxDisplayId.Text, out l_DisplayId))
            {
                l_ActivePlayer.DisplayId = l_DisplayId;
            }
        }

        private void MainWindow_OnUpdate(object sender, OnUpdateEventArgs e)
        {

        }

        private void DropDownMenuLoadSettings_OnLoad(object sender, DropDownMenu.OnLoadEventArgs e)
        {
            //foreach (var l_Routine in WoWSharp.Plugins.LoadedRoutines)
            //{
            //    this.DropDownMenuCombatRoutines.AddButton(new DropDownMenu.DropDownButtonInfo()
            //    {
            //        Text = l_Routine.Name,
            //        OnClick = () =>
            //        {

            //        }
            //    });
            //}
        }

        public void Dispose()
        {

        }
    }
}
