namespace rds
{
	/// <summary>
	/// This is the default class for a "null" entry in an LootTable.
	/// It just contains a value that is null (if added to a table of LootValue objects),
	/// but is a class as well and can be checked via a "if (obj is LootNullValue)..." construct
	/// </summary>
	public class LootNullValue : LootValue<object>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LootNullValue"/> class.
		/// </summary>
		/// <param name="probability">The probability.</param>
		public LootNullValue(double probability)
			: base(null, probability, false, false, true) { }
	}
}
