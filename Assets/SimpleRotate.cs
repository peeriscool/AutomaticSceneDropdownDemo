using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float speed;
    private GameObject rotatable;
    public bool increment;
    public float incrementValue; [Range(0,(float)0.1)]        
    public bool Usexyz;
    public Vector3 xyz;
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
            speed += incrementValue;
        }
        if (Usexyz)
        {
            rotatable.transform.localRotation =  Quaternion.Euler(xyz * speed) ;
        }

        if(!Usexyz)
        {
            rotatable.transform.localRotation = transform.localRotation = Quaternion.Euler(Time.time * speed, 0, 0); //new Quaternion (Time.deltaTime * speed,0,0,0); //Rotate(rotatable.transform.eulerAngles, Time.deltaTime * speed);

        }
    }
}
