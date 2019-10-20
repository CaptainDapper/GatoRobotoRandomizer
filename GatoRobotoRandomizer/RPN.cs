using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatoRobotoRandomizer {
	public static class RPN {
		internal const string AND = "&";
		internal const string OR = "|";

		public static string Parse(string fix, bool toPostfix = true) {
			if (toPostfix) {
				return infixToPostfix(fix);
			} else {
				return postfixToInfix(fix);
			}
		}





















		private static string getNextOperator(string strValue, ref int i) {
			int start = i;

			if (strValue[i] == '(' || strValue[i] == ')' || strValue[i].ToString() == AND || strValue[i].ToString() == OR) {
				i++;
				return strValue[i - 1].ToString();
			}

			while (i < strValue.Length && strValue[i] != '(' && strValue[i] != ')' && strValue[i].ToString() != AND && strValue[i].ToString() != OR) {
				i++;
			}

			return strValue.Substring(start, i - start).Trim(' ');
		}

		private static string infixToPostfix(string infix) {
			int i = 0;
			Stack<string> stack = new Stack<string>();
			List<string> postfix = new List<string>();

			while (i < infix.Length) {
				string op = getNextOperator(infix, ref i);

				// Easiest way to deal with whitespace between operators
				if (op.Trim(' ') == string.Empty) {
					continue;
				}

				// Order of Operations:  "&" > "|"
				if (op == AND || op == OR) {
					while (
						stack.Count != 0 && (
							op == OR || (
								op == AND &&
								stack.Peek() != OR
							)
						) &&
						stack.Peek() != "("
					) {
						postfix.Add(stack.Pop());
					}

					stack.Push(op);
				} else if (op == "(") {
					stack.Push(op);
				} else if (op == ")") {
					while (stack.Peek() != "(") {
						postfix.Add(stack.Pop());
					}

					stack.Pop();
				} else {
					postfix.Add(op);
				}
			}

			while (stack.Count != 0) {
				postfix.Add(stack.Pop());
			}

			return string.Join(" ", postfix.ToArray());
		}
		private static string getNextToken(string strValue, ref int i) {
			int start = i;
			//Assume we start on first char of new token

			while (i < strValue.Length) {
				//Console.WriteLine("TOKEN: " + i + " " + strValue[i]);
				if (strValue[i] == ' ') {
					//Found our first space, the rest of this must be the token...
					break;
				}
				i++;
			}

			int tokenLength = i - start;
			string retVal = strValue.Substring(start, tokenLength);
			//Console.WriteLine("NEW TOKEN: **" + retVal + "**");
			return retVal;
		}

		private static string postfixToInfix(string postfix) {
			if (postfix == "") return "";
			Stack<string> stack = new Stack<string>();

			postfix = postfix.Trim();
			for (int i = 0; i < postfix.Length; i++) {
				string token = getNextToken(postfix, ref i);
				//Console.WriteLine(i + " Sym: " + token);

				if (token == AND || token == OR) {
					string arg1 = stack.Pop();
					string arg2 = stack.Pop();
					stack.Push("( " + arg1 + " " + token + " " + arg2 + " )");
				} else {
					stack.Push(token);
				}

				//Console.WriteLine(i + " Stack: " + string.Join(" ", stack.ToArray()));
			}
			List<string> infixList = stack.Pop().Split(' ').ToList();

			//Console.WriteLine("POSTFIX NO TRIM: " + string.Join(" ", infixList));

			string infix = string.Join(" ", TrimParentheses(infixList));
			infix = infix.Replace("( ", "(").Replace(" )", ")");

			string dumbSimp = DumbSimplify(string.Join(" ", infix));
			if (dumbSimp != infix) {
				Console.WriteLine("INFIX: " + infix);
				Console.WriteLine("SIMPFIX: " + dumbSimp);
			}
			infix = dumbSimp;

			return string.Join(" ", infix);
		}

		private static int getMatchingParens(string @in, int start) {
			int count = 0;
			for (int i = start; i < @in.Length; i++) {
				if (@in[i] == '(') {
					count++;
					continue;
				}

				if (@in[i] == ')') {
					count--;
					if (count == 0) {
						return i;
					}
					continue;
				}

				continue;
			}

			throw new Exception("Couldn't find a matching parens!");
		}

		private static string DumbSimplify(string v) {
			// Tokenize all parentheticals
			List<string> tokens = new List<string>();
			while (v.Contains("(")) {
				int start = v.IndexOf('(');
				int end = getMatchingParens(v, start);
				string newToken = v.Substring(start, 1 + end - start);
				v = v.Replace(newToken, $"DS_TOKEN#{tokens.Count}");
				tokens.Add(newToken.Substring(1, newToken.Length-2));
			}

			// Split along -or- boundaries and trim
			List<string> OrTerms = v.Split('|').ToList();

			// Get distinct &s
			List<string> newOrs = new List<string>();
			foreach (string term in OrTerms) {
				List<string> distinct = term.Replace(" ","").Split('&').Distinct().ToList();

				// Recurse the Tokens
				for (int i = 0; i < distinct.Count; i++) {
					if (distinct[i].StartsWith("DS_TOKEN")) {
						distinct[i] = "(" + DumbSimplify(tokens[int.Parse(distinct[i].Split('#')[1])]) + ")";
					}
				}

				newOrs.Add(string.Join(" & ", distinct));
			}

			return string.Join(" | ", newOrs);
		}

		private static List<string> TrimParentheses(List<string> infix) {
			Func<List<string>, int, int> getMatchingParens = (@in, start) => {
				int count = 0;
				for (int i = start; i < @in.Count; i++) {
					if (@in[i] == "(") {
						count++;
						continue;
					}

					if (@in[i] == ")") {
						count--;
						if (count == 0) {
							return i;
						}
						continue;
					}

					continue;
				}

				throw new Exception("Couldn't find a matching parens!");
			};

			Func<List<string>, int, bool, string> findNearestOp = (prmIn, prmStart, prmForward) => {
				int i = prmStart;
				while (true) {
					if (prmForward) {
						i++;
						if (i >= prmIn.Count) {
							return string.Empty;
						}
					} else {
						i--;
						if (i < 0) {
							return string.Empty;
						}
					}

					string sym = prmIn[i];
					if (sym == "(" || sym == ")") {
						return string.Empty;
					}

					if (sym == AND || sym == OR) {
						return prmIn[i];
					}
				}
			};

			Func<string, int> getOpPriority = (sym) => {
				switch (sym) {
					case "(":
					case ")":
						return 0;
					case AND:
						return 2;
					case OR:
						return 1;
					default:
						return -1;
				}
			};

			Func<string, string, string, bool> isRedundant = (pL, pLow, pR) => {
				int lowPri = getOpPriority(pLow);
				if (lowPri < getOpPriority(pL) || lowPri < getOpPriority(pR)) {
					return false;
				}
				return true;
			};

			List<string> newInfix = new List<string>();

			for (int i = 0; i < infix.Count; i++) {
				//Console.WriteLine(string.Join(" ", infix.ToArray()));

				if (infix[i] != "(") {
					newInfix.Add(infix[i]);
					continue;
				}

				int j = getMatchingParens(infix, i);

				string L = findNearestOp(infix, i, false);
				string low = string.Empty;
				if (i + 1 != j) {
					for (int k = i + 1; k < j; k++) {
						string sym = infix[k];
						if (sym == "(") {
							k = getMatchingParens(infix, k);
							continue;
						}
						if (sym == OR || sym == AND) {
							if (sym == OR) {
								low = sym;
								break;  //Lowest priority op possible...
							} else if (sym == AND && low == string.Empty) {
								low = sym;
								continue;
							}
						}
					}
				}
				string R = findNearestOp(infix, j, true);

				//Console.WriteLine(L + "-" + i + "-" + low + "-" + j + "-" + R);

				if (isRedundant(L, low, R)) {
					//We can remove these parentheses.
					infix.RemoveAt(j);
					infix.RemoveAt(i);
					i--;
					continue;
				}
			}

			return infix;
		}
	}
}
