namespace Оболочка_ЭС
{
	public class Fact
	{
		public Fact(string text)
		{
			Text = text;
		}

		public string Text;
		
		#region Сравнение фактов
		public override bool Equals(object obj)
		{
			Fact other = (Fact)obj;

			return other != null && Text == other.Text;
		}

		// Без этой функции ругается на переопределение Equals
		public override int GetHashCode()
		{
			// return 1249999374 + EqualityComparer<string>.Default.GetHashCode(Text);
			return 0;
		}
		#endregion
	}
}
