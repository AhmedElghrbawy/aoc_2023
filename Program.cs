using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Transactions;


(int i, int j) UP =   (-1, 0);
(int i, int j) DOWN = (1, 0);
(int i, int j) LEFT = (0, -1);
(int i, int j) RIGHT = (0, 1);


var transitions = new Dictionary<char, (int i, int j)[]>()
{
    {'|', [DOWN, UP]},
    {'-', [LEFT, RIGHT]},
    {'L', [UP, RIGHT]},
    {'J', [UP, LEFT]},
    {'7', [DOWN, LEFT]},
    {'F', [DOWN, RIGHT]},
    {'S', [DOWN, UP, LEFT, RIGHT]},
};



string path = @"input.txt";


var lines = File.ReadAllLines(path);
var mat = lines.Select(line => line.ToCharArray()).ToArray();
int n = mat.Length;
int m = mat[0].Length;
long ans = 0;

var visited = new bool[n, m];
var start = GetStartIndex(mat);

Dfs(start.i, start.j, (-1, -1), 0);

System.Console.WriteLine(ans);







void Dfs(int i, int j, (int i, int j) parent, int order)
{
    visited[i, j] = true;
    char val = mat[i][j];

    if (!transitions.ContainsKey(val))
        return;
    // System.Console.WriteLine((val, i, j));

    foreach (var trans in transitions[val])
    {
        (int i, int j) next = (trans.i + i, trans.j + j);

        if (next.i < 0 || next.i >= n || next.j < 0 || next.j >= m)
            continue;
        // System.Console.WriteLine(next);
        // System.Console.WriteLine(visited[next.i, next.j]);

        if (visited[next.i, next.j] && parent != next && next == start)
        {
            long score = order / 2;
            score += order % 2 == 0 ? 0 : 1;
            ans = Math.Max(score, ans);
        }

        if (!visited[next.i, next.j])
            Dfs(next.i, next.j, (i, j), order + 1);
    }


}



(int i, int j) GetStartIndex(char[][] mat)
{
    for (int i = 0; i < mat.Length; i++)
    {
        for (int j = 0; j < mat.Length; j++)
        {
            if (mat[i][j] == 'S')
                return (i, j);
        }
    }

    throw new UnreachableException();
}


