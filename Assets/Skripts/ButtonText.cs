using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour
{
    private Text text;
    public string text1;
    public string text2;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        text.text = text1;
    }

    public void changeText() {
        if (text.text.Equals(text1)) {
            text.text = text2;
        }
        else { 
            text.text = text1;
        }
    }

    public void ToggleOnScreenOnly() {
        Stats.onScreenOnly = !Stats.onScreenOnly;
    }
}
