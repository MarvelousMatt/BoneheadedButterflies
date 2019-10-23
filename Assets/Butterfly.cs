using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    //Velocity handling
    float xVel;
    float yVel;
    public float drag;
    public float gravity;

    //Inherited values
    public float flapY;
    public float flapX;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug control
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(Vector3.left);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(Vector3.left * -1);
        }

        VelocityHandler();


    }

    void VelocityHandler()
    {
        transform.position += transform.forward * xVel;
        transform.position += transform.up * yVel;

        if(xVel > 0)
        {
            xVel -= drag;

            if (xVel < 0)
                xVel = 0;

        }

        //if!touching ground

        yVel -= gravity;


    }

    void Flap()
    {
        yVel += flapY;
        xVel += flapX;
    }

}
