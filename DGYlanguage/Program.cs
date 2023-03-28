int countArgs = args.Length;

int indexTextCode;
if (countArgs > 1)
    indexTextCode = 1;
else
    indexTextCode = 0;

string result = ExpressionEvaluator.Analize(args[indexTextCode]);

Console.WriteLine(result);