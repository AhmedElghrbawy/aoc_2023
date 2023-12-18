using System.Diagnostics;

string path = @"input.txt";


var lines = File.ReadAllLines(path);
int ans = Int32.MaxValue;

var mat = lines.Select(l => l.ToCharArray().Select(c => (c - '0')).ToArray()).ToArray();
System.Console.WriteLine("hello");

int n = mat.Length;
int m = mat[0].Length;
// System.Console.WriteLine((n, m));

//          u   r  d  l                     
int[] di = [-1, 0, 1, 0];
int[] dj = [0, 1, 0, -1];
const int MINSETPS = 4;
const int MAXSTEPS = 10;

// each node in the graph is represented as a node that has a state of (cell pisition (i, j), in what direction I came to this cell, how many steps are made in that direction)
var adjList = new Dictionary<Node, List<(Node node, int cost)>>();

for (int i = 0; i < n; i++)
{
    for (int j = 0; j < m; j++)
    {
        for (int d = 0; d < di.Length; d++)
        {
            for (int s = MAXSTEPS; s >= MINSETPS; s--)
            {
                var node = new Node(i, j, d, s);
                adjList[node] = [];

                // from the current state, what are all the possible moves?
                // the possible moves depend on the next direction (nd)
                // the number of steps I can make (sr) in the next direction depends on (current direction (d) and num of steps made in d (s))

                for (int nd = 0; nd < di.Length; nd++)
                {
                    if (Math.Abs(nd - d) == 2)
                        continue;

                    int lim = d == nd ? MAXSTEPS - s : MAXSTEPS;
                    
                    int ni = i;
                    int nj = j;
                    int cost = 0;

                    for (int sr = 1; sr <= lim; sr++)
                    {
                        ni += di[nd];
                        nj += dj[nd];

                        if (ni < 0 || ni >= n || nj < 0 || nj >= m)
                            continue;

                        cost += mat[ni][nj];

                        if (sr < MINSETPS)
                            continue;

                        int cs = d == nd ? sr + s : sr;
                        var nextNode = new Node(ni, nj, nd, cs);
                        adjList[node].Add((nextNode, cost));
                    }
                    
                }
            }
        
        }
    }
}

int cc = 0;
for (int j = 1; j <= MAXSTEPS; j++)
{
    if (j >= m)
        break;

    cc += mat[0][j];

    if (j < MINSETPS)
        continue;

    var node = new Node(0, j, 1, j);
    var x = Sp(node, cc);
    System.Console.WriteLine(x);

    ans = Math.Min(x, ans);
    System.Console.WriteLine(ans);
    // return;
}

cc = 0;

for (int i = 1; i <= MAXSTEPS; i++)
{
    if (i >= n)
        break;

    cc += mat[i][0];

    if (i < MINSETPS)
        continue;

    var node = new Node(i, 0, 2, i);
    var x = Sp(node, cc);
    System.Console.WriteLine(x);

    ans = Math.Min(x, ans);
}

System.Console.WriteLine(ans);


int Sp(Node start, int startCost)
{
    var distTo = new Dictionary<Node, int>();
    var pq = new PriorityQueue<Node, int>();
    var parent = new Dictionary<Node, Node>();


    pq.Enqueue(start, startCost);
    distTo[start] = startCost;

    while (pq.Count > 0)
    {
        pq.TryDequeue(out var curNode, out int dist);
        if (distTo.ContainsKey(curNode) && distTo[curNode] < dist)
            continue;
        // System.Console.WriteLine((curNode, dist));

        if (curNode.I == n - 1 && curNode.J == m - 1)
        {
            // var c = curNode;
            // while (parent.ContainsKey(c))
            // {
            //     c = parent[c];
            //     System.Console.WriteLine((c, distTo[c]));
            // }

            return dist;
        }

        foreach (var edge in adjList[curNode])
        {
            int newDist = dist + edge.cost;

            if (!distTo.ContainsKey(edge.node) || distTo[edge.node] > newDist)
            {
                distTo[edge.node] = newDist;
                pq.Enqueue(edge.node, newDist);
                parent[edge.node] = curNode;
            }
        }
    } 

    return Int32.MaxValue;
}



record Node(int I, int J, int Direction, int StepsMade);