using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarAlgorithm : MonoBehaviour
{
    public enum Col { none, red, black, green, orange, yellow, white = 100 };

    // Button to run/pause the execution of the algorithm
    public Button runbtn;

    // Button to either reset world (no running alg) or to stop the running alg
    public Text resetWorld;

    // saves all blocks that have to get visited next in the sequence of their f-value
    private PriorityQueue queue;

    // blocks get only added to priority queue as long as there is no way to the target. after that, the queue only gets emptied.
    private bool queueAdding;

    public bool running, stopped;

    private int timer;

    // active block, start block and target block
    private Vector2Int active, start, target;

    // saves the expected distance from each block to the target
    private int[][] hValues;

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

            hValues = new int[len][];
            for (int i = 0; i < len; i++)
            {
                hValues[i] = new int[len];
                for (int j = 0; j < hValues[i].Length; j++)
                {
                    hValues[i][j] = int.MaxValue;
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
                AStarStep();
            }
        }
    }

    public void AStarStart() {
        queueAdding = true;
        start = FindStart();
        target = FindTarget();
        GetHValues(target);
        if (start.x == int.MaxValue) Debug.LogError("NO STARTING POINT FOUND!");
        visited[start.x][start.y] = true;
        distances[start.x][start.y] = 0;
        queue.Add(start, hValues[start.x][start.y]);
        running = true;
    }

    void AStarStep() {
        if (queue.length == 0) {
            MarkPath(start, target);
            EndAStar();
            return;
        }
        active = queue.Extract();
        // mark path orange
        if (active != start && active != target)
        {
            GameObject cube = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[active.x][active.y];
            cube.GetComponent<BlockMove>().mark((int)Col.orange);
        }
        for(int i = -1; i<=1; i += 2) {
            Vector2Int neighbor = new Vector2Int(active.x + i, active.y);
            GameObject block = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y];
            bool exists = block && block.GetComponent<BlockMove>().colorStatus != (int)Col.black;
            bool onScreen = false;
            // if block exists check if the block has to be on screen (border) and if so, check if the Renderer says, that the block is visible
            if (exists) {
                Vector3 pos = Camera.main.WorldToScreenPoint(block.transform.position);
                float uiHeight = GameObject.Find("UIBackground").GetComponent<RectTransform>().sizeDelta.y;
                onScreen = !Stats.onScreenOnly || Util.IsOnScreen(block, uiHeight);
            }
            if (distances[neighbor.x][neighbor.y] > distances[active.x][active.y] + 1 && exists && onScreen)
            {
                distances[neighbor.x][neighbor.y] = distances[active.x][active.y] + 1;
                pred[neighbor.x][neighbor.y] = active;
                if(queueAdding) queue.Add(neighbor, distances[neighbor.x][neighbor.y] + hValues[neighbor.x][neighbor.y]);
            }
        }
        for (int i = -1; i <= 1; i += 2)
        {
            Vector2Int neighbor = new Vector2Int(active.x, active.y + i);
            GameObject block = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[neighbor.x][neighbor.y];
            bool exists = block && block.GetComponent<BlockMove>().colorStatus != (int)Col.black;
            bool onScreen = false;
            if (exists) {
                Vector3 pos = Camera.main.WorldToScreenPoint(block.transform.position);
                float uiHeight = GameObject.Find("UIBackground").GetComponent<RectTransform>().sizeDelta.y;
                onScreen = !Stats.onScreenOnly || Util.IsOnScreen(block, uiHeight);
            }
            if (distances[neighbor.x][neighbor.y] > distances[active.x][active.y] + 1 && exists && onScreen)
            {
                distances[neighbor.x][neighbor.y] = distances[active.x][active.y] + 1;
                pred[neighbor.x][neighbor.y] = active;
                if(queueAdding) queue.Add(neighbor, distances[neighbor.x][neighbor.y] + hValues[neighbor.x][neighbor.y]);
            }
        }
        visited[active.x][active.y] = true;

        if (active.Equals(target)) {
            queueAdding = false;
        }
    }

    public void EndAStar() {
        queueAdding = true;
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

        hValues = new int[len][];
        for (int i = 0; i < len; i++)
        {
            hValues[i] = new int[len];
            for (int j = 0; j < hValues[i].Length; j++)
            {
                hValues[i][j] = int.MaxValue;
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
                if (GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[i][j] && GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[i][j].GetComponent<BlockMove>().colorStatus == 1)
                {
                    output.x = i;
                    output.y = j;
                    return output;
                }
            }
        }
        return output;
    }

    // gives back grid coordinates of start or int.MaxValue, int.MaxValue if no start is found
    private Vector2Int FindTarget()
    {
        Vector2Int output = new Vector2Int(int.MaxValue, int.MaxValue);
        int len = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks.Length;
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                if (GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[i][j] && GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[i][j].GetComponent<BlockMove>().colorStatus == 3)
                {
                    output.x = i;
                    output.y = j;
                    return output;
                }
            }
        }
        return output;
    }

    private void GetHValues(Vector2Int target) {
        for(int i = 0; i<hValues.Length; i++) {
            for(int j = 0; j<hValues[i].Length; j++) {
                hValues[i][j] = Mathf.Abs(i - target.x) + Mathf.Abs(j - target.y);
            }
        }
    }

    private void MarkPath(Vector2Int start, Vector2Int target)
    {
        Vector2Int active = pred[target.x][target.y];
        while (!active.Equals(start))
        {
            GameObject block = GameObject.Find("GridSpawner").GetComponent<GridSpawner>().blocks[active.x][active.y];
            if (block)
            {
                block.GetComponent<BlockMove>().mark((int)Col.yellow);
            }
            else {
                print("no path!");
                return;
            }
            active = pred[active.x][active.y];
        }
    }
}
