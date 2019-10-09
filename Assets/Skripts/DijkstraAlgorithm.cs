using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DijkstraAlgorithm : MonoBehaviour
{
    public enum Col { none, red, black, green, orange, yellow, white = 100 };

    // Button to run/pause the execution of the algorithm
    public Button runbtn;

    // Button to either reset world (no running alg) or to stop the running alg
    public Text resetWorld;

    // saves all blocks that have to get visited next in the sequence of their f-value
    private PriorityQueue queue;

    // blocks get only added to priority queue as long as there is no way to the target. after that, the queue only gets emptied.
    public bool running, stopped;

    private int timer;

    // active block, start block and target block
    private Vector2Int active, start;

    // saves which blocks are visited
    private bool[][] visited;

    // saves the distances to all blocks from the start
    private int[][] distances;

    // saves the predecessors of each block as Vector2Int
    private Vector2Int[][] pred;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if (timer == 20)
        {
            queue = new PriorityQueue();
            int len = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks.Length;
            distances = new int[len][];
            for (int i = 0; i < len; i++)
            {
                distances[i] = new int[len];
                for (int j = 0; j < distances[i].Length; j++)
                {
                    distances[i][j] = int.MaxValue;
                }
            }

            pred = new Vector2Int[len][];
            for (int i = 0; i < len; i++)
            {
                pred[i] = new Vector2Int[len];
            }

            visited = new bool[len][];
            for (int i = 0; i < len; i++)
            {
                visited[i] = new bool[len];
            }

        }

        if (timer % (int)(1 / Stats.speed) == 0)
        {
            if (running && timer > 20)
            {
                DijkstraStep();
            }
        }
    }


    public void DijkstraStart()
    {
        start = FindStart();
        if (start.x == int.MaxValue) Debug.LogError("NO STARTING POINT FOUND!");
        visited[start.x][start.y] = true;
        distances[start.x][start.y] = 0;
        queue.Add(start, 0);
        running = true;
    }

    void DijkstraStep()
    {
        GameObject cube = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[active.x][active.y];
        if (cube == GridManagement.target)
        {
            MarkPath(start, active);
            EndDijkstra();
            return;
        }
        active = queue.Extract();
        cube = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[active.x][active.y];
        // mark path orange
        if (cube != GridManagement.start && cube != GridManagement.target)
        {
            cube.GetComponent<BlockMove>().mark((int)Col.orange);
        }

        Vector2Int neighbor = new Vector2Int(active.x + 1, active.y);
        bool exists = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y] && GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y].GetComponent<BlockMove>().colorStatus != (int)Col.black;
        if (distances[neighbor.x][neighbor.y] > distances[active.x][active.y] + 1 && exists)
        {
            distances[neighbor.x][neighbor.y] = distances[active.x][active.y] + 1;
            pred[neighbor.x][neighbor.y] = active;
            queue.Add(neighbor, distances[neighbor.x][neighbor.y]);
        }
        
       
        neighbor = new Vector2Int(active.x, active.y + 1);
        exists = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y] && GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y].GetComponent<BlockMove>().colorStatus != (int)Col.black;
        if (distances[neighbor.x][neighbor.y] > distances[active.x][active.y] + 1 && exists)
        {
            distances[neighbor.x][neighbor.y] = distances[active.x][active.y] + 1;
            pred[neighbor.x][neighbor.y] = active;
            queue.Add(neighbor, distances[neighbor.x][neighbor.y]);
        }

        neighbor = new Vector2Int(active.x - 1, active.y);
        exists = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y] && GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y].GetComponent<BlockMove>().colorStatus != (int)Col.black;
        if (distances[neighbor.x][neighbor.y] > distances[active.x][active.y] + 1 && exists)
        {
            distances[neighbor.x][neighbor.y] = distances[active.x][active.y] + 1;
            pred[neighbor.x][neighbor.y] = active;
            queue.Add(neighbor, distances[neighbor.x][neighbor.y]);
        }

        neighbor = new Vector2Int(active.x, active.y - 1);
        exists = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y] && GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y].GetComponent<BlockMove>().colorStatus != (int)Col.black;
        if (distances[neighbor.x][neighbor.y] > distances[active.x][active.y] + 1 && exists)
        {
            distances[neighbor.x][neighbor.y] = distances[active.x][active.y] + 1;
            pred[neighbor.x][neighbor.y] = active;
            queue.Add(neighbor, distances[neighbor.x][neighbor.y]);
        }

        visited[active.x][active.y] = true;
    }

    public void EndDijkstra()
    {
        running = false;
        stopped = false;
        runbtn.GetComponentInChildren<Text>().text = "Run";
        queue = new PriorityQueue();
        int len = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks.Length;
        distances = new int[len][];
        for (int i = 0; i < len; i++)
        {
            distances[i] = new int[len];
            for (int j = 0; j < distances[i].Length; j++)
            {
                distances[i][j] = int.MaxValue;
            }
        }

        pred = new Vector2Int[len][];
        for (int i = 0; i < len; i++)
        {
            pred[i] = new Vector2Int[len];
        }

        visited = new bool[len][];
        for (int i = 0; i < len; i++)
        {
            visited[i] = new bool[len];
        }

    }



    // gives back grid coordinates of start or int.MaxValue, int.MaxValue if no start is found
    private Vector2Int FindStart()
    {
        Vector2Int output = new Vector2Int(int.MaxValue, int.MaxValue);
        int len = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks.Length;
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                if (GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[i][j] && GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[i][j].GetComponent<BlockMove>().colorStatus == (int)Col.red)
                {
                    output.x = i;
                    output.y = j;
                    return output;
                }
            }
        }
        return output;
    }

    private void MarkPath(Vector2Int start, Vector2Int target)
    {
        Vector2Int active = pred[target.x][target.y];
        while (!active.Equals(start))
        {
            if (!GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[active.x][active.y])
            {
                Debug.LogError("No path found!");
                return;
            }
            GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[active.x][active.y].GetComponent<BlockMove>().mark((int)Col.yellow);
            active = pred[active.x][active.y];
        }
    }
}

public class PriorityQueue {
    public List<Vector3Int> sortedList;
    public int length;

    public PriorityQueue() {
        length = 0;
        sortedList = new List<Vector3Int>();
    }

    public void Add(Vector2Int coordinates, int key) {
        Vector3Int toAdd = new Vector3Int { x = coordinates.x, y = coordinates.y, z = key };
        for (int i = length - 1; i >= 0; i--)
        {
            if (sortedList[i].z < key)
            {
                sortedList.Insert(i + 1, toAdd);
                length++;
                return;
            }
        }
        sortedList.Insert(0, toAdd);
        length++;
    }

    public Vector2Int Extract() {
        Vector2Int output = new Vector2Int();
        output.x = sortedList[0].x;
        output.y = sortedList[0].y;
        sortedList.RemoveAt(0);
        length--;
        return output;
    }
}
