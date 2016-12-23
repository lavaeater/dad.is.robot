using System;
using System.Collections.Generic;

namespace rds
{
	#region ILootObject
	/// <summary>
	/// This interface contains the properties an object must have to be a valid rds result object.
	/// </summary>
	public interface ILootObject
	{
		#region FIELDS
		/// <summary>
		/// Gets or sets the probability for this object to be (part of) the result
		/// </summary>
		double Probability { get; set; }

		/// <summary>
		/// Gets or sets whether this object may be contained only once in the result set
		/// </summary>
		bool Unique { get; set; }

		/// <summary>
		/// Gets or sets whether this object will always be part of the result set
		/// (Probability is ignored when this flag is set to true)
		/// </summary>
		bool Always { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ILootObject"/> is enabled.
		/// Only enabled entries can be part of the result of a LootTable.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the table this Object belongs to.
		/// Note to inheritors: This property has to be auto-set when an item is added to a table via the AddEntry method.
		/// </summary>
		LootTable Table { get; set; }
		#endregion

		#region EVENTS
		/// <summary>
		/// Occurs before all the probabilities of all items of the current LootTable are summed up together.
		/// This is the moment to modify any settings immediately before a result is calculated.
		/// </summary>
		event EventHandler rdsPreResultEvaluation;
		/// <summary>
		/// Occurs when this LootObject has been hit by the Result procedure.
		/// (This means, this object will be part of the result set).
		/// </summary>
		event EventHandler rdsHit;
		/// <summary>
		/// Occurs after the result has been calculated and the result set is complete, but before
		/// the LootTable's Result method exits.
		/// </summary>
		event ResultEventHandler rdsPostResultEvaluation;

		/// <summary>
		/// Raises the <see cref="E:PreResultEvaluation"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void OnRDSPreResultEvaluation(EventArgs e);
		/// <summary>
		/// Raises the <see cref="E:Hit"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void OnRDSHit(EventArgs e);
		/// <summary>
		/// Raises the <see cref="E:PostResultEvaluation"/> event.
		/// </summary>
		/// <param name="e">The <see cref="rds.ResultEventArgs"/> instance containing the event data.</param>
		void OnRDSPostResultEvaluation(ResultEventArgs e);
		#endregion

		#region TOSTRING WITH INDENTATION
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="indentationlevel">The indentationlevel. 4 blanks at the beginning of each line for each level.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		string ToString(int indentationlevel);
		#endregion
	}
	#endregion

	#region LootObjectCreator
	/// <summary>
	/// This interface holds a method that creates an instance of an object where it is implemented.
	/// If an object gets hit by RDS, it checks whether it is an ORDSObjectCreator. If yes, the result
	/// of .CreateInstance() is added to the result; if not, the object itself is returned.
	/// </summary>
	public interface LootObjectCreator : ILootObject
	{
		/// <summary>
		/// Creates an instance of the object where this method is implemented in.
		/// Only paramaterless constructors are supported in the base implementation.
		/// Override (without calling base.CreateInstance()) to instanciate more complex constructors.
		/// </summary>
		/// <returns>A new instance of an object of the type where this method is implemented</returns>
		ILootObject CreateInstance();
	}
	#endregion

	#region ILootTable
	/// <summary>
	/// This interface describes a table of IRDSObjects. One (or more) of them is/are picked as the result set.
	/// </summary>
	public interface ILootTable : ILootObject
	{
		/// <summary>
		/// The maximum number of entries expected in the Result. The final count of items in the result may be lower
		/// if some of the entries may return a null result (no drop)
		/// </summary>
		int rdsCount { get; set; }

		/// <summary>
		/// Gets or sets the contents of this table
		/// </summary>
		IEnumerable<ILootObject> rdsContents { get; }

		/// <summary>
		/// Gets the result. Calling this method will start the random pick process and generate the result.
		/// This result remains constant for the lifetime of this table object.
		/// Use the ResetResult method to clear the result and create a new one.
		/// </summary>
		IEnumerable<ILootObject> rdsResult { get; }
	}
	#endregion

	#region ILOOTVALUE<T>
	/// <summary>
	/// Generic template for classes that return only one value, which will be good enough in most use cases.
	/// </summary>
	/// <typeparam name="T">The type of the value of this object</typeparam>
	public interface ILootValue<T> : ILootObject
	{
		/// <summary>
		/// The value of this object
		/// </summary>
		T Value { get; set; }
	}
	#endregion
}
