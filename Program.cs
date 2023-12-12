string path = @"input.txt";


var lines = File.ReadAllLines(path);

long ans = 0;
foreach (var line in lines)
{
    var splitedLine = line.Split();
    var symbols = splitedLine[0];
    var groups = splitedLine[1].Split(',').Select(num => Int32.Parse(num)).ToArray();
    
    symbols = String.Join('?', Enumerable.Repeat(symbols, 5));
    groups = Enumerable.Repeat(groups, 5).SelectMany(x => x).ToArray();

    
    int n = groups.Length;
    int m = symbols.Length;

    var dp = new long?[n, m];
    var suffixBounds = new int[m + 2];
    for (int i = m - 1; i >= 0; i--)
    {
        suffixBounds[i] = suffixBounds[i + 1];

        if (symbols[i] == '#')
            suffixBounds[i]++;
    }

    ans += Dfs(0, 0, symbols, groups, suffixBounds, dp);

}


System.Console.WriteLine(ans);

long Dfs(int index, int gIdx, string symbols, int[] groups, int[] suffix, long?[,] dp)
{
    if (gIdx >= groups.Length)
    {
        return suffix[index] == 0 ? 1 : 0;
    }

    if (index >= symbols.Length)
        return 0;

    if (dp[gIdx, index] is not null)
        return (long) dp[gIdx, index]!;

    var g = groups[gIdx];
    long curAnswer = 0;
    for (int i = index; i < symbols.Length; i++)
    {
        if (i > index && symbols[i - 1] == '#')
            break;
        
        if (CanTake(symbols, i, g))
        {
            // System.Console.WriteLine(("can take", i, gIdx));
            curAnswer += Dfs(i + g + 1, gIdx + 1, symbols, groups, suffix, dp);
        }

    }

    dp[gIdx, index] = curAnswer;
    return curAnswer;
}

bool CanTake(string line, int j, int group)
{
    if (j + group - 1 >= line.Length)
        return false;

    if (j != 0 && line[j - 1] == '#')
        return false;

    if (j + group < line.Length && line[j + group] == '#')
        return false;

    return Enumerable.Range(j, group).Count(i => line[i] == '?' || line[i] == '#') == group;
}