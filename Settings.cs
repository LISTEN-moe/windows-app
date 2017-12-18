using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace ListenMoeClient
{
	class Settings
	{
		private const string settingsFileLocation = "listenMoeSettings.ini";

		static object settingsMutex = new object();
		static object fileMutex = new object();

		static Dictionary<Type, object> typedSettings = new Dictionary<Type, object>();
		static Dictionary<char, Type> typePrefixes = new Dictionary<char, Type>()
		{
			{ 'i', typeof(int) },
			{ 'f', typeof(float) },
			{ 'b', typeof(bool) },
			{ 's', typeof(string) },
			{ 'c', typeof(Color) }
		};
		static Dictionary<Type, char> reverseTypePrefixes = new Dictionary<Type, char>()
		{
			{ typeof(int), 'i'},
			{ typeof(float), 'f'},
			{ typeof(bool), 'b'},
			{ typeof(string), 's'},
			{ typeof(Color), 'c' }
		};

		static Dictionary<Type, Func<string, (bool Success, object Result)>> parseActions = new Dictionary<Type, Func<string, (bool, object)>>()
		{
			{ typeof(int), s => {
				bool success = int.TryParse(s, out int i);
				return (success, i);
			}},
			{ typeof(float), s => {
				bool success = float.TryParse(s, out float f);
				return (success, f);
			}},
			{ typeof(bool), s => {
				bool success = bool.TryParse(s, out bool b);
				return (success, b);
			}},
			{ typeof(string), s => {
				return (true, s);
			}},
			{ typeof(Color), s => {
				if (int.TryParse(s.Replace("#", ""), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int argb))
					return (true, Color.FromArgb(255, Color.FromArgb(argb)));
				else
					throw new Exception("Could not parse color '" + s + "'. Check your settings file for any errors.");
			}}
		};

		static Dictionary<Type, Func<dynamic, string>> saveActions = new Dictionary<Type, Func<dynamic, string>>()
		{
			{ typeof(int), i => i.ToString() },
			{ typeof(float), f => f.ToString() },
			{ typeof(bool), b => b.ToString() },
			{ typeof(string), s => s },
			{ typeof(Color), c => ("#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")).ToLowerInvariant() }
		};

		static Settings()
		{
			LoadDefaultSettings();
		}

		public static T Get<T>(string key)
		{
			lock (settingsMutex)
			{
				return ((Dictionary<string, T>)(typedSettings[typeof(T)]))[key];
			}
		}

		public static void Set<T>(string key, T value)
		{
			Type t = typeof(T);
			lock (settingsMutex)
			{
				if (!typedSettings.ContainsKey(t))
				{
					typedSettings.Add(t, new Dictionary<string, T>());
				}
				((Dictionary<string, T>)typedSettings[t])[key] = value;
			}
		}

		private static void LoadDefaultSettings()
		{
			Set("LocationX", 100);
			Set("LocationY", 100);
			Set("VisualiserResolutionFactor", 3);
			Set("UpdateInterval", 3600); //in seconds

			Set("Volume", 0.3f);
			Set("VisualiserBarWidth", 3.0f);
			Set("VisualiserTransparency", 0.5f); //TODO: rename this to opacity
			Set("FormOpacity", 1.0f);
			Set("Scale", 1.0f);

			Set("TopMost", false);
			Set("UpdateAutocheck", true);
			Set("CloseToTray", false);
			Set("HideFromAltTab", false);
			Set("ThumbnailButton", true);
			Set("EnableVisualiser", true);
			Set("VisualiserBars", true);
			Set("VisualiserFadeEdges", false);

			Set("Token", "");
			Set("Username", "");

			Set("VisualiserColor", Color.FromArgb(236, 26, 85));
			Set("BaseColor", Color.FromArgb(44, 46, 59));
			Set("AccentColor", Color.FromArgb(236, 26, 85));
		}

		public static void LoadSettings()
		{
			if (!File.Exists(settingsFileLocation))
			{
				WriteSettings();
				return;
			}

			string[] lines = File.ReadAllLines(settingsFileLocation);
			foreach (string line in lines)
			{
				string[] parts = line.Split(new char[] { '=' }, 2);
				if (string.IsNullOrWhiteSpace(parts[0]))
					continue;

				char prefix = parts[0][0];
				Type t = typePrefixes[prefix];
				var parseAction = parseActions[t];
				(bool success, object o) = parseAction(parts[1]);
				if (!success)
					continue;

				MethodInfo setMethod = typeof(Settings).GetMethod("Set", BindingFlags.Static | BindingFlags.Public);
				MethodInfo genericSet = setMethod.MakeGenericMethod(t);
				genericSet.Invoke(null, new object[] { parts[0].Substring(1), o });
			}
		}

		public static void WriteSettings()
		{
			StringBuilder sb = new StringBuilder();
			lock (settingsMutex)
			{
				foreach (var dict in typedSettings)
				{
					Type t = dict.Key;
					var typedDict = (System.Collections.IDictionary)dict.Value;
					var saveAction = saveActions[t];

					foreach (dynamic setting in typedDict)
					{
						sb.AppendLine(reverseTypePrefixes[t] + setting.Key + "=" + saveAction(setting.Value));
					}
				}
			}

			lock (fileMutex)
			{
				using (var fileStream = new FileStream(settingsFileLocation, FileMode.Create, FileAccess.Write))
				{
					using (var streamWriter = new StreamWriter(fileStream))
						streamWriter.Write(sb.ToString());
				}
			}
		}
	}
}
