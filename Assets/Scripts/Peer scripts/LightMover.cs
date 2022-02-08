using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMover : MonoBehaviour
{
    public GameObject Moveable;
    public Vector3 ControlVector;
    public Vector3 Distord;
    public float TimeScale;
    public float min;
    public float max;
    public bool flip;
    [SerializeField] bool _pauze = false;

    public bool pauze
    {
        get { return _pauze; }
        set
        {
            if (_pauze != value)
            {
                _pauze = value;
                // Run some function or event here
           
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!pauze)
        Moveable.transform.Rotate(ControlVector - Distord, Mathf.PingPong(Time.deltaTime * TimeScale, Random.Range(min, max)));
        if (flip) { Flip(); }
    }




    void Flip()
    {
        max = -max;
        min = -min;
    }
}
