string path = @"input.txt";


var lines = File.ReadAllLines(path);

var mat = lines.Select(line => line.ToCharArray()).ToArray();

int n = mat.Length;
int m = mat[0].Length;

long ans = 0;

for (int j = 0; j < m; j++)
{
    int last = 0;
    for (int i = 0; i < n; i++)
    {
        var cell = mat[i][j];

        if (cell == 'O')
        {
            ans += n - last;
            last++;
        }
        else if (cell == '#')
        {
            last = i + 1;
        }
    }
}


System.Console.WriteLine(ans);