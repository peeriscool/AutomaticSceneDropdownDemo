using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate(1920,1080,60);
        }
    }
}
