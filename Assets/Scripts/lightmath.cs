using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class lightmath : MonoBehaviour
{
    Light input;
    public bool increment;
    public float speed;
    public float value;
    // Start is called before the first frame update
    void Start()
    {
        input = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (increment)
        {
            speed += 0.01f;
        }
        input.intensity = Mathf.PingPong(Time.time, value) + Random.Range(0.1f, Time.deltaTime * speed);
    }
}
