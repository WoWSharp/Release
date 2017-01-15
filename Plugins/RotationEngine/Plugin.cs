using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WoWSharp;
using WoWSharp.WoW.Impl.Frames;

namespace RotationEngine
{
    public class Plugin : WoWSharp.IPlugin
    {
        private MainWindow m_MainWindow = null;

        public string Name { get { return "Rotation Engine"; } }

        public string Author { get { return "WoWSharp"; } }

        public void OnLoad()
        {
            UserSettings.Instance = UserSettings.LoadFromDisk();

            WoWSharp.GUI.OnCreateGUI += GUI_OnCreateGUI;
            WoWSharp.GUI.OnDisposeGUI += GUI_OnDisposeGUI;
            WoWSharp.WoW.Pulsator.OnPulse += Rotator.OnPulse;

            GUI_OnCreateGUI(this, new GUI.OnCreateGUIEventArgs());
        }

        private void GUI_OnDisposeGUI(object sender, WoWSharp.GUI.OnDisposeGUIEventArgs e)
        {
            m_MainWindow = null;
        }

        private void GUI_OnCreateGUI(object sender, WoWSharp.GUI.OnCreateGUIEventArgs e)
        {
            m_MainWindow = WoWSharp.WoW.GameUI.CreateFrame<MainWindow>();
        }

        public void OnUnload()
        {
            WoWSharp.GUI.OnDisposeGUI -= GUI_OnDisposeGUI;
            WoWSharp.GUI.OnCreateGUI -= GUI_OnCreateGUI;
            WoWSharp.WoW.Pulsator.OnPulse -= Rotator.OnPulse;

            UserSettings.Instance.SaveToDisk();
        }

        public SimpleFrame MainFrame
        {
            get { return m_MainWindow; }
        }
    }
}
