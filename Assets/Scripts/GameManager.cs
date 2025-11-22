using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public PersonManager personManager;
    public TimeManager timeManager;
    
    public int score = 0;
    public int multiplier = 1;
    public int streak = 0;

    public int maxHealth = 3;
    public int health;
    
    public TMP_Text scoreText;
    public TMP_Text combo;

    public Sprite[] sprites;
    public Image healthCount;
    void Start()
    {
        health = maxHealth;
        healthCount.sprite = sprites[health-1];
        scoreText.text = "Score: " + score;
        combo.text = "";
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReussiteCheck();
        }
    }
    
    //Augmente le score du joueur et sa streak lors d'une réussite
    public void ReussiteCheck()
    {
        score += (int)(personManager.timer * 100 / personManager.baseTimer)*multiplier;
        if (multiplier < 5) multiplier++;
        streak++;
        scoreText.text = "Score: " + score;
        if (streak > 1)
        {
            combo.text = "x" + streak;
            //change la couleur en fonction du multiplicateur
            switch (streak)
            {
                case 2:combo.color = Color.white; break;
                case 3:combo.color = Color.yellow; break;
                case 4:combo.color = Color.darkOrange; break;
                default: combo.color = Color.red; break;
            }
        }
    }
    
    //Reset la streak et perte d'une vie lors de l'échec
    public void EchecCheck()
    {
        streak = 0;
        multiplier = 1;
        health--;
        combo.text = "";
        if (health <= 0)
        {
            SceneManager.LoadScene(4);
        } else {
            healthCount.sprite = sprites[health - 1];
        }
    }
    
    
}
