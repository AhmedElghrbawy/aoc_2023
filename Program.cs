using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

string path = @"input.txt";


var lines = File.ReadAllLines(path);

var times = new List<long>();
var distances = new List<long>();
long ans = 0;

foreach (var line in lines)
{
    var s = line.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
    var v = Int64.Parse(String.Join("", s));

    if (line.StartsWith("Time"))
    {
        times = [v];
    }
    else
    {
        distances = [v];
    }
}


for (int i = 0; i < times.Count; i++)
{
    int count = 0;

    for (int t = 1; t < times[i]; t++)
    {
        if (distances[i] < (times[i] - t) * t)
            count++;
    }
    System.Console.WriteLine(count);
    ans = ans == 0 ? count : ans * count;
}

System.Console.WriteLine(ans);
