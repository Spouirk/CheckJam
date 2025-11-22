using UnityEngine;
using System.Collections.Generic;

public static class KeyGenerator
{
    // Générer une liste de touche de taille "count", prend en compte uniquement les 26 lettres de l'alphabet
    public static List<KeyCode> GetRandomKeyList(int count)
    {
        if (count < 1)
        {
            Debug.Log("List cannot be empty.");
            return null;
        }

        KeyCode[] pool = new KeyCode[]
        {
            KeyCode.A, KeyCode.Z, KeyCode.E, KeyCode.R,
            KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P
        };

        List<KeyCode> keys = new List<KeyCode>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, pool.Length);
            keys.Add(pool[randomIndex]);
        }

        return keys;
    }

}

