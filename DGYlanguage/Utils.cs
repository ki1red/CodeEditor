using System.Text;
public static class Utils
{
    public static readonly HashSet<string> banWords = new HashSet<string>()
    {
        {"@"},{"!"},{"$"}
    };
    public static readonly HashSet<string> allowedWords = new HashSet<string>()
    {
        {"@\""},{"!="},{"$\"."}
    };
    public static readonly Dictionary<string, TokenType> TokenTypes = new Dictionary<string, TokenType>
    {
        {"+", TokenType.Plus},
        {"-", TokenType.Minus},
        {"*", TokenType.Multiplication},
        {"/", TokenType.Division},
        {"%", TokenType.Modulo},
        {"=", TokenType.Assignment},
        {"==",TokenType.Euqlse},
        {"!=", TokenType.NotEuqlse},
        {">=",TokenType.EuqlseAndMore},
        {"<=",TokenType.EuqlseAndLess},
        {"<",TokenType.Less},
        {">",TokenType.More},
        {"(", TokenType.LeftParen},
        {")", TokenType.RightParen},
        {"+=", TokenType.CompoundAddition},
        {"-=", TokenType.CompoundSubtraction},
        {"*=", TokenType.CompoundMultiplication},
        {"/=", TokenType.CompoundDivision},
        {"%=", TokenType.CompoundModulo},
        {"/*", TokenType.MultiLineComment},
        {"*/", TokenType.MultiLineComment},
        {"{", TokenType.LeftBlockCode},
        {"}", TokenType.RightBlockCode},
        {"if", TokenType.IfKeyword},
        {"else", TokenType.ElseKeyword}
    };
    public static List<string> SplitTextIntoLines(string input)
    {
        const char leftParen = '{';
        const char rightParen = '}';
        const char splitter = ';';
        input = input.Replace("\n", "");
        List<string> parts = new List<string>();
        StringBuilder currentPart = new StringBuilder();
        bool insideBraces = false;

        foreach (char c in input)
        {
            if (c == leftParen)
            {
                insideBraces = true;
            }
            else if (c == rightParen)
            {
                if (!insideBraces)
                {
                    throw new Exception("incorrect curly braces");
                }
                insideBraces = false;
            }

            if (c == splitter && !insideBraces)
            {
                parts.Add(currentPart.ToString().Trim());
                currentPart.Clear();
            }
            else
            {
                currentPart.Append(c);
            }
        }

        if (currentPart.Length > 0)
        {
            parts.Add(currentPart.ToString().Trim());
        }

        return parts;
    }
    public static List<string> SplitTextIntoWords(string input)
    {
        const char leftParen = '{';
        const char rightParen = '}';
        const char splitter = ' ';
        var words = new List<string>();
        bool insideBraces = false;
        StringBuilder currentWord = new StringBuilder();

        foreach (char c in input)
        {
            if (c == leftParen)
            {
                insideBraces = true;
                currentWord.Append(c);
            }
            else if (c == rightParen)
            {
                insideBraces = false;
                currentWord.Append(c);
                string word = currentWord.ToString();
                words.Add(word);
                currentWord.Clear().Append(word);
            }
            else if (c == splitter && !insideBraces)
            {
                if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
            }
            else
            {
                currentWord.Append(c);
            }
        }

        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
        }

        return words;
    }
    public static string ToGlue(List<string> lines)
    {
        string result = "";
        foreach (var str in lines)
        {
            result += str + "\n";
        }
        return result;
    }
    public static List<Position> CheckParens(char left, char right, List<string> input)
    {
        var stack = new Stack<Position>();
        var result = new List<Position>();

        for (int i = 0; i < input.Count; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                uint row = (uint)i;
                uint column = (uint)j;

                char c = input[i][j];
                if (c == left)
                {
                    stack.Push(new Position(row, column));
                }
                else if (c == right)
                {
                    if (stack.Count == 0)
                    {
                        result.Add(new Position(row, column));
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
            }
        }

        while (stack.Count > 0)
        {
            result.Add(stack.Pop());
        }

        return result;
    }
    public static List<Position> GetPositionsWords(string inputLine, uint numberLine)
    {
        const char leftBrace = '{';
        const char rightBrace = '}';
        const char space = ' ';

        var positions = new List<Position>();
        bool insideBraces = false;

        for (int i = 0, wordStart = 0; i <= inputLine.Length; i++)
        {
            char c = i == inputLine.Length ? space : inputLine[i];

            if (c == leftBrace)
            {
                insideBraces = true;
            }
            else if (c == rightBrace)
            {
                insideBraces = false;
            }
            else if (c == space && !insideBraces)
            {
                if (i > wordStart)
                {
                    positions.Add(new Position(numberLine, (uint)wordStart));
                    positions.Add(new Position(numberLine, (uint)(i - 1)));
                    wordStart = i + 1;
                }
                else
                {
                    wordStart++;
                }
            }
        }

        return positions;
    }
    public static List<Token> GetTokens(List<string> inputLine, List<Position> positions, ref Dictionary<string, object> values)
    {
        List<Token> tokens = new List<Token>();
        for (int i = 0, j = 0; i < inputLine.Count; i++)
        {
            // Инициализация всех составляющаъ объекта Token
            string str = inputLine[i];
            Token token;
            TokenType type;
            object value;
            Position start = positions[j++];
            Position end = positions[j++];

            // Одно из зарезервированных типов
            if (TokenTypes.ContainsKey(str))
            {
                type = TokenTypes[str];
                value = str;
            }
            else
            {
                // Значение
                if (ValueType(str) == TokenType.Int)
                {
                    type = ValueType(str);
                    value = int.Parse(str);
                }
                // Имя переменной (инициализация или использование)
                else if (values.ContainsKey(str))
                {
                    type = TokenType.Name;
                    value = str;
                }
                else if (i + 1 < inputLine.Count && TokenTypes[inputLine[i + 1]] == TokenType.Assignment)
                {
                    type = TokenType.Name;
                    value = str;

                    values.Add(str, null);
                }
                else
                {
                    type = TokenType.Unknown;
                    value = str;
                }
            }
            token = new Token(type, value, start, end);
            tokens.Add(token);
        }
        return tokens;
    }
    public static TokenType ValueType(string value)
    {
        int cNumber = 0;
        int cPoint = 0;
        int cChar = 0;

        foreach (char c in value)
        {
            if (Char.IsDigit(c))
            {
                cNumber++;
            }
            else if (c == '.')
            {
                cPoint++;
            }
            else
            {
                cChar++;
            }
        }

        if (cNumber > 0 && cPoint == 0 && cChar == 0)
        {
            return TokenType.Int;
        }
        else
        {
            return TokenType.Unknown;
        }
    }
}
