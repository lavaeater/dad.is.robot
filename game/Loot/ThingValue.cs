using System;
using Newtonsoft.Json;

namespace rds
{
	/// <summary>
	/// This class holds a single RDS value.
	/// It's a generic class to allow the developer to add any type to a ThingTable.
	/// T can of course be either a value type or a reference type, so it's possible,
	/// to add ThingValue objects that contain a reference type, too.
	/// </summary>
	/// <typeparam name="T">The type of the value</typeparam>
	public class ThingValue<T> : IThingValue<T>
	{
		
		#region CONSTRUCTOR
		/// <summary>
		/// Initializes a new instance of the <see cref="ThingValue{T}"/> class.
		/// The Unique and Always flags are set to (default) false with this constructor, and Enabled is set to true.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="probability">The probability.</param>
		public ThingValue(T value, double probability)
			: this(value, probability, false, false, true)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ThingValue{T}"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="probability">The probability.</param>
		/// <param name="unique">if set to <c>true</c> [unique].</param>
		/// <param name="always">if set to <c>true</c> [always].</param>
		/// <param name="enabled">if set to <c>true</c> [enabled].</param>
		public ThingValue(T value, double probability, bool unique, bool always, bool enabled)
		{
			mvalue = value;
			Probability = probability;
			Unique = unique;
			Always = always;
			Enabled = enabled;
			Table = null;
		}
		#endregion

		#region EVENTS
		/// <summary>
		/// Occurs before all the probabilities of all items of the current ThingTable are summed up together.
		/// This is the moment to modify any settings immediately before a result is calculated.
		/// </summary>
		public event EventHandler rdsPreResultEvaluation;
		/// <summary>
		/// Occurs when this Thing has been hit by the Result procedure.
		/// (This means, this object will be part of the result set).
		/// </summary>
		public event EventHandler rdsHit;
		/// <summary>
		/// Occurs after the result has been calculated and the result set is complete, but before
		/// the ThingTable's Result method exits.
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

		#region IThingValue<T> Members
		/// <summary>
		/// The value of this object
		/// </summary>
		public virtual T Value 
		{
			get { return mvalue; }
			set { mvalue = value; }
		}
		private T mvalue;
		#endregion

		#region IThing Members
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
		/// Gets or sets a value indicating whether this <see cref="IThing"/> is enabled.
		/// Only enabled entries can be part of the result of a ThingTable.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; set; }
		/// <summary>
		/// Gets or sets the table this Object belongs to.
		/// Note to inheritors: This property has to be auto-set when an item is added to a table via the AddEntry method.
		/// </summary>
		[JsonIgnore]
		public ThingTable Table { get; set; }
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
			
			string valstr = "(null)";
			if (Value != null)
				valstr = Value.ToString();
				return string.Format(indent + "(ThingValue){0} \"{1}\",Prob:{2},UAE:{3}{4}{5}", 
					this.GetType().Name, valstr, Probability,
					(Unique ? "1" : "0"), (Always ? "1" : "0"), (Enabled ? "1" : "0"));
		}
		#endregion
	}
}
