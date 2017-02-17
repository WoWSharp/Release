using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWSharp.WoW.Impl.Frames;

namespace WoWSharp.Morpher
{
    public class Plugin : WoWSharp.IPlugin
    {
        public override string Name => "Morpher";

        public override string Author => "WoWSharp";

        public override void OnLoad()
        {
            BuildGUI();

            WoWSharp.GUI.OnCreateGUI += GUI_OnCreateGUI;
        }

        private void GUI_OnCreateGUI(object sender, WoWSharp.GUI.OnCreateGUIEventArgs e)
        {
            BuildGUI();
        }

        public override void OnUnload()
        {
            WoWSharp.GUI.OnCreateGUI -= GUI_OnCreateGUI;

            m_MainWindow?.Dispose();
        }

        public override SimpleFrame MainFrame => m_MainWindow;

        private MainWindow m_MainWindow = null;
        private void BuildGUI()
        {
            m_MainWindow = WoWSharp.WoW.GameUI.CreateFrame<MainWindow>();
            m_MainWindow.Visible = false;
        }
    }
}
