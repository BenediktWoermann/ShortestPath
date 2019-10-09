using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockMove : MonoBehaviour
{
    // Button to run/pause the execution of the algorithm
    public Button runbtn;

    public int colorStatus;
    // markingpos: 1 for up, -1 for down
    public int markingcol, markingpos;
    public enum Col { none = 0, red, black, green, orange, yellow, white = 100 };
    void Start()
    {
        markingcol = 0;
        markingpos = 0;
        colorStatus = 0;
    }

    void Update()
    {
        UpdatePosition();
        UpdateColor();
    }

    public void mark(int identifier) {
        bool algrunning = GameObject.Find("Algorithms").GetComponent<DijkstraAlgorithm>().running || GameObject.Find("Algorithms").GetComponent<AStarAlgorithm>().running;
        bool algstopped = GameObject.Find("Algorithms").GetComponent<DijkstraAlgorithm>().stopped || GameObject.Find("Algorithms").GetComponent<AStarAlgorithm>().stopped;
        if ((algrunning || algstopped) && (identifier == (int)Col.black || identifier == (int)Col.red || identifier == (int)Col.green)) return;

        if (colorStatus == identifier && colorStatus != (int)Col.orange)
        {
            // get block white again
            markingcol = 100;
            markingpos = -1;
            if(identifier == (int)Col.green) {
                GridManagement.target = null;
            }
            if (identifier == (int)Col.red)
            {
                GridManagement.start = null;
            }
        }
        else {
            // color the block like the identifier specifies
            markingcol = identifier;
            // white and orange blocks stay down while red, black and green ones move up
            if (identifier == (int)Col.orange || identifier == (int)Col.white) markingpos = -1;
            else markingpos = 1;
            // if the block should become red (start) the existing start gets white again
            if(identifier == (int)Col.red && GridManagement.start != gameObject) {
                if (GridManagement.start != null)
                {
                    GridManagement.start.GetComponent<BlockMove>().mark(100);
                }
                GridManagement.start = gameObject;
            }
            if(identifier == (int)Col.green && GridManagement.target != gameObject) {
                if (GridManagement.target != null)
                {
                    GridManagement.target.GetComponent<BlockMove>().mark(100);
                }
                GridManagement.target = gameObject;
            }
        }
    }

    private void UpdateColor() {
        Color col = gameObject.GetComponent<Renderer>().material.color;
        switch (markingcol)
        {
            // 1: RED     2: BLACK     3: GREEN     4: ORANGE     5: YELLOW     100: WHITE    0: NO CHANGE
            case (int)Col.white:
                col.r += .05f;
                col.g += .05f;
                col.b += .05f;
                col.r = Mathf.Clamp(col.r, 0, 1);
                col.g = Mathf.Clamp(col.g, 0, 1);
                col.b = Mathf.Clamp(col.b, 0, 1);
                markingcol = (col.b == 1f && col.g == 1f && col.r == 1f) ? (int)Col.none : (int)Col.white;
                if (markingcol == (int)Col.none) colorStatus = (int)Col.none;
                break;
            case (int)Col.red:
                col.b -= .05f;
                col.g -= .05f;
                col.r += .05f;
                col.r = Mathf.Clamp(col.r, 0, 1);
                col.g = Mathf.Clamp(col.g, 0, 1);
                col.b = Mathf.Clamp(col.b, 0, 1);
                markingcol = (col.b == 0f && col.g == 0f && col.r == 1f) ? (int)Col.none : (int)Col.red;
                if (markingcol == (int)Col.none) colorStatus = (int)Col.red;
                break;
            case (int)Col.black:
                col.r -= .05f;
                col.g -= .05f;
                col.b -= .05f;
                col.r = Mathf.Clamp(col.r, 0, 1);
                col.g = Mathf.Clamp(col.g, 0, 1);
                col.b = Mathf.Clamp(col.b, 0, 1);
                markingcol = (col.r == 0f && col.b == 0f && col.g == 0f) ? (int)Col.none : (int)Col.black;
                if (markingcol == (int)Col.none) colorStatus = (int)Col.black;
                break;
            case (int)Col.green:
                col.b -= .05f;
                col.r -= .05f;
                col.g += .05f;
                col.r = Mathf.Clamp(col.r, 0, 1);
                col.g = Mathf.Clamp(col.g, 0, 1);
                col.b = Mathf.Clamp(col.b, 0, 1);
                markingcol = (col.b == 0f && col.r == 0f && col.g == 1f) ? (int)Col.none : (int)Col.green;
                if (markingcol == (int)Col.none) colorStatus = (int)Col.green;
                break;
            case (int)Col.orange: //0.9 .6 .3
                if (Mathf.Abs(col.r - 0.9f) < .05f) col.r = 0.9f;
                else
                {
                    if (col.r > 0.9f) col.r -= .05f;
                    else col.r += .05f;
                }
                if (Mathf.Abs(col.g - 0.6f) < .05f) col.g = 0.6f;
                else
                {
                    if (col.g > 0.6f) col.g -= .05f;
                    else col.g += .05f;
                }
                if (Mathf.Abs(col.b - 0.3f) < .05f) col.b = 0.3f;
                else
                {
                    if (col.b > 0.3f) col.b -= .05f;
                    else col.b += .05f;
                }

                markingcol = (col.r == 0.9f && col.g == 0.6f && col.b == 0.3f) ? (int)Col.none : (int)Col.orange;
                if (markingcol == (int)Col.none) colorStatus = (int)Col.orange;
                break;
            case (int)Col.yellow: //0.9 .9 .1
                if (Mathf.Abs(col.r - 0.9f) < .05f) col.r = 0.9f;
                else
                {
                    if (col.r > 0.9f) col.r -= .05f;
                    else col.r += .05f;
                }
                if (Mathf.Abs(col.g - 0.9f) < .05f) col.g = 0.9f;
                else
                {
                    if (col.g > 0.9f) col.g -= .05f;
                    else col.g += .05f;
                }
                if (Mathf.Abs(col.b - 0.1f) < .05f) col.b = 0.1f;
                else
                {
                    if (col.b > 0.1f) col.b -= .05f;
                    else col.b += .05f;
                }

                markingcol = (col.r == 0.9f && col.g == 0.9f && col.b == 0.1f) ? (int)Col.none : (int)Col.yellow;
                if (markingcol == (int)Col.none) colorStatus = (int)Col.yellow;
                break;
            default:
                break;
        }
        gameObject.GetComponent<Renderer>().material.color = col;
    }

    private void UpdatePosition() {
        Vector3 pos = gameObject.transform.position;
        switch (markingpos)
        {
            case -1:
                pos.y -= 0.05f;
                pos.y = Mathf.Clamp(pos.y, 0, 1);
                if (pos.y == 0) markingpos = 0;
                break;
            case 1:
                pos.y += 0.05f;
                pos.y = Mathf.Clamp(pos.y, 0, 1);
                if (pos.y == 1) markingpos = 0;
                break;
            default:
                break;
        }
        gameObject.transform.position = pos;
    }
}
