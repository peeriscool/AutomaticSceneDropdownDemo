using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class imagensequence : MonoBehaviour
{
    public Sprite[] images;
    public Image AnimatedImage;
    public int offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatedImage.sprite = images[(int)(Time.time * 10+ offset) % images.Length]; //time/10 % length of array
    }
}
