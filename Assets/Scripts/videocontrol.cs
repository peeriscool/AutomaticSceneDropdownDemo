using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class Videocontrol : MonoBehaviour
{
    VideoPlayer controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<VideoPlayer>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            controller.Play();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            controller.Pause();
        }
    }
}
