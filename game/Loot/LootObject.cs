using System;

namespace rds
{
	/// <summary>
	/// Base implementation of the ILootObject interface.
	/// This class only implements the interface and provides all events required.
	/// Most methods are virtual and ready to be overwritten. Unless there is a good reason,
	/// do not implement ILootObject for yourself, instead derive your base classes that shall interact
	/// in *any* thinkable way as a result source with any LootTable from this class.
	/// </summary>
	public class LootObject : ILootObject
	{
		#region CONSTRUCTORS
		/// <summary>
		/// Initializes a new instance of the <see cref="LootObject"/> class.
		/// </summary>
		public LootObject()
			: this(0)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="LootObject"/> class.
		/// </summary>
		/// <param name="probability">The probability.</param>
		public LootObject(double probability)
			: this(probability, false, false, true) 
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="LootObject"/> class.
		/// </summary>
		/// <param name="probability">The probability.</param>
		/// <param name="unique">if set to <c>true</c> this object can only occur once per result.</param>
		/// <param name="always">if set to <c>true</c> [always] this object will appear always in the result.</param>
		/// <param name="enabled">if set to <c>false</c> [enabled] this object will never be part of the result (even if it is set to always=true!).</param>
		public LootObject(double probability, bool unique, bool always, bool enabled)
		{
			Probability = probability;
			Unique = unique;
			Always = always;
			Enabled = enabled;
			Table = null;
		}
		#endregion

		#region EVENTS
		/// <summary>
		/// Occurs before all the probabilities of all items of the current LootTable are summed up together.
		/// This is the moment to modify any settings immediately before a result is calculated.
		/// </summary>
		public event EventHandler rdsPreResultEvaluation;
		/// <summary>
		/// Occurs when this LootObject has been hit by the Result procedure.
		/// (This means, this object will be part of the result set).
		/// </summary>
		public event EventHandler rdsHit;
		/// <summary>
		/// Occurs after the result has been calculated and the result set is complete, but before
		/// the LootTable's Result method exits.
		/// </summary>
		public event ResultEventHandler rdsPostResultEvaluation;

		/// <summary>
		/// Raises the <see cref="E:PreResultEvaluation"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		public virtual void OnRDSPreResultEvaluation(EventArgs e)
		{
			if (rdsPreResultEvaluation != null) rdsPreResultEvaluation(this, e);
		}
		/// <summary>
		/// Raises the <see cref="E:Hit"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		public virtual void OnRDSHit(EventArgs e)
		{
			if (rdsHit != null) rdsHit(this, e);
		}
		/// <summary>
		/// Raises the <see cref="E:PostResultEvaluation"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		public virtual void OnRDSPostResultEvaluation(ResultEventArgs e)
		{
			if (rdsPostResultEvaluation != null) rdsPostResultEvaluation(this, e);
		}
		#endregion

		#region ILootObject Members
		/// <summary>
		/// Gets or sets the probability for this object to be (part of) the result
		/// </summary>
		public double Probability { get; set; }
		/// <summary>
		/// Gets or sets whether this object may be contained only once in the result set
		/// </summary>
		public bool Unique { get; set; }
		/// <summary>
		/// Gets or sets whether this object will always be part of the result set
		/// (Probability is ignored when this flag is set to true)
		/// </summary>
		public bool Always { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ILootObject"/> is enabled.
		/// Only enabled entries can be part of the result of a LootTable.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; set; }
		/// <summary>
		/// Gets or sets the table this Object belongs to.
		/// Note to inheritors: This property has to be auto-set when an item is added to a table via the AddEntry method.
		/// </summary>
		public LootTable Table { get; set; }
		#endregion

		#region TOSTRING
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return ToString(0);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public string ToString(int indentationlevel)
		{
			string indent = "".PadRight(4 * indentationlevel, ' ');

			return string.Format(indent + "(LootObject){0} Prob:{1},UAE:{2}{3}{4}",
				this.GetType().Name, Probability,
				(Unique ? "1" : "0"), (Always ? "1" : "0"), (Enabled ? "1" : "0"));
		}
		#endregion
	}
}
