using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    
    public Image timeLeft;

    public void SetMaxTime(float time)
    {
        slider.maxValue = time;
        slider.value = time;

        timeLeft.color = gradient.Evaluate(1f);
    }

    public void SetTime(float time)
    {
        slider.value = time;
        timeLeft.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void ResetTime(float minTime, float maxTime)
    {
        
        SetMaxTime(maxTime);
        slider.minValue = minTime;
    }
}
