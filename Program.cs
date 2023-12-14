string path = @"input.txt";


var lines = File.ReadAllLines(path);

var mat = lines.Select(line => line.ToCharArray()).ToArray();

int n = mat.Length;
int m = mat[0].Length;

long ans = 0;
long limit = (long) 1e9;
var statesToIndex = new Dictionary<string, long>();

long numCycles = 1;

while (numCycles <= limit)
{
    var matCopy = mat.Select(row => row.ToArray()).ToArray();
    var matState = String.Join(" ", matCopy.Select(row => String.Join("", row)));


    if (statesToIndex.ContainsKey(matState))
    {
        long begIndex = statesToIndex[matState];
        long repeatLen = numCycles - begIndex;
        numCycles += ((limit - numCycles + 1) / repeatLen) * repeatLen;
    }

    statesToIndex[matState] = numCycles;

    DoCycle(matCopy);

    mat = matCopy;    
    numCycles++;
}


for (int i = 0; i < n; i++)
{
    for (int j = 0; j < m; j++)
    {
        if (mat[i][j] == 'O')
            ans += n - i;
    }
}
System.Console.WriteLine(ans);

void DoCycle(char[][] mat)
{
    TiltVertical(mat, 0, n, 1);
    TiltHorizontal(mat, 0, m, 1);
    TiltVertical(mat, n - 1, -1, -1);
    TiltHorizontal(mat, m - 1, -1, -1);
}

// North: begin = 0, end = n, shift = 1
// South: begin = n - 1, end = -1, shift = -1
void TiltVertical(char[][] mat, int begin, int end, int shift)
{
    
    for (int j = 0; j < m; j++)
    {
        int last = begin;
        for (int i = begin; i != end; i += shift)
        {
            var cell = mat[i][j];

            if (cell == 'O')
            {
                mat[i][j] = '.';
                mat[last][j] = 'O';
                last += shift;
            }
            else if (cell == '#')
            {
                last = i + shift;
            }
        }
    }
}

// east: begin = m - 1, end = -1, shift = -1
// west: begin = 0, end = m, shift = 1
void TiltHorizontal(char[][] mat, int begin, int end, int shift)
{
    for (int i = 0; i < n; i++)
    {
        int last = begin;
        for (int j = begin; j != end; j += shift)
        {
            var cell = mat[i][j];

            if (cell == 'O')
            {
                mat[i][j] = '.';
                mat[i][last] = 'O';
                last += shift;
            }
            else if (cell == '#')
            {
                last = j + shift;
            }
        }
    }
}
