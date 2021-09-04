using System.Collections.Generic;

namespace Оболочка_ЭС
{
	class DecisionFinder
	{
		WorkMemory wm;
		KnowledgeBase kb;
		Explanationer expl = new Explanationer();

		// Компаратор для сравнения правил при сортировке
		AmountConditions ac = new AmountConditions();

		public DecisionFinder(WorkMemory wm, KnowledgeBase kb)
		{
			this.wm = wm;
			this.kb = kb;
		}

		public Explanationer Start()
		{
			List<Rule> ConflictSet = new List<Rule>();
			
			// Здесь начнется общий цикл, для которого условие выхода - пустой конфликтный набор или пустая база знаний
			for(int i = 0; kb.Rules.Count > 0; i ++)
			{
				ConflictSet.Clear();
				expl.Add("ЭТАП " + (i + 1) + "\nКоличество правил = " + kb.Rules.Count);
				// Цикл по правилам
				foreach (var rule in kb.Rules)
				{
					// Проверка все ли условия данного правила есть в рабочей памяти
					if (CheckRuleInWorkMemory(rule))
					{
						// Добавить правило в конфликтный набор
						ConflictSet.Add(rule);
					}
					else
					{
						// Если правило не будет выполняться
						expl.Add(rule, false);
					}
				}
				if(ConflictSet.Count == 0)
				{
					expl.Add("Конфликтный набор пуст");
					break;
				}
				else
				{
					// Отправка конфликтного набора в систему объяснений
					expl.Add(ConflictSet);
					// Сортировка правил по убыванию количества условий
					ConflictSet.Sort(ac);

					// Удалить из рабочей памяти факты-условия этого правила (???) надо ли после использования удалять?
					// wm.Delete(ConflictSet[0].Conditions);
					// Добавить в рабочую память факты-действия этого правила
					wm.Add(ConflictSet[0].Actions);

					// Правило будет выполняться
					expl.Add(ConflictSet[0], true);

					// Удалить использованное правило из базы знаний
					kb.Delete(ConflictSet[0]);
				}
			}

			return expl;
		}

		// Возвращает true, если все условия правила есть в рабочей памяти
		private bool CheckRuleInWorkMemory(Rule rule)
		{
			foreach(var condition in rule.Conditions)
			{
				if(wm.HasFact(condition))
					continue;
				else
					return false;
			}
			return true;
		}

		// Как сравнивать правила при сортировке
		class AmountConditions : IComparer<Rule>
		{
			public int Compare(Rule x, Rule y)
			{
				// Сравнение по количеству условий (Сортировка по убыванию)
				return y.Conditions.Count.CompareTo(x.Conditions.Count);
			}
		}
	}
}
