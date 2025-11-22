using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color32 color;           // PLACEHOLDER : Couleur/image de la main

    public string title;            // Nom du check
    public int nbOfKeys;            // Nombre de touches à appuyer

    private List<KeyCode> keys;     // Liste des touches à appuyer pour effectuer le check

    void Start()
    {
        // Génération des touches
        keys = KeyGenerator.GetRandomKeyList(nbOfKeys);
        if (keys.Count > 0 )
        {
            foreach (var key in keys)
            {
                Debug.Log("Key: " + key);
            }
        }

        // Couleur du sprite (SERA REMPLACE PAR IMAGE)
        sr.color = color;
    }

    void Update()
    {
        
    }
}
