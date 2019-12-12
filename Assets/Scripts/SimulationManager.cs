using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    //Intended to store informaiton about the simulation as well as spawning new butterflies and killing off old ones


    [Header("Global Restrictions")]
    public float maxVelocityLimit = 0.5f;
    public float minVelocityLimit = -0.5f;
    public float yLimit = 100;
    public float flapTimeVariation = 0.5f;
    public int maxButterflies = 100;

    public List<GameObject> butterflies = new List<GameObject>();

    //Chance for the butterfly to not turn every frame while wandering
    [Range(0, 100)]
    public float wanderNoTurnChance = 98;

    //How many breed points are needed to enter the breed state
    public int breedPointsNeeded = 3;

    public float flapYMin;
    public float flapYMax;

    public float flapXMin;
    public float flapXMax;

    public float rotSpeedMin;
    public float rotSpeedMax;

    public float wanderRotSpeedMin;
    public float wanderRotSpeedMax;

    public int stomachCapactityMin;
    public int stomachCapactityMax;

    public float wanderFlapFreqMin;
    public float wanderFlapFreqMax;

    public float targetingFlapBelowFreqMin;
    public float targetingFlapBelowFreqMax;

    public float targetingFlapAboveFreqMin;
    public float targetingFlapAboveFreqMax;

    public float visionRangeMin;
    public float visionRangeMax;

    public float feedTimeMin;
    public float feedTimeMax;

    public float eggHatchTimeMin;
    public float eggHatchTimeMax;

    public int eggCostMin;
    public int eggCostMax;

    public int childNumberMin;
    public int childNumberMax;

    public float xBoundary = 250;
    public float zBoundary = 250;

    public float maxFlowerY = 50;

    public int maxFlowers;

    public float flowerSpawnRate;

    public float flowerSpawnAmount;

    public bool spawnFlowers = true;

    public int flowerFoodValue = 10;

    public int initialFlowers = 50;

    public float objectSpawnAreaModifier = 10;

    public GameObject eggPrefab;
    public GameObject butterflyPrefab;
    public GameObject flowerPrefab;


    public static SimulationManager instance;

    float timeScale = 1;

    //UI stuff
    public int eggSpawnNo;
    public int flowerSpawnNo;

    public List<GameObject> flowers = new List<GameObject>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        StartCoroutine(FlowerSpawnPulse());

        for (int i = 0; i < initialFlowers; i++)
        {
            CreateFlower();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = (timeScale += 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = (timeScale -= 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator FlowerSpawnPulse()
    {
        while (true)
        {
            yield return new WaitForSeconds(flowerSpawnRate);

            if (spawnFlowers && flowers.Count < maxFlowers)
            {
                for (int i = 0; i < flowerSpawnAmount; i++)
                {
                    if (flowers.Count > maxFlowers)
                    {
                        break;
                    }

                    CreateFlower();
                }
            }

        }
    }

    void CreateFlower()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-xBoundary + objectSpawnAreaModifier,xBoundary - objectSpawnAreaModifier),Random.Range(0,maxFlowerY),Random.Range(-zBoundary + objectSpawnAreaModifier,zBoundary - objectSpawnAreaModifier));
        Flower newFlower = Instantiate(flowerPrefab, spawnPos, Quaternion.identity).GetComponentInChildren<Flower>();
        newFlower.food = (int)flowerFoodValue;
        flowers.Add(newFlower.gameObject.transform.parent.gameObject);
    }

    void CreateEgg()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-xBoundary + objectSpawnAreaModifier, xBoundary - objectSpawnAreaModifier), Random.Range(0, 5), Random.Range(-zBoundary + objectSpawnAreaModifier, zBoundary - objectSpawnAreaModifier));
        Instantiate(eggPrefab, spawnPos, Quaternion.identity);
    }

    public void ButtonSpawn(bool isEgg)
    {
        if (isEgg)
        {
            for (int i = 0; i < eggSpawnNo; i++)
            {
                CreateEgg();
            }
        }
        else
        {
            for (int i = 0; i < flowerSpawnNo; i++)
            {
                CreateFlower();
            }
        }
    }

    public void UpdateSpawnNumber(GameObject input)
    {
        if (input.name == "EggInput")
            eggSpawnNo = int.Parse(input.GetComponent<InputField>().text);
        else
        {
            flowerSpawnNo = int.Parse(input.GetComponent<InputField>().text);
        }
    }

}
