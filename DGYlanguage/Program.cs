int countArgs = args.Length;

if (countArgs <= 1)
    throw new Exception("Not flags");

string result;
if (args[0] == "c")
    result = ExpressionEvaluator.Analize(args[1]);
else if (args[0] == "g")
    result = ExpressionEvaluator.GetData(args[1]);
else
    result = "";

Console.WriteLine(result);