public struct Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
    public int StartPosition { get; set; }
    public int EndPosition { get; set; }
    public int Line { get; set; }
    public int Num { get; set; }

    public Token(int line, int num, TokenType type, string value, int startPosition, int endPosition)
    {
        Line = line;
        Num = num;
        Type = type;
        Value = value;
        StartPosition = startPosition;
        EndPosition = endPosition;
    }

    public override string ToString()
    {
        return $"{Line}: {Num} {Type.ToString()} \"{Value}\" from {StartPosition} to {EndPosition}";
    }
}


public enum TokenType
{
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
    EndOfStatement,         // ;
    WhiteSpace,             // space
    NewLine,                // \n
    Tab,                    // \t
    MultiLineComment,       // /* */
    Identifier,             // variable
    Value,                  // ЦБЗ
    Unknown
}