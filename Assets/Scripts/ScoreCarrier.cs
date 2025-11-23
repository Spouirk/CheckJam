using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Carrier : MonoBehaviour
{
    public int score;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Rechercher le TextMeshPro dans la scène
        TMP_Text scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();

        if (scoreText != null)
        {
            scoreText.text = "Votre score:" + "\n" + score;
        }
        else
        {
            Debug.LogWarning("ScoreText introuvable dans la scène : " + scene.name);
        }

        // Nettoyage
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Destroy(gameObject);
    }
}
