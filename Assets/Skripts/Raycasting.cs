using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycasting : MonoBehaviour
{
    public enum Col { none, red, black, green, orange, yellow, white = 100 };

    public Scrollbar scrollbar;

    private bool streak, whitestreak;
    private List<GameObject> streakList;

    private void Start()
    {
        whitestreak = false;
        streak = false;
        streakList = new List<GameObject>();
    }

    void Update()
    {
        if (!streak) streakList = new List<GameObject>();

        if (Input.GetMouseButton(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float uiHeight = GameObject.Find("UIBackground").GetComponent<RectTransform>().sizeDelta.y;
            float referenceWidth = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
            bool clickonUI = Input.mousePosition.y > Screen.height - (uiHeight * Screen.width / referenceWidth);
            if (Physics.Raycast(ray, out hit, 100.0f) && !clickonUI) {
                if (hit.transform) {
                    BlockMove bm = hit.transform.gameObject.GetComponent<BlockMove>();
                    if (streakList.Count != 0 && streakList.LastIndexOf(hit.transform.gameObject) == streakList.Count-1) return;
                    if (!streak) whitestreak = bm.colorStatus == (int)Col.red || bm.colorStatus == (int)Col.green || bm.colorStatus == (int)Col.black;
                    int a = scrollbar.value > 0.25f ? scrollbar.value > 0.75f ? 3 : 2 : 1;
                    if (whitestreak != (bm.colorStatus == a)) return;
                    hit.transform.gameObject.GetComponent<BlockMove>().mark(a);
                    streakList.Add(hit.transform.gameObject);
                }
            }
        }
        streak = Input.GetMouseButton(0);
    }

}
