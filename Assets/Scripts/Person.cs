using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public string personName;
    public Color32 skinColor;
    public bool hasTatoo;
    public bool isKnown = false;        // Permet de savoir si la personne est connu pour afficher ou non son/ses checksPool et sa/ses touches

    // Liste des checks que cette personne peut faire
    public List<Check> personChecks = new List<Check>();

    public void MarkAsKnown()
    {
        isKnown = true;
    }
}
