using System;
using System.Collections.Generic;
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
			{ 's', typeof(string) }
		};
		static Dictionary<Type, char> reverseTypePrefixes = new Dictionary<Type, char>()
		{
			{ typeof(int), 'i'},
			{ typeof(float), 'f'},
			{ typeof(bool), 'b'},
			{ typeof(string), 's'}
		};

		static Dictionary<Type, Func<string, (bool Success, object Result)>> parseActions = new Dictionary<Type, Func<string, (bool, object)>>()
		{
			{ typeof(int), s => {
				int i;
				bool success = int.TryParse(s, out i);
				return (success, i);
			}},
			{ typeof(float), s => {
				float f;
				bool success = float.TryParse(s, out f);
				return (success, f);
			}},
			{ typeof(bool), s => {
				bool b;
				bool success = bool.TryParse(s, out b);
				return (success, b);
			}},
			{ typeof(string), s => {
				return (true, s);
			}},
		};

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

			Set("Volume", 1.0f);
			Set("VisualiserBarWidth", 3.0f);

			Set("TopMost", false);
			Set("IgnoreUpdates", false);
			Set("CloseToTray", false);
			Set("HideFromAltTab", false);
			Set("EnableVisualiser", true);

			Set("Token", "");
			Set("Username", "");
		}

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

					foreach (dynamic setting in typedDict)
					{
						sb.AppendLine(reverseTypePrefixes[t] + setting.Key + "=" + setting.Value.ToString());
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
