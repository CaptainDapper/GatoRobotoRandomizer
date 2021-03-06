﻿using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace GatoRobotoRandomizer {

	public static class Logic {
		public static string GetBase(string postfix, bool useMacroBase = true) {
			bool keepGoing = true;
			string postfixL = postfix;
			while (keepGoing) {
				keepGoing = false;
				foreach (string k in RandoData.Macros.Keys) {
					if (postfixL.Contains(k)) {
						keepGoing = true;
						if (useMacroBase) {
							postfixL = postfixL.Replace(k, RandoData.Macros[k].LogicBase);
						} else {
							postfixL = postfixL.Replace(k, RandoData.Macros[k].LogicPost);
						}
					}
				}
			}
			return postfixL;
		}

		public static string Dumps() {
			List<RandoLocation> locs = RandoData.GetAllLocations();
			StringBuilder sb = new StringBuilder();
			char area = 'L';

			foreach (RandoLocation location in locs) {
				if (area != location.ID.First()) {
					area = location.ID.First();
					sb.AppendLine();
				}
				sb.AppendLine($"{location.ID}: {RPN.Parse(location.LogicBase, false)}");
			}

			return sb.ToString();
		}

		public static bool Eval(RandoLocation location, List<RandoItem> obtained) {
			return Eval(location.LogicBase, obtained);
		}

		public static bool Eval(string postfix, List<RandoItem> obtained) {
			if (postfix == "") return true;		// We can always access this location
			if (obtained.Count == 0) return false;		// Location logic not blank, we have nothing; ergo we can't get to this location.

			//Predicate for items; options
			Predicate<string> predicate = sym => {
				postfix = postfix;
				if (sym.Contains(">")) {
					//Item comparators
					string[] split = sym.Split('>');
					int count = obtained.Where(v => v.Name == split[0]).Count();
					return count > int.Parse(split[1]);
				}

				if (obtained.Select(v => v.Name).Contains(sym)) {
					//Item
					return true;
				}

				if (RandoSettings.Options_bool.TryGetValue(sym, out bool optionVal)) {
					//Option
					return optionVal;
				}

				//Anything Else... Do we throw?

				//if (!validLogicSymbol()) {
				//	throw new LogicException($"Something ({sym}) unexpected was in the Logic. It's not a bird, it's not a plane, and it's definitely not Superman.");
				//} else {
					//We don't have this thing...
					return false;
				//}
			};

			bool result = Eval(postfix, predicate);
			return result;
		}

		public static bool Eval(string postfix, Predicate<string> eval) {
			if (postfix == "") {
				return true;
			}

			Stack<bool> stack = new Stack<bool>();
			string[] symbols = postfix.Split(' ');

			for (int i = 0; i < symbols.Length; i++) {
				switch (symbols[i]) {
					case RPN.AND:
						stack.Push(stack.Pop() & stack.Pop());
						break;
					case RPN.OR:
						stack.Push(stack.Pop() | stack.Pop());
						break;
					default:
						stack.Push(eval(symbols[i]));
						break;
				}
			}

			if (stack.Count != 1) {
				throw new LogicException($"Could not parse logic. Is it formatted correctly?");
			}

			return stack.Pop();
		}
	}

	[Serializable]
	internal class LogicException : Exception {
		public LogicException() {
		}

		public LogicException(string message) : base(message) {
		}

		public LogicException(string message, Exception innerException) : base(message, innerException) {
		}

		protected LogicException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}
