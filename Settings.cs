using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrappyListenMoe
{
    class Settings
    {
        private const string settingsFileLocation = "listenMoeSettings.ini";

        static Dictionary<string, int> intValues = new Dictionary<string, int>();
        static Dictionary<string, float> floatValues = new Dictionary<string, float>();

        static object mutex = new object();

        public static void LoadSettings()
        {
            if (!File.Exists(settingsFileLocation))
            {
                LoadDefaultSettings();
                WriteSettings();
                return;
            }

            string[] lines = File.ReadAllLines(settingsFileLocation);
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (string.IsNullOrWhiteSpace(parts[0]))
                    continue;

                if (parts[0][0] == 'i')
                {
                    int val;
                    if (!int.TryParse(parts[1], out val))
                        continue;
                    intValues.Add(parts[0].Substring(1), val);
                }
                else if (parts[0][0] == 'f')
                {
                    float val;
                    if (!float.TryParse(parts[1], out val))
                        continue;
                    floatValues.Add(parts[0].Substring(1), val);
                }
            }
        }

        private static void LoadDefaultSettings()
        {
            intValues.Add("LocationX", 100);
            intValues.Add("LocationY", 100);
            floatValues.Add("Volume", 1.0f);
        }

        public static void WriteSettings()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kp in intValues)
            {
                sb.AppendLine("i" + kp.Key + "=" + kp.Value.ToString());
            }
            foreach (var kp in floatValues)
            {
                sb.AppendLine("f" + kp.Key + "=" + kp.Value.ToString());
            }

            lock (mutex)
            {
                using (var fileStream = new FileStream(settingsFileLocation, FileMode.Create, FileAccess.Write))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                        streamWriter.Write(sb.ToString());
                }
            }
        }

        public static int GetIntSetting(string key)
        {
            return intValues[key];
        }

        public static float GetFloatSetting(string key)
        {
            return floatValues[key];
        }

        public static void SetIntSetting(string key, int value)
        {
            intValues[key] = value;
        }

        public static void SetFloatSetting(string key, float value)
        {
            floatValues[key] = value;
        }
    }
}
