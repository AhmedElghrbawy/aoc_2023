using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

string path = @"input.txt";


var lines = File.ReadAllLines(path);

long ans = 0;

var hands = new List<(Hand hand, long bid)>();

foreach (var line in lines)
{
    var s = line.Split();

    hands.Add((new Hand(s[0]), Int64.Parse(s[1])));
}


hands = hands.OrderBy(h => h.hand).ToList();

// System.Console.WriteLine(String.Join(" ", hands));

for (int i = 0; i < hands.Count; i++)
{
    ans += (i + 1) * hands[i].bid;
}

System.Console.WriteLine(ans);






class Hand : IComparable<Hand>
{
    public string Value { get; set; }


    public Hand(string value)
    {
        Value = value;

        var count = new Dictionary<char, int>();
        foreach (var c in value)
        {
            count[c] = count.GetValueOrDefault(c) + 1;
        }

        if (count.ContainsKey(joker) && count.Count > 1)
        {
            // System.Console.WriteLine("found joker");
            int jokerCount = count[joker];
            count.Remove(joker);

            var largestKey = count.Keys.MaxBy(k => count[k]);
            count[largestKey] += jokerCount;
        }

        this.type = String.Join("", count.Values.OrderByDescending(v => v));
        // System.Console.WriteLine((value, type));
        
        if (!typePower.ContainsKey(type))
            throw new InvalidDataException($"The give value doesn't have an equvilant power {type}");

    }

    private string type;

    private static char joker = 'J';
    private static Dictionary<string, int> typePower = new Dictionary<string, int>
    {
        {"11111", 0},
        {"2111", 1},
        {"221", 2},
        {"311", 3},
        {"32", 4},
        {"41", 5},
        {"5", 6},
    };

    private static Dictionary<char, int> cardPower = new Dictionary<char, int>
    {
        {'J', -1},
        {'2', 0},
        {'3', 1},
        {'4', 2},
        {'5', 3},
        {'6', 4},
        {'7', 5},
        {'8', 6},
        {'9', 7},
        {'T', 8},
        {'Q', 10},
        {'K', 11},
        {'A', 12},
    };

    public int CompareTo(Hand? other)
    {
        if (other is null)
            return -1;

        if (typePower[this.type] == typePower[other.type])
        {
            for (int i = 0; i < this.Value.Length; i++)
            {
                if (this.Value[i] == other.Value[i])
                    continue;

                return cardPower[this.Value[i]].CompareTo(cardPower[other.Value[i]]);
            }
        }

        return typePower[this.type].CompareTo(typePower[other.type]);
    }

    public override string ToString()
    {
        return this.Value;
    }
}