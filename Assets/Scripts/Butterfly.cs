using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    //Global butterfly restrictions
    static float maxVelocityLimit = 0.5f;
    static float minVelocityLimit = -0.5f;

    //Velocity handling
    public float xVel;
    public float yVel;
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
        //Restriction on butterfly max speed
        xVel = Mathf.Clamp(xVel, minVelocityLimit, maxVelocityLimit);
        yVel = Mathf.Clamp(yVel, minVelocityLimit, maxVelocityLimit);

        transform.position += transform.forward * xVel;
        transform.position += transform.up * yVel;

        //if!touching ground

        yVel -= gravity;

        if (xVel > 0)
        {
            xVel -= drag;

            if (xVel < 0)
                xVel = 0;

        }

      


    }

    void Flap()
    {
        yVel = 0;

        yVel += flapY;
        xVel += flapX;
    }

}
