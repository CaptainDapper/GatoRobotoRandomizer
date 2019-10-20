
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GatoRobotoRandomizer {
	public static class DataWin {
		public static void CheckForSections() {
			using (BinaryReader br = new BinaryReader(File.Open("./data.win", FileMode.Open))) {
				Queue<byte> four = new Queue<byte>();
				do {
					byte cur = br.ReadByte();
					if (cur >= 65 && cur <= 90 || cur >= 48 && cur <= 57) {
						four.Enqueue(cur);
					} else if (four.Count > 0) {
						four.Clear();
					}

					if (four.Count == 4) {
						for (int i = 0; i < 4; i++) {
							Debug.Write(System.Convert.ToChar(four.Dequeue()));
						}
						Debug.WriteLine("\t" + (br.BaseStream.Position-4).ToString("X"));
					}
				} while (br.BaseStream.Position < br.BaseStream.Length);
			}
		}
	}
}
