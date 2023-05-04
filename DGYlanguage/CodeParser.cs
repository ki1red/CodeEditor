public class CodeParser
{
    public string Text { get; private set; }
    public List<string> Lines { get; private set; }
    public string Result { get; private set; }
    public CodeParser(string file)
    {
        Text = File.ReadAllText(file);
        Lines = new List<string>();
        Lines = Utils.SplitTextIntoLines(Text);
        Result = Utils.ToGlue(Lines);
    }

}