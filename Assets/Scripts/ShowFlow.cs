using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


    //public SceneloaderDropdowns Scenes;
    public class ShowFlow : MonoBehaviour
    {
    static GameObject Owner;
    // Start is called before the first frame update
       static int i = 0;
        void Start()
        {
        Owner = this.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
        if(Input.GetKeyDown("space"))
        {
            i++;
            OpenScene(i);
        }
        }
    private static void OpenScene(int value)
    {
        DontDestroyOnLoad(Owner);
        //  SceneManager.GetSceneByBuildIndex(value);
        if (value < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(value);
            Debug.Log(i);
        }
        else { Debug.Log("EndOfProjectReached"); }
    }
}
