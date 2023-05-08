public class CodeParser
{
    private Dictionary<string, object> values = new Dictionary<string, object>();
    public string Text { get; private set; }
    public List<string> Lines { get; private set; }
    public string Result { get; private set; }
    public CodeParser(string file)
    {
        Text = File.ReadAllText(file);
        Lines = new List<string>();
        Result = "";
    }
    public void Analize()
    {
        Lines = Utils.SplitTextIntoLines(Text);
        if ((Utils.CheckParens('{', '}', Lines).Count > 0) || (Utils.CheckParens('(', ')', Lines).Count > 0))
            Result = "error parens";
        else
        {
            List<List<string>> stringer = new List<List<string>>();
            int c = 0;
            while (c < Lines.Count)
            {
                stringer.Add(new List<string>());
                List<Position> positions = Utils.GetPositionsWords(Lines[c], (uint)c);
                stringer[c] = Utils.SplitTextIntoWords(Lines[c]);
                List<Token> tokens = Utils.GetTokens(stringer[c], positions, ref values);
                MathUtils.Calculate(tokens, ref values);
                c++;
            }
        }
    }
}