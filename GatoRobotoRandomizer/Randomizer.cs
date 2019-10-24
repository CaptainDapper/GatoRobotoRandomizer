using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using SimpleJSON;

namespace GatoRobotoRandomizer {
	public class Randomizer {
		private Random rnd;

		private Dictionary<string, RandoItem> itemPool;
		private Dictionary<string, RandoLocation> locationPool;

		private static Dictionary<string, JSONNode> pMaps = null;
		public static Dictionary<string, JSONNode> Maps { get {
				if (pMaps == null) {
					pMaps = IO.DecodeAllMaps();
				}
				return pMaps;
			}
		}

		public Randomizer() {
			rnd = new Random(RandoSettings.Seed);
		}

		public List<RandoLocation> Randomize() {
			IO.Output("Get Stuff");
			itemPool = RandoData.GetAllItems();
			locationPool = RandoData.GetAllLocations();

			List<RandoLocation> obtainedLocations = new List<RandoLocation>();
			List<string> obtainedItems = new List<string>();

			IO.Output("Main Rando Loop");
			while (locationPool.Count > 0) {
				IO.Output($"locpool count: {locationPool.Count}");
				int currentAvailable = countAvailableLocations(obtainedItems, out List<string> availableLocations);
				Candidate chosenItem;
				RandoLocation chosenLocation;

				Debug.WriteLine(string.Join(" ", availableLocations));

				if (locationPool.Count == 1 && itemPool.Count == 1) {
					IO.Output("Almost done! Last location.");
					//The last metroid is in captivity. The galaxy is almost complete.
					string iKey = itemPool.Keys.ToArray()[0];
					chosenItem = new Candidate(iKey, itemPool[iKey]); //itemPool.Keys[0];
					chosenLocation = locationPool.Values.ToArray()[0];
				} else {
					if (currentAvailable == 1 && !Logic.Eval(RandoData.Macros["CAN_COMPLETE_GAME"].LogicPost, obtainedItems)) {
						IO.Output("Must force!");
						//Force Progression
						List<Candidate> candItems = getItemCandidates(obtainedItems, currentAvailable, availableLocations);
						if (candItems.Count == 0) {
							continue;
						}

						// Pick an item
						chosenItem = candItems[rnd.Next(candItems.Count - 1)];
					} else {
						// Choose a totes random item.
						string chosenID = itemPool.Keys.ToArray()[rnd.Next(itemPool.Keys.Count - 1)];
						chosenItem = new Candidate(chosenID, itemPool[chosenID]);
						chosenItem.AddLocations(availableLocations);
					}
					// Choose the spot
					chosenLocation = chooseLocation(chosenItem, chosenItem.GetLocations().ToArray());       //This also removes item, loc from pools
				}

				// Place the item; remove from pools
				placeAtLocation(chosenItem, chosenLocation);

				// Mark as obtained
				obtainedLocations.Add(chosenLocation);
				obtainedItems.Add(chosenItem.ItemID);

				Debug.WriteLine($"{chosenLocation.Name}: I got put a {chosenItem.Item.Type.Name} on me.");
			}
			
			IO.Output("Rando Finish");
			Debug.WriteLine("Randomizing is finished... Let's see what we've got!");

			IO.Output("Rando Debug Dump");
			foreach (RandoLocation loc in obtainedLocations) {
				Debug.WriteLine($"{loc.Name} now has {loc.Item.Type.Name}");
			}

			return obtainedLocations;
		}

		private int countAvailableLocations(List<string> obtainedItems, out List<string> availableLocations) {
			availableLocations = new List<string>();
			foreach (string l in locationPool.Keys) {
				if (Logic.Eval(locationPool[l], obtainedItems)) {
					availableLocations.Add(l);
				}
			}

			return availableLocations.Count();
		}

		private List<Candidate> getItemCandidates(List<string> obtainedItems, int currentAvailable, List<string> availableLocations) {
			List<Candidate> candidateItems = new List<Candidate>();

			foreach (string k in itemPool.Keys) {
				obtainedItems.Add(k);
				Candidate cand = new Candidate(k, itemPool[k]);
				if (countAvailableLocations(obtainedItems, out _) > currentAvailable) {
					cand.AddLocations(availableLocations);
				}
				if (cand.CountLocations() > 0) {
					candidateItems.Add(cand);
				}
				obtainedItems.Remove(k);

				Debug.Write("");
			}

			Debug.Write("");
			return candidateItems;
		}

		private class Candidate {
			public string ItemID { get; private set; }
			public RandoItem Item { get; private set; }
			private List<string> locations = new List<string>();

			public Candidate(string itemID, RandoItem itemObj) {
				this.ItemID = itemID;
				this.Item = itemObj;
			}

			internal void AddLocations(List<string> newLocations) {
				locations.AddRange(newLocations);
			}

			internal int CountLocations() {
				return locations.Count();
			}

			internal List<string> GetLocations() {
				return locations;
			}

			public override string ToString() {
				return $"{ItemID}: \"{{ {string.Join(", ", locations)} }}\"";
			}
		}

		private RandoLocation chooseLocation(Candidate chosenItem, string[] targsArr = null, bool whiteList = true) {
			//whiteList = true;   can only place in the following locations
			//whiteList = false;  can't place in the following locations

			List<string> removeThese = new List<string>();
			List<string> targs;
			if (targsArr == null) {
				targs = locationPool.Keys.ToList();
			} else if (whiteList == true) {
				targs = targsArr.ToList();
			} else {
				//Blacklist... gotta do some weird stuff here!
				targs = locationPool.Keys.ToList();						//All avail locations become targs
				List<string> negTargs = targsArr.ToList();		//The arg is a negative, so we
				foreach (string targ in targs) {				//Go through all locations
					foreach (string neg in negTargs) {			//And compare them to the blacklist
						if (neg == targ) {
							removeThese.Add(neg);				//Then flag the appropriate ones for removal.
						}										//Now we're good to go as if it's a whitelist.
					}
				}
			}

			//Get rid of already used locations
			foreach (string targ in targs) {
				if (!locationPool.ContainsKey(targ)) {
					removeThese.Add(targ);
				}
			}

			if (removeThese.Count > 0) {
				foreach (string loc in removeThese) {
					targs.Remove(loc);
				}
			}

			//Get the chosen location
			RandoLocation chosenLocation = locationPool[targs[rnd.Next(targs.Count - 1)]];

			return chosenLocation;
		}

		private void placeAtLocation(Candidate chosenItem, RandoLocation chosenLocation) {
			//Place the item
			chosenLocation.PlaceItem(itemPool[chosenItem.ItemID]);

			//Remove from the pools
			locationPool.Remove(chosenLocation.Name);
			itemPool.Remove(chosenItem.ItemID);
		}
	}
}
