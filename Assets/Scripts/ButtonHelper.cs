using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHelper : MonoBehaviour
{
    [SerializeField] string previousSceneName;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && previousSceneName != null)
        {
            LoadScene(previousSceneName);
        }
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
