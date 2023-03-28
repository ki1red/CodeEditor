using System;

namespace Lab_1.Exceptions
{
    public class UninitializedVariableException : Exception { public UninitializedVariableException(string message) : base(message) { } }

    public class NonExistentTypeException : Exception { public NonExistentTypeException(string message) : base(message) { } }

    public class MissingExpressionNearOperatorException : Exception { public MissingExpressionNearOperatorException(string message) : base(message) { } }

    public class MissedOperatorException : Exception { public MissedOperatorException(string message) : base(message) { } }

    public class TypeAlreadyDeclaredException : Exception { public TypeAlreadyDeclaredException(string message) : base(message) { } }
}
