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
                Destroy(transform.parent.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
