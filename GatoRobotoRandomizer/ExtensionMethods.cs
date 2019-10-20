using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatoRobotoRandomizer {
	static class ExtensionMethods {
		public static bool TryReplace(this string t, string oldValue, string newValue, out string returnString) {
			// This is incredibly stupid?
			if (t.Contains(oldValue)) {
				returnString = t.Replace(oldValue, newValue);
				return true;
			} else {
				returnString = t;
				return false;
			}
		}
	}
}
