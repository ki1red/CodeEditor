public static class MathUtils
{
    public static void Calculate(List<Token> tokens, ref Dictionary<string, object> values)
    {
        string name = tokens[0].Value.ToString();

        Stack<Token> stack = CreatePriorityStack(tokens);
        Stack<Token> result = new Stack<Token>();
        while(stack.Count > 0)
        {
            Token token = stack.Pop();
            if (token.Type == TokenType.Int)
                result.Push(token);
            else if (token.Type == TokenType.Name)
            {
                object value = values[token.Value.ToString()]; // TODO ERROR
                token = new Token(TokenType.Int, value, token.PositionStart, token.PositionEnd); // ONLY INT
                result.Push(token);
            }
            else
            {
                Token first = result.Pop();
                Token second = result.Pop();
                switch (token.Type)
                {
                    case TokenType.Assignment:
                        first = new Token(TokenType.Int, second.Value, first.PositionStart, first.PositionEnd);
                        break;
                    case TokenType.CompoundAddition:
                    case TokenType.Plus:
                        first = new Token(TokenType.Int, (int)first.Value + (int)second.Value, first.PositionStart, first.PositionEnd);
                        break;
                    case TokenType.CompoundSubtraction:
                    case TokenType.Minus:
                        first = new Token(TokenType.Int, (int)first.Value - (int)second.Value, first.PositionStart, first.PositionEnd);
                        break;
                    case TokenType.CompoundMultiplication:
                    case TokenType.Multiplication:
                        first = new Token(TokenType.Int, (int)first.Value * (int)second.Value, first.PositionStart, first.PositionEnd);
                        break;
                    case TokenType.CompoundDivision:
                    case TokenType.Division:
                        first = new Token(TokenType.Int, (int)first.Value / (int)second.Value, first.PositionStart, first.PositionEnd);
                        break;
                    case TokenType.CompoundModulo:
                    case TokenType.Modulo:
                        first = new Token(TokenType.Int, (int)first.Value % (int)second.Value, first.PositionStart, first.PositionEnd);
                        break;
                    case TokenType.Unknown:
                        break;
                    default:
                        break;
                }
                result.Push(first);
            }
        }
        values[name] = result.Pop().Value; // TODO CHECK VALUE
    }
    public static Stack<Token> CreatePriorityStack(List<Token> source)
    {
        Stack<Token> tokens = new Stack<Token>();
        int c = 0, j = 0;
        for (int i = 0; i < source.Count; i++)
            if (IsAssignment(source[i].Type))
            {
                c++;
                j = i;
            }
        if (c > 1)
            throw new Exception();
        HashSet<int> ints = new HashSet<int>();
        tokens.Push(source[j]);
        tokens.Push(source[j - 1]);

        List<Token> cutedSource = new List<Token>();
        cutedSource.Add(new Token(TokenType.LeftParen, '(', new Position(0,0), new Position(0,0)));
        for (int i = j + 1; i < source.Count; i++)
            cutedSource.Add(source[i]);
        cutedSource.Add(new Token(TokenType.RightParen, ')', new Position(0, 0), new Position(0, 0)));

        Rec(ref tokens, cutedSource, 0, ref ints);

        return tokens;
    }
    public static void Rec(ref Stack<Token> tokens, List<Token> source, int pos, ref HashSet<int> values)
    {
        int start, end, tmp = SearchPair(source, pos);
        if (source[pos].Type == TokenType.LeftParen)
        {
            start = pos;
            end = tmp;
            if (end == -1)
                throw new Exception();
        }
        else if (source[pos].Type == TokenType.RightParen)
        {
            end = pos;
            start = tmp;
            if (start == -1)
                throw new Exception();
        }
        else
        {
            start = pos-1;
            end = source.Count;
        }
        values.Add(start);
        values.Add(end);

        if ((end - start) / 2 == 1 && (source[1].Type == TokenType.Name || source[1].Type == TokenType.Int))
        {
            tokens.Push(source[1]);
            return;
        }

        int tp = 0;
        for (int i = start+1; i < end; i++)
        {
            if (i+1 == end)
            {
                tp++;
                i = start + 1;
            }
            if (tp > 2)
                break;
            if (source[i].Type == TokenType.LeftParen)
            {
                int j = SearchPair(source, i);
                if (j == -1)
                    throw new Exception();
                i = j;
            }
            if (IsOperator(source[i].Type))
            {
                if (LevelPriority(source[i].Type) == tp)
                {
                    tokens.Push(source[i]);
                    values.Add(i);

                    if (source[i - 1].Type == TokenType.RightParen)
                        Rec(ref tokens, source, i - 1, ref values);
                    else if (!values.Contains(i - 1))
                    {
                        tokens.Push(source[i - 1]);
                        values.Add(i - 1);
                    }
                    if (source[i + 1].Type == TokenType.LeftParen)
                        Rec(ref tokens, source, i + 1, ref values);
                    else if (!values.Contains(i + 1))
                    {
                        tokens.Push(source[i + 1]);
                        values.Add(i + 1);
                    }
                }
            }
              
        }
    }

    public static bool IsAssignment(TokenType type)
    {
        switch (type)
        {
            case TokenType.Assignment:
            case TokenType.CompoundAddition:
            case TokenType.CompoundSubtraction:
            case TokenType.CompoundMultiplication:
            case TokenType.CompoundDivision:
            case TokenType.CompoundModulo:
                return true;
            default:
                return false;
        }
    }
    public static bool IsOperator(TokenType type)
    {
        switch (type)
        {
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.Multiplication:
            case TokenType.Division:
            case TokenType.Modulo:
                return true;
            default:
                return false;
        }
    }
    public static int SearchPair(List<Token> tokens, int i)
    {
        if (i > tokens.Count - 1)
            throw new IndexOutOfRangeException();
        if (tokens[i].Type != TokenType.LeftParen && tokens[i].Type != TokenType.RightParen)
            return -1;
        int j = i;
        int level = 0;
        if (tokens[i].Type == TokenType.LeftParen)
        {
            j++;
            while (level != 0 || tokens[j].Type != TokenType.RightParen)
            {
                if (tokens[j].Type == TokenType.LeftParen)
                    level++;
                else if (tokens[j].Type == TokenType.RightParen)
                    level--;
                j++;
            }
        }
        else
        {
            j--;
            while (level != 0 || tokens[j].Type != TokenType.LeftParen)
            {
                if (tokens[j].Type == TokenType.LeftParen)
                    level--;
                else if (tokens[j].Type == TokenType.RightParen)
                    level++;
                j--;
            }
        }
        return j;
    }
    public static int LevelPriority(TokenType token)
    {
        int level = 0;
        switch (token)
        {
            case TokenType.Plus:
                level = 1;
                break;
            case TokenType.Minus:
                level = 1;
                break;
            case TokenType.Multiplication:
                level = 2;
                break;
            case TokenType.Division:
                level = 2;
                break;
            case TokenType.Modulo:
                level = 2;
                break;
            case TokenType.Assignment:
                level = 0;
                break;
            case TokenType.LeftParen:
                level = 3;
                break;
            case TokenType.RightParen:
                level = 3;
                break;
            case TokenType.CompoundAddition:
                level = 0;
                break;
            case TokenType.CompoundSubtraction:
                level = 0;
                break;
            case TokenType.CompoundMultiplication:
                level = 0;
                break;
            case TokenType.CompoundDivision:
                level = 0;
                break;
            case TokenType.CompoundModulo:
                level = 0;
                break;
            case TokenType.MultiLineComment:
                break;
            case TokenType.IfKeyword:
                break;
            case TokenType.ElseKeyword:
                break;
            case TokenType.LeftBlockCode:
                break;
            case TokenType.RightBlockCode:
                break;
            case TokenType.Unknown:
                break;
            default:
                break;
        }
        return level;
    }
}