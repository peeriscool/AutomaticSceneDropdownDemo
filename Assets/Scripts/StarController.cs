using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public GameObject lightrefrence;
    GameObject[] lightpoints;    
    int amount = 10;
    void Start()
    {
        lightpoints = new GameObject[amount];
        for (int i = 1; i < amount; i++)
        {
            lightpoints[i]= Instantiate(lightrefrence, new Vector3(Random.Range(0, amount), Random.Range(0, amount), Random.Range(0, amount)), new Quaternion());
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject item in lightpoints)
        {
            item.transform.position = new Vector3(Random.Range(0,amount), Random.Range(0, amount));
            item.GetComponent<Light>().intensity = Mathf.PingPong(Time.deltaTime,3f);
        }
    }
}
