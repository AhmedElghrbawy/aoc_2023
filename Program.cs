using System.Text.RegularExpressions;

string path = @"input.txt";


var lines = File.ReadAllLines(path);
long ans = 0;

foreach (var line in lines)
{
    var nums = line.Split().Select(num => Int64.Parse(num)).ToList();
    var first = new Dictionary<long, long>();

    var levels = new List<List<long>>();
    levels.Add(nums);
    first[0] = nums[0];
    long curlevel = 1;

    while (levels[^1].Any(num => num != 0))
    {
        var lastLevel = levels[^1];

        var newLevel = new List<long>();
        for (int i = 1; i < lastLevel.Count; i++)
        {
            newLevel.Add(lastLevel[i] - lastLevel[i - 1]);
        }

        levels.Add(newLevel);
        first[curlevel] = newLevel[0];
        curlevel++;
    }

    for (int i = levels.Count - 2; i >= 0; i--)
    {
        first[i] = first[i] - first[i + 1];
    }

    ans += first[0];
}

System.Console.WriteLine(ans);