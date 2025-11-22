using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCreditsButton : MonoBehaviour
{
    public void LoadCredits()
    {
        SceneManager.LoadScene(2);
    }
}
