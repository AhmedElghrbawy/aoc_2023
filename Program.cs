string path = @"input.txt";


var lines = File.ReadAllLines(path);
long ans = 0;
var mat = lines.Select(line => line.ToCharArray()).ToArray();

int n = mat.Length;
int m = mat[0].Length;



for (int j = 0; j < m; j++)
{
    ans = Math.Max(ans, StartBeam(0, j, Direction.DOWN));
    ans = Math.Max(ans, StartBeam(n - 1, j, Direction.UP));
}

for (int i = 0; i < n; i++)
{
    ans = Math.Max(ans, StartBeam(i, 0, Direction.RIGHT));
    ans = Math.Max(ans, StartBeam(i, m - 1, Direction.LEFT));
}



System.Console.WriteLine(ans);


long StartBeam(int x, int y, Direction d)
{
    var visited = new bool[n, m, 4];
    var q = new Queue<(int i, int j, Direction direction)>();
    q.Enqueue((x, y, d));
    visited[x, y, (int) d] = true;

    while (q.Count != 0)
    {
        var curCell = q.Dequeue();

        var nextCells = GetNextCells(curCell);

        foreach (var cell in nextCells)
        {
            if (!IsValidTransition(cell, visited))
                continue;

            q.Enqueue(cell);
            visited[cell.i, cell.j, (int) cell.direction] = true;    
        }

    }

    long score = 0;
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
        { 
            if (Enumerable.Range(0, 4).Any(d => visited[i, j, d]))
                score++;
        }
    }

    return score;
}




(int i, int j, Direction direction)[] GetNextCells((int i, int j, Direction direction) cell) 
{
    var (i, j, d) = cell;
    char type = mat[i][j];

    return (type, cell.direction) switch 
    {
        ('.', Direction.UP) => [(i - 1, j, d)],
        ('.', Direction.DOWN) => [(i + 1, j, d)],
        ('.', Direction.LEFT) => [(i, j - 1, d)],
        ('.', Direction.RIGHT) => [(i, j + 1, d)],

        ('/', Direction.UP) => [(i, j + 1, Direction.RIGHT)],
        ('/', Direction.DOWN) => [(i, j - 1, Direction.LEFT)],
        ('/', Direction.LEFT) => [(i + 1, j, Direction.DOWN)],
        ('/', Direction.RIGHT) => [(i - 1, j, Direction.UP)],


        ('\\', Direction.UP) => [(i, j - 1, Direction.LEFT)],
        ('\\', Direction.DOWN) => [(i, j + 1, Direction.RIGHT)],
        ('\\', Direction.LEFT) => [(i - 1, j, Direction.UP)],
        ('\\', Direction.RIGHT) => [(i + 1, j, Direction.DOWN)],


        ('-', Direction.UP or Direction.DOWN) => [(i, j - 1, Direction.LEFT), (i, j + 1, Direction.RIGHT)],
        ('-', Direction.LEFT) => [(i, j - 1, d)],
        ('-', Direction.RIGHT) => [(i, j + 1, d)],


        ('|', Direction.LEFT or Direction.RIGHT) => [(i - 1, j, Direction.UP), (i + 1, j, Direction.DOWN)],
        ('|', Direction.DOWN) => [(i + 1, j, d)],
        ('|', Direction.UP) => [(i - 1, j, d)],

        _ => throw new InvalidOperationException($"{type} {d} ({i},{j})")
    };
}


bool IsValidTransition((int i, int j, Direction direction) cell, bool[,,] visited)
{
    var (i, j, d) = cell;
    return i >= 0 && i < n && j >= 0 && j < m && !visited[i, j, (int)d];
}




enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}