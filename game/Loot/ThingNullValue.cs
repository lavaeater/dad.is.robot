namespace rds
{
	/// <summary>
	/// This is the default class for a "null" entry in an ThingTable.
	/// It just contains a value that is null (if added to a table of ThingValue objects),
	/// but is a class as well and can be checked via a "if (obj is ThingNullValue)..." construct
	/// </summary>
	public class ThingNullValue : ThingValue<object>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ThingNullValue"/> class.
		/// </summary>
		/// <param name="probability">The probability.</param>
		public ThingNullValue(double probability)
			: base(null, probability, false, false, true) { }
	}
}
