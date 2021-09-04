using System;
using System.Collections.Generic;

namespace Оболочка_ЭС
{
	class KnowledgeBase
	{
		public List<Rule> Rules = new List<Rule>();

		public void Add(List<Rule> rules_new)
		{
			if(rules_new != null)
				Rules.AddRange(rules_new);
		}

		internal void Delete(Rule rule_del)
		{
			// Индекс по убыванию, чтобы после удаления не перескочить один факт
			for (int i = Rules.Count - 1; i > -1; i--)
			{
				if (Rules[i].Equals(rule_del))
					Rules.RemoveAt(i);
			}
		}
	}
}
