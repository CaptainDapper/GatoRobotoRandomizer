using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatoRobotoRandomizer {
	public class InstDef {
		#region STATIC

		private static List<InstDef> pAll = null;

		public static List<InstDef> All {
			get {
				if (pAll == null) {
					InitAll();
				}
				return pAll;
			}
		}

		private static void InitAll() {
			pAll = new List<InstDef>();

			foreach (string mkey in Randomizer.Maps.Keys) {
				JSONNode map_data = Randomizer.Maps[mkey];
				foreach (string rkey in map_data.Keys) {
					{
						string[] bad_rkeys = new[] {
								"default_room",
								//map3 weird stuff
								"diffY",
								"posY",
								"touch",
								"guistartposY",
								"rawdiffX",
								"viewstartposY",
								"guiposY",
								"rawposY",
								"guidiffX",
								"rawstartposX",
								"guistartposX",
								"posX",
								"diffX",
								"rawdiffY",
								"viewstartposX",
								"guidiffY",
								"gesture",
								"rawposX",
								"guiposX",
								"rawstartposY",
							};
						if (bad_rkeys.Contains(rkey))
							continue;
					}

					JSONNode room_data = map_data[rkey];
					foreach (string ikey in room_data.Keys) {
						{
							string[] bad_ikeys = new[] {
								"downopen",
								"upopen",
								"rightopen",
								"leftopen",
								"doors",
								"quest",
								//Elevator triggers?
								"maptarget",
								"roomtarget_x",
								"roomtarget_y",
							};

							if (bad_ikeys.Contains(ikey))
								continue;
						}

						InstDef item = new InstDef(mkey, rkey, ikey, room_data[ikey]);
						pAll.Add(item);
					}
				}
			}
		}

		#endregion


		private int pI = 0;
		private int pJ = 0;

		public string map { get; internal set; } = "";
		public string room { get; internal set; } = "";
		public string inst { get; internal set; } = "";

		public int id { get; internal set; } = 0;
		public string obj { get; internal set; } = "obj_tile";
		public string spr { get; internal set; } = "tileset1_42";
		public string layer { get; internal set; } = "player";

		public int i {
			get => pI;
			internal set {
				pI = value;
				x = value * 16 + 8;
			}
		}
		public int j {
			get => pJ;
			internal set {
				pJ = value;
				y = value * 16 + 8;
			}
		}
		public int x { get; internal set; } = 0;
		public int y { get; internal set; } = 0;

		public int image_xscale { get; internal set; } = 1;
		public int image_yscale { get; internal set; } = 1;
		public int image_angle { get; internal set; } = 0;
		public int direction { get; internal set; } = 0;

		public string method0 { get; internal set; } = "";
		public string method1 { get; internal set; } = "";
		public string method2 { get; internal set; } = "";
		public string method3 { get; internal set; } = "";

		public bool randomizer_new { get; internal set; } = false;


		public int InstNum => int.Parse(inst.Replace("inst", ""));

		public InstDef() {

		}

		public InstDef(string pMap, string pRoom, string pInst, JSONNode node) {
			map = pMap;
			room = pRoom;
			inst = pInst;

			id = node["id"];
			obj = node["obj"];
			spr = node["spr"];
			layer = node["layer"];

			i = node["i"];
			j = node["j"];
			x = node["x"];
			y = node["y"];

			image_xscale = node["image_xscale"];
			image_yscale = node["image_yscale"];
			image_angle = node["image_angle"];
			direction = node["direction"];

			method0 = node["method0"];
			method1 = node["method1"];
			method2 = node["method2"];
			method3 = node["method3"];

			if (node["randomizer_new"] == null || node["randomizer_new"] == "") {
				randomizer_new = false;
			} else {
				randomizer_new = bool.Parse(node["randomizer_new"]);
			}
		}

		public JSONObject ToJSONObject() {
			JSONObject ret = new JSONObject();
			ret["id"] = id;
			ret["obj"] = obj;
			ret["spr"] = spr;
			ret["layer"] = layer;

			ret["i"] = i;
			ret["j"] = j;
			ret["x"] = x;
			ret["y"] = y;

			ret["image_xscale"] = image_xscale;
			ret["image_yscale"] = image_yscale;
			ret["image_angle"] = image_angle;
			ret["direction"] = direction;

			if (method0 != "") ret["method0"] = method0;
			if (method1 != "") ret["method1"] = method1;
			if (method2 != "") ret["method2"] = method2;
			if (method3 != "") ret["method3"] = method3;

			if (randomizer_new != false) ret["randomizer_new"] = randomizer_new.ToString();

			return ret;
		}

		public override string ToString() {
			return ToString("");
		}

		public string ToString(string indent) {
			return $"{indent}map: {map}\n" +
			$"{indent}room: {room}\n" +
			$"{indent}inst: {inst}\n" +
			$"{indent}id: {id}\n" +
			$"{indent}obj: {obj}\n" +
			$"{indent}spr: {spr}\n" +
			$"{indent}layer: {layer}\n" +
			$"{indent}i: {i}\n" +
			$"{indent}j: {j}\n" +
			$"{indent}x: {x}\n" +
			$"{indent}y: {y}\n" +
			$"{indent}image_xscale: {image_xscale}\n" +
			$"{indent}image_yscale: {image_yscale}\n" +
			$"{indent}image_angle: {image_angle}\n" +
			$"{indent}direction: {direction}\n" +
			$"{indent}method0: {method0}\n" +
			$"{indent}method1: {method1}\n" +
			$"{indent}method2: {method2}\n" +
			$"{indent}method3: {method3}\n" +
			$"{indent}randomizer_new: {randomizer_new}";
		}
	}
}
