using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    [HideInInspector] public List<Check> checksPool = new List<Check>();        // Liste des différents checksPool possibles
    [HideInInspector] public List<Person> personsPool = new List<Person>();     // Pool de personnes générées

    private KeyCode[] keysPool = { KeyCode.A, KeyCode.Z, KeyCode.E, KeyCode.R, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P };

    public string[] checkTitles = new string[8];
    public Sprite[] checkSprites = new Sprite[8];

    [Header("Person Generation Settings")]
    public int numberOfPersons = 10;        // Combien de personnes générer
    public int maxChecksPerPerson = 3;      // Combien de checks maximum par personne

    void Start()
    {
        InitializeChecks();
        GeneratePersons();
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

    // Génère un certain nombre de personnes
    void GeneratePersons()
    {
        personsPool.Clear();

        for (int i = 0; i < numberOfPersons; i++)
        {
            GameObject personGO = new GameObject("Person_" + i);
            Person newPerson = personGO.AddComponent<Person>();

            newPerson.personName = "Person_" + i;

            // Couleur de peau aléatoire parmi beige, marron clair et marron foncé
            Color32[] skinColors = new Color32[]
            {
                new Color32(240, 215, 180, 255), // beige
                new Color32(160, 82, 45, 255),   // marron clair
                new Color32(90, 50, 30, 255)     // marron foncé
            };

            newPerson.skinColor = skinColors[Random.Range(0, skinColors.Length)];

            // Assignation de 1 à maxChecksPerPerson checks
            int checksCount = Random.Range(1, maxChecksPerPerson + 1);
            newPerson.personChecks = new List<Check>();

            for (int j = 0; j < checksCount; j++)
            {
                // Choisir un check aléatoire dans le pool (peut y avoir doublons)
                Check randomCheck = checksPool[Random.Range(0, checksPool.Count)];
                newPerson.personChecks.Add(randomCheck);
            }

            personsPool.Add(newPerson);

            Debug.Log($"{newPerson.personName} generated with {newPerson.personChecks.Count} check(s)");
        }
    }
}
