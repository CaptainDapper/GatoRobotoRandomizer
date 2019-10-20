using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace GatoRobotoRandomizer {
	public static class MapChanger {
		private static readonly string[] TileSprites = {"tileset0_118", "tileset0_163", "tileset0_219", "tileset0_149", "tileset0_102", "tileset0_194", "tileset0_52", "tileset0_1", "tileset0_107", "tileset0_3", "tileset0_2", "tileset0_0", "tileset0_103", "tileset0_133", "tileset0_74", "tileset0_189", "tileset0_68", "tileset0_85", "tileset0_132", "tileset0_58", "tileset0_138", "tileset0_86", "tileset0_178", "tileset0_179", "tileset0_104", "tileset0_89", "tileset0_105", "tileset0_137", "tileset0_139", "tileset0_99", "tileset0_150", "tileset0_190", "tileset0_188", "tileset0_100", "tileset0_155", "tileset0_153", "tileset0_151", "tileset0_101", "tileset0_115", "tileset0_135", "tileset0_134", "tileset0_157", "tileset0_136", "tileset0_131", "tileset0_195", "tileset1_36", "tileset0_211", "tileset1_41", "tileset1_39", "tileset1_40", "tileset0_106", "tileset0_147", "tileset0_117", "tileset0_226", "tileset0_220", "tileset0_182", "tileset0_146", "tileset0_156", "tileset1_37", "tileset0_75", "tileset0_123", "tileset0_140", "tileset1_1", "tileset1_17", "tileset1_2", "tileset1_38", "tileset0_87", "tileset1_5", "tileset1_4", "tileset1_6", "tileset0_145", "tileset0_120", "tileset0_121", "tileset0_124", "tileset0_161", "tileset0_122", "tileset1_64", "tileset1_18", "tileset1_80", "tileset0_114", "tileset0_116", "tileset1_48", "tileset1_65", "tileset1_54", "tileset2_52", "tileset1_66", "tileset1_7", "tileset2_49", "tileset2_3", "tileset2_25", "tileset2_22", "tileset1_23", "tileset2_51", "tileset2_2", "tileset1_22", "tileset1_50", "tileset1_24", "tileset2_1", "tileset1_52", "tileset1_0", "tileset0_162", "tileset2_8", "tileset2_241", "tileset2_6", "tileset2_7", "tileset2_5", "tileset1_42", "tileset2_4", "tileset2_10", "tileset2_9", "tileset0_173", "tileset2_17", "tileset2_16", "tileset2_18", "tileset2_19", "tileset2_20", "tileset1_55", "tileset1_53", "tileset0_88", "tileset0_152", "tileset0_227", "tileset0_172", "tileset0_236", "tileset2_42", "tileset2_26", "tileset2_23", "tileset2_24", "tileset2_21", "tileset1_8", "tileset2_48", "tileset2_50", "tileset2_240", "tileset1_81", "tileset0_130", "tileset1_20", "tileset1_56", "tileset1_9", "tileset1_11", "tileset1_43", "tileset1_10", "tileset1_44", "tileset1_57", "tileset1_58", "tileset1_59", "tileset1_12", "tileset1_21", "tileset1_19", "tileset1_26", "tileset1_25", "tileset1_27", "tileset0_210"};
		//{ "spr_vhs_orb", "spr_health_orb", "spr_rocket_orb", "spr_decoder_orb", "spr_orb", "spr_cooler_orb", "spr_dash_orb", "spr_bigshot_orb" };//, "spr_repeater_orb", "spr_hopper_orb" };//

		private static readonly Queue<int> unusedIDs = new Queue<int>(new[] { 2311, 2312, 4816, 4817, 4822, 4823, 4828, 4829, 12867, 12881, 20649, 20778, 26376, 26379, 26760, 26765, 29182, 29183, 29189, 29195 }); //Many more available; this should do for now; check data_structs

		internal static void MakeChanges(bool doVanilla = false) {
			Dictionary<string, JSONNode> maps = Randomizer.Maps;

			//Remove all rando_new Insts
			List<InstDef> batch = new List<InstDef>();
			foreach (InstDef def in InstDef.All.Where(val => val.randomizer_new)) {
				maps[def.map][def.room].Remove(def.inst);
				batch.Add(def);
			}
			InstDef.All.RemoveAll(val => batch.Contains(val));

			if (doVanilla) {
				//Unencode seed on first screen
				maps["map0"]["0808"]["inst17"]["spr"] = "tileset0_52";
				maps["map0"]["0808"]["inst20"]["spr"] = "tileset0_1";
				maps["map0"]["0808"]["inst23"]["spr"] = "tileset0_1";
				maps["map0"]["0808"]["inst26"]["spr"] = "tileset0_52";
				maps["map0"]["0808"]["inst29"]["spr"] = "tileset0_52";
				maps["map0"]["0808"]["inst32"]["spr"] = "tileset0_52";
				maps["map0"]["0808"]["inst35"]["spr"] = "tileset0_52";
				maps["map0"]["0808"]["inst38"]["spr"] = "tileset0_52";

				return;
			}

			//Encode String on first screen
			{
				string[] s = SpriteSeed();
				maps["map0"]["0808"]["inst17"]["spr"] = s[0];
				maps["map0"]["0808"]["inst20"]["spr"] = s[1];
				maps["map0"]["0808"]["inst23"]["spr"] = s[2];
				maps["map0"]["0808"]["inst26"]["spr"] = s[3];
				maps["map0"]["0808"]["inst29"]["spr"] = s[4];
				maps["map0"]["0808"]["inst32"]["spr"] = s[5];
				maps["map0"]["0808"]["inst35"]["spr"] = s[6];
				maps["map0"]["0808"]["inst38"]["spr"] = s[7];
			}

			//Add blocks to Heater Core
			AddTile("map3", "1817", "tileset1_42", 13, 7);
			AddTile("map3", "1817", "tileset1_42", 14, 7);
			AddTile("map3", "1817", "tileset1_42", 14, 6);
			//i, j, spr
			//13, 7, tileset1_42
			//14, 7
			//14, 6
		}

		private static void AddTile(string map, string room, string spr, int i, int j) {
			InstDef newDef = new InstDef();

			newDef.obj = "obj_tile";
			newDef.spr = spr;
			newDef.layer = "player";

			newDef.i = i;
			newDef.j = j;

			newDef.image_xscale = 1;
			newDef.image_yscale = 1;
			newDef.image_angle = 0;
			newDef.direction = 0;

			newDef.randomizer_new = true;

			AddNewInst(map, room, newDef);
		}

		private static void AddNewInst(string map, string room, InstDef def) {
			if (def.id == 0) def.id = unusedIDs.Dequeue();
			if (def.inst == "") def.inst = $"inst{GetNewInst(map, room)}";
			def.map = map;
			def.room = room;

			InstDef.All.Add(def);
			Randomizer.Maps[map][room][def.inst] = def.ToJSONObject();

			Debug.WriteLine($"Adding inst to {map} - {room}:");
			Debug.WriteLine(def.ToString("--"));
		}

		private static int GetNewInst(string map, string room) {
			int max = 0;
			foreach (InstDef item in InstDef.All.Where(val => val.map == map && val.room == room)) {
				if (item.InstNum > max) {
					max = item.InstNum;
				}
			}

			return max+1;
		}

		public static string[] SpriteSeed(int seed = -1) {
			if (seed == -1) seed = RandoSettings.Seed;

			int seed2 = 0;
			int temp = 0;
			if (seed > 499999999) {
				temp = seed;
				seed2 = seed - int.Parse("246897135");
				seed = int.Parse("531798642") - temp;
			} else {
				temp = seed;
				seed2 = seed + int.Parse("317598246");
				seed = int.Parse("642895713") + temp;
			}

			string[] e1 = EncodeFromDecimal(seed-500000000);
			string[] e2 = EncodeFromDecimal(seed2-500000000);

			return new[] { e2[0], e1[3], e2[1], e1[2], e2[2], e1[1], e2[3], e1[0] };
		}

		public static string[] EncodeFromDecimal(long decimalNumber) {
			if (TileSprites.Length < 2)
				throw new ArgumentException("The radix must be >= 2");

			if (decimalNumber == 0)
				return new[] { TileSprites[0] };

			long currentNumber = Math.Abs(decimalNumber);
			List<string> ret = new List<string>();

			while (currentNumber != 0) {
				int remainder = (int)(currentNumber % TileSprites.Length);
				ret.Add(TileSprites[remainder]);
				currentNumber = currentNumber / TileSprites.Length;
			}

			while (ret.Count < 4) {
				ret.Add(TileSprites[0]);
			}

			return ret.ToArray();
		}
	}
}
