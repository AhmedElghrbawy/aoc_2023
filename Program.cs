string path = @"input.txt";


var lines = File.ReadAllLines(path);
long ans = 0;

var mat = new List<char[]>();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (String.IsNullOrEmpty(line.Trim()) || i == lines.Length - 1)
    {
        ans += SolvePatern(mat);
        mat = new List<char[]>();
        continue;
    }

    mat.Add(line.ToCharArray());

}
System.Console.WriteLine(ans);

long SolvePatern(List<char[]> mat)
{
    int n = mat.Count;
    int m = mat[0].Length;
    long res = 0;

    for (int j = 1; j < m; j++)
    {
        if (IsMirroredH(j, mat))
            return j;
    }

    for (int i = 1; i < n; i++)
    {
        if (IsMirroredV(i, mat))
            return i * 100;
    }

    return res;
}


bool IsMirroredH(int c, List<char[]> mat)
{
    int m = mat[0].Length;

    int numColums = Math.Min(c, m - c);
    int start = c - numColums;
    int end = c + numColums - 1;

    bool alreadySmuged = false;

    foreach (var row in mat)
    {
        string before = String.Join("", row[start..c]);
        string after = String.Join("", row[c..(end + 1)].Reverse());

        if (before != after)
        {
            if (alreadySmuged)
                return false;

            if (!Smudgable(before, after))
                return false;

            alreadySmuged = true;
        }
    }

    return alreadySmuged;
}


bool IsMirroredV(int r, List<char[]> mat)
{
    int n = mat.Count;
    int m = mat[0].Length;

    int numRows = Math.Min(r, n - r);
    int start = r - numRows;
    int end = r + numRows - 1;

    bool alreadySmuged = false;

    for (int j = 0; j < m; j++)
    {
        string before = String.Join("", Enumerable.Range(start, numRows).Select(index => mat[index][j]));
        string after = String.Join("", Enumerable.Range(r, numRows).Select(index => mat[index][j]).Reverse());

        if (before != after)
        {
            if (alreadySmuged)
                return false;

            if (!Smudgable(before, after))
                return false;

            alreadySmuged = true;
        }
    }

    return alreadySmuged;
}


bool Smudgable(string a, string b)
{
    int n = a.Length;

    return Enumerable.Range(0, n).Count(i => a[i] != b[i]) == 1;
}