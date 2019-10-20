using System;
using System.Collections.Generic;
using System.Linq;
using GatoRobotoRandomizer;
using SimpleJSON;
using Altar;

namespace ConsoleApp1 {
	public static class Extension {
		public static (int, int) Add(this (int, int) t, (int, int) x) {
			return (t.Item1 + x.Item1, t.Item2 + x.Item2);
		}
	}

	class Program {
		static void Main(string[] args) {
			IO.Path = "C:/Program Files (x86)/Steam/steamapps/common/Gato Roboto";
			IO.Path = "E:\\Projects\\GatoRobotoRandomizer\\GatoRobotoRandomizer\\bin\\Debug";

			

			mapExplore();

			//Console.Write(Logic.Dumps());
		}

		private static void AddItem(JSONNode room, int x, int y, int id, int instNum) {
			room[$"inst{instNum}"]["spr"] = "spr_wall28";
			room[$"inst{instNum}"]["image_yscale"] = "1";
			room[$"inst{instNum}"]["j"] = y;
			room[$"inst{instNum}"]["direction"] = "0";
			room[$"inst{instNum}"]["image_angle"] = "0";
			room[$"inst{instNum}"]["x"] = x * 16 + 8;
			room[$"inst{instNum}"]["i"] = x;
			room[$"inst{instNum}"]["obj"] = "obj_wall28";
			room[$"inst{instNum}"]["image_xscale"] = "1";
			room[$"inst{instNum}"]["id"] = "159263";
			room[$"inst{instNum}"]["layer"] = "player";
			room[$"inst{instNum}"]["y"] = y * 16 + 8;
		}

		private static void mapExplore() {
			foreach (InstDef item in InstDef.All.Where(val => val.map=="map0" && val.room=="0608")) {
				Console.WriteLine($"{item.inst} - {Randomizer.Maps[item.map][item.room]["quest"]}");
				Console.WriteLine(item.ToString("--"));
				Console.WriteLine();
			}

			return;
		}

		private static void EncodeSeedTest() {
			for (int i = 0; i < 100; i++) {
				RandoSettings.Seed = RandoSettings.NewSeed();
				Console.WriteLine("Encoding Seed: " + RandoSettings.Seed);
				foreach (string sprite in MapChanger.SpriteSeed()) {
					Console.WriteLine("--" + sprite);
				}
			}
		}
	}
}
