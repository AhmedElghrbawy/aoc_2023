using System.Text.RegularExpressions;

string path = @"input.txt";


var lines = File.ReadAllLines(path);

var directionIndex = new Dictionary<char, int>()
{
    {'L', 0},
    {'R', 1}
};

var adjList = new Dictionary<string, string[]>();
var directions = lines[0];

for (int i = 2; i < lines.Length; i++)
{
    var line = lines[i];
    var l = line.Split("=");
    string node = l[0].Trim();

    var edges = l[1].Trim().Replace("(", "").Replace(")", "").Split(',', StringSplitOptions.TrimEntries);
    adjList[node] = edges;
}

// System.Console.WriteLine(adjList.Count); 802

long ans = 0;
var inProgress = adjList.Keys.Where(k => k.EndsWith('A')).ToArray();
System.Console.WriteLine(inProgress.Length);
var dp = new Dictionary<string, HashSet<int>>();


foreach (var item in inProgress)
{
    string node = item;
    var zSteps = new HashSet<long>();
    long step = 0;


    for (int i = 0; i < directions.Length; i = (i + 1) % directions.Length)
    {
        if (node.EndsWith('Z'))
            zSteps.Add(step);

        if (zSteps.Count == inProgress.Length)
        {
            System.Console.WriteLine("Found all");
            break;
        }
        if (dp.ContainsKey(node) && dp[node].Contains(i))
        {
            System.Console.WriteLine("That is a cycle bro");
            System.Console.WriteLine((node, i));
            break;
        }

        if (!dp.ContainsKey(node))
            dp[node] = new();
        
        dp[node].Add(i);


        char direction = directions[i];
        node = adjList[node][directionIndex[direction]];
        step++;
    }

    System.Console.WriteLine(String.Join(" ", zSteps));
}

// this problem is not well descriped
System.Console.WriteLine(ans);
