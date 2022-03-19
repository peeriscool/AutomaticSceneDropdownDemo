using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScriptsWthkeybard : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] activateitems;
    int index = 0;
    void Start()
    {
        foreach (GameObject item in activateitems)
        {            
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (activateitems[index].active) { activateitems[index].SetActive(false); }
            else if (!activateitems[index].active) activateitems[index].SetActive(true);

            if(index != activateitems.Length)
            { index++; }
            else
            {
                index = 0;
            }
           
        }
    }
}
