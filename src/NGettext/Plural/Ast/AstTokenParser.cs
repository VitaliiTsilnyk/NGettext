using System;
using System.Collections.Generic;
using System.Text;

namespace NGettext.Plural.Ast
{
	/// <summary>
	/// Plural rule formula parser.
	/// Ported from the I18n component from Zend Framework (https://github.com/zendframework/zf2).
	/// </summary>
	public class AstTokenParser
    {
		protected readonly Dictionary<TokenType, TokenDefinition> TokenDefinitions = new Dictionary<TokenType, TokenDefinition>();

		protected string Input;

		protected int Position;

		protected Token CurrentToken;

		/// <summary>
		/// Initializes a new instance of the <see cref="AstTokenParser"/> class with default token definitions.
		/// </summary>
		public AstTokenParser()
		{
			// Ternary operators
			this.RegisterTokenDefinition(TokenType.TernaryIf, 20)
				.SetLeftDenotationGetter((self, left) => {
					self.Children[0] = left;
					self.Children[1] = this.ParseNextExpression();
					this.AdvancePosition(TokenType.TernaryElse);
					self.Children[2] = this.ParseNextExpression();
					return self;
				});
			this.RegisterTokenDefinition(TokenType.TernaryElse);

			// Boolean operators
			this.RegisterLeftInfixTokenDefinition(TokenType.Or, 30);
			this.RegisterLeftInfixTokenDefinition(TokenType.And, 40);

			// Equal operators
			this.RegisterLeftInfixTokenDefinition(TokenType.Equals, 50);
			this.RegisterLeftInfixTokenDefinition(TokenType.NotEquals, 50);

			// Compare operators
			this.RegisterLeftInfixTokenDefinition(TokenType.GreaterThan, 50);
			this.RegisterLeftInfixTokenDefinition(TokenType.LessThan, 50);
			this.RegisterLeftInfixTokenDefinition(TokenType.GreaterOrEquals, 50);
			this.RegisterLeftInfixTokenDefinition(TokenType.LessOrEquals, 50);

			// Add operators
			this.RegisterLeftInfixTokenDefinition(TokenType.Minus, 60);
			this.RegisterLeftInfixTokenDefinition(TokenType.Plus, 60);

			// Multiply operators
			this.RegisterLeftInfixTokenDefinition(TokenType.Multiply, 70);
			this.RegisterLeftInfixTokenDefinition(TokenType.Divide, 70);
			this.RegisterLeftInfixTokenDefinition(TokenType.Modulo, 70);

			// Not operator
			this.RegisterPrefixTokenDefinition(TokenType.Not, 80);

			// Literals
			this.RegisterTokenDefinition(TokenType.N)
				.SetNullDenotationGetter((self) => {
					return self;
				});
			this.RegisterTokenDefinition(TokenType.Number)
				.SetNullDenotationGetter((self) => {
					return self;
				});

			// Parentheses
			this.RegisterTokenDefinition(TokenType.LeftParenthesis)
				.SetNullDenotationGetter((self) => {
					var expression = this.ParseNextExpression();
					this.AdvancePosition(TokenType.RightParenthesis);
					return expression;
				});
			this.RegisterTokenDefinition(TokenType.RightParenthesis);

			// EOF
			this.RegisterTokenDefinition(TokenType.EOF);
		}

		protected TokenDefinition RegisterTokenDefinition(TokenType tokenType, int leftBindingPower = 0)
		{
			TokenDefinition definition;
			if (this.TokenDefinitions.TryGetValue(tokenType, out definition))
			{
				definition.LeftBindingPower = Math.Max(definition.LeftBindingPower, leftBindingPower);
			}
			else
			{
				definition = new TokenDefinition(tokenType, leftBindingPower);
				this.TokenDefinitions[tokenType] = definition;
			}

			return definition;
		}

		protected TokenDefinition RegisterLeftInfixTokenDefinition(TokenType tokenType, int leftBindingPower)
		{
			return this.RegisterTokenDefinition(tokenType, leftBindingPower)
				.SetLeftDenotationGetter((self, left) => {
					self.Children[0] = left;
					self.Children[1] = this.ParseNextExpression(leftBindingPower);
					return self;
				});
		}

		protected TokenDefinition RegisterRightInfixTokenDefinition(TokenType tokenType, int leftBindingPower)
		{
			return this.RegisterTokenDefinition(tokenType, leftBindingPower)
				.SetLeftDenotationGetter((self, left) => {
					self.Children[0] = left;
					self.Children[1] = this.ParseNextExpression(leftBindingPower - 1);
					return self;
				});
		}

		protected TokenDefinition RegisterPrefixTokenDefinition(TokenType tokenType, int leftBindingPower)
		{
			return this.RegisterTokenDefinition(tokenType, leftBindingPower)
				.SetNullDenotationGetter((self) => {
					self.Children[0] = this.ParseNextExpression(leftBindingPower);
					self.Children[1] = null;
					return self;
				});
		}

		protected TokenDefinition GetDefinition(TokenType tokenType)
		{
			TokenDefinition tokenDefinition;
			if (!this.TokenDefinitions.TryGetValue(tokenType, out tokenDefinition))
			{
				throw new ParserException(String.Format("Can not find token definition for \"\" token type.", tokenType));
			}
			return tokenDefinition;
		}

		/// <summary>
		/// Parses the input string that contains a plural rule formula and generates an abstract syntax tree.
		/// </summary>
		/// <param name="input">Input string.</param>
		/// <returns>Root node of the abstract syntax tree.</returns>
		public Token Parse(string input)
		{
			this.Input = input + "\0";
			this.Position = 0;
			this.CurrentToken = this.GetNextToken();

			return this.ParseNextExpression();
		}

		protected Token ParseNextExpression(int rightBindingPower = 0)
		{
			var token = this.CurrentToken;
			this.CurrentToken = this.GetNextToken();
			var left = this.GetDefinition(token.Type).GetNullDenotation(token);

			while (rightBindingPower < this.GetDefinition(this.CurrentToken.Type).LeftBindingPower)
			{
				token = this.CurrentToken;
				this.CurrentToken = this.GetNextToken();
				left = this.GetDefinition(token.Type).GetLeftDenotation(token, left);
			}

			return left;
		}

		protected void AdvancePosition()
		{
			this.CurrentToken = this.GetNextToken();
		}

		protected void AdvancePosition(TokenType expectedTokenType)
		{
			if (this.CurrentToken.Type != expectedTokenType)
			{
				throw new ParserException(String.Format("Expected token \"{0}\" but received \"{1}\"", expectedTokenType, this.CurrentToken.Type));
			}
			this.AdvancePosition();
		}

		protected Token GetNextToken()
		{
			while (this.Input[this.Position] == ' ' || this.Input[this.Position] == '\t') {
				this.Position++;
			}

			var character = this.Input[this.Position++];
			var tokenType = TokenType.None;
			var value = 0L;

			switch (character)
			{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					var sb = new StringBuilder();
					sb.Append(character);
					while (Char.IsNumber(this.Input[this.Position]))
					{
						sb.Append(this.Input[this.Position++]);
					}
					tokenType = TokenType.Number;
					value = long.Parse(sb.ToString());
					break;

				case '=':
				case '&':
				case '|':
					if (this.Input[this.Position] == character)
					{
						this.Position++;
						switch (character)
						{
							case '=': tokenType = TokenType.Equals; break;
							case '&': tokenType = TokenType.And; break;
							case '|': tokenType = TokenType.Or; break;
						}
					}
					else
					{
						throw new ParserException(String.Format("Found invalid character \"{0}\" after character \"{1}\" in input stream.", this.Input[this.Position], character));
					}
					break;

				case '!':
					if (this.Input[this.Position] == '=')
					{
						this.Position++;
						tokenType = TokenType.NotEquals;
					}
					else
					{
						tokenType = TokenType.Not;
					}
					break;

				case '<':
					if (this.Input[this.Position] == '=')
					{
						this.Position++;
						tokenType = TokenType.LessOrEquals;
					}
					else
					{
						tokenType = TokenType.LessThan;
					}
					break;

				case '>':
					if (this.Input[this.Position] == '=')
					{
						this.Position++;
						tokenType = TokenType.GreaterOrEquals;
					}
					else
					{
						tokenType = TokenType.GreaterThan;
					}
					break;

				case '*': tokenType = TokenType.Multiply; break;
				case '/': tokenType = TokenType.Divide; break;
				case '%': tokenType = TokenType.Modulo; break;
				case '+': tokenType = TokenType.Plus; break;
				case '-': tokenType = TokenType.Minus; break;
				case 'n': tokenType = TokenType.N; break;
				case '?': tokenType = TokenType.TernaryIf; break;
				case ':': tokenType = TokenType.TernaryElse; break;
				case '(': tokenType = TokenType.LeftParenthesis; break;
				case ')': tokenType = TokenType.RightParenthesis; break;

				case ';':
				case '\n':
				case '\0':
					tokenType = TokenType.EOF;
					this.Position--;
					break;

				default:
					throw new ParserException(String.Format("Found invalid character \"{0}\" in input stream at position {1}.", character, this.Position));
			}

			return new Token(tokenType, value);
		}
	}
}
