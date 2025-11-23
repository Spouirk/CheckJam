using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TMP_Text roundText;

    [Header("Current & Next Person")]
    [HideInInspector] public Person currentPerson;
    [HideInInspector] public Person nextPerson;

    private int currentCheckIndex = 0;
    [HideInInspector] public float timer;
    public float baseTimer = 100f;
    public float timerDecrease = 0.99f;

    public float keysDuration;
    private float keysTimer = 0f;
    public float keysDurationDecrease = 0.95f;
    private int previousRound = -1;

    [SerializeField] private int nbOfPersonGenerated;
    public int nbOfRound;

    [Header("Animation")]
    private bool isPlayingAnimation = false;

    [Header("Canvas")]
    public Transform keysContainer;
    public Transform posesContainer;

    private bool pauseTimer;

    void Start()
    {
        pauseTimer = false;

        keysDuration = 1;
        nbOfRound = 1; nbOfPersonGenerated = 0;
        roundText.text = "Person:" + nbOfRound.ToString();

        InitializeChecks();
        GeneratePersons();          // On génère les 2 premières personnes
        ResetTimer();
        DisplayCurrentCheckInfo();
    }

    private void Update()
    {
        HandleTimer();
        if (!pauseTimer) HandlePlayerInput();

        HandleKeysScale();
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
        // Pool vide : Créer une nouvelle personne (nouvelle = inconnue)
        if (personsPool.Count == 0)
        {
            Person person = CreateUniquePerson();
            person.isKnown = false;
            personsPool.Add(person);
            return person;
        }

        // Pool existant : 70% de chance de prendre une personne existante, sinon en créer une nouvelle
        bool useExisting;       

        if (nbOfRound == 1)
        {
            useExisting = false;
        }
        else
        {
            if (nbOfRound % 3 == 0)
            {
                useExisting = false;   // Tous les 4 rounds
            }
            else
            {
                useExisting = true;    // Les autres rounds
            }
        }

        if (useExisting)
        {
            // Ne pas modifier isKnown ici on ne marque connu que quand on l'affiche réellement
            return personsPool[Random.Range(0, personsPool.Count)];
        }
        else
        {
            // Cr�er une personne unique (toujours inconnue)
            Person newPerson = CreateUniquePerson();

            if (newPerson != null)
            {
                newPerson.isKnown = false;
                personsPool.Add(newPerson);
                return newPerson;
            }
            else
            {
                return personsPool[Random.Range(0, personsPool.Count)];
            }
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

        int safety = 0; // Anti-loop

        while (!unique)
        {
            safety++;

            if (safety > 500)
            {
                return null;
            }

            // Créer le GO
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
            accessorySr.sortingOrder = 2;

            // Génération des checks
            nbOfPersonGenerated++;
            int checksCount = 1;            

            if (nbOfPersonGenerated >= 4 && nbOfPersonGenerated <= 5)
            {
                checksCount = Random.Range(1, 3);       // 1 ou 2 checks
            }
            else if (nbOfPersonGenerated >= 6 && nbOfPersonGenerated <= 13)
            {
                checksCount = Random.Range(1, 4);       // 1 à 3 checks
            }
            else if (nbOfPersonGenerated >= 14)
            {
                checksCount = Random.Range(3, 5);       // 2 à 4 checks
            }

            person.personChecks = new List<Check>();

            for (int i = 0; i < checksCount; i++)
            {
                Check randomCheck = checksPool[Random.Range(0, checksPool.Count)];
                person.personChecks.Add(randomCheck);
            }

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = person.personChecks[0].sprite;
            sr.color = person.skinColor;
            sr.sortingOrder = 1;

            // Vérifier que la personne est unique
            bool exists = false;

            foreach (var existing in personsPool)
            {
                bool sameSkin = existing.skinColor.Equals(person.skinColor);
                bool sameAccessory = existing.accessory == person.accessory;

                if (sameSkin && sameAccessory)
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

    /*
    private bool ListsAreEqual(List<Check> a, List<Check> b)
    {
        if (a.Count != b.Count) return false;

        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }

        return true;
    }
    */

    // Définir la personne actuelle et la personne suivante
    public void GeneratePersons()
    {
        currentPerson = GetRandomPerson();
        nextPerson = GetRandomPerson();
        if (currentPerson != nextPerson) nextPerson.gameObject.SetActive(false);
    }

    // Passer à la personne suivante quand le joueur � check la personne acutelle
    public void NextPerson()
    {
        nbOfRound++;
        roundText.text = "Person:" + nbOfRound.ToString();

        // La personne next devient current
        currentPerson.gameObject.SetActive(false);
        currentPerson = nextPerson;
        currentPerson.gameObject.SetActive(true);

        // G�n�rer une nouvelle next
        nextPerson = GetRandomPerson();
        if (currentPerson != nextPerson) nextPerson.gameObject.SetActive(false);

        baseTimer *= timerDecrease;
        keysDuration *= keysDurationDecrease;

        currentCheckIndex = 0;
        ResetTimer();

        DisplayCurrentCheckInfo();
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
                    //SoundManager.PlaySound(SoundType.SWOOCH);
                    srPlayer.GetComponent<Animator>().SetTrigger("Success");
                    currentPerson.GetComponent<Animator>().SetTrigger("Success");


                    // Jouer la musique
                    switch (expected.checkName)
                    {
                        case "Jutsu":
                            SoundManager.PlaySound(SoundType.KATON);
                            break;

                        case "Team Jul":
                            SoundManager.PlaySound(SoundType.JUL);
                            break;

                        case "Pinky":
                            SoundManager.PlaySound(SoundType.PINKY);
                            break;

                        case "Love U":
                            SoundManager.PlaySound(SoundType.HEARTBEAT);
                            break;

                        case "Kon":
                            SoundManager.PlaySound(SoundType.KON);
                            break;

                        case "K-Pop Heart":
                            SoundManager.PlaySound(SoundType.KPOP);
                            break;

                        case "High five":
                            SoundManager.PlaySound(SoundType.SLAP);
                            break;

                        case "Fist Bump":
                            SoundManager.PlaySound(SoundType.PUNCH);
                            break;

                        default:
                            break;
                    }

                    if (currentCheckIndex >= currentPerson.personChecks.Count)
                    {
                        StartCoroutine(HandlePoseSequence(expected));

                        /*
                        for (int i = 0; i < posesContainer.childCount; i++)
                            posesContainer.GetChild(i).gameObject.SetActive(false);

                        posesContainer.GetChild(expected.keyIndex).gameObject.SetActive(true);
                        srPlayer.gameObject.SetActive(false);
                        currentPerson.gameObject.SetActive(false);

                        if (!currentPerson.isKnown)
                        {
                            currentPerson.isKnown = true;
                        }

                        NextPerson();

                        posesContainer.GetChild(expected.keyIndex).gameObject.SetActive(false);
                        srPlayer.gameObject.SetActive(true);
                        currentPerson.gameObject.SetActive(true);
                        */
                    }
                    else
                    {
                        DisplayCurrentCheckInfo();
                    }
                }
                else
                {
                    SoundManager.PlaySound(SoundType.FAIL);
                    gameManager.EchecCheck();
                }
            }
        }
    }

    private IEnumerator HandlePoseSequence(Check expected)
    {
        // Désactiver toutes les poses
        for (int i = 0; i < posesContainer.childCount; i++)
            posesContainer.GetChild(i).gameObject.SetActive(false);

        // Activer la pose correcte
        posesContainer.GetChild(expected.keyIndex).gameObject.SetActive(true);

        // Cacher joueur + personne
        srPlayer.gameObject.SetActive(false);
        currentPerson.gameObject.SetActive(false);

        // Marquer la personne connue si ce n'est pas le cas
        if (!currentPerson.isKnown)
            currentPerson.isKnown = true;

        keysContainer.gameObject.SetActive(false);

        pauseTimer = true;

        // Pause de 1 seconde
        yield return new WaitForSeconds(0.5f);

        pauseTimer = false;

        keysContainer.gameObject.SetActive(true);

        // Passer à la personne suivante
        NextPerson();

        // Nettoyage : désactiver la pose et réafficher
        posesContainer.GetChild(expected.keyIndex).gameObject.SetActive(false);
        srPlayer.gameObject.SetActive(true);
        currentPerson.gameObject.SetActive(true);
    }


    void DisplayCurrentCheckInfo()
    {
        if (currentPerson == null) return;

        Check check = currentPerson.personChecks[currentCheckIndex];

        for (int i = 0; i < keysContainer.childCount; i++)
            keysContainer.GetChild(i).gameObject.SetActive(false);

        keysContainer.GetChild(check.keyIndex).gameObject.SetActive(true);

        // CAS 1 : Personne inconnue
        if (!currentPerson.isKnown)
        {
            // Tant que la personne est inconnue : on montre TOUS les checks
            currentPerson.GetComponent<SpriteRenderer>().sprite = check.sprite;

            return;
        }

        // CAS 2 : Personne connue
        if (currentCheckIndex == 0)
        {
            // Premier check : sprite neutre
            currentPerson.GetComponent<SpriteRenderer>().sprite = baseCheckSprite;

            Debug.Log("Personne connue : pas d'indice pour le premier check.");
            return;
        }

        // CAS 3 : Personne connue + check > 0
        // On montre le check précédent
        Check previousCheck = currentPerson.personChecks[currentCheckIndex - 1];

        currentPerson.GetComponent<SpriteRenderer>().sprite = previousCheck.sprite;
    }

    void HandleKeysScale()
    {
        bool roundChanged = nbOfRound != previousRound;

        // --- Reset si nécessaire ---
        if (!currentPerson.isKnown || roundChanged)
        {
            keysContainer.transform.localScale = Vector3.one;
            keysTimer = 0f;
            previousRound = nbOfRound;   // très important
            return;                      // on ne fait rien d’autre ce frame
        }

        // --- Cas isKnown = true et round identique -> réduction ---
        keysTimer += Time.deltaTime / keysDuration;

        float newScale = Mathf.Lerp(1f, 0f, keysTimer);
        keysContainer.transform.localScale = new Vector3(newScale, newScale, 1f);
    }

    void HandleTimer()
    {
        if (!pauseTimer)
        {
            timer -= Time.deltaTime;
        }

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

