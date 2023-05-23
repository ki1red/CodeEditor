public class CodeParser
{
    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
    {
        {"int", TokenType.Int},
        {"+", TokenType.Plus},
        {"-", TokenType.Minus},
        {"*", TokenType.Multiplication },
        {"/", TokenType.Division},
        {"% ", TokenType.Modulo },
        {"=", TokenType.Assignment },
        {"(", TokenType.LeftParen },
        {")", TokenType.RightParen },
        {"+=", TokenType.CompoundAddition },
        {"-=", TokenType.CompoundSubtraction },
        {"*=", TokenType.CompoundMultiplication },
        {"/=", TokenType.CompoundDivision },
        {"%=", TokenType.CompoundModulo },
        {";", TokenType.EndOfStatement },
        {" ", TokenType.WhiteSpace },
        {"\n", TokenType.NewLine },
        {"\t", TokenType.Tab }
    };

    public string Text { get; private set; }

    public CodeParser(string file)
    {
        Text = File.ReadAllText(file);
    }
    public List<Token> Parse()
    {
        return Tokenize(Text);
    }
    private static List<Token> Tokenize(string input)
    {
        List<Token> tokens = new List<Token>();
        int i = 0;
        int line = 1;

        while (i < input.Length)
        {
            char c = input[i];

            if (char.IsWhiteSpace(c))
            {
                if (c == ' ')
                {
                    tokens.Add(new Token(line, (int)TokenType.WhiteSpace, TokenType.WhiteSpace, "(space)", i + 1, i + 1));
                }
                else if (c == '\n')
                {
                    tokens.Add(new Token(line, (int)TokenType.NewLine, TokenType.NewLine, "\\n", i + 1, i + 1));
                    line++;
                }
                else if (c == '\t')
                {
                    tokens.Add(new Token(line, (int)TokenType.Tab, TokenType.Tab, "\\t", i + 1, i + 1));
                }
                i++;
                continue;
            }

            if (c == ';')
            {
                tokens.Add(new Token(line, (int)TokenType.EndOfStatement, TokenType.EndOfStatement, ";", i + 1, i + 1));
                i++;
                continue;
            }

            if (i+1 < input.Length && c == '/' && input[i+1] == '*')
            {
                string str = "/*";
                int start = i;
                i+=2;

                while (i+1 < input.Length && (input[i] != '*' && input[i+1] != '/'))
                {
                    str += input[i];
                    i++;
                }

                if (i+1 < input.Length && input[i] == '*' && input[i+1] == '/')
                {
                    str += "*/";
                    tokens.Add(new Token(line, (int)TokenType.MultiLineComment, TokenType.MultiLineComment, str, start + 1, i+2));
                    i+=2;
                }
                else
                {
                    tokens.Add(new Token(line, (int)TokenType.Unknown, TokenType.Unknown, str="/*", start + 1, start+2));
                    i = start + 2;
                }

                continue;
            }

            if (IsDigit(c))
            {
                string number = "";
                int start = i;

                while (i < input.Length && IsDigit(input[i]))
                {
                    number += input[i];
                    i++;
                }

                if (i + 1 == input.Length || !IsLetter(input[i + 1]))
                {
                    tokens.Add(new Token(line, (int)TokenType.Value, TokenType.Value, number, start + 1, i));
                }
                else
                {
                    tokens.Add(new Token(line, (int)TokenType.Unknown, TokenType.Unknown, number, start + 1, i));
                }

                continue;
            }

            if (IsLetter(c))
            {
                string word = "";
                int start = i;

                while (i < input.Length && (IsLetter(input[i]) || IsDigit(input[i])))
                {
                    word += input[i];
                    i++;
                }

                if (keywords.ContainsKey(word))
                {
                    tokens.Add(new Token(line, (int)TokenType.Int, TokenType.Int, word, start + 1, i));
                }
                else
                {
                    tokens.Add(new Token(line, (int)TokenType.Identifier, TokenType.Identifier, word, start + 1, i));
                }

                continue;
            }

            if (IsOperator(c))
            {
                string oper = ""+c;
                int start = i;
                while (i+1<input.Length && IsOperator(input[i+1]))
                {
                    oper += input[i + 1];
                    i++;
                }
                if (keywords.ContainsKey(oper))
                {
                    tokens.Add(new Token(line, (int)keywords[oper], keywords[oper], oper, start + 1, i+1));
                }
                else
                {
                    tokens.Add(new Token(line, (int)TokenType.Unknown, TokenType.Unknown, oper, start + 1, i+1));
                }
                i++;
                continue;
            }

            if (c == '(')
            {
                tokens.Add(new Token(line, (int)TokenType.LeftParen, TokenType.LeftParen, "(", i + 1, i + 1));
                i++;
                continue;
            }
            else if (c == ')')
            {
                tokens.Add(new Token(line, (int)TokenType.RightParen, TokenType.RightParen, ")", i + 1, i + 1));
                i++;
                continue;
            }

            tokens.Add(new Token(line, (int)TokenType.Unknown, TokenType.Unknown, c.ToString(), i + 1, i + 1));
            i++;
        }

        return tokens;
    }
    private static bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
    private static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }
    private static bool IsOperator(char c)
    {
        switch (c)
        {
            case '+':
            case '-':
            case '*':
            case '/':
            case '=':
            case '%':
                return true;
            default:
                return false;
        }
    }
}
