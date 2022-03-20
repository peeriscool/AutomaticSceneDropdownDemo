//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StarController : MonoBehaviour
{
    public GameObject lightrefrence;
    public int amount = 10;
    public bool shiftstars;
    public float waitTime = 6f;
    float timer = 0.0f;
    GameObject[] lightpoints;
    void Start()
    {
        lightpoints = new GameObject[amount];
        for (int i = 1; i < amount; i++) //spawn stars
        {
            lightpoints[i] = GameObject.Instantiate(lightrefrence); //Instantiate(, new Vector3(Random.Range(0, amount), Random.Range(0, amount), Random.Range(0, amount)), new Quaternion());
            lightpoints[i].transform.position = new Vector3(Random.Range(-amount, amount), Random.Range(-amount, amount), Random.Range(-amount, amount));
           // lightpoints[i].AddComponent<Light>();
        }
        for (int i = 1; i < amount; i++) //parent all stars
        {
            lightpoints[i].transform.parent = lightrefrence.transform;
        }
    }
    void Update()
    {
       // if (Input.GetKeyDown(KeyCode.Minus)) { Time.timeScale -= 0.1f; Debug.Log(Time.timeScale); }
     //   if (Input.GetKeyDown(KeyCode.Equals)){ Time.timeScale += 0.1f; Debug.Log(Time.timeScale); }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            float movefactor = -3000f;
            foreach (Transform body in lightrefrence.transform)
            { body.transform.position = new Vector3(body.transform.position.x, Mathf.Lerp(body.transform.position.y, movefactor, Time.deltaTime), body.transform.position.z); }
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            float movefactor = 3000f;
            foreach (Transform body in lightrefrence.transform)
            { body.transform.position = new Vector3(body.transform.position.x, Mathf.Lerp(body.transform.position.y, movefactor, Time.deltaTime), body.transform.position.z); }
        }
        if (shiftstars)
        {
            timer += Time.deltaTime;

            // Check if we have reached beyond x seconds.
            // Subtracting two is more accurate over time than resetting to zero.
            if (timer > waitTime)
            {
                Debug.Log(timer);
                //visualTime = timer;

                // Remove the recorded 2 seconds.
                timelightscale();
                timer = timer - waitTime;
                // Time.timeScale = scrollBar;

            }
        }
       
        

        //if(Input.GetKeyDown(KeyCode.Equals))
        //{

        //}

        // cam.backgroundColor = Lerp3(color1, color2, color3, t);
    }
    private void LateUpdate()
    {
        foreach (Transform light in lightrefrence.transform)
        {

            updatelight(light.GetComponent<Light>());
            updatehalo(light.GetComponent<Light>());
        }
    }

    void timelightscale() //used to distord location in a scaleable way
    {
        float t = Mathf.PingPong(Time.time, 30f);

        foreach (Transform item in lightrefrence.transform)
        {
            item.transform.position += new Vector3(Random.Range(-t, t), Random.Range(-t, t));
         //   updatelight(item.GetComponent<Light>());
        }

    }
    void updatelight(Light input)
    {

        input.intensity = Mathf.PingPong(Time.time, 3f) + Random.Range(0.1f,Time.deltaTime);
    }
    void updatehalo(Light input)
    {
        input.range = Mathf.SmoothStep(input.range,Random.Range(2,5), Time.time * 4);
    }
    Color Lerp3(Color a, Color b, Color c, float t)
    {
        if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
            return Color.Lerp(a, b, t / 0.5f);
        else // 0.5 to 1.0 goes to b -> c
            return Color.Lerp(b, c, (t - 0.5f) / 0.5f);
    }

}
