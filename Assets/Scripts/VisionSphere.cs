using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSphere : MonoBehaviour
{
    //Adds all seen objects to a list, prioritises these objects and sends a response to the parent butterfly

    /*
    //List of all gameobjects in the trigger
    List<GameObject> inTriggerList = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        //Is it food, breedable butterflies and is it not right next to us
        if (other.CompareTag("Flower") || other.CompareTag("Breedable") && Vector3.Distance(transform.position, other.transform.position) > 2)
            inTriggerList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {

        for (int i = 0; i < inTriggerList.Count; i++)
        {
            if (other.gameObject == player)
            {
                inTriggerList.Remove(other.gameObject);
                isSeeingPlayer = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(inTriggerList.Count.ToString());

        if (isSensing == true)
        {
            for (int i = 0; i < inTriggerList.Count; i++)
            {
                //Debug.Log(isSeeingPlayer + " " + inTriggerList[i].name);

                if (player == inTriggerList[i])
                {
                    isSeeingPlayer = true;
                    break;
                }
                else
                {
                    isSeeingPlayer = false;
                }
            }
        }


    }

    */
}
