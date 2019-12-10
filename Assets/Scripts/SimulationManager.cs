using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    //Intended to store informaiton about the simulation as well as spawning new butterflies and killing off old ones


    [Header("Global Restrictions")]
    //Global butterfly restrictions ( will move to sim manager in future)
    public float maxVelocityLimit = 0.5f;
    public float minVelocityLimit = -0.5f;
    public float yLimit = 100;
    public float flapTimeVariation = 0.5f;

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



    public static SimulationManager instance;

    float timeScale = 1;

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
    }

    public GameObject eggPrefab;
    public GameObject butterflyPrefab;





    //Max velocity
    //Breed points required


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = timeScale + 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = timeScale - 0.1f;
        }
    }
}
