using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    [HideInInspector] public List<Check> checksPool = new List<Check>();        // Liste des diff�rents checksPool possibles
    [HideInInspector] public List<Person> personsPool = new List<Person>();     // Pool de personnes g�n�r�es

    public List<Sprite> accessoriesPool = new List<Sprite>();                   // Liste des accessoires diff�rents

    private KeyCode[] keysPool = { KeyCode.A, KeyCode.Z, KeyCode.E, KeyCode.R, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P };

    public string[] checkTitles = new string[8];
    public Sprite[] checkSprites = new Sprite[8];
    public Sprite baseCheckSprite;

    [Header("Imports")]
    public SpriteRenderer srPlayer;
    public RuntimeAnimatorController personAnimator;
    public GameManager gameManager;

    [Header("Current & Next Person")]
    [HideInInspector] public Person currentPerson;
    [HideInInspector] public Person nextPerson;

    public int maxChecksPerPerson = 3;

    private int currentCheckIndex = 0;
    [HideInInspector] public float timer;
    public float baseTimer = 100f;
    public float timerDecrease = 0.95f;

    [Header("Animation")]
    private bool isPlayingAnimation = false;

    [Header("Canvas")]
    public Transform keysContainer;

    void Start()
    {
        InitializeChecks();
        GeneratePersons();          // On g�n�re les 2 premi�res personnes
        ResetTimer();
        DisplayCurrentCheckInfo();
    }

    private void Update()
    {
        HandleTimer();
        HandlePlayerInput();

        // DEBUG : passage forc� � la personne suivante
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextPerson();
        }
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

            check.AssignKey(keysPool[i], i);

            checksPool.Add(check);
        }
    }

    // G�n�rer une personne al�atoire avec couleur et checks assign�s
    public Person GetRandomPerson()
    {
        // Pool vide : Cr�er une nouvelle personne (nouvelle = inconnue)
        if (personsPool.Count == 0)
        {
            Person person = CreateUniquePerson();
            person.isKnown = false;
            personsPool.Add(person);
            return person;
        }

        // Pool existant : 70% de chance de prendre une personne existante, sinon en cr�er une nouvelle
        bool useExisting = Random.value < 0.7f;

        if (useExisting)
        {
            // Ne pas modifier isKnown ici � on ne marque connu que quand on l'affiche r�ellement
            return personsPool[Random.Range(0, personsPool.Count)];
        }
        else
        {
            // Cr�er une personne unique (toujours inconnue)
            Person newPerson = CreateUniquePerson();
            newPerson.isKnown = false;
            personsPool.Add(newPerson);
            return newPerson;
        }
    }


    private Person CreateUniquePerson()
    {
        Color32[] skinColors = new Color32[]
        {
            new Color32(240, 215, 180, 255), // Beige
            new Color32(90, 50, 30, 255)     // Marron fonc�
        };

        Person newPerson = null;
        bool unique = false;

        while (!unique)
        {
            // Cr�er le GO
            GameObject go = new GameObject("Person_" + Random.Range(0, 99999));
            go.transform.SetParent(transform, false);
            Person person = go.AddComponent<Person>();
            Animator pAnimator = go.AddComponent<Animator>();
            pAnimator.runtimeAnimatorController = personAnimator;

            person.personName = go.name;
            person.skinColor = skinColors[Random.Range(0, skinColors.Length)];
            person.accessory = accessoriesPool[Random.Range(0, accessoriesPool.Count)];

            GameObject accessoryGo = new GameObject("Accessory");
            accessoryGo.transform.SetParent(go.transform, false);
            SpriteRenderer accessorySr = accessoryGo.AddComponent<SpriteRenderer>();
            accessorySr.sprite = person.accessory;

            // G�n�ration des checks
            int checksCount = Random.Range(1, maxChecksPerPerson + 1);
            person.personChecks = new List<Check>();

            for (int i = 0; i < checksCount; i++)
            {
                Check randomCheck = checksPool[Random.Range(0, checksPool.Count)];
                person.personChecks.Add(randomCheck);
            }

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = person.personChecks[0].sprite;
            sr.color = person.skinColor;

            // V�rifier que la personne est unique
            bool exists = false;

            foreach (var existing in personsPool)
            {
                bool sameSkin = existing.skinColor.Equals(person.skinColor);
                bool sameAccessory = existing.accessory == person.accessory;
                bool sameChecks = ListsAreEqual(existing.personChecks, person.personChecks);

                // Si l'exacte m�me existe
                if (sameSkin && sameAccessory &&  sameChecks)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                unique = true;
                newPerson = person;
            }
            else
            {
                GameObject.Destroy(go); // Eviter fuite d'objets
            }
        }

        return newPerson;
    }

    private bool ListsAreEqual(List<Check> a, List<Check> b)
    {
        if (a.Count != b.Count) return false;

        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }

        return true;
    }

    // D�finir la personne actuelle et la personne suivante
    public void GeneratePersons()
    {
        currentPerson = GetRandomPerson();
        nextPerson = GetRandomPerson();
        if (currentPerson != nextPerson) nextPerson.gameObject.SetActive(false);

        Debug.Log($"Current: {currentPerson.personName}, Next: {nextPerson.personName}");
    }

    // Passer � la personne suivante quand le joueur � check la personne acutelle
    public void NextPerson()
    {
        // La personne next devient current
        currentPerson.gameObject.SetActive(false);
        currentPerson = nextPerson;
        currentPerson.gameObject.SetActive(true);

        // G�n�rer une nouvelle next
        nextPerson = GetRandomPerson();
        if (currentPerson != nextPerson) nextPerson.gameObject.SetActive(false);

        baseTimer *= timerDecrease;

        currentCheckIndex = 0;
        ResetTimer();

        DisplayCurrentCheckInfo();

        Debug.Log($"New Current: {currentPerson.personName}, New Next: {nextPerson.personName}");
    }

    void HandlePlayerInput()
    {
        if (currentPerson == null) return;

        Check expected = currentPerson.personChecks[currentCheckIndex];

        // V�rifie si une touche du pool est appuy�e
        foreach (KeyCode key in keysPool)
        {
            if (Input.GetKeyDown(key))
            {
                if (key == expected.key)
                {
                    gameManager.ReussiteCheck();

                    currentCheckIndex++;
                    currentPerson.GetComponent<SpriteRenderer>().sprite = expected.sprite;

                    srPlayer.sprite = currentPerson.GetComponent<SpriteRenderer>().sprite;
                    srPlayer.GetComponent<Animator>().SetTrigger("Success");
                    currentPerson.GetComponent<Animator>().SetTrigger("Success");

                    // Faire une pause jusqu'a la fin de l'animation puis jouet le reste de la condition

                    if (currentCheckIndex >= currentPerson.personChecks.Count)
                    {
                        NextPerson();
                    }
                    else
                    {
                        DisplayCurrentCheckInfo();
                    }
                }
                else
                {
                    Debug.Log($"Mauvaise touche");
                    gameManager.EchecCheck();
                }
            }
        }
    }

    /*
    void HandlePlayerInput()
    {
        if (currentPerson == null || isPlayingAnimation) return;

        Check expected = currentPerson.personChecks[currentCheckIndex];

        foreach (KeyCode key in keysPool)
        {
            if (Input.GetKeyDown(key))
            {
                if (key == expected.key)
                {
                    StartCoroutine(HandleCheckAnimation(expected));
                }
                else
                {
                    Debug.Log("Mauvaise touche");
                }
            }
        }
    }*/

    private IEnumerator HandleCheckAnimation(Check expected)
    {
        isPlayingAnimation = true;

        Debug.Log($"Bonne touche : {expected.key}");

        currentCheckIndex++;

        // Changer le sprite du personnage
        currentPerson.GetComponent<SpriteRenderer>().sprite = expected.sprite;
        srPlayer.sprite = expected.sprite;

        // Lancer les animations
        Animator playerAnim = srPlayer.GetComponent<Animator>();
        Animator personAnim = currentPerson.GetComponent<Animator>();

        playerAnim.SetTrigger("Success");
        personAnim.SetTrigger("Success");

        // Attendre la fin de l'animation player
        yield return new WaitUntil(() => playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        // Logique apr�s l�animation
        if (currentCheckIndex >= currentPerson.personChecks.Count)
        {
            NextPerson();
        }
        else
        {
            DisplayCurrentCheckInfo();
        }

        isPlayingAnimation = false;
    }

    void DisplayCurrentCheckInfo()
    {
        if (currentPerson == null) return;

        Check check = currentPerson.personChecks[currentCheckIndex];

        // CAS 1 : Personne inconnue
        if (!currentPerson.isKnown)
        {
            // Tant que la personne est inconnue : on montre TOUS les checks
            currentPerson.GetComponent<SpriteRenderer>().sprite = check.sprite;

            for (int i = 0; i < keysContainer.childCount; i++)
                keysContainer.GetChild(i).gameObject.SetActive(false);

            keysContainer.GetChild(check.keyIndex).gameObject.SetActive(true);

            // SI TOUS LES CHECKS SONT VALIDÉS ELLE DEVIENT CONNUE
            if (currentCheckIndex >= currentPerson.personChecks.Count - 1)
            {
                currentPerson.isKnown = true;
                Debug.Log($"Personne {currentPerson.personName} devient CONNUE !");
            }


            return;
        }

        // CAS 2 : Personne connue
        if (currentCheckIndex == 0)
        {
            // Premier check : sprite neutre
            currentPerson.GetComponent<SpriteRenderer>().sprite = baseCheckSprite;

            for (int i = 0; i < keysContainer.childCount; i++)
                keysContainer.GetChild(i).gameObject.SetActive(false);

            Debug.Log("Personne connue : pas d'indice pour le premier check.");
            return;
        }

        // CAS 3 : Personne connue + check > 0
        // On montre le check précédent
        Check previousCheck = currentPerson.personChecks[currentCheckIndex - 1];

        currentPerson.GetComponent<SpriteRenderer>().sprite = previousCheck.sprite;

        for (int i = 0; i < keysContainer.childCount; i++)
            keysContainer.GetChild(i).gameObject.SetActive(false);
    }


    void HandleTimer()
    {
        timer -= Time.deltaTime;

        gameManager.timeManager.SetTime(timer);

        if (timer <= 0)
        {
            // R�duire vie
            gameManager.EchecCheck();

            NextPerson();
        }
    }

    void ResetTimer()
    {
        timer = baseTimer;
        gameManager.timeManager.SetMaxTime(timer);
    }
}

