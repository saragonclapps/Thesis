using UnityEngine.SceneManagement;

public class MasterManager
{
    public static int nextScene;

    public static Scene[] scenes;

    public static void GetNextScene(Scene current)
    {
        nextScene = current.buildIndex + 1;
    }

    public static void GetPreviousScene(Scene current)
    {
        nextScene = current.buildIndex - 1;
    }
}
