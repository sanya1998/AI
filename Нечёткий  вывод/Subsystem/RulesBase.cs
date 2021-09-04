using System.Collections.Generic;

namespace Нечёткий__вывод
{
	public class RulesBase
	{
		public List<Rule> Rules = new List<Rule>();

		public void Add(List<Rule> RulesNew)
		{
			// Добавление новых
			Rules.AddRange(RulesNew);
		}

	}
}