using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using SimpleJSON;
using System.Runtime.Serialization;

namespace GatoRobotoRandomizer {
	public class Randomizer {
		private Random rnd;

		private List<RandoItem> unusedItems;
		private List<RandoLocation> unusedLocations;

		private static Dictionary<string, JSONNode> pMaps = null;
		public static Dictionary<string, JSONNode> Maps {
			get {
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
			unusedItems = RandoData.GetAllItems();
			unusedLocations = RandoData.GetAllLocations();

			List<RandoLocation> obtainedLocations = new List<RandoLocation>();
			List<RandoItem> obtainedItems = new List<RandoItem>();

			IO.Output("Main Rando Loop");
			while (unusedLocations.Count > 0) {
				IO.Output($"Start Loop: {unusedLocations.Count} locs left");
				int availCount = countAvailableLocations(obtainedItems, out List<RandoLocation> allAvailLocations);
				RandoItem chosenItem;
				RandoLocation chosenLocation;

				if (unusedLocations.Count == 1 && unusedItems.Count == 1) {
					IO.Output("Almost done! Last location.");
					// The last metroid is in captivity. The galaxy is almost complete.
					chosenItem = unusedItems[0];
					chosenLocation = unusedLocations[0];
				} else {
					bool canCompleteGame = Logic.Eval(RandoData.Macros["CAN_COMPLETE_GAME"].LogicBase, obtainedItems);

					if (availCount == 1 && !canCompleteGame) {
						IO.Output("Must force!");
						// Force Progression
						List<RandoItem> candItems = getItemCandidates(obtainedItems, availCount);
						if (candItems.Count == 0) {
							if (unusedItems.Count == 2) {
								int vhsCount = unusedItems.Where(v => v.Name == "vhs").Count();

								if (vhsCount == 2) {
									IO.Output("A wild Panic Swap appeared!");
									logCurrentItems(null, unusedLocations, unusedItems);

									// Last Loc VHS Problem
									RandoItem panicItem = unusedItems[0];
									RandoLocation panicLocation = chooseLocation(obtainedLocations.Where(v => v.Locked == false && v.Item.Name != "vhs"));

									RandoItem swapItem = panicLocation.Item;

									placeAtLocation(panicItem, panicLocation);
									unusedItems.Add(swapItem);
									vhsCount--;

									logCurrentItems(null, unusedLocations, unusedItems);
									IO.Output($"Okay, panic resolved. I put {panicItem} at {panicLocation.ID} and will find a new home for {swapItem}");
									continue;
								}

								logCurrentItems(obtainedLocations, unusedLocations, unusedItems);
								throw new RandoException("No Candidate Items Left!");
							}
						}

						// Pick an item
						chosenItem = candItems[rnd.Next(candItems.Count - 1)];
					} else {
						// Choose a totes random item.
						chosenItem = unusedItems[rnd.Next(unusedItems.Count - 1)];
					}
					// Choose the spot
					chosenLocation = chooseLocation(allAvailLocations.Except(obtainedLocations));
				}

				// Place the item; remove from pools
				placeAtLocation(chosenItem, chosenLocation);

				// Mark as obtained
				obtainedItems.Add(chosenItem);
				if (countAvailableLocations(obtainedItems, out _) > availCount) {
					//This item opened up progression; lock it in place.
					chosenLocation.Locked = true;
				}
				obtainedLocations.Add(chosenLocation);

				IO.Output($"{chosenLocation.ID}: I got put a {chosenItem.Name} on me.");
			}

			IO.Output("Rando Finish");

#if DEBUG
			logCurrentItems(obtainedLocations, null, null);
#endif

			return obtainedLocations;
		}

		private void logCurrentItems(List<RandoLocation> obLoc, List<RandoLocation> unLoc, List<RandoItem> unIt) {
			if (obLoc != null) {
				IO.Output($"\n# OBTAINED #");
				foreach (RandoLocation location in obLoc) {
					IO.Output($"--{location.ToString()}");
				}
			}

			if (unLoc != null) {
				IO.Output($"\n# UNUSED LOCS #");
				foreach (RandoLocation location in unLoc) {
					IO.Output($"--{location.ToString()}");
				}
			}

			if (unIt != null) {
				IO.Output($"\n# UNUSED ITEMS #");
				foreach (RandoItem item in unIt) {
					IO.Output($"--{item.ToString()}");
				}
			}
		}

		private int countAvailableLocations(List<RandoItem> obtainedItems, out List<RandoLocation> availableLocations) {
			availableLocations = new List<RandoLocation>();
			foreach (RandoLocation l in unusedLocations) {
				if (Logic.Eval(l, obtainedItems)) {
					availableLocations.Add(l);
				}
			}

			return availableLocations.Count();
		}

		private List<RandoItem> getItemCandidates(List<RandoItem> obtainedItems, int currentAvailable) {
			List<RandoItem> candidateItems = new List<RandoItem>();

			foreach (RandoItem k in unusedItems) {
				obtainedItems.Add(k);
				if (countAvailableLocations(obtainedItems, out _) > currentAvailable) {
					candidateItems.Add(k);
				}
				obtainedItems.Remove(k);

				Debug.Write("");
			}

			Debug.Write("");
			return candidateItems;
		}

		private RandoLocation chooseLocation(IEnumerable<RandoLocation> choices) {
			return choices.ElementAt(rnd.Next(choices.Count()));
		}

		private void placeAtLocation(RandoItem chosenItem, RandoLocation chosenLocation) {
			//Place the item
			chosenLocation.PlaceItem(chosenItem);

			//Remove from the pools
			unusedLocations.Remove(chosenLocation);
			unusedItems.Remove(chosenItem);
		}

		[Serializable]
		private class RandoException : Exception {
			public RandoException() {
			}

			public RandoException(string message) : base(message) {
			}

			public RandoException(string message, Exception innerException) : base(message, innerException) {
			}

			protected RandoException(SerializationInfo info, StreamingContext context) : base(info, context) {
			}
		}
	}
}
