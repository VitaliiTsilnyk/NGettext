using System;

namespace NGettext.Plural
{
	/// <summary>
	/// Default plural rule implementation whitch uses a evaluation delegate instance to evaluate a plural form.
	/// </summary>
	public class PluralRule : IPluralRule
	{
		/// <summary>
		/// Evaluation delegate instance.
		/// </summary>
		protected PluralRuleEvaluatorDelegate EvaluatorDelegate;

		/// <summary>
		/// Maximum number of plural forms supported.
		/// </summary>
		public int NumPlurals { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PluralRule"/> class using specified maximum plural
		/// forms value and an evaluation delegate.
		/// </summary>
		/// <param name="numPlurals"></param>
		/// <param name="evaluatorDelegate"></param>
		public PluralRule(int numPlurals, PluralRuleEvaluatorDelegate evaluatorDelegate)
		{
			if (numPlurals <= 0)
			{
				throw new ArgumentOutOfRangeException("numPlurals");
			}
			if (evaluatorDelegate == null)
			{
				throw new ArgumentNullException("evaluatorDelegate");
			}

			this.NumPlurals = numPlurals;
			this.EvaluatorDelegate = evaluatorDelegate;
		}

		/// <summary>
		/// Evaluates a number and returns a plural form index.
		/// </summary>
		/// <param name="number">Number whitch needs to be evaluated.</param>
		/// <returns>Plural form index.</returns>
		public virtual int Evaluate(long number)
		{
			return this.EvaluatorDelegate.Invoke(number);
		}
	}
}