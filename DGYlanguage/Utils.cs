using System.Text;
public static class Utils
{
    public static readonly Dictionary<string, TokenType> TokenTypes = new Dictionary<string, TokenType>
    {
        {"int", TokenType.Int},
        {"+", TokenType.Plus},
        {"-", TokenType.Minus},
        {"*", TokenType.Multiplication},
        {"/", TokenType.Division},
        {"%", TokenType.Modulo},
        {"=", TokenType.Assignment},
        {"(", TokenType.LeftParen},
        {")", TokenType.RightParen},
        {"+=", TokenType.CompoundAddition},
        {"-=", TokenType.CompoundSubtraction},
        {"*=", TokenType.CompoundMultiplication},
        {"/=", TokenType.CompoundDivision},
        {"%=", TokenType.CompoundModulo},
        {"/*", TokenType.MultiLineComment},
        {"*/", TokenType.MultiLineComment},
        {"{", TokenType.BlockCode},
        {"}", TokenType.BlockCode},
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
    public static List<Position> GetPositionsWords(string input, uint numberLine)
    {
        const char leftBrace = '{';
        const char rightBrace = '}';
        const char space = ' ';

        var positions = new List<Position>();
        bool insideBraces = false;

        for (int i = 0, wordStart = 0; i <= input.Length; i++)
        {
            char c = i == input.Length ? space : input[i];

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
    public static List<Token> GetTokens(List<string> input, List<Position> positions)
    {
        List<Token> tokens = new List<Token>();
        for(int i = 0, j = 0; i < input.Count; i++)
        {
            string s = input[i];
            Position start = positions[j];
            Position end = positions[j + 1];
            j += 2;

            TokenType token;
            try
            {
                token = TokenTypes[s];
            }
            catch(Exception e)
            {
                if (s[0] == '{' && s[s.Length - 1] == '}')
                    token = TokenType.BlockCode;
                else
                    token = TokenType.Unknown;
            }

            object value;
            if (token == TokenType.Int)
                value = Convert.ToInt32(s);
            else
                value = s;

            tokens.Add(new Token(token, value, start, end));
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
