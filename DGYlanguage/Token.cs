using System.Diagnostics;
using static VariableType;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Lab_1.Exceptions;

public static class VariableType
{
    public abstract class ObjectType { public abstract string toString(); }

    public sealed class INTEGER : ObjectType
    {
        public const string Name = "int";

        public override string toString() => Name;
    }

    public sealed class FLOAT : ObjectType
    {
        public const string Name = "float";

        public override string toString() => Name;
    }

    public readonly static INTEGER Integer = new INTEGER();
    public readonly static FLOAT Floating = new FLOAT();

    public static IEnumerable<string> ObjectTypesNames()
    {
        var namesTypes = new List<string>() { FLOAT.Name, INTEGER.Name };
        return namesTypes;
    }

    public static ObjectType GetTypeAtName(string type)
    {
        switch (type)
        {
            case FLOAT.Name:
                return Floating;
            case INTEGER.Name:
                return Integer;
            default:
                throw new ArgumentException("Unknow type");
        }
    }
}

class Token
{
    public TokenType Type { get;private set; }
    public string Value { get; set; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
}

class Variable : Token
{
    public string Name { get; set; }
    public ObjectType TypeVariable { get; set; }
    public Variable(string name, ObjectType variableType, TokenType type, string value = "null") : base(type, value)
    {
        Name = name;
        TypeVariable = variableType;
    }

    public string toString()
    {
        return $"{Name}: {Value}";
    }
}

enum TokenType
{
    TYPE,
    INTEGER,
    FLOAT,
    IDENTIFIER,
    ADD,
    SUBTRACT,
    MULTIPLY,
    DIVIDE,
    LEFT_PAREN,
    RIGHT_PAREN,
    SET,
    INVALID
}

class ExpressionEvaluator
{

    private static Dictionary<string, TokenType> _operatorMap = new Dictionary<string, TokenType>
    {
        { "+", TokenType.ADD },
        { "-", TokenType.SUBTRACT },
        { "*", TokenType.MULTIPLY },
        { "/", TokenType.DIVIDE },
        { "(", TokenType.LEFT_PAREN },
        { ")", TokenType.RIGHT_PAREN },
        { "=", TokenType.SET}
    };

    private static List<Token> Tokenize(string inputLine, List<Variable> variables = null)
    {

        ObjectType typeVariable = null;
        bool isVariableInit = false;

        var tokens = new List<Token>();
        int i = 0;

        while (i < inputLine.Length)
        {
            char c = inputLine[i];
            if (char.IsDigit(c))
            {
                int j = i;
                while (j < inputLine.Length && (char.IsDigit(inputLine[j]) || inputLine[j] == '.'))
                {
                    j++;
                }

                string number = inputLine.Substring(i, j - i);
                tokens.Add(new Token(number.Contains('.') ? TokenType.FLOAT : TokenType.INTEGER, number));
                i = j;
            }
            else if (char.IsLetter(c))
            {
                int j = i;
                while (j < inputLine.Length && char.IsLetterOrDigit(inputLine[j]))
                {
                    j++;
                }
   
                string identifier = inputLine.Substring(i, j - i);

                if (ObjectTypesNames().Contains(identifier))
                {
                    if (isVariableInit) throw new TypeAlreadyDeclaredException($"{identifier} type already announced");

                    tokens.Add(new Token(TokenType.TYPE, identifier));
                    typeVariable = GetTypeAtName(identifier);
                }
                else
                {
                    var variableInList = variables?.FirstOrDefault(variable => variable.Name == identifier);

                    if (typeVariable == null && variableInList == null) throw new NonExistentTypeException($"{identifier} is not existent type");
                    else if (variableInList != null)
                    {
                        tokens.Add(variableInList);
                    }
                    
                    tokens.Add(new Variable(identifier, typeVariable, TokenType.IDENTIFIER));
                    isVariableInit = true;
                }

                i = j;
            }
            else if (_operatorMap.ContainsKey(c.ToString()))
            {
                tokens.Add(new Token(_operatorMap[c.ToString()], c.ToString()));
                i++;
            }
            else
            {
                i++;
            }
        }

        return tokens;
    }

    private static int Precedence(TokenType op)
    {
        switch (op)
        {
            case TokenType.MULTIPLY:
            case TokenType.DIVIDE:
                return 2;
            case TokenType.ADD:
            case TokenType.SUBTRACT:
                return 1;
            default:
                return 0;
        }
    }
    private static List<Token> InfixToRPN(List<Token> tokens)
    {
        List<Token> output = new List<Token>();
        Stack<Token> stack = new Stack<Token>();

        foreach (Token t in tokens)
        {
            if (t.Type == TokenType.INTEGER || t.Type == TokenType.FLOAT)
            {
                output.Add(t);
            }
            else if (t.Type == TokenType.IDENTIFIER && t.Value != "null") //TODO CHECK
            {
                output.Add(t);
            }
            else if (t.Type == TokenType.ADD || t.Type == TokenType.SUBTRACT ||
                     t.Type == TokenType.MULTIPLY || t.Type == TokenType.DIVIDE)
            {
                while (stack.Count > 0 && Precedence(stack.Peek().Type) >= Precedence(t.Type))
                {
                    output.Add(stack.Pop());
                }
                stack.Push(t);
            }
            else if (t.Type == TokenType.LEFT_PAREN)
            {
                stack.Push(t);
            }
            else if (t.Type == TokenType.RIGHT_PAREN)
            {
                while (stack.Count > 0 && stack.Peek().Type != TokenType.LEFT_PAREN)
                {
                    output.Add(stack.Pop());
                }
                stack.Pop();
            }
        }

        while (stack.Count > 0)
        {
            output.Add(stack.Pop());
        }

        return output;
    }

    private static float Evaluate(List<Token> rpn)
    {
        Stack<float> stack = new Stack<float>();
        foreach (Token t in rpn)
        {
            if (t.Type == TokenType.INTEGER || t.Type == TokenType.FLOAT || t.Type == TokenType.IDENTIFIER)
            {
                stack.Push(float.Parse(t.Value));
            }
            else
            {
                float right = stack.Pop();
                float left = stack.Pop();
                switch (t.Type)
                {
                    case TokenType.ADD:
                        stack.Push(left + right);
                        break;
                    case TokenType.SUBTRACT:
                        stack.Push(left - right);
                        break;
                    case TokenType.MULTIPLY:
                        stack.Push(left * right);
                        break;
                    case TokenType.DIVIDE:
                        stack.Push(left / right);
                        break;
                }
            }
        }
        return stack.Pop();
    }

    private static void InitVariable(List<Token> tokens,out Variable variable)
    {
        variable = GetVariableOrNull(tokens);

        if(variable == null)
            return;

        tokens.Remove(variable);
        List<Token> rpn = InfixToRPN(tokens);
        var result = Evaluate(rpn);

        variable.Value = result.ToString();
    }

    private static Variable GetVariableOrNull(List<Token> tokens) 
    {
        Variable variable = null;
        bool isFindFlag = false;
        int index = 0;
        
        foreach (Token t in tokens)
        {
            if (t is Variable && t.Value == "null")
            {
                if (tokens.First().Type != TokenType.TYPE && tokens[1].Type != TokenType.IDENTIFIER) 
                    throw new Exception("Bad Tokens.");

                if (tokens[index + 1].Value == "=" && tokens[index + 1].Type == TokenType.SET && isFindFlag == false)
                {
                    variable = t as Variable;
                    isFindFlag = true;
                }
                else
                    throw new Exception("Too many null variables found.");
            }
            index++;
        }
        return variable;
    }

    public static string Analize(string line) 
    {
        List<Token> tokens = Tokenize(line);
        Variable variable;

        InitVariable(tokens, out variable);

        if (variable != null)
        {
            return variable.toString();
        }
        else
        {
            return Evaluate(InfixToRPN(tokens)).ToString();
        }
    }
}