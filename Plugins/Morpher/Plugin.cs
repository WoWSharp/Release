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

        private bool m_IsEnabled = false;

        public string Name { get { return "Morpher"; } }

        public string Author { get { return "WoWSharp"; } }

        public bool AutoEnable { get { return false; } }

        public bool IsEnabled { get { return m_IsEnabled; } }

        public void OnLoad()
        {
            BuildGUI();

            WoWSharp.GUI.OnCreateGUI += GUI_OnCreateGUI;
        }

        private void GUI_OnCreateGUI(object sender, WoWSharp.GUI.OnCreateGUIEventArgs e)
        {
            BuildGUI();
        }

        public void OnUnload()
        {
            WoWSharp.GUI.OnCreateGUI -= GUI_OnCreateGUI;

            if (m_MainWindow != null)
                m_MainWindow.Dispose();
        }

        public SimpleFrame MainFrame { get { return null; } }

        public void OnEnable()
        {
            if (m_MainWindow != null)
            {
                m_MainWindow.Show();
            }

            m_IsEnabled = true;
        }

        public void OnDisable()
        {
            if (m_MainWindow != null)
            {
                m_MainWindow.Hide();
            }

            m_IsEnabled = false;
        }

        private MainWindow m_MainWindow = null;
        private void BuildGUI()
        {
            m_MainWindow = WoWSharp.WoW.GameUI.CreateFrame<MainWindow>();
        }
    }
}
