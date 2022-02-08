using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class deur : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rotateRigidBodyAroundPointBy(GetComponent<Rigidbody>(),transform.position,Vector3.right,0f);
    }

    // Update is called once per frame
    //https://answers.unity.com/questions/10093/rigidbody-rotating-around-a-point-instead-on-self.html
    public void rotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MovePosition(q * (rb.transform.position - origin) + origin);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}
