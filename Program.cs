using System.Globalization;

string path = @"input.txt";


var lines = File.ReadAllLines(path);
var mat = lines.Select(line => line.ToCharArray()).ToArray();
int n = mat.Length;
int m = mat[0].Length;
long ans = 0;

const long factor = 1000000;
var galaxies = new List<(int i, int j)>();

for (int i = 0; i < n; i++)
{
    for (int j = 0; j < m; j++)
    {
        if (mat[i][j] == '#')
            galaxies.Add((i, j));
    }
}


// (x, y) = prefix[x + 1] - preifx[y]
var prefixColumn = new long[m + 1];
var prefixRows = new long[n + 1];

for (int j = 0; j < m; j++)
{
    prefixColumn[j + 1] = prefixColumn[j];
    if (Enumerable.Range(0, n).All(i => mat[i][j] == '.'))
    {
        prefixColumn[j + 1]++;
    }
    
}

for (int i = 0; i < n; i++)
{
    prefixRows[i + 1] = prefixRows[i];
    if (mat[i].All(item => item == '.'))
    {
        prefixRows[i + 1]++;
    }
}


for (int i = 0; i < galaxies.Count; i++)
{
    var g1 = galaxies[i];
    for (int j = i + 1; j < galaxies.Count; j++)
    {
        var g2 = galaxies[j];
        long distance = Math.Abs(g1.i - g2.i) + Math.Abs(g1.j - g2.j);

        distance += GetPrefix(g1.j, g2.j, prefixColumn);
        distance += GetPrefix(g1.i, g2.i, prefixRows);

        ans += distance;
    }
}
System.Console.WriteLine(ans);

long GetPrefix(int x, int y, long[] prefix)
{
    if (x < y)
    {
        (x, y) = (y, x);
    }

    return ((prefix[x + 1] - prefix[y])*factor) - (prefix[x + 1] - prefix[y]);
}