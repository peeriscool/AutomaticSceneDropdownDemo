using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opendoor : MonoBehaviour
{
    public Vector3 degree;
    //open door 130 degrees
    public GameObject door;
    public float speed;
    void Start()
    {
       // door = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion original = door.transform.localRotation;
        door.transform.localRotation = Quaternion.Lerp(original, Quaternion.Euler(degree.x, degree.y, degree.z),Time.deltaTime/ speed);   
    }
}
