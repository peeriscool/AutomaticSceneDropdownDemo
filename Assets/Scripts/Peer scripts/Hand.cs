using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    bool knobhold = false;
    bool MaxRange = false;
    Quaternion doorreset;
    //  public Rigidbody Rigidbodycomponent;
    // Start is called before the first frame update
    void Start()
    {
        // Rigidbodycomponent.isKinematic = true; 
       doorreset = GameObject.Find("Deur").gameObject.transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if(other.gameObject.tag == "deur")
        {
            Debug.Log("door");
            MaxRange = true;
        }
        if (other.gameObject.tag == "knob")
        {
            Debug.Log("knob");
            knobhold = true;

        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        
        if(collision.gameObject.tag == "deur")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            Debug.DrawLine(position, this.transform.position,Color.red,5f);
        }
      

       // Instantiate(explosionPrefab, position, rotation);
       // Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (knobhold == true)
        {
            //   GameObject.Find("Deur").gameObject.transform.rotation =  Quaternion.FromToRotation(this.gameObject.transform.position, Vector3.up);

            rotateRigidBodyAroundPointBy(GameObject.Find("Deur").GetComponent<Rigidbody>(), transform.position, Vector3.right, 2f);
        

        // Update is called once per frame
        //https://answers.unity.com/questions/10093/rigidbody-rotating-around-a-point-instead-on-self.html
       
        Debug.Log("Time to open");
            if(MaxRange == true)
            {
                //no longer in range of the door joints
            }
          //  knobhold = false;
        }
    }

    public void rotateRigidBodyAroundPointBy(Rigidbody rb, Vector3 origin, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        rb.MovePosition(q * (rb.transform.position - origin) + origin);
        rb.MoveRotation(rb.transform.rotation * q);
    }
}
