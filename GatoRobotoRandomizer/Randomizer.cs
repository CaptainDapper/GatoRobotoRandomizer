using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using SimpleJSON;
using System.Runtime.Serialization;

//curl -s https://api.github.com/repos/CaptainDapper/GatoRobotoRandomizer/releases | findstr download_count

namespace GatoRobotoRandomizer {
	public class Randomizer {
		private Random rnd;

		private List<RandoItem> unusedItems;
		private List<RandoLocation> unusedLocations;

		private List<RandoLocation> obtainedLocations;
		private List<RandoItem> obtainedItems;

		private int availCount;

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
			IO.Log("Get Stuff");
			unusedItems = RandoData.GetAllItems();
			unusedLocations = RandoData.GetAllLocations();

			obtainedLocations = new List<RandoLocation>();
			obtainedItems = new List<RandoItem>();

			IO.Log("Main Rando Loop");
			while (unusedLocations.Count > 0) {
				IO.Log($"Top of Loop: {unusedLocations.Count} locs left");
				availCount = countAvailableLocations(obtainedItems, out List<RandoLocation> allAvailLocations);
				RandoItem chosenItem;
				RandoLocation chosenLocation;

				if (unusedLocations.Count == 1 && unusedItems.Count == 1) {
					IO.Log("Almost done! Last location.");
					// The last metroid is in captivity. The galaxy is almost complete.
					chosenItem = unusedItems[0];
					chosenLocation = unusedLocations[0];
				} else {
					bool canCompleteGame = Logic.Eval(RandoData.Macros["CAN_COMPLETE_GAME"].LogicBase, obtainedItems);

					if (availCount == 1 && !canCompleteGame) {
						IO.Log("Must force!", IO.LogType.Warn);
						// Force Progression
						List<RandoItem> candItems = getItemCandidates(obtainedItems, availCount);
						if (candItems.Count == 0) {
							if (unusedItems.Count == 2) {
								int vhsCount = unusedItems.Where(v => v.Name == "vhs").Count();

								if (vhsCount == 2) {
									IO.Log("A wild Panic Swap appeared!", IO.LogType.Warn);
									logCurrentItems(null, unusedLocations, unusedItems);

									// Last Loc VHS Problem
									RandoItem panicItem = unusedItems[0];
									RandoLocation panicLocation = chooseLocation(obtainedLocations.Where(v => v.Locked == false && v.Item.Name != "vhs"));

									RandoItem swapItem = panicLocation.Item;

									removeFromLocation(swapItem, panicLocation);
									placeAtLocation(panicItem, panicLocation);
									vhsCount--;

									IO.Log($"Okay, panic resolved.");
									IO.Log($"I put {panicItem} at {panicLocation.ID} and will find a new home for {swapItem}", IO.LogType.Debug);

									logCurrentItems(null, unusedLocations, unusedItems);
									continue;
								}

								IO.Log($"I am reasonably certain something has gone wrong... I know you're upset, but please refrain from turning people into animals JUST yet.");
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

				IO.Log($"{chosenLocation.ID}: I got put a {chosenItem.Name} on me.", IO.LogType.Debug);
			}

			IO.Log("Rando Finish");
			logCurrentItems(obtainedLocations, null, null);

			return obtainedLocations;
		}

		private void logCurrentItems(List<RandoLocation> obLoc, List<RandoLocation> unLoc, List<RandoItem> unIt) {
			if (obLoc != null) {
				IO.Log($"# OBTAINED : {obLoc.Count}/32 #");
				foreach (RandoLocation location in obLoc) {
					IO.Log($"--{location}", IO.LogType.Debug);
				}
			}

			if (unLoc != null) {
				IO.Log($"# UNUSED LOCS : {unLoc.Count}/32 #");
				foreach (RandoLocation location in unLoc) {
					IO.Log($"--{location}", IO.LogType.Debug);
				}
			}

			if (unIt != null) {
				IO.Log($"# UNUSED ITEMS : {unIt.Count}/32 #");
				foreach (RandoItem item in unIt) {
					IO.Log($"--{item}", IO.LogType.Debug);
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

			// Mark as obtained
			obtainedItems.Add(chosenItem);
			if (countAvailableLocations(obtainedItems, out _) > availCount) {
				//This item opened up progression; lock it in place.
				chosenLocation.Locked = true;
			}
			obtainedLocations.Add(chosenLocation);
		}

		private void removeFromLocation(RandoItem chosenItem, RandoLocation chosenLocation) {
			if (chosenLocation.Locked == true) {
				throw new RandoException("The attempt on a Locked Location has left the program scarred and deformed.");
			}

			//Clear the item
			chosenLocation.ClearItem();

			//Add back into pools
			unusedLocations.Add(chosenLocation);
			unusedItems.Add(chosenItem);

			// Mark as unobtained
			obtainedItems.Remove(chosenItem);
			obtainedLocations.Remove(chosenLocation);
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
