using System.Diagnostics;

string path = @"input.txt";


var lines = File.ReadAllLines(path);
long ans = 0;

var mat = lines.Select(l => l.ToCharArray().Select(c => (c - '0')).ToArray()).ToArray();

int n = mat.Length;
int m = mat[0].Length;
// System.Console.WriteLine((n, m));

//          u   r  d  l                     
int[] di = [-1, 0, 1, 0];
int[] dj = [0, 1, 0, -1];
const int STEPLIMIT = 3;


var adjList = new Dictionary<Node, List<(Node node, int cost)>>();

for (int i = 0; i < n; i++)
{
    for (int j = 0; j < m; j++)
    {
        for (int d = 0; d < di.Length; d++)
        {
            for (int s = STEPLIMIT - 1; s >= 0; s--)
            {
                var node = new Node(i, j, d, s);
                adjList[node] = [];

                for (int nd = 0; nd < di.Length; nd++)
                {
                    if (Math.Abs(nd - d) == 2)
                        continue;

                    int sr = d == nd ? s - 1 : STEPLIMIT - 1;
                    
                    int ni = i;
                    int nj = j;
                    int cost = 0;

                    for (; sr >= 0; sr--)
                    {
                        ni += di[nd];
                        nj += dj[nd];

                        if (ni < 0 || ni >= n || nj < 0 || nj >= m)
                            continue;

                        cost += mat[ni][nj];

                        var nextNode = new Node(ni, nj, nd, sr);
                        adjList[node].Add((nextNode, cost));
                    }
                    
                }
            }
        
        }
    }
}

var distTo = new Dictionary<Node, int>();


var pq = new PriorityQueue<Node, int>();


// should run the algorithm for both of these two nodes and get the min result
// but I'm lazy and will just change the code manually
var rNode = new Node(0, 1, 1, 2); 
var dNode = new Node(1, 0, 3, 2);


pq.Enqueue(dNode, mat[1][0]);
distTo[dNode] = mat[1][0];

while (pq.Count > 0)
{
    pq.TryDequeue(out var curNode, out int dist);
    if (distTo.ContainsKey(curNode) && distTo[curNode] < dist)
        continue;

    if (curNode.I == n - 1 && curNode.J == m - 1)
    {
        System.Console.WriteLine(dist);
        break;
    }

    foreach (var edge in adjList[curNode])
    {
        int newDist = dist + edge.cost;

        if (!distTo.ContainsKey(edge.node) || distTo[edge.node] > newDist)
        {
            distTo[edge.node] = newDist;
            pq.Enqueue(edge.node, newDist);
        }
    }
} 



record Node(int I, int J, int Direction, int StepsRem);