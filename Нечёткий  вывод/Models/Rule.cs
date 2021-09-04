using System.Collections.Generic;

namespace Нечёткий__вывод
{
	public class Rule
	{
		public KeyValuePair<Variable, FuzzyValue> Condition = new KeyValuePair<Variable, FuzzyValue>();
		public KeyValuePair<Variable, FuzzyValue> Action = new KeyValuePair<Variable, FuzzyValue>();

		public int Id;

		public Rule(int id, KeyValuePair<Variable, FuzzyValue> conditions, KeyValuePair<Variable, FuzzyValue> actions)
		{
			Id = id;
			Condition = conditions;
			Action = actions;
		}
	}
}