using System.Collections.Generic;

namespace Оболочка_ЭС
{
	class WorkMemory
	{
		public List<Fact> Facts = new List<Fact>();

		// Добавление новых фактов
		public void Add(List<Fact> facts_new)
		{
			foreach (var fact_new in facts_new)
			{
				// Если этого факта еще нет
				if (!HasFact(fact_new))
				{
					Facts.Add(fact_new);
				}
			}
		}

		// Наличие факта в рабочей памяти
		public bool HasFact(Fact fact_check)
		{
			foreach(var fact in Facts)
			{
				if (fact.Equals(fact_check))
					return true;
			}
			return false;
		}

		// Удаление фактов из рабочей памяти
		public void Delete(List<Fact> facts_old)
		{
			// Цикл по тем фактам, которые нужно удалить
			foreach (var fact_old in facts_old)
			{
				// Цикл по фактам, которые пока еще есть в Рабочей памяти
				// Индекс по убыванию, чтобы после удаления не перескочить один факт
				for (int i = Facts.Count - 1; i > -1; i--)
				{
					if (Facts[i].Equals(fact_old))
						Facts.RemoveAt(i);
				}
			}
		}
	}
}
