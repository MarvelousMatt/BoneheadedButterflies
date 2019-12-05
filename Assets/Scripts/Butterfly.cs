using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Butterfly : MonoBehaviour
{
    //Player control values
    bool isPlayer = false;

    [Header("Global Restrictions")]
    //Global butterfly restrictions ( will move to sim manager in future)
    float maxVelocityLimit = 0.5f;
    float minVelocityLimit = -0.5f;

    public float flapTimeVariation = 0.5f;

    //Chance for the butterfly to not turn every frame while wandering
    [Range(0, 100)]
    public float wanderNoTurnChance = 98;

    //How many breed points are needed to enter the breed state (will move to sim manager in future)
    public int breedPointsNeeded = 3;

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
    public float wanderRotSpeed;
    public int stomachCapactity;
    public float wanderFlapFreq;
    public float targetingFlapBelowFreq;
    public float targetingFlapAboveFreq;
    public Color wingColour;
    public float visionRange;
    public float feedTime;


    public enum State { wander, targeting, breeding, feeding };
    [Header("AI Values")]

    public State state = State.wander;
    //Enum for main AI states

    //How full the butterfly's stomach is
    public int stomachFill;

    //bool that decides whether the wandering butterfly is turning left or right
    public bool right;

    //Current breed points
    public int breedPoints;

    //What this butterfly is headed toward
    public GameObject currentTarget;

    //How often the butterfly flaps
    public float currentFlapTime;

    //The instance of visionsphere attatched to this butterfly
    VisionSphere vision;

    //What the butterfly is atop
    GameObject landedOn;

    //The default butterfly rotation, for resetting the angle of the butterfly
    Quaternion defaultRot;

    //Controls the flapping of the coroutine
    bool flapActive = true;

    //Is the feeding coroutine already active
    bool isFeeding = false;

    //

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlapTimer());

        defaultRot = transform.rotation;

        //Manipulate colour later

        vision = transform.GetComponentInChildren<VisionSphere>();

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
            flapActive = false;
            PlayerInput();
            
        }
        else
        {
            flapActive = true;
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
            transform.Rotate(new Vector3(0, -wanderRotSpeed * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, wanderRotSpeed * Time.deltaTime, 0));
        }
    }

    //Base state machine setup
    void ButterflAI()
    {
        switch (state)
        {
            case State.wander:
                Wander();
                break;
            case State.targeting:
                Targeting();
                break;
            case State.feeding:
                Feeding();
                break;
            case State.breeding:
                break;
        }
    }

    //Random flapping and turning until the butterfly sees something interesting
    void Wander()
    {

        if(currentFlapTime != wanderFlapFreq)
        {
            currentFlapTime = wanderFlapFreq;
            flapActive = true;
        }


        if (right)
        {
            transform.Rotate(new Vector3(0, wanderRotSpeed * Time.deltaTime, 0));
        }
        else
        {
            transform.Rotate(new Vector3(0, -wanderRotSpeed * Time.deltaTime, 0));
        }

        if (Random.Range(0, 100) > wanderNoTurnChance)
        {
            right = !right;
        }

        if(stomachFill < stomachCapactity / 2 && vision.RequestTarget(false) != null)
        {
            currentTarget = vision.RequestTarget(false);
            state = State.targeting;
        }

        
        if(vision.RequestTarget(true) != null && vision.RequestTarget(true).CompareTag("Breedable") && breedPoints >= breedPointsNeeded)
        {
            currentTarget = vision.RequestTarget(true);
            state = State.targeting;
        }
        
    }

    //Flaps and turns towards the object that's marked as a target
    void Targeting()
    {
        if (currentTarget == null)
        {
            state = State.wander;
            return;
        }  

        Quaternion lookAngle = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAngle, rotSpeed * Time.deltaTime);

        if (transform.position.y > currentTarget.transform.position.y)
        {
            currentFlapTime = targetingFlapAboveFreq;
        }
        else
        {
            currentFlapTime = targetingFlapBelowFreq;
        }

        if (grounded && landedOn == currentTarget && landedOn.CompareTag("Flower"))
        {
            state = State.feeding;
            breedPoints++;

            if (breedPoints >= breedPointsNeeded)
            {
                gameObject.tag = "Breedable";
            }

        }
        else if(grounded && landedOn == currentTarget && landedOn.CompareTag("Breedable"))
        {
            state = State.breeding;
        }

    }

    void ResetRotation()
    {
        transform.rotation = defaultRot;
    }

    //Sitting still and then taking off after eating
    void Feeding()
    {
        flapActive = false;

        if (!isFeeding)
        {
            StartCoroutine(FeedTimer());
        }

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
            //Extra 0.1f is added to balance out max being exclusive
            yield return new WaitForSeconds(currentFlapTime + Random.Range(-flapTimeVariation, flapTimeVariation + 0.1f));

            if (flapActive)
            {
                Flap();
            }
        }
    }

    IEnumerator FeedTimer()
    {
        isFeeding = true;
        yield return new WaitForSeconds(feedTime);

        if(landedOn == null)
        {
            state = State.wander;
            grounded = false;
            ResetRotation();
        }
        else if(landedOn.GetComponent<Flower>())
        {
            landedOn.GetComponent<Flower>().food--;
            stomachFill++;

            if(stomachFill >= stomachCapactity)
            {
                grounded = false;
                stomachFill = stomachCapactity;
                state = State.wander;
                ResetRotation();
            }


        }
        else
        {
            state = State.wander;
            grounded = false;
            ResetRotation();
        }

        isFeeding = false;

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
        stomachFill--;

        if(stomachFill <= 0)
        {
            Destroy(gameObject);
        }

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
            landedOn = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Butterfly"))
        {
            grounded = false;
            landedOn = null;
        }
    }


}
