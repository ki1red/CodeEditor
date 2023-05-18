public class CodeParser
{
    public string Text { get; private set; }
    public List<string> Errors { get; private set; }
    public List<string> Completes { get; private set; }
    public CodeParser(string file)
    {
        Text = File.ReadAllText(file);
        Text = Text.Replace("\r", String.Empty);
        Errors = new List<string>();
        Completes = new List<string>();
    }
    public void Parse()
    {
        int index = 0;
        int startPosition = index;
        do
        {
            startPosition = index;
            char cur = Text[index];
            if (cur == '{')
            {
                int toN = Whiler(index, '\n');
                int toR = Whiler(index, '\r');
                int toParen = Whiler(index, '}');
                if (toParen == -1)
                {
                    Errors.Add($"Error: Unfinished single-line comment at {startPosition}");
                    index++;
                }
                else if (toN != -1 && toParen > toN)
                {
                    Errors.Add($"Error: Unfinished single-line comment from {startPosition} to the end of the line");
                    index = toN + 1;
                }
                else if (toR != -1 && toParen > toR)
                {
                    Errors.Add($"Error: Unfinished single-line comment from {startPosition} to the end of the line");
                    index = toR + 1;
                }
                else
                {
                    Completes.Add($"Complete: Finished single-line comment from {startPosition} to {toParen}");
                    index = toParen + 1;
                }
            }
            else if (index + 1 < Text.Length && cur == '/' && Text[index+1] == '/')
            {
                int toN = Whiler(index, '\n');
                int toR = Whiler(index, '\r');
                int result = (toN > toR) ? toN : toR;
                if (result != -1)
                {
                    Completes.Add($"Complete: Finished single-line comment from {startPosition} to the end of the line");
                    index = result;
                }
                else
                {
                    Completes.Add($"Complete: Finished single-line comment from {startPosition} to the end of the line");
                    index = Text.Length;
                }
            }
            else if (index + 1 < Text.Length && cur == '(' && Text[index + 1] == '*')
            {
                if (index + 2 >= Text.Length)
                {
                    Errors.Add($"Error: Unfinished multi-line comment at {startPosition}");
                    index +=2;
                    continue;
                }
                int toParen = index+2;
                do
                {
                    toParen = Whiler(toParen+1, ')');
                } while (toParen != -1 && Text[toParen - 1] != '*');

                if (toParen != -1)
                {
                    Completes.Add($"Complete: Finished multi-line comment at {startPosition} to {toParen}");
                    index = toParen + 1;
                }
                else
                {
                    Errors.Add($"Error: Unfinished multi-line comment at {startPosition}");
                    index +=2;
                }
            }
            else
            {
                int toMulti = index;
                do
                {
                    toMulti = Whiler(toMulti+1, '(');
                } while (toMulti != -1 && Text[toMulti + 1] != '*');
                int toComplete = index;
                do
                {
                    toComplete = Whiler(toComplete+1, '/');
                } while (toComplete != -1 && Text[toComplete + 1] != '/');
                int toPartial = Whiler(index, '{');

                int min = Text.Length;
                min = (toPartial != -1 && toPartial < min)?toPartial:min;
                if (index + 1 < Text.Length)
                {
                    min = (toComplete != -1 && toComplete < min) ? toComplete : min;
                    min = (toMulti != -1 && toMulti < min) ? toMulti : min;
                }
                index = min;
                Errors.Add($"Error: Unrecognized characters from {startPosition} to {min - 1}");
            }
        } while (index < Text.Length);
    }
    
    private int Whiler(int index, char stopCharacter)
    {
        for (int i = index; i < Text.Length; i++)
        {
            if (Text[i] == stopCharacter)
                return i;
        }
        return -1;
    }
}