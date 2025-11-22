using UnityEngine;

[System.Serializable]
public class Check
{
    public string checkName;    // Nom du check
    public Sprite sprite;       // Sprite du check
    public KeyCode key;         // Touche assignée

    public Check(string title, Sprite sprite = null)
    {
        this.checkName = title;
        this.sprite = sprite;
        // La touche sera assignée par le PersonManager
    }

    public void AssignKey(KeyCode newKey)
    {
        key = newKey;
    }
}
