using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

string path = @"input.txt";


var lines = File.ReadAllLines(path);


var seeds = lines[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => Int64.Parse(s)).ToArray();

var intervals = new List<Interval>();
for (int i = 0; i < seeds.Length; i += 2)
{
    intervals.Add(new Interval(seeds[i], seeds[i] + seeds[i + 1] - 1));
}

var levels = new List<List<(long dest, long src, long len)>>();

foreach (var line in lines)
{
    if (line.EndsWith("map:"))
    {
        levels.Add([]);
        continue;
    }


    var rangess = line.Split(" ");

    if (rangess.Length != 3)
        continue;

    var ranges = rangess.Select(r => Int64.Parse(r)).ToArray();

    var l = levels[^1];
        
    l.Add((ranges[0], ranges[1], ranges[2]));
}

foreach (var level in levels)
{
   level.Sort((x, y) => x.src.CompareTo(y.src)); 
//    // System.Console.WriteLine(String.Join(" ", level));
}



// long ans = Int64.MaxValue;



int curLevel = 0;

while (curLevel < levels.Count)
{
    // System.Console.WriteLine(curLevel + "----------");
    // System.Console.WriteLine(string.Join(" ", intervals));
    var nextLevelIntervals = new List<Interval>();

    foreach (var interval in intervals)
    {
        for (int i = 0; i < levels[curLevel].Count; i++)
        {
            var range = levels[curLevel][i];
            // System.Console.WriteLine((interval, range));
            
            
            if (interval.End < range.src) // before
            {
                // System.Console.WriteLine("before");
                nextLevelIntervals.Add(new Interval
                {
                    Start = interval.Start,
                    End = interval.End,
                });
                break;
            }

            if (interval.Start >= range.src && interval.End < range.src + range.len) // all in
            {
                // System.Console.WriteLine("all in");
                nextLevelIntervals.Add(new Interval
                {
                    Start = range.dest + interval.Start - range.src,
                    End = range.dest + interval.End - range.src
                });

                break;
            }

            if (interval.Start < range.src && interval.End < range.src + range.len) // starts before
            {
                // System.Console.WriteLine("starts before");
                nextLevelIntervals.Add(new Interval
                {
                    Start = interval.Start,
                    End = range.src - 1,
                });

                nextLevelIntervals.Add(new Interval
                {
                    Start = range.dest,
                    End = range.dest + interval.End - range.src,
                });

                break;
            }


            if (interval.Start < range.src + range.len && interval.End >= range.src + range.len) // starts after
            {
                // System.Console.WriteLine("starts after");
                nextLevelIntervals.Add(new Interval
                {
                    Start = range.dest + interval.Start - range.src,
                    End = range.dest + range.len - 1,
                });

                interval.Start = range.src + range.len;
            }

            

            if (interval.Start < range.src && interval.End >= range.src + range.len) // include me
            {
                // System.Console.WriteLine("include me");
                nextLevelIntervals.Add(new Interval
                {
                    Start = interval.Start,
                    End = range.src - 1,
                });

                nextLevelIntervals.Add(new Interval
                {
                    Start = range.dest,
                    End = range.dest + range.len - 1,
                });


                interval.Start = range.src + range.len;
            }

            if (interval.Start >= range.src + range.len)
            {
                // System.Console.WriteLine(("out", interval));
                if (i == levels[curLevel].Count - 1)
                {
                    nextLevelIntervals.Add(new Interval
                    {
                        Start = interval.Start,
                        End = interval.End,
                    });
                }

                continue;
            }
        }
    }
    

    intervals = nextLevelIntervals;
    curLevel++;
    // System.Console.WriteLine(string.Join(" ", intervals));
}


System.Console.WriteLine(intervals.Min(interval => interval.Start));


class Interval
{
    public Interval() {}
    public Interval(long start, long end)
    {
        Start = start;
        End = end;
    }

    public long Start { get; set; }
    public long End { get; set; }

    public override string ToString()
    {
        return $"({Start}, {End})";
    }
}