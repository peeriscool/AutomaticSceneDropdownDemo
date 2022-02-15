using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//http://www.41post.com/4742/programming/unity-animated-texture-from-image-sequence-part-2.
public class ImageSequenceSingleTexture : MonoBehaviour
{
    //A texture object that will output the animation  
    private Texture texture;
    //With this Material object, a reference to the game object Material can be stored  
    private Material goMaterial;
    //An integer to advance frames  
    public int frameCounter = 0;

    //A string that holds the name of the folder which contains the image sequence  
    public string folderName;
    //The name of the image sequence  
    public string imageSequenceName;
    //The number of frames the animation has  
    public int numberOfFrames;
    private int reverseframes;
    //The base name of the files of the sequence  
    private string baseName;

    void Awake()
    {
        //Get a reference to the Material of the game object this script is attached to  
        this.goMaterial = GetComponent<Renderer>().material;
        //With the folder name and the sequence name, get the full path of the images (without the numbers)  
        this.baseName = this.folderName + "/" + this.imageSequenceName;
        reverseframes = numberOfFrames;
       
    }

    void Start()
    {
        //set the initial frame as the first texture. Load it from the first image on the folder  
        texture = (Texture)Resources.Load(baseName + "00000", typeof(Texture));
     //   GameObject.Find("curtain").GetComponent<MeshRenderer>().enabled = true;
    }

    void Update()
    {
        //Start the 'PlayLoop' method as a coroutine with a 0.04 delay  
        StartCoroutine("Play", 0.04f);
       
        //Set the material's texture to the current value of the frameCounter variable  
        goMaterial.mainTexture = this.texture;
    }

    //The following methods return a IEnumerator so they can be yielded:  
    //A method to play the animation in a loop  
    IEnumerator PlayLoop(float delay)
    {
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);

        //advance one frame  
        frameCounter = (++frameCounter) % numberOfFrames;

        //load the current frame  
        this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D4"), typeof(Texture));
        
        //Stop this coroutine  
        StopCoroutine("PlayLoop");
    }

    //A method to play the animation just once  
    IEnumerator Play(float delay)
    {
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);

        //if it isn't the last frame  
        if (frameCounter < numberOfFrames - 1)
        {
            //Advance one frame  
            ++frameCounter;

            //load the current frame  
            this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString("D4"), typeof(Texture));
        }
        if (frameCounter > numberOfFrames - 2)
        {
            Debug.Log("omg delete this");
           // Destroy(GameObject.Find("curtain"));
        }

        //Stop this coroutine  
        StopCoroutine("Play");
      
    }
    IEnumerator Playreversed(float delay)
    {
       
        //wait for the time defined at the delay parameter  
        yield return new WaitForSeconds(delay);
       
        //if it isn't the last frame  
        if (reverseframes > 1)
        {
            //Advance one frame  
            --reverseframes;

            //load the current frame  
            this.texture = (Texture)Resources.Load(baseName + reverseframes.ToString("D4"), typeof(Texture));
        }

        //Stop this coroutine  
        StopCoroutine("Play");

    }
}