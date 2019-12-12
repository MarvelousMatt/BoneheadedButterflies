using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{

    const int numberOfStats = 13;

    private Butterfly parent1;
    private Butterfly parent2;

    private int childNumber;
    private float hatchTime = 5;

    private Color colour;

    bool hasParents = false;

    // Start is called before the first frame update
    void Start()
    {
        //Work out childNumber and hatchTime;


        StartCoroutine(Hatch());
    }

    public void SetParents(Butterfly parentA, Butterfly parentB)
    {
        parent1 = parentA;
        parent2 = parentB;

        hatchTime = parent1.eggHatchTime;
        childNumber = parent2.childNumber;

        hasParents = true;
    }

    IEnumerator Hatch()
    {
        yield return new WaitForSeconds(hatchTime);

        if (hasParents)
        {
            if (SimulationManager.instance.butterflies.Count + childNumber > SimulationManager.instance.maxButterflies)
            {
                Destroy(gameObject);
                StopAllCoroutines();
            }
            else
            {
                for (int i = 0; i < childNumber; i++)
                {
                    Generate();
                }
            }


        }
        else
        {
            if (SimulationManager.instance.butterflies.Count + 1 > SimulationManager.instance.maxButterflies)
            {
                Destroy(gameObject);
                StopAllCoroutines();
            }
            else
            {
                Generate();
            }


        }


        Destroy(gameObject);

    }

    void Generate()
    {
        if(!hasParents)
        {
            //Generate between simulation manager restrictions
            Butterfly child = Instantiate(SimulationManager.instance.butterflyPrefab, transform.position, Quaternion.identity).GetComponent<Butterfly>();

            child.flapY = Random.Range(SimulationManager.instance.flapYMin, SimulationManager.instance.flapYMax);

            child.flapX = Random.Range(SimulationManager.instance.flapXMin, SimulationManager.instance.flapXMax);

            child.rotSpeed = Random.Range(SimulationManager.instance.rotSpeedMin, SimulationManager.instance.rotSpeedMax);

            child.wanderRotSpeed = Random.Range(SimulationManager.instance.wanderRotSpeedMin, SimulationManager.instance.wanderRotSpeedMax); 

            child.stomachCapactity = (int)Random.Range(SimulationManager.instance.stomachCapactityMin, SimulationManager.instance.stomachCapactityMax); 

            child.wanderFlapFreq = Random.Range(SimulationManager.instance.wanderFlapFreqMin, SimulationManager.instance.wanderFlapFreqMax); 

            child.targetingFlapBelowFreq = Random.Range(SimulationManager.instance.targetingFlapBelowFreqMin, SimulationManager.instance.targetingFlapBelowFreqMax);
            
            child.targetingFlapAboveFreq = Random.Range(SimulationManager.instance.targetingFlapAboveFreqMin, SimulationManager.instance.targetingFlapBelowFreqMax); 

            child.visionRange = Random.Range(SimulationManager.instance.visionRangeMin, SimulationManager.instance.visionRangeMax); 

            child.feedTime = Random.Range(SimulationManager.instance.feedTimeMin, SimulationManager.instance.feedTimeMax); 

            child.eggHatchTime = Random.Range(SimulationManager.instance.eggHatchTimeMin, SimulationManager.instance.eggHatchTimeMax); 

            child.eggCost = Random.Range(SimulationManager.instance.eggCostMin, SimulationManager.instance.eggCostMax); 

            child.childNumber = Random.Range(SimulationManager.instance.childNumberMin, SimulationManager.instance.childNumberMax);

            child.stomachFill = (int)(child.stomachCapactity * 0.25f);

            SimulationManager.instance.butterflies.Add(child.gameObject);

        }
        else
        {
            //Generate using the two parents


            //Choosing between parents
            //An array of booleans is created. There is a 50% chance that the information will come from either parent which swaps depending on the result of this boolean array
            bool[] statRoll = new bool[numberOfStats];
           
            for (int i = 0; i < statRoll.Length; i++)
            {
                statRoll[i] = (Random.Range(0, 2) != 0);
            }

            //Turning butterflies into stats to be swapped between
            float[] b1 = ButterflyToArray(parent1);
            float[] b2 = ButterflyToArray(parent2);

            float[] childStats = new float[numberOfStats];

            for (int i = 0; i < childStats.Length; i++)
            {
                float num;

                if (statRoll[i])
                {
                    num = b1[i];
                }
                else
                {
                    num = b2[i];
                }

                childStats[i] = num;
            }

            Butterfly child = Instantiate(SimulationManager.instance.butterflyPrefab, transform.position, Quaternion.identity).GetComponent<Butterfly>();

            ArrayToButterfly(childStats,child);

            SimulationManager.instance.butterflies.Add(child.gameObject);
        }
    }

    float[] ButterflyToArray(Butterfly parent)
    {
        float[] stats = new float[numberOfStats];

        stats[0] = parent.flapY;
        stats[1] = parent.flapX;
        stats[2] = parent.rotSpeed;
        stats[3] = parent.wanderRotSpeed;
        stats[4] = parent.stomachCapactity;
        stats[5] = parent.wanderFlapFreq;
        stats[6] = parent.targetingFlapBelowFreq;
        stats[7] = parent.targetingFlapAboveFreq;
        stats[8] = parent.visionRange;
        stats[9] = parent.feedTime;
        stats[10] = parent.eggHatchTime;
        stats[11] = parent.eggCost;
        stats[12] = parent.childNumber;

        return stats;
    }

    Butterfly ArrayToButterfly(float[] stats,Butterfly target)
    {
        Butterfly butterfly = target;

        butterfly.flapY = stats[0];
        butterfly.flapX = stats[1];
        butterfly.rotSpeed = stats[2];
        butterfly.wanderRotSpeed = stats[3];
        butterfly.stomachCapactity = (int)stats[4];
        butterfly.wanderFlapFreq = stats[5];
        butterfly.targetingFlapBelowFreq = stats[6];
        butterfly.targetingFlapAboveFreq = stats[7];
        butterfly.visionRange = stats[8];
        butterfly.feedTime = stats[9];
        butterfly.eggHatchTime = stats[10];
        butterfly.eggCost = (int)stats[11];
        butterfly.childNumber = (int)stats[12];

        butterfly.stomachFill = (int)(butterfly.stomachCapactity * 0.25f);

        return butterfly;
    }

}
