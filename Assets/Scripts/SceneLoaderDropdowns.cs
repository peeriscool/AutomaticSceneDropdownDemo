using UnityEditor;
namespace KingdomOfNight
{
    public partial class SceneLoader
    {
#if UNITY_EDITOR
        [MenuItem("Scenes/SceneA")]
        public static void LoadSceneA() { OpenScene("Assets/scenes/SceneA.unity"); }
        [MenuItem("Scenes/SceneB")]
        public static void LoadSceneB() { OpenScene("Assets/scenes/SceneB.unity"); }
        [MenuItem("Scenes/Folder_X/SceneC")]
        public static void LoadSceneC() { OpenScene("Assets/scenes/Folder_X/SceneC.unity"); }
        [MenuItem("Scenes/Folder_X/SceneD")]
        public static void LoadSceneD() { OpenScene("Assets/scenes/Folder_X/SceneD.unity"); }
        [MenuItem("Scenes/Folder_X/SceneE")]
        public static void LoadSceneE() { OpenScene("Assets/scenes/Folder_X/SceneE.unity"); }
        [MenuItem("Scenes/Folder_Y/SceneF")]
        public static void LoadSceneF() { OpenScene("Assets/scenes/Folder_Y/SceneF.unity"); }
        [MenuItem("Scenes/Folder_Y/SceneG")]
        public static void LoadSceneG() { OpenScene("Assets/scenes/Folder_Y/SceneG.unity"); }
        [MenuItem("Scenes/Folder_Y/SceneH")]
        public static void LoadSceneH() { OpenScene("Assets/scenes/Folder_Y/SceneH.unity"); }
        [MenuItem("Scenes/Folder_Z/SceneI")]
        public static void LoadSceneI() { OpenScene("Assets/scenes/Folder_Z/SceneI.unity"); }
        [MenuItem("Scenes/Folder_Z/SceneJ")]
        public static void LoadSceneJ() { OpenScene("Assets/scenes/Folder_Z/SceneJ.unity"); }
        [MenuItem("Scenes/Folder_Z/SceneK")]
        public static void LoadSceneK() { OpenScene("Assets/scenes/Folder_Z/SceneK.unity"); }
#endif
    }
}