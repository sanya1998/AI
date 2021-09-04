using System;
using System.Collections.Generic;

namespace Нечёткий__вывод
{
	public class Variable
	{
		public string Name;
		public double Value = 0;

		// Является ли выходной переменной
		public bool IsOutput = false;

		public List<FuzzyValue> FuzzyValues = new List<FuzzyValue>();

		public Variable(string name, double value, List<FuzzyValue> fuzzyValues)
		{
			Name = name;
			Value = value;
			FuzzyValues = fuzzyValues;
		}

		// Поиск самого правого значения
		public double FindMaximumX()
		{
			double max = Double.MinValue;
			foreach (var fuzzy in FuzzyValues)
				if (fuzzy.End > max)
					max = fuzzy.End;
			return max;
		}
		// Поиск самого левого значения
		public double FindMinimumX()
		{
			double min = Double.MaxValue;
			foreach (var fuzzy in FuzzyValues)
				if (fuzzy.Begin < min)
					min = fuzzy.Begin;
			return min;
		}

		// Поиск максимума функции истинности для всех нечетких значений
		public double FindMaximumY()
		{
			double max = Double.MinValue;
			foreach(var fuzzy in FuzzyValues)
				if (fuzzy.Koef > max)
					max = fuzzy.Koef;
			return max;
		}
	}
}