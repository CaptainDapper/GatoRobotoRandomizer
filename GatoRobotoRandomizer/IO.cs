using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System;

namespace GatoRobotoRandomizer {
	public static class IO {
		public static string DataWinFile {
			get {
				return $"{Path}/{_files[6]}";
			}
		}
		public static string Path {
			get; set;
		}
		public static string BackupPath {
			get {
				return $"{Path}/GRR_BACKUP";
			}
		}


		private static string _outputLog {
			get {
				return $"{Path}/output_log.txt";
			}
		}
		private static string[] _files = { "map0", "map1", "map2", "map3", "map4", "map5", "data.win" };
		private static bool _doClearOutput = false;

		public static void MakeBackups() {
			string backup_path = BackupPath;
			if (!Directory.Exists(backup_path)) {
				Directory.CreateDirectory(backup_path);
			}
			foreach (string file in _files) {
				File.Copy(Path + "/" + file, backup_path + "/" + file, true);
			}
		}

		public static bool CheckFiles() {
			foreach (string file in _files) {
				if (!File.Exists($"{Path}/{file}")) {
					return false;
				}
			}

			return true;
		}

		public enum LogType {
			Debug,
			Info,
			Warn,
			CRITICAL,
			___
		}

		public static LogType LogLevel { get; set; } = LogType.Info;

		public static void Log(string message, LogType level = LogType.___) {
			LogType lev = level == LogType.___ ? LogLevel : level;

			if (lev == LogType.Debug) {
#if !DEBUG
				return;		//No debug stuff in the release versions ;)
#endif
			}

			if (message.Trim() != string.Empty) {
				message = $"[{lev}] {message}";
			}

			Debug.WriteLine("logged -> " + message);
			if (!File.Exists(_outputLog) || _doClearOutput) {
				File.Create(_outputLog).Dispose();
				_doClearOutput = false;
			}

			File.AppendAllText(_outputLog, message + Environment.NewLine);
		}

		public static void LogClear() {
			_doClearOutput = true;
		}

		private static void writeItemToLocation(JSONNode node, RandoItem item) {
			//Remove method0
			if (node.AsObject.ContainsKey("method0")) {
				node.Remove("method0");
			}

			//Place the item's data
			node["spr"] = item.Spr;
			node["obj"] = item.Obj;
			node["image_xscale"] = item.Image_xscale;

			//Place the right palette choice method for VHS carts
			if (item.Method0 != "") {
				node["method0"] = item.Method0;
			}
		}

		public static string encode(JSONNode node) {
			StringBuilder sb = new StringBuilder();

			sb.Append("{ ");
			bool first = true;
			foreach (KeyValuePair<string, JSONNode> child in node.AsObject.Children2) {
				string k = child.Key;
				JSONNode v = child.Value;

				if (!first) {
					sb.Append(", ");
				}
				first = false;
				sb.Append("\"").Append(k).Append("\": ");
				if (v.IsObject) {
					sb.Append("\"").Append(encode(v).Replace("\\", "\\\\").Replace("\"", "\\\"")).Append("\"");
				} else {
					sb.Append(v.ToString());
				}
			}
			sb.Append(" }");

			return sb.ToString();
		}

		public static Dictionary<string, JSONNode> DecodeAllMaps() {
			Dictionary<string, JSONNode> maps = new Dictionary<string, JSONNode>();

			foreach (string file in _files.Where(val => val.StartsWith("map"))) {
				//decode existing map data
				string json = DecodeMap(Path + "/" + file);

				//deserialize valid json
				maps.Add(file, JSON.Parse(json));
			}

			return maps;
		}

		public static void WriteData(List<RandoLocation> locations, bool doVanilla = false) {
			Dictionary<string, JSONNode> maps = Randomizer.Maps;

			IO.Log("Make Map Changes");
			//make map changes
			MapChanger.MakeChanges(doVanilla);

			IO.Log("Write Items");
			//make item rando changes
			foreach (RandoLocation loc in locations) {
				JSONNode node = maps[loc.Map][loc.Room][loc.Inst];

				writeItemToLocation(node, loc.Item);
			}

			IO.Log("Serialize & encode all");
			foreach (string map in maps.Keys) {
				//serialize and encode
				JSONNode node = maps[map];
				string encoded = encode(node);

				using (StreamWriter sw = File.CreateText(Path + "/" + map)) {
					sw.Write(encoded);
				}

				IO.Log($"{map} done.");
			}
		}

		public static string DecodeMap(string path) {
			string map = File.ReadAllText(path)
				.Replace("\\\\\\\"", "\"")
				.Replace("\\\"", "\"")
				.Replace("\"{", "{")
				.Replace("}\"", "}") + "\n"
				;

			return map;
		}
	}
}
