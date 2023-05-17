using System.Text.RegularExpressions;

public class CodeParser
{
    private static readonly (string pattern, TokenType type)[] tokenDefinitions =
    {
        (@"\bint\b", TokenType.Int),
        (@"\b[^\d]\w*\b", TokenType.Identifier),
        (@"\+", TokenType.Plus),
        (@"-", TokenType.Minus),
        (@"\*", TokenType.Multiplication),
        (@"\/", TokenType.Division),
        (@"%", TokenType.Modulo),
        (@"=", TokenType.Assignment),
        (@"\(", TokenType.LeftParen),
        (@"\)", TokenType.RightParen),
        (@"\+=", TokenType.CompoundAddition),
        (@"-=", TokenType.CompoundSubtraction),
        (@"\*=", TokenType.CompoundMultiplication),
        (@"\/=", TokenType.CompoundDivision),
        (@"%=", TokenType.CompoundModulo),
        (@"\/\*", TokenType.MultiLineCommentStart),
        (@"\*\/", TokenType.MultiLineCommentEnd),
        (@"\s", TokenType.WhiteSpace),
        (@";", TokenType.EndOfStatement),
        (@"\d+", TokenType.Unknown),
        (@"\w+", TokenType.Unknown),
    };
    public string Text { get; private set; }
    public CodeParser(string file)
    {
        Text = File.ReadAllText(file);
    }
    public static List<Token> Parse(string code)
    {
        var tokens = new List<Token>();
        var inCommentMode = false;
        var currentPos = 0;

        while (code.Length > 0)
        {
            var match = tokenDefinitions
                .Select(d => Regex.Match(code, $"^{d.pattern}"))
                .Where(m => m.Success)
                .OrderByDescending(m => m.Value.Length)
                .FirstOrDefault();

            if (match != null)
            {
                var tokenType = tokenDefinitions.First(d => Regex.Match(match.Value, $"^{d.pattern}$").Success).type;

                if (tokenType == TokenType.MultiLineCommentStart)
                {
                    inCommentMode = true;
                }

                if (!inCommentMode)
                {
                    tokens.Add(new Token(tokenType, match.Value, currentPos + 1));
                }

                if (tokenType == TokenType.MultiLineCommentEnd)
                {
                    inCommentMode = false;
                }

                currentPos += match.Value.Length;
                code = code.Substring(match.Value.Length);
            }
            else
            {
                throw new Exception($"Unrecognized symbol '{code[0]}'");
            }
        }
        return tokens;
    }
}