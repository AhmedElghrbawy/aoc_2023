string path = @"input.txt";


var lines = File.ReadAllLines(path);

const int LEN = 256;
var steps = lines[0].Split(",");
long ans = 0;


var map = new LinkedList<Step>[LEN];
for (int i = 0; i < LEN; i++)
{
    map[i] = new();
}


foreach (var s in steps)
{
    var splitedStep = s.Split(new char[] { '=', '-' }, StringSplitOptions.RemoveEmptyEntries);
    string label = splitedStep[0];
    long lens = splitedStep.Length == 1 ? -1 : Int64.Parse(splitedStep[1]);

    var step = new Step(label, lens);
    
    if (splitedStep.Length == 1)
        Remove(step);
    else
        AddOrOverWrite(step);

}


for (int i = 0; i < LEN; i++)
{
    var slots = map[i];
    int ord = 1;
    foreach (var step in slots)
    {
        ans += (i + 1) * ord * step.Lens;
        ord++;
    }
}




System.Console.WriteLine(ans);


void AddOrOverWrite(Step step)
{
    int boxNum = step.GetHashCode();
    var slots = map[boxNum];

    foreach (var s in slots)
    {
        if (s == step)
        {
            s.Lens = step.Lens;
            return;
        }
    }

    slots.AddLast(step);

}

void Remove(Step step)
{
    int boxNum = step.GetHashCode();
    var slots = map[boxNum];

    var temp = slots.First;

    while (temp is not null && temp.Value != step)
    {
        temp = temp.Next;
    }

    if (temp is null)
        return;

    slots.Remove(temp);
}



class Step : IEquatable<Step>
{
    public string Label { get; set; }
    public long Lens { get; set; }
    public Step(string label, long lens)
    {
        Label = label;
        Lens = lens;
    }

    private const int MOD = 256;

    public bool Equals(Step? other)
    {
        if (other is null)
            return false;

        return this.Label == other.Label;
    }

    public override int GetHashCode()
    {
        int hashValue = 0;
        foreach (var c in Label)
        {
            hashValue += (int)c;
            hashValue *= 17;
            hashValue %= MOD;
        }

        return hashValue;
    }

    public static bool operator ==(Step step1, Step step2)
    {
        return step1.Equals(step2);
    }
    public static bool operator !=(Step step1, Step step2)
    {
        return !step1.Equals(step2);
    }

    public override string ToString()
    {
        return $"[{this.Label}, {this.Lens}]";
    }
}