using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenuButton : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
