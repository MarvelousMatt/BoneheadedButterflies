using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSphere : MonoBehaviour
{
    //Adds all seen objects to a list, prioritises these objects and sends a response to the parent butterfly

  
    //List of all gameobjects in the trigger
    List<GameObject> inTriggerList = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        //Is it food, breedable butterflies and is it not right next to us
        if (other.CompareTag("Flower") || other.CompareTag("Breedable"))
            inTriggerList.Add(other.gameObject);
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

    public GameObject RequestTarget()
    {



        return null;
    }
    

    
}
