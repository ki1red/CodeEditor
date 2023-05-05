
int countArgs = args.Length;
string result;
//if (countArgs <= 1)
//    result = "not flags";
//else if (args[1].IndexOf(".dgy") == -1|| !File.Exists(args[1]))
//    result = "file not found";
//else if (args[0] == "-c")
//{
//    try
//    {
//        CodeParser codeParser = new CodeParser(args[1]);

//        result = codeParser.Result;
//    }
//    catch (Exception e)
//    {
//        result = e.Message;
//    }
//}
//else
//{
//    result = "wtf";
//}

//Console.WriteLine(result);

result = "code.txt";
try
{
    CodeParser codeParser = new CodeParser(result);
    codeParser.Analize();

    result = codeParser.Result;
}
catch (Exception e)
{
    result = e.Message;
}

Console.WriteLine(result);
// FLAGS:
// -c - compile the code (example: dgy.exe test.dgy -c)