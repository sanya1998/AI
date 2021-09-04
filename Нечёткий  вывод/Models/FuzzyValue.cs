namespace Нечёткий__вывод
{
	public class FuzzyValue
	{
		public string Name;

		// Совпадает со значением в серединной точке
		public double Koef = 1;

		public double Begin;
		public double Middle;
		public double End;

		public FuzzyValue(string text, double begin, double middle, double end)
		{
			Name = text;
			Begin = begin;
			Middle = middle;
			End = end;
		}
	}
}
