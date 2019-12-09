using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSphere : MonoBehaviour
{
    //Adds all seen objects to a list, prioritises these objects and sends a response to the parent enemy

    //List of all gameobjects in the trigger
    public List<GameObject> inTriggerList = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {

        //Is it food or breedable
        if (other.CompareTag("Breedable") || other.CompareTag("Flower"))
        {
            inTriggerList.Add(other.gameObject);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < inTriggerList.Count; i++)
        {
            if (inTriggerList.Contains(other.gameObject))
            {
                inTriggerList.Remove(other.gameObject);
            }
        }
    }

    public GameObject RequestTarget(bool breedRequest)
    {
        //Search list for targets

        if (inTriggerList.Count == 0)
            return null;

        List<GameObject> possibleTargets = new List<GameObject>();

        //Trim the inTriggerList for objects that may no longer be present, and then finding objects that were requested
        for (int i = 0; i < inTriggerList.Count; i++)
        {
            if (inTriggerList[i] == null)
            {
                inTriggerList.Remove(inTriggerList[i]);
                continue;
            }

            if (breedRequest && inTriggerList[i].CompareTag("Breedable") || inTriggerList[i].CompareTag("Flower"))
            {
                possibleTargets.Add(inTriggerList[i]);
            }
            else if (!breedRequest && inTriggerList[i].CompareTag("Flower"))
            {
                possibleTargets.Add(inTriggerList[i]);
            }
        }


        if (possibleTargets.Count > 0)
        {
            return possibleTargets[0];
        }
        else
        {
            return null;
        }



    }

}