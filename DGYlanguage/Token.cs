public enum TokenType
{
    Identifier,
    Int,                    // тип данных int
    Plus,                   // оператор сложения (+)
    Minus,                  // оператор вычитания (-)
    Multiplication,         // оператор умножения (*)
    Division,               // оператор деления (/)
    Modulo,                 // оператор модуля (%)
    Assignment,             // оператор присваивания (=)
    LeftParen,              // (
    RightParen,             // )
    CompoundAddition,       // составной оператор сложения (+=)
    CompoundSubtraction,    // составной оператор вычитания (-=)
    CompoundMultiplication, // составной оператор умножения (*=)
    CompoundDivision,       // составной оператор деления (/=)
    CompoundModulo,         // составной оператор модуля (%=)
    MultiLineCommentStart,
    MultiLineCommentEnd,
    WhiteSpace,
    EndOfStatement,
    Unknown
}

public struct Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
    public int StartPosition { get; set; }
    public int EndPosition { get; set; }

    public Token(TokenType type, string value, int startPosition)
    {
        Type = type;
        Value = value;
        StartPosition = startPosition;
        EndPosition = startPosition + value.Length - 1;
    }

    public override string ToString()
    {
        return $"{Type} - {Value} - from {StartPosition} to {EndPosition} symbols";
    }
}