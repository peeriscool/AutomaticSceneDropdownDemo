using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuInstancing : MonoBehaviour
{
    public GameObject Object;
    public int amount = 10;
    public int clusterrange;
    public bool UseRandom;
    public Vector3 location;
    int respawn;
    GameObject[] instances;
    GameObject transformowner;
    void Start()
    {
       // Object.GetComponent<Rigidbody>().isKinematic = true;
        transformowner = new GameObject();
        transformowner.transform.position = location;
        instances = new GameObject[amount];
        instantSpawn();
    }
    void instantSpawn()
    {
        for (int i = 1; i < amount; i++) //spawn stars
        {
            instances[i] = GameObject.Instantiate(Object); //Instantiate(, new Vector3(Random.Range(0, amount), Random.Range(0, amount), Random.Range(0, amount)), new Quaternion());
            if (UseRandom)
            {
                Vector3 random = new Vector3(Random.Range(-clusterrange, clusterrange), Random.Range(-clusterrange, clusterrange), Random.Range(-clusterrange, clusterrange)).normalized;
                instances[i].transform.position = Vector3.Cross(this.gameObject.transform.position, random) / 4;
            }
            else
            {
                instances[i].transform.position = this.gameObject.transform.position;
            }
        }
        for (int i = 1; i < amount; i++) //parent all stars
        {
            instances[i].transform.parent = transformowner.transform;

        }

       // enablerigidbodies(instances);
    }
    //void enablerigidbodies(GameObject[] list)
    //{
    //    foreach (GameObject Object in list)
    //    {
    //        Object.GetComponent<Rigidbody>().isKinematic = false;
    //    }
    //}
    private void Update()
    {
        foreach (Transform item in transformowner.transform)
        {
            if(item.transform.localPosition.y < -50)
            {
                Destroy(item.gameObject);
                respawn++;
            }
        }

        if(respawn >= 0)
        {
            int index = 0;
            foreach (GameObject item in instances)
            {

                if(item == null)
                {
                    instances.SetValue(GameObject.Instantiate(Object,transformowner.transform.position,Quaternion.identity, transformowner.transform),index);
                }
                index++;
            }
        }
    }
}
