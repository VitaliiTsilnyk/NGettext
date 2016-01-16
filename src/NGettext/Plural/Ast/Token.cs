using System;

namespace NGettext.Plural.Ast
{
	/// <summary>
	/// Represents a node in the abstract syntax tree.
	/// </summary>
    public class Token
    {
		public const int MAX_CHILDREN_COUNT = 3;

		/// <summary>
		/// Gets the type of the current token.
		/// </summary>
		public TokenType Type { get; protected set; }

		/// <summary>
		/// Gets or sets an optional value associated with this token.
		/// </summary>
		public long Value { get; set; }

		/// <summary>
		/// Gets token children.
		/// </summary>
		public readonly Token[] Children = new Token[MAX_CHILDREN_COUNT];

		/// <summary>
		/// Initializes a new instance of the <see cref="Token"/> class with given type and (optional) value.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public Token(TokenType type, long value = 0)
		{
			this.Type = type;
			this.Value = value;
		}
    }
}
