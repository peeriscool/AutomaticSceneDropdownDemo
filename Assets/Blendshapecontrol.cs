using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blendshapecontrol : MonoBehaviour
{
    public float animationSpeed = 0.2f;
    public int blendShapeCount = 1; //Didn't find other way to get the count of the blend shapes. I am going to look again tomorrow
    public SkinnedMeshRenderer meshRenderer;

    int curFrame;
    float index = 1;
    float frameLength;
    IEnumerator routine;
    IEnumerator clear;
    float max = 0;

    private void Start()
    {
        clear = lerpcontrol();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            if(IEnumerator.Equals(routine,null))
            {
                routine = lerpcontrol(); // 
                StartCoroutine(routine);

            }
            if(IEnumerator.Equals(routine, clear))
            {
                //routine not yet cleared or done lerping
                return;
            }
            //meshRenderer.SetBlendShapeWeight((int)(Time.time * 100) % index, index++);

        }
        //if(Input.GetKeyUp(KeyCode.Y))
        //{
        //    index = 1;
        //}
        //if (frameLength >= animationSpeed)
        //{
        //    UpdateAnimationFrame();
        //    frameLength = 0;
        //}
        //else
        //{
        //    frameLength += Time.deltaTime;
        //}
    }
    IEnumerator lerpcontrol()
    {
        
        meshRenderer.SetBlendShapeWeight(curFrame, max);
        max = Mathf.Lerp(0, animationSpeed, Mathf.Sin(Time.deltaTime * 100));
        yield return new WaitForSeconds(0.1f);     
            routine = null;
          //  meshRenderer.SetBlendShapeWeight(curFrame, max);
            Debug.Log("Routine = null");
    }
    void UpdateAnimationFrame()
    {
        meshRenderer.SetBlendShapeWeight(curFrame, 0);
        curFrame++;
        if (curFrame > blendShapeCount) Loop();
        meshRenderer.SetBlendShapeWeight(curFrame, 100);
    }
    void Loop()
    {
        meshRenderer.SetBlendShapeWeight(curFrame - 1, 0);
        curFrame = 0;
    }
}
