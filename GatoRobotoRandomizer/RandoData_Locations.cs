using System.Collections.Generic;
using System.Diagnostics;

namespace GatoRobotoRandomizer {
	static partial class RandoData {
		public static List<RandoLocation> GetAllLocations() {
			List<RandoLocation> locations = new List<RandoLocation>();

			locations.Add(new RandoLocation("L-C2", "L-C2", "map0", "1810", "inst58", "rocket"));
			locations.Add(new RandoLocation("L-H2", "L-H2", "map0", "1812", "inst74", "rocket"));
			locations.Add(new RandoLocation("L-C1", "L-C1", "map0", "0710", "inst79"));
			locations.Add(new RandoLocation("L-H1", "L-H1", "map0", "0408", "inst35"));
			locations.Add(new RandoLocation("L-U1", "rocket", "map0", "0814", "inst67"));
			locations.Add(new RandoLocation("L-U2", "decoder", "map0", "0807", "inst23", "(OPTB_phase_save & dash) | (CLEARED_AQUE & CLEARED_HEAT & CLEARED_VENT)"));

			locations.Add(new RandoLocation("A-C3", "A-C3", "map1", "2106", "inst35", "NEXUS"));
			locations.Add(new RandoLocation("A-H1", "A-H1", "map1", "0406", "inst88", "NEXUS"));
			locations.Add(new RandoLocation("A-H2", "A-H2", "map1", "1606", "inst79", "NEXUS"));
			locations.Add(new RandoLocation("A-U1", "djump", "map1", "2410", "inst57", "NEXUS"));
			locations.Add(new RandoLocation("A-C1", "A-C1", "map1", "0707", "inst18", "NEXUS & djump"));
			locations.Add(new RandoLocation("A-C2", "A-C2", "map1", "1106", "inst37", "NEXUS"));

			locations.Add(new RandoLocation("N-H2", "N-H2", "map2", "2314", "inst42", "NEXUS"));
			locations.Add(new RandoLocation("N-C1", "N-C1", "map2", "0914", "inst24", "NEXUS"));
			locations.Add(new RandoLocation("N-H1", "N-H1", "map2", "1014", "inst25", "NEXUS & (djump | cooler | ((OPTB_phase_save | OPTB_advanced) & dash))"));
			locations.Add(new RandoLocation("N-C3", "N-C3", "map2", "2113", "inst28", "NEXUS & CLEARED_VENT"));
			locations.Add(new RandoLocation("N-C2", "N-C2", "map2", "1413", "inst47", "NEXUS & ((djump & dash) | OPTB_not_beginner)"));

			locations.Add(new RandoLocation("H-C1", "H-C1", "map3", "0414", "inst39", "CLEARED_HEAT"));
			locations.Add(new RandoLocation("H-C3", "H-C3", "map3", "1916", "inst33", "CLEARED_HEAT"));
			locations.Add(new RandoLocation("H-U2", "cooler", "map3", "0113", "inst46", "CLEARED_HEAT"));
			locations.Add(new RandoLocation("H-C2", "H-C2", "map3", "1319", "inst27", "CLEARED_HEAT"));
			locations.Add(new RandoLocation("H-U1", "dash", "map3", "1114", "inst29", "NEXUS"));
			locations.Add(new RandoLocation("H-H2", "H-H2", "map3", "1713", "inst47", "CLEARED_HEAT"));
			locations.Add(new RandoLocation("H-H1", "H-H1", "map3", "0417", "inst71", "CLEARED_HEAT"));

			locations.Add(new RandoLocation("V-U1", "bigshot", "map4", "1718", "inst74", "NEXUS"));
			locations.Add(new RandoLocation("V-C1", "V-C1", "map4", "0517", "inst27", "NEXUS"));
			locations.Add(new RandoLocation("V-C2", "V-C2", "map4", "1613", "inst71", "NEXUS"));
			locations.Add(new RandoLocation("V-H1", "V-H1", "map4", "0815", "inst19", "NEXUS"));

			locations.Add(new RandoLocation("I-H1", "I-H1", "map5", "2413", "inst20", "INCUBATOR & dash & djump & vhs>13"));
			locations.Add(new RandoLocation("I-C1", "I-C1", "map5", "1513", "inst35", "INCUBATOR"));

			/*
			NEXUS = "rocket"
			CLEARED_AQUE = "NEXUS"
			CLEARED_HEAT = "NEXUS & (OPTB_small_mech | dash)"
			CLEARED_VENT = "NEXUS"
			INCUBATOR = "CLEARED_AQUE & CLEARED_HEAT & CLEARED_VENT & decoder"
			*/

			return locations;
		}
	}

	public class RandoLocation {
		public RandoLocation(string id, string itemID, string map, string room, string inst, string logic = "") {
			this.ID = id;
			this.OrigItemID = itemID;
			this.Map = map;
			this.Room = room;
			this.Inst = inst;
			this.Logic = logic;
		}

		public string ID { get; private set; } = "Z-T0";
		public string OrigItemID { get; private set; } = "";
		public string Map { get; private set; } = "map9";
		public string Room { get; private set; } = "9999";
		public string Inst { get; private set; } = "inst00";
		public string Logic { get; private set; } = "";
		public bool Locked { get; set; } = false;

		private string pLogicPost = "";
		public string LogicPost {
			get {
				if (pLogicPost == "") {
					pLogicPost = RPN.Parse(Logic);
				}

				return pLogicPost;
			}
		}

		private string pLogicBase = "";
		public string LogicBase {
			get {
				if (pLogicBase == "") {
					pLogicBase = GatoRobotoRandomizer.Logic.GetBase(LogicPost);
				}

				return pLogicBase;
			}
		}

		public RandoItem Item { get; private set; } = null;

		public void PlaceItem(RandoItem item) {
			this.Item = item;
		}

		public override string ToString() {
			return $"{ID}: {{{(Item != null ? Item.Name : "")}}}";
		}
	}
}
