using System;
using System.Collections.Generic;

namespace Нечёткий__вывод
{
	public class ProcessingProcedure
	{
		private VariablesMemory vm;
		private RulesBase kb;
		private Explanationer expl;

		public ProcessingProcedure(ref VariablesMemory vm, ref RulesBase kb, ref Explanationer expl)
		{
			this.vm = vm;
			this.kb = kb;
			this.expl = expl;
		}

		// Главная функция
		public void Start()
		{
			// Цикл по правилам
			foreach (var rule in kb.Rules)
			{
				// ЭТАП 1

				// rule.Condition - левая часть правила (условие)
				// rule.Condition.Key - переменная из условия
				// rule.Condition.Key.Name - имя переменной из условия
				// rule.Condition.Key.Value - double-значение переменной из условия
				// rule.Condition.Value - нечеткое значение из условия
				// rule.Condition.Value.Name - имя нечеткого значения из условия

				// Значение функции принадлежности (истинности) для нечеткого множества
				double confidence = TruthFunction(rule.Condition.Key.Value, rule.Condition.Value);

				// ЭТАП 2

				// rule.Action - правая часть правила (действие)
				// rule.Action.Key - переменная из действия
				// rule.Action.Key.Name - имя переменной из действия
				// rule.Action.Key.Value - double-значение переменной из действия
				// rule.Action.Value - нечеткое значение из действия
				// rule.Action.Value.Name - имя нечеткого значения из действия
				// rule.Action.Value.Koef - коэффициент функции истинности

				// Модификация нечёткого значения в правой части правила (в действии)
				rule.Action.Value.Koef = confidence;
				// Пометка о том, что переменная является выходной
				rule.Action.Key.IsOutput = true;

				expl.Add(rule.Condition.Key.Name + " " + rule.Condition.Value.Name + " - " + confidence + " - " + rule.Action.Key.Name + " " + rule.Action.Value.Name);
			
				}

			// ЭТАП 3

			// Объединение (суперпозиция) модифицированных множеств
			// Получившиеся фигуры разбиваются на треугольники (в этом списке будут треугольники от всех выходных переменных)
			List<Triangle> Triangles = SplitOnTriangles();

			// Убрать треугольники, у которых площадь = 0
			Triangles.RemoveAll((x) => x.Square == 0);

			// Треугольники в систему объяснений
			expl.Add(Triangles);

			// ЭТАП 4
			// Скаляризация результата суперпозиции – переход от НМ к скалярным значениям.

			// Цикл по всем выходным переменным
			foreach (var variable in vm.Variables.FindAll((x)=>x.IsOutput) )
			{
				// Центр тяжести всей фигуры (параметром являются только те треугольники, у которых имя совпадает с именем рассматриваемой переменной)
				Point GravityCenter = calcGravityCenter(Triangles.FindAll((x) => x.VarName == variable.Name));

				expl.Add("ЦТ фигуры (" + variable.Name + ")" + GravityCenter.ToString());

				// Присваиваем значение выхоной переменной
				variable.Value = GravityCenter.X;

				expl.Add(variable.Name + " = " + Math.Round(variable.Value, 3));
			}
		}


		#region Вспомогательные функции
		// Деление на треугольники
		private List<Triangle> SplitOnTriangles()
		{
			List<Triangle> Triangles = new List<Triangle>();
			// Цикл по всем выходным переменным
			foreach (var variable in vm.Variables.FindAll((x) => x.IsOutput))
			{
				// Левая вершина самого левого треугольника. Затем она будет левой вершиной и верхнего и нижнего треугольника
				Point left_vert = new Point(variable.FuzzyValues[0].Begin, 0);

				// Серединная вершина нижнего треугольника (у первого нижнего треугольника площадь точно будет нулевой). 
				// Если треугольники НЗ не будут пересекаться, то на самом деле она будет левой, но вычислениям это не помешает
				Point middle_vert_down = new Point(variable.FuzzyValues[0].Middle, 0);

				for (int i = 0; i < variable.FuzzyValues.Count; i++)
				{
					// Чтобы меньше писать
					FuzzyValue fv_cur = variable.FuzzyValues[i];

					// Верхний треугольник есть всегда
					// Серединная вершина верхнего треугольника
					Point middle_vert_up = new Point(fv_cur.Middle, fv_cur.Koef);
					// Правая вершина и верхнего, и нижнего треугольника
					Point right_vert = new Point(fv_cur.End, 0);

					Triangle triangle_up = new Triangle(left_vert, middle_vert_up, right_vert, variable.Name);
					// Добавление верхнего треугольника в список треугольников
					Triangles.Add(triangle_up);

					// Нижний треугольник есть не всегда, но он не помешает (если что, у него просто площадь будет равна нулю)
					Triangle triangle_down = new Triangle(left_vert, middle_vert_down, right_vert, variable.Name);
					// Добавление нижнего треугольника в список треугольников
					Triangles.Add(triangle_down);

					// Серединная вершина нижнего треугольника переходит в правую вершину верхнего треугольника
					middle_vert_down = new Point(right_vert.X, right_vert.Y);

					// Если это не последний треугольник НЗ
					if (i < variable.FuzzyValues.Count - 1)
					{
						// Необходимо пересчитать левую вершину верхнего и нижнего треугольника
						// Если треугольники НЗ не пересекаются в нужной зоне, то она будет просто левой вершиной следующего треугольника НЗ
						// Иначе она будет точкой пересечения ребер треугольников НЗ

						//.. найти уравнения прямых
						FindLineEquation(out double k1, out double m1, fv_cur.Middle, fv_cur.Koef, fv_cur.End, 0);

						// Чтобы меньше писать
						FuzzyValue fv_next = variable.FuzzyValues[i + 1];
						FindLineEquation(out double k2, out double m2, fv_next.Begin, 0, fv_next.Middle, fv_next.Koef);

						// Точка пересечения двух прямых
						Point IntersectionPoint = FindIntersectionPoint(k1, m1, k2, m2);

						// если она ниже 0, значит не пересекаются в нужной зоне
						if (IntersectionPoint.Y < 0)
							left_vert = new Point(fv_next.Begin, 0);
						// если между нулем и верхней точкой, значит пересекаются
						else if (IntersectionPoint.Y <= fv_cur.Koef)
							left_vert = IntersectionPoint;
						

						// Серединная вершина следующего нижнего треугольнка - правая вершина текущих треугольников
						middle_vert_down = right_vert;
					}
				}
			}
			return Triangles;
		}

		// Функция истинности
		private double TruthFunction(double val, FuzzyValue fV)
		{
			// Уверенность
			double confidence;

			// Если левее начала или правее конца
			if (val <= fV.Begin || val >= fV.End)
				confidence = 0;
			// Между началом и серединой
			else if(val < fV.Middle)
			{
				FindLineEquation(out double k, out double m, fV.Begin, 0, fV.Middle, fV.Koef);
				confidence = k * val + m;
			}
			// Между серединой и концом
			else
			{
				FindLineEquation(out double k, out double m, fV.Middle, fV.Koef, fV.End, 0);
				confidence = k * val + m;
			}

			return Math.Round(confidence, 3);
		}
		#endregion
		#region Геометрия
		// Найти уравнение прямой по двум точкам
		public void FindLineEquation(out double k, out double m, double x1, double y1, double x2, double y2)
		{			
			if(x1 != x2)
			{
				k = (y1 - y2) / (x1 - x2);
				m = y2 - k * x2;
			}
			else
			{// прямая вертикальная прямая..
				k = Double.MaxValue;
				m = Double.MinValue;
			}
		}

		// Точка пересечения двух прямых
		private Point FindIntersectionPoint(double k1, double m1, double k2, double m2)
		{
			double x = (m2 - m1) / (k1 - k2);
			return new Point(x, k1 * x + m1);
		}

		// Нахождение центра тяжести общей фигуры
		private Point calcGravityCenter(List<Triangle> Triangles)
		{
			// Числители
			double Numerator_x = 0.0;
			double Numerator_y = 0.0;

			// Знаменатели
			double Denominator_x_y = 0.0;

			foreach (var triangle in Triangles)
			{
				Numerator_x += triangle.GravityCenter.X * triangle.Square;
				Numerator_y += triangle.GravityCenter.Y * triangle.Square;

				Denominator_x_y += triangle.Square;
			}

			// Если знаменатель не равен нулю
			if (Denominator_x_y != 0)
			{
				double X = Numerator_x / Denominator_x_y;
				double Y = Numerator_y / Denominator_x_y;
				return new Point(X, Y);
			}
			else
			{
				return new Point(Double.MaxValue, Double.MaxValue);
			}
		}
		#endregion
	}
}