using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RotationEngine
{
    public class UserSettings
    {
        public static UserSettings Instance = new UserSettings();

        public static UserSettings LoadFromDisk()
        {
            string l_Filename = Path.Combine(WoWSharp.EntryPoint.SettingsDirectory, "RotationEngine.xml");

            if (!File.Exists(l_Filename))
                return new UserSettings();

            try
            {
                XmlSerializer l_Serializer = new XmlSerializer(typeof(UserSettings));
                using (StreamReader l_StreamReader = new StreamReader(l_Filename))
                {
                    return l_Serializer.Deserialize(l_StreamReader) as UserSettings;
                }
            }
            catch
            {
                return new UserSettings();
            }
        }

        public bool MainWindowVisibility = false;
        public bool Enabled = false;
        public bool AttackOutOfCombatUnits = false;
        public bool BigCooldownsBossOnly = true;
        public string LastCombatRotation = string.Empty;
        
        public void SaveToDisk()
        {
            XmlSerializer l_Serializer = new XmlSerializer(typeof(UserSettings));
            using (StreamWriter l_StreamWriter = new StreamWriter(Path.Combine(WoWSharp.EntryPoint.SettingsDirectory, "RotationEngine.xml")))
            {
                l_Serializer.Serialize(l_StreamWriter, this);
            }
        }
    }
}
