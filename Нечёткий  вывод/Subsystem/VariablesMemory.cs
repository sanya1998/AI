using System.Collections.Generic;

namespace Нечёткий__вывод
{
	public class VariablesMemory
	{
		// Переменные
		public List<Variable> Variables = new List<Variable>();

		// Добавление переменных и нечетких значений из текста
		public void Add(List<Variable> VariablesNew)
		{
			// Добавление новых переменных
			Variables.AddRange(VariablesNew);
		}
	}
}