
string path = @"input.txt";

var lines = File.ReadAllLines(path);

//          U   R  D  L
int[] di = [-1, 0, 1, 0];
int[] dj = [0, 1, 0, -1];
const string UP = "U";
const string DOWN = "D";
const string RIGHT = "R";
const string LEFT = "L";

var directionIdx = new Dictionary<string, int>
    {{UP, 0}, {RIGHT, 1}, {DOWN, 2}, {LEFT, 3}};

int ans = 0;

var HLine = new Dictionary<int, List<(int i, int j)>>();
var edgeDirection = new Dictionary<(int i, int j), string>(); 

int i = 0;
int j = 0;


for (int k = 0; k < lines.Length; k++)
{
    var line = lines[k];
    var splitedLine = line.Split();
    string direction = splitedLine[0];
    int shift = Int32.Parse(splitedLine[1]);
    string color = splitedLine[2];


    int dIdx = directionIdx[direction];
    for (int s = 1; s <= shift; s++)
    {
        i += di[dIdx];
        j += dj[dIdx];
        ans++;

        if (!HLine.ContainsKey(i))
                HLine[i] = [];

        HLine[i].Add((i, j));

        if (direction == UP || direction == DOWN)
        {
            edgeDirection[(i, j)] = direction;
        }
        else
        {
            if (k == lines.Length - 1 || s != shift)
                continue;

            edgeDirection[(i, j)] = lines[k + 1][0].ToString();

        }

    }

}

System.Console.WriteLine(ans);



foreach (var hLine in HLine.Values)
{
    hLine.Sort();
    string startDirection = edgeDirection[hLine[^1]];
    string curDirec = startDirection;

    for (int k = hLine.Count - 2; k >= 0; k--)
    {
        if (curDirec == startDirection)
            ans += hLine[k + 1].j - hLine[k].j - 1;

        curDirec = edgeDirection.ContainsKey(hLine[k]) ? edgeDirection[hLine[k]] : curDirec;
    }

}


System.Console.WriteLine(ans);