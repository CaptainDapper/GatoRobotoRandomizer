using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatoRobotoRandomizer {
	public static class RandoSettings {
		private const string SubVersion = "bug";

		public static Dictionary<string, bool> Options_bool = new Dictionary<string, bool>() {
			{ "OPTB_small_mech", false },
			{ "OPTB_phase_save", false },
			{ "OPTB_advanced", false }
		};

		public static int Seed { get; set; } = NewSeed();

		public static int NewSeed() {
			return new Random().Next(1000000000);
		}

		public static string Version { get {
				string vFull = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
				return $"{vFull.Split('.')[0]}.{vFull.Split('.')[1]}{SubVersion}";
			} }
	}
}
