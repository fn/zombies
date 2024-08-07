using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.StateRun();
    }

    public void Restart()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);

        GameManager.Instance.StateRun();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
        //GameManager.Instance.StateRun();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
