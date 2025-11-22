using System;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    [HideInInspector] public List<Check> checksPool = new List<Check>();  // Liste des différents checksPool possibles

    private KeyCode[] keysPool = { KeyCode.A, KeyCode.Z, KeyCode.E, KeyCode.R, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P };

    public string[] checkTitles = new string[8];
    public Sprite[] checkSprites = new Sprite[8];

    void Start()
    {
        InitializeChecks();
    }

    // Initialise les checks avec touches uniques
    void InitializeChecks()
    {
        checksPool.Clear();

        List<KeyCode> availableKeys = new List<KeyCode>(keysPool);

        for (int i = 0; i < keysPool.Length; i++)
        {
            if (availableKeys.Count == 0)
                break;

            Check check = new Check(checkTitles[i], checkSprites[i]);

            check.AssignKey(keysPool[i]);

            checksPool.Add(check);
        }

        // DEBUG
        foreach (var c in checksPool)
        {
            Debug.Log($"Check: {c.checkName}, Touche: {c.key}");
        }
    }
}
