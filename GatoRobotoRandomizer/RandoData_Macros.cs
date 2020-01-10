using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatoRobotoRandomizer {
	static partial class RandoData {
		public static Dictionary<string, RandoMacro> Macros = new Dictionary<string, RandoMacro>() {
			{ "NEXUS", new RandoMacro("NEXUS", "rocket") },
			{ "INCUBATOR", new RandoMacro("INCUBATOR", "CLEARED_AQUE & CLEARED_HEAT & CLEARED_VENT & decoder") },
			{ "CLEARED_AQUE", new RandoMacro("CLEARED_AQUE", "NEXUS") },
			{ "CLEARED_HEAT", new RandoMacro("CLEARED_HEAT", "NEXUS & (OPTB_small_mech | dash)") },
			{ "CLEARED_VENT", new RandoMacro("CLEARED_VENT", "NEXUS") },
			{ "CAN_COMPLETE_GAME", new RandoMacro("CAN_COMPLETE_GAME", "INCUBATOR & dash & (OPTB_not_100 | vhs>13)") },
		};
	}
	class RandoMacro {
		public string Name;
		public string Logic;

		private string pLogicPost = "";
		public string LogicPost {
			// This simply returns the macro as Postfixed; Macros NOT replaced.
			// eg. 'INCUBATOR dash &'
			get {
				if (pLogicPost == "") {
					pLogicPost = RPN.Parse(Logic);
				}

				return pLogicPost;
			}
		}

		private string pLogicBase = "";
		public string LogicBase {
			// Gives us the Logic as basal as possible; Macros ARE replaced.
			// eg. 'rocket rocket OPTB_small_mech dash | & & rocket & decoder & dash &'
			get {
				if (pLogicBase == "") {
					//Can't use macro base; that's what we're building here!
					pLogicBase = GatoRobotoRandomizer.Logic.GetBase(LogicPost, false);
				}

				return pLogicBase;
			}
		}

		public RandoMacro(string name, string logic) {
			this.Name = name;
			this.Logic = logic;
		}
	}
}
