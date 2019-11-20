using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Butterfly : MonoBehaviour
{
    //Player control values
    bool isPlayer = false;

    //Global butterfly restrictions ( will move to sim manager in future)
    float maxVelocityLimit = 0.5f;
    float minVelocityLimit = -0.5f;

    //How many breed points are needed to enter the breed state (will move to sim manager in future)
    int breedPointsNeeded = 3;

    [Header("Velocity Handling")]
    public float xVel;
    public float yVel;

    //Essentially gravity but sideways. Slows horizontal momentum to zero but not below
    public float drag;

    public float gravity;

    //Is the butterfly touching a landable surface
    bool grounded = false;

    [Header ("Inherited Values")]
    public float flapY;
    public float flapX;
    public float rotSpeed;
    public int stomachCapactity;
    public float wanderFlapFreq;
    public float targetingFlapBelowFreq;
    public float targetingFlapAboveFreq;
    public Color wingColour;
    public float visionRange;


    public enum State { wander, targeting, breeding, feeding };
    [Header("AI Values")]

    public State state = State.wander;
    //Enum for main AI states

    //How full the butterfly's stomach is
    public int stomachFill;

    

    //Current breed points
    public int breedPoints;

    //What this butterfly is headed toward
    public GameObject currentTarget;

    //How often the butterfly flaps
    public float currentFlapTime;

    // Start is called before the first frame update
    void Start()
    {
        //Manipulate colour later


        //Colour application for wings
        Renderer rend = transform.GetChild(0).GetComponent<Renderer>();

        Material[] materials = rend.materials;
        materials[0].color = wingColour;
        rend.materials = materials;
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPlayer = !isPlayer;
        }

        //Swapping between player and AI input
        if (isPlayer)
        {
            PlayerInput();
            
        }
        else
        {
            ButterflAI();
        }
        
        
        VelocityHandler();


    }

    //Allows the player to directly control a selected butterfly
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -rotSpeed, 0));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, rotSpeed, 0));
        }
    }

    //Base state machine setup
    void ButterflAI()
    {
        switch (state)
        {
            case State.wander:
                break;
            case State.targeting:
                break;
            case State.feeding:
                break;
            case State.breeding:
                break;
        }
    }

    //Random flapping and turning until the butterfly sees something interesting
    void Wander()
    {
        StopAllCoroutines();
        currentFlapTime = wanderFlapFreq;
    }

    //Flaps and turns towards the object that's marked as a target
    void Targeting()
    {

    }

    //Sitting still and then taking off after eating
    void Feeding()
    {

    }

    //Creating children (and possibly laying them)
    void Breeding()
    {

    }

    //Keeps the butterfly flapping. CurrentFlapTime is altered when the butterfly's state is changed
    IEnumerator FlapTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentFlapTime);
            Flap();
        }
    }

    //Enacts gravity and moves the butterfly around
    void VelocityHandler()
    {
        //Restriction on butterfly max speed
        xVel = Mathf.Clamp(xVel, minVelocityLimit, maxVelocityLimit);
        yVel = Mathf.Clamp(yVel, minVelocityLimit, maxVelocityLimit);

        transform.position += transform.forward * xVel;
        transform.position += transform.up * yVel;

        if (!grounded)
        {
            yVel -= gravity;
        }

        if (xVel > 0)
        {
            xVel -= drag;

            if (xVel < 0)
                xVel = 0;

        }
    }

    //Adds the inherited flap values into velocity
    void Flap()
    {
        yVel = 0;

        yVel += flapY;
        xVel += flapX;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Butterfly"))
        {
            yVel = 0;
            xVel = 0;
            grounded = true;
    
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Butterfly"))
        {
            grounded = false;

        }
    }


}
