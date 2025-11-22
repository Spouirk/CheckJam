using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOptionsButton : MonoBehaviour
{
    public void LoadOptions()
    {
        SceneManager.LoadScene(3);
    }
}
