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
    public TMP_Text combo;
    void Start()
    {
        timeManager.SetMaxTime(maxTime);
        health = maxHealth;
        scoreText.text = "Score: " + score;
        combo.text = "";
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            EchecCheck();
        }
    }

    public void ResetTimer()
    {
        minTime = time;
        maxTime = time + timeInterval;
        timeManager.ResetTime(minTime,maxTime);
    }
    
    //Augmente le score du joueur et sa streak lors d'une réussite
    public void ReussiteCheck()
    {
        score += (int)(maxTime-time-timeInterval+10)*multiplier;
        if (multiplier < 5) multiplier++;
        streak++;
        scoreText.text = "Score: " + score;
        if (streak > 1)
        {
            combo.text = "x" + streak;
            switch (streak)
            {
                case 2:combo.color = Color.cyan; break;
                case 3:combo.color = Color.yellow; break;
                case 4:combo.color = Color.darkOrange; break;
                default: combo.color = Color.red; break;
            }
        }
        ResetTimer();
    }
    
    //Reset la streak et perte d'une vie lors de l'échec
    public void EchecCheck()
    {
        streak = 0;
        multiplier = 1;
        health--;
        combo.text = "";
        minTime = time;
        maxTime = time + timeInterval;
        timeManager.ResetTime(minTime,maxTime);
    }
    
    
}
