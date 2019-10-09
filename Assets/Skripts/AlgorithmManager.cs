using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmManager : MonoBehaviour
{
    public Dropdown dropdown;
    public Text runbtn;
    public Text resetWorld;


    public void StartSelectedAlgorithm() {
        if (runbtn.text.Equals("Run")) {
            resetWorld.text = "Stop Algorithm";
        }
        if (gameObject.GetComponent<DijkstraAlgorithm>().running) 
        {
            gameObject.GetComponent<DijkstraAlgorithm>().running = false;
            gameObject.GetComponent<DijkstraAlgorithm>().stopped = true;
            runbtn.text = "Run";
            return;
        }
        if (gameObject.GetComponent<AStarAlgorithm>().running)
        {
            gameObject.GetComponent<AStarAlgorithm>().running = false;
            gameObject.GetComponent<AStarAlgorithm>().stopped = true;
            runbtn.text = "Run";
            return;
        }
        if (gameObject.GetComponent<DijkstraAlgorithm>().stopped)
        {
            gameObject.GetComponent<DijkstraAlgorithm>().running = true;
            gameObject.GetComponent<DijkstraAlgorithm>().stopped = false;
            runbtn.text = "Pause";
            return;
        }
        if (gameObject.GetComponent<AStarAlgorithm>().stopped)
        {
            gameObject.GetComponent<AStarAlgorithm>().running = true;
            gameObject.GetComponent<AStarAlgorithm>().stopped = false;
            runbtn.text = "Pause";
            return;
        }
        switch (dropdown.value) {
            case 0:
                gameObject.GetComponent<DijkstraAlgorithm>().DijkstraStart();
                break;
            case 1:
                gameObject.GetComponent<AStarAlgorithm>().AStarStart();
                break;
        }
        runbtn.text = "Pause";
    }

    public void EndAll()
    {
        gameObject.GetComponent<DijkstraAlgorithm>().EndDijkstra();
        gameObject.GetComponent<AStarAlgorithm>().EndAStar();
    }
}
