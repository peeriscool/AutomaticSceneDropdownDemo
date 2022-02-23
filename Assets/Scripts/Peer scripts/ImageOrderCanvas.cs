using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class ImageOrderCanvas : MonoBehaviour
{

    /// <summary>
    /// ToDo: Make a cap at the max of the prins list and 0 for ImageIndex
    /// 
    /// </summary>
    public float scaleOfImage;
    private GameObject DisplayHost;
   // Image displayed;
    List<Image> Prins;
    Image displayed;
    AspectRatioFitter aspect;
    Canvas owner;
    int ImageIndex = 0;
    void Start()
    {
        Prins = new List<Image>();
        owner = GetComponent<Canvas>();
        GameObject item = owner.transform.GetChild(0).gameObject;
        foreach (Transform obj in item.transform)
        {
            Prins.Add(obj.gameObject.GetComponentInChildren<Image>());
        }
        Debug.Log("Total images= "+Prins.Count);
        foreach (Image img in Prins)
        {
            Debug.Log(img.name);
        }
        InitPlayhost();
    }
    void Update()
    {
        if(Input.anyKey)
        {
            Keypressed();
        }
    }
    void InitPlayhost()
    {
        DisplayHost = new GameObject();
        displayed = DisplayHost.AddComponent<Image>();
        aspect =  DisplayHost.AddComponent<AspectRatioFitter>();
        DisplayHost.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        DisplayHost.transform.localScale  = new Vector3(0.8f,0.3f);//*= scaleOfImage; //sets the base scale for all the images
        DisplayHost.transform.parent = owner.gameObject.transform;
        aspect.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        // displayed = DisplayHost.GetComponent<Image>();
    }
    void Keypressed()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ImageIndex--;
            Debug.Log(ImageIndex);
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            ImageIndex++;
            Debug.Log(ImageIndex);
        }
        AssignIndexToImage();
    }
    void AssignIndexToImage()
    {
        displayed.sprite = Prins[ImageIndex].sprite;
        //Image Viewable = DisplayHost.GetComponent<Image>();
        //Viewable.SetNativeSize(Prins[ImageIndex].sprite.rect); // = Prins[ImageIndex].preferredWidth;
        // DisplayHost.GetComponent<Image>().sprite = Prins[ImageIndex].sprite;
        //  DisplayHost.GetComponent<Image>().rectTransform = Prins[ImageIndex].rectTransform;
    }
}

//if(item.gameObject.name == "PhotoHolder")
//{
//    foreach (GameObject img in item.transform)
//    {
//        //    if(img.GetComponent<Image>())
//        //    {
//        Prins.Add(img.GetComponent<Image>());
//    //    }

//    }
//}
//if(item.GetComponent<Image>() != null)
//{
//    Prins.Add(item.GetComponent<Image>());
//}
// }
