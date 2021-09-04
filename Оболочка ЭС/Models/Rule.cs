using System.Collections.Generic;

namespace Оболочка_ЭС
{
	public class Rule
	{
		public int Id;
		public List<Fact> Conditions;
		public List<Fact> Actions;

		public Rule(int id, List<Fact> conditions, List<Fact> actions)
		{
			Id = id;
			Conditions = conditions;
			Actions = actions;
		}

		#region Сравнение правил по Id
		public override bool Equals(object obj)
		{
			Rule other = (Rule)obj;

			return other != null && Id == other.Id;
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
