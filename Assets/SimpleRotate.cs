using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float speed;
    private GameObject rotatable;
    public bool increment;
    // Start is called before the first frame update
    void Start()
    {
        rotatable = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if(increment)
        {
            speed +=0.05f;
        }
        rotatable.transform.localRotation = transform.localRotation = Quaternion.Euler(Time.time * speed, 0, 0); //new Quaternion (Time.deltaTime * speed,0,0,0); //Rotate(rotatable.transform.eulerAngles, Time.deltaTime * speed);
    }
}
