
int countArgs = args.Length;
if (countArgs <= 1)
    Console.WriteLine("not flags");
else if (args[1].IndexOf(".dgy") == -1 || !File.Exists(args[1]))
    Console.WriteLine("file not found");
else if (args[0] == "-c")
{
    CodeParser codeParser = new CodeParser(args[1]);
    var tokens = CodeParser.Parse(codeParser.Text);

    foreach (var token in tokens)
    {
        Console.WriteLine(token);
    }
}
else
{
    Console.WriteLine("wtf");
}

// FLAGS:
// -c - compile the code (example: dgy.exe test.dgy -c)