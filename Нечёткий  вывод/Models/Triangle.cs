using System;

namespace Нечёткий__вывод
{
	public class Triangle
	{
		public string VarName;

		// Вершины треугольника
		public Point V1;
		public Point V2;
		public Point V3;

		// Центр тяжести
		public Point GravityCenter;

		// Площадь
		public double Square;

		public Triangle(Point v1, Point v2, Point v3, string varName)
		{
			VarName = varName;

			V1 = v1;
			V2 = v2;
			V3 = v3;

			Square = CalcSquare(V1, V2, V3);
			GravityCenter = CalcGravityCenter(V1, V2, V3);
		}

		private Point CalcGravityCenter(Point v1, Point v2, Point v3)
		{
			return new Point((v1.X + v2.X + v3.X) / 3, (v1.Y + v2.Y + v3.Y) / 3);
		}

		private double CalcSquare(Point v1, Point v2, Point v3)
		{
			var det = (v1.X - v3.X) * (v2.Y - v3.Y) - (v2.X - v3.X) * (v1.Y - v3.Y);
			return Math.Abs(0.5 * det);
		}

		public override string ToString()
		{
			return VarName + " " + V1.ToString() + V2.ToString() + V3.ToString() +
							" S=" + Math.Round(Square, 3) + 
							" ЦТ" + GravityCenter.ToString();
		}
	}
}