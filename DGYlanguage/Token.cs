public enum TokenType
{
    Name,
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
    MultiLineComment,       // многострочный комментарий (/* */)
    IfKeyword,              // ключевое слово if
    ElseKeyword,            // ключевое слово else
    BlockCode,              // {....}
    Unknown
}

public struct Token
{
    public TokenType Type { get; private set; }
    public object Value { get; private set; }
    public Position PositionStart { get; private set; }
    public Position PositionEnd { get; private set; }
    public Token(TokenType type, object value, Position positionStart, Position positionEnd)
    {
        Type = type;
        Value = value;
        PositionStart = positionStart;
        PositionEnd = positionEnd;
    }
    public static bool operator ==(Token a, Token b) => (a.Type == b.Type && a.Value == b.Value);
    public static bool operator !=(Token a, Token b) => !(a == b);
}