using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Management : MonoBehaviour
{
    public Button speedbtn;

    // Start is called before the first frame update
    void Start()
    {
        Stats.SetDefault();
    }

    public void setSpeed(float speed) {
        Stats.speed = speed;
    }

    public void SpeedStep() {
        // Round and multiplicate with 1000 to avoid bad precision due to floating point numbers
        switch (Mathf.Round(Stats.speed*1000)) {
            case 1000:
                Stats.speed = .5f;
                speedbtn.GetComponentInChildren<Text>().text = "0.5x";
                break;
            case 500:
                Stats.speed = .1f;
                speedbtn.GetComponentInChildren<Text>().text = "0.1x";
                break;
            case 100:
                Stats.speed = 1f;
                speedbtn.GetComponentInChildren<Text>().text = "1x";
                break;
            default:
                Stats.speed = 1f;
                break;
        }

        
    }

    public void ResetWorldPressed() {
        Text txt = GameObject.Find("Reset World").GetComponentInChildren<Text>();
        if(txt.text.Equals("Reset World")) {
            GameObject.Find("GridSpawner").GetComponent<GridSpawner>().ResetWorld();
        }
        else {
            GameObject.Find("Algorithms").GetComponent<AlgorithmManager>().EndAll();
            GameObject.Find("GridSpawner").GetComponent<GridSpawner>().ResetWorldExcept(false);
            txt.text = "Reset World";
        }
    }
}
