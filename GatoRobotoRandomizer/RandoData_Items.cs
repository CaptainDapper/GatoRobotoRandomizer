﻿using System.Collections.Generic;
using System.Diagnostics;

namespace GatoRobotoRandomizer {
	static partial class RandoData {
		public static List<RandoItem> GetAllItems() {
			List<RandoItem> items = new List<RandoItem>();

			items.Add(new RandoItem("L-C2", new ItemTypes.vhs(), "choose_palette_3"));
			items.Add(new RandoItem("L-H2", new ItemTypes.hp()));
			items.Add(new RandoItem("L-C1", new ItemTypes.vhs(), "choose_palette_2"));
			items.Add(new RandoItem("L-H1", new ItemTypes.hp()));
			items.Add(new RandoItem("L-U1", new ItemTypes.rocket()));
			items.Add(new RandoItem("L-U2", new ItemTypes.decoder()));

			items.Add(new RandoItem("A-C3", new ItemTypes.vhs(), "choose_palette_4"));
			items.Add(new RandoItem("A-H1", new ItemTypes.hp()));
			items.Add(new RandoItem("A-H2", new ItemTypes.hp()));
			items.Add(new RandoItem("A-U1", new ItemTypes.djump()));
			items.Add(new RandoItem("A-C1", new ItemTypes.vhs(), "choose_palette_2", "-1"));
			items.Add(new RandoItem("A-C2", new ItemTypes.vhs(), "choose_palette_7"));

			items.Add(new RandoItem("N-H2", new ItemTypes.hp()));
			items.Add(new RandoItem("N-C1", new ItemTypes.vhs(), "choose_palette_5"));
			items.Add(new RandoItem("N-H1", new ItemTypes.hp()));
			items.Add(new RandoItem("N-C3", new ItemTypes.vhs(), "choose_palette_7", "-1"));
			items.Add(new RandoItem("N-C2", new ItemTypes.vhs(), "choose_palette_3", "-1"));

			items.Add(new RandoItem("H-C1", new ItemTypes.vhs(), "choose_palette_8"));
			items.Add(new RandoItem("H-C3", new ItemTypes.vhs(), "choose_palette_6", "-1"));
			items.Add(new RandoItem("H-U2", new ItemTypes.cooler()));
			items.Add(new RandoItem("H-C2", new ItemTypes.vhs(), "choose_palette_6"));
			items.Add(new RandoItem("H-U1", new ItemTypes.dash()));
			items.Add(new RandoItem("H-H2", new ItemTypes.hp()));
			items.Add(new RandoItem("H-H1", new ItemTypes.hp()));

			items.Add(new RandoItem("V-U1", new ItemTypes.bigshot()));
			items.Add(new RandoItem("V-C1", new ItemTypes.vhs(), "choose_palette_8", "-1"));
			items.Add(new RandoItem("V-C2", new ItemTypes.vhs(), "choose_palette_5", "-1"));
			items.Add(new RandoItem("V-H1", new ItemTypes.hp()));

			items.Add(new RandoItem("I-H1", new ItemTypes.hp()));
			items.Add(new RandoItem("I-C1", new ItemTypes.vhs(), "choose_palette_4", "-1"));

			return items;
		}
	}

	public class RandoItem {
		public RandoItem(string origLoc, ItemTypes type, string method0 = "", string image_xscale = "1") {
			this.OrigLoc = origLoc;
			this.Type = type;
			this.Method0 = method0;
			this.Image_xscale = image_xscale;
		}

		public string OrigLoc { get; private set; } = "Z-T0";
		public ItemTypes Type { get; private set; } = null;
		public string Method0 { get; private set; } = "";
		public string Image_xscale { get; private set; } = "1";
		public string Spr { get { return Type.Spr; } }
		public string Obj { get { return Type.Obj; } }
		public string Name { get { return Type.Name; } }

		public override string ToString() {
			return $"{Name}";
		}
	}

	public class ItemTypes {
		public string Name { get; private set; } = "UNK";
		public string Spr { get; private set; } = "UNK";
		public string Obj { get; private set; } = "UNK";

		public class vhs : ItemTypes {
			public vhs() : base() {
				Name = "vhs";
				Spr = "spr_vhs_orb";
				Obj = "obj_vhs_orb";
			}
		}
		public class hp : ItemTypes {
			public hp() : base() {
				Name = "hp";
				Spr = "spr_health_orb";
				Obj = "obj_health_upgrade";
			}
		}
		public class rocket : ItemTypes {
			public rocket() : base() {
				Name = "rocket";
				Spr = "spr_rocket_orb";
				Obj = "obj_orb_rocket";
			}
		}
		public class decoder : ItemTypes {
			public decoder() : base() {
				Name = "decoder";
				Spr = "spr_decoder_orb";
				Obj = "obj_decoder_orb";
			}
		}
		public class djump : ItemTypes {
			public djump() : base() {
				Name = "djump";
				Spr = "spr_orb";
				Obj = "obj_djump_orb";
			}
		}
		public class cooler : ItemTypes {
			public cooler() : base() {
				Name = "cooler";
				Spr = "spr_cooler_orb";
				Obj = "obj_orb_cooler";
			}
		}
		public class dash : ItemTypes {
			public dash() : base() {
				Name = "dash";
				Spr = "spr_dash_orb";
				Obj = "obj_dash_orb";
			}
		}
		public class bigshot : ItemTypes {
			public bigshot() : base() {
				Name = "bigshot";
				Spr = "spr_bigshot_orb";
				Obj = "obj_bigshot_orb";
			}
		}
	}

}
