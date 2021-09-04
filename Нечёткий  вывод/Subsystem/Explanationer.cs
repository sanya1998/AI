using System.Collections.Generic;
using System.Text;

namespace Нечёткий__вывод
{
	public class Explanationer
	{
		StringBuilder sb = new StringBuilder("");

		// Вывод
		public override string ToString()
		{
			return sb.ToString();
		}

		// Добавление текста
		public void Add(string text)
		{
			sb.Append(text + "\n");
		}

		public void Add(List<Triangle> Triangles)
		{
			Add("Получившиеся треугольники:");
			int chet = 0;
			foreach(var triangle in Triangles)
				Add(chet++ + ") " + triangle.ToString());
		}

	}
}