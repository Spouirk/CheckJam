using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public float time;
    public float maxTime = 5f;
    public float timeInterval = 5f;
    public float minTime;
    public TimeManager timeManager;
    
    public int score = 0;
    public int multiplier = 1;
    public int streak = 0;

    public int maxHealth = 3;
    public int health;
    
    public TMP_Text scoreText;
    void Start()
    {
        timeManager.SetMaxTime(maxTime);
        health = maxHealth;
        scoreText.text = "Score: " + score;
        
    }
    
    void Update()
    {
        time = Time.timeSinceLevelLoad;
        timeManager.SetTime(time,minTime,maxTime);
        //Réinitialisation de la barre de temps
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReussiteCheck();
        }
            
    }
    
    //Augmente le score du joueur et sa streak lors d'une réussite
    public void ReussiteCheck()
    {
        score += 10*multiplier;
        if (maxTime - time > timeInterval / 4 && multiplier < 4)
        {
            multiplier++;
        }
        streak++;
        minTime = time;
        maxTime = time + timeInterval;
        timeManager.ResetTime(minTime,maxTime);
        scoreText.text = "Score: " + score;
    }
    
    //Reset la streak et perte d'une vie lors de l'échec
    public void EchecCheck()
    {
        streak = 0;
        multiplier = 1;
        health--;
    }
    
    
}
