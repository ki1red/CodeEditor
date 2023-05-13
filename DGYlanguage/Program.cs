string t = args[1];
int countArgs = args.Length;
if (countArgs <= 1)
    Console.WriteLine("not flags");
else if (t.IndexOf(".txt") == -1 || !File.Exists(t))
    Console.WriteLine("file not found");
else if (args[0] == "-c")
{
    try
    {
        CodeParser codeParser = new CodeParser(args[1]);
        var tokens = CodeParser.Parse(codeParser.Text);

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
else
{
    Console.WriteLine("wtf");
}

// FLAGS:
// -c - compile the code (example: dgy.exe -c test.txt)