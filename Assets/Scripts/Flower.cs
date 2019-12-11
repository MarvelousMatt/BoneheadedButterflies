using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{

    [SerializeField]
    private int foodAmount = 100;

    [SerializeField]
    public int food
    {
        get
        {
            return foodAmount;
        }
            
        set
        {
            foodAmount = value;

            if(foodAmount <= 0)
            {
                SimulationManager.instance.flowers.Remove(transform.parent.gameObject);
                Destroy(transform.parent.gameObject);
            }
        }
    }

}
