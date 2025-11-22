using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public float time;
    public float maxTime = 5f;
    public float timeInterval = 5f;
    public float minTime = 0f;
    
    public TimeManager timeManager;
    void Start()
    {
        timeManager.SetMaxTime(maxTime);
    }
    
    void Update()
    {
        time = Time.timeSinceLevelLoad;
        timeManager.SetTime(time,minTime,maxTime);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            minTime = time;
            maxTime = time + timeInterval;
            timeManager.ResetTime(minTime,maxTime);
        }
            
    }
}
