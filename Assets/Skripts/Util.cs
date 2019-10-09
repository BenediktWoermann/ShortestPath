using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static bool IsOnScreen(GameObject cube, float upperUIBorder = 133){
        Debug.Log(upperUIBorder);
        Vector3 pos = cube.transform.position;
        Vector3 toControl;

        // checks all 4 upper corners, if they're on screen
        for(int i = 0; i<4; i++){
            if (i < 2) toControl.x = pos.x + cube.transform.localScale.x / 2;
            else toControl.x = pos.x - cube.transform.localScale.x / 2;

            if(i%2 == 0) toControl.z = pos.z + cube.transform.localScale.z / 2;
            else toControl.z = pos.z - cube.transform.localScale.z / 2;

            toControl.y = pos.y + cube.transform.localScale.y / 2;

            Vector3 screenPntToCtrl = Camera.main.WorldToScreenPoint(toControl);
            Debug.Log(screenPntToCtrl.y + "  " + upperUIBorder);
            if(screenPntToCtrl.x > 0 && screenPntToCtrl.x < Screen.width && screenPntToCtrl.y > 0 && screenPntToCtrl.y < Screen.height - upperUIBorder) return true;
        }

        return false;
    }
}
