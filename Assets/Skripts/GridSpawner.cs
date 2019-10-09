using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpawner : MonoBehaviour
{
    public enum Col { none, red, black, green, orange, yellow, white = 100 };
    private int timer;
    public GameObject block;
    public GameObject[][] blocks;
    // contains all blocks in the order they got created, dont use for pathfinding
    public List<GameObject> unsortedBlocks = new List<GameObject>();

    void Start()
    {
        Stats.SetDefault();
        SpawnGrid();
        timer = 0;
    }

    private void Update()
    {
        timer++;
        if (timer == 10)
        {
            List<int[]> toRemove = new List<int[]>();
            for (int i = 0; i<blocks.Length; i++)
            {
                for (int j = 0; j<blocks[i].Length; j++)
                {
                    if (blocks[i][j])
                    {
                        GameObject b = blocks[i][j];
                        Vector3 screenPoint = Camera.main.WorldToViewportPoint(b.transform.position);
                        bool onScreen = screenPoint.z > 0 && screenPoint.x > -.1f && screenPoint.x < 1.1f && screenPoint.y > -.3f && screenPoint.y < .9f;
                        if (!onScreen)
                        {
                            Destroy(b);
                            // save coordinates of deletable position
                            int[] arr = { i, j };
                            toRemove.Add(arr);
                        }
                    }
                }
            }
            // Remove deleted blocks from list
            foreach(int[] a in toRemove)
            {
                blocks[a[0]][a[1]] = null;
            }
        }
    }

    private void SpawnGrid() {
        //Spawn all blocks till each corner of the screen is filled
        bool uncompleted = true;
        Vector3 pos;
        for(int i = 0; uncompleted; i++) {
            for(int j = -i; j<=i; j++) {
                if (i > 100) uncompleted = false;
                if (Mathf.Abs(j) == i)
                {
                    // Spawn side rows
                    for (int k = -i; k <= i; k++)
                    {
                        pos = new Vector3(Stats.gridSize * k, 0, Stats.gridSize * j);
                        unsortedBlocks.Add(Instantiate(block, pos, Quaternion.identity));
                    }
                }
                else
                {
                    // Spawn uper and lower cubes
                    pos = new Vector3(Stats.gridSize * i, 0, Stats.gridSize * j);
                    unsortedBlocks.Add(Instantiate(block, pos, Quaternion.identity));
                    pos = new Vector3(Stats.gridSize * -i, 0, Stats.gridSize * j);
                    unsortedBlocks.Add(Instantiate(block, pos, Quaternion.identity));
                }
            }

            // Check if the whole screen is full of blocks with rays at each corner of the screen
            Ray ray1 = Camera.main.ScreenPointToRay(new Vector2(Screen.width, Screen.height));
            Ray ray2 = Camera.main.ScreenPointToRay(new Vector2(Screen.width, 0));
            Ray ray3 = Camera.main.ScreenPointToRay(new Vector2(0, Screen.height));
            Ray ray4 = Camera.main.ScreenPointToRay(new Vector2(0, 0));
            Physics.Raycast(ray1, out RaycastHit hit1, 100000.0f);
            Physics.Raycast(ray2, out RaycastHit hit2, 100000.0f);
            Physics.Raycast(ray3, out RaycastHit hit3, 100000.0f);
            Physics.Raycast(ray4, out RaycastHit hit4, 100000.0f);
            if (hit1.transform && hit2.transform && hit3.transform && hit4.transform)
            {
                uncompleted = false;
                int len = i * 2 + 1;
                // initialize 2 dimensional array "blocks" (l * l)
                blocks = new GameObject[len][];
                for(int j = 0; j<len; j++) {
                    blocks[j] = new GameObject[len];
                }
            }
        }

        // fill the blocks array with GameObjects, then delete the temporary list
        foreach(GameObject b in unsortedBlocks) {
            int x, z;
            x = (int) Mathf.Round(b.transform.position.x / Stats.gridSize);
            z = (int) Mathf.Round(b.transform.position.z / Stats.gridSize);
            int halflen = (int)(blocks.Length / 2);
            blocks[x + halflen][z + halflen] = b;
        }

    }

    private void DespawnGrid() {
        for(int i = 0; i<unsortedBlocks.Count; i++) {
            Destroy(unsortedBlocks[i]);
        }
        unsortedBlocks = new List<GameObject>();
    }

    public void ResetWorld() {
        for(int i = 0; i<unsortedBlocks.Count; i++)
        {
            if (unsortedBlocks[i])
            {
                unsortedBlocks[i].GetComponent<BlockMove>().mark(100);
            }
        }
        GameObject.Find("Algorithms").GetComponent<AlgorithmManager>().EndAll();
    }

    public Button runbtn;
    //resets the world except the user colored blocks; only if button says "run" and not "stop"
    public void ResetWorldExcept(bool checkIfStopped = true) {
        GameObject alg = GameObject.Find("Algorithms");
        bool oneAlgStopped = alg.GetComponent<DijkstraAlgorithm>().stopped || alg.GetComponent<AStarAlgorithm>().stopped;
        if (checkIfStopped && (!runbtn.GetComponentInChildren<Text>().text.Equals("run") || oneAlgStopped)) return;
        for(int i = 0; i < blocks.Length; i++) { 
            for(int j = 0; j<blocks[i].Length; j++) {
                if (blocks[i][j]){
                    int color = blocks[i][j].GetComponent<BlockMove>().colorStatus;
                    if (color != (int)Col.black && color != (int)Col.red && color != (int)Col.green) {
                        blocks[i][j].GetComponent<BlockMove>().mark(100);
                    }
                }
            }
        }
    }
}
