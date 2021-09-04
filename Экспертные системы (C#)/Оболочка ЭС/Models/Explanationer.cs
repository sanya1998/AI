using System;
using System.Collections.Generic;
using System.Text;

namespace Оболочка_ЭС
{
	class Explanationer
	{
		StringBuilder explanation = new StringBuilder("");

		public void Add(string text)
		{
			explanation.Append(text + "\n");
		}
		// Информация о правиле. (Used==true) => правило выполняется
		public void Add(Rule rule, bool used)
		{
			if(used)
			{
				Add("Выполнить правило " + rule.Id);

				/*
				// Перечислить факты, которые удалятся при выполнении правила
				if (rule.Conditions.Count > 0)
				{
					Add("Удаление из рабочей памяти:");
					foreach (var cond in rule.Conditions)
					{
						Add("- " + cond.Text);
					}
				}*/

				// Перечислить факты, которые добавятся при выполнении правила
				if (rule.Actions.Count > 0)
				{
					Add("Добавление в рабочую память:");
					foreach (var act in rule.Actions)
					{
						Add("+ " + act.Text);
					}
				}
			}
			else
			{
				Add("Для правила " + rule.Id + " не хватает условий");
			}
		}

		public void Add(List<Rule> conflictSet)
		{
			string str = "Конфликтный набор:";
			foreach(var rule in conflictSet)
			{
				str += " " + rule.Id;
			}
			Add(str);
		}
		public override string ToString()
		{
			return explanation.ToString();
		}
	}
}
