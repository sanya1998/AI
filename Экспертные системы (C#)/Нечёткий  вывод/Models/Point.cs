using System;

namespace Нечёткий__вывод
{
	public class Point
	{
		public double X = 0.0;
		public double Y = 0.0;

		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return " (" + Math.Round(X, 3) + ";" + Math.Round(Y, 3) + ")";
		}
	}
}