using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Butterfly : MonoBehaviour
{
    //Player control values
    bool isPlayer = false;

    [Header("Velocity Handling")]
    public float xVel;
    public float yVel;

    //Essentially gravity but sideways. Slows horizontal momentum to zero but not below
    public float drag;

    public float gravity;

    //Is the butterfly touching a landable surface
    bool grounded = false;

    [Header("Inherited Values")]
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
    public float eggHatchTime;
    public int eggCost;
    public int childNumber;


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
        vision.gameObject.GetComponent<SphereCollider>().radius = visionRange;

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
                Breeding();
                break;
        }
    }

    //Random flapping and turning until the butterfly sees something interesting
    void Wander()
    {

        if (currentFlapTime != wanderFlapFreq)
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

        if (Random.Range(0, 100) > SimulationManager.instance.wanderNoTurnChance)
        {
            right = !right;
        }

        if(breedPoints >= SimulationManager.instance.breedPointsNeeded)
        {
            transform.GetChild(0).tag = "Breedable";
        }
        else
        {
            transform.GetChild(0).tag = "Untagged";
        }


        if (stomachFill < stomachCapactity / 2 && vision.RequestTarget(false) != null)
        {
            currentTarget = vision.RequestTarget(false);
            state = State.targeting;
        }


        if (vision.RequestTarget(true) != null && vision.RequestTarget(true).CompareTag("Breedable") && breedPoints >= SimulationManager.instance.breedPointsNeeded && vision.RequestTarget(true) != transform.GetChild(0).gameObject)
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
            ResetRotation();
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

        if (grounded && landedOn.CompareTag("Flower"))
        {
            state = State.feeding;
            breedPoints++;

            if (breedPoints >= SimulationManager.instance.breedPointsNeeded)
            {
                gameObject.tag = "Breedable";
            }

        }
        else if (grounded && landedOn.transform.childCount > 0) 
        {
            if  (landedOn.transform.GetChild(0).gameObject == currentTarget && landedOn.transform.GetChild(0).CompareTag("Breedable"))
            {
                state = State.breeding;
                flapActive = false;
                ResetRotation();
            }
  
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
        //Get stats from both butterflies
        //Disable other butterfly
        //create eggs with stats from both
        //egg hatches after a time

        Butterfly other = currentTarget.GetComponentInParent<Butterfly>();
        
        //Possibly move this to a function on other butterfly?
        other.breedPoints -= SimulationManager.instance.breedPointsNeeded;
        other.state = State.wander;
        other.stomachFill -= eggCost;
        other.flapActive = true;
        other.currentTarget = null;

        breedPoints -= SimulationManager.instance.breedPointsNeeded;
        stomachFill -= eggCost;
        state = State.wander;
        flapActive = true;
        currentTarget = null;

        GameObject egg = Instantiate(SimulationManager.instance.eggPrefab,transform.position + transform.up,Quaternion.identity);
        egg.GetComponent<Egg>().SetParents(this,other);

        
    }


    //Keeps the butterfly flapping. CurrentFlapTime is altered when the butterfly's state is changed
    IEnumerator FlapTimer()
    {
        while (true)
        {
            //Extra 0.1f is added to balance out max being exclusive
            yield return new WaitForSeconds(currentFlapTime + Random.Range(-SimulationManager.instance.flapTimeVariation, SimulationManager.instance.flapTimeVariation + 0.1f));

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

        if (landedOn == null)
        {
            state = State.wander;
            grounded = false;
            ResetRotation();
        }
        else if (landedOn.transform.GetComponentInChildren<Flower>())
        {
            landedOn.GetComponent<Flower>().food--;
            stomachFill++;

            if (stomachFill >= stomachCapactity)
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
        xVel = Mathf.Clamp(xVel, SimulationManager.instance.minVelocityLimit, SimulationManager.instance.maxVelocityLimit);
        yVel = Mathf.Clamp(yVel, SimulationManager.instance.minVelocityLimit, SimulationManager.instance.maxVelocityLimit);

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

        if (stomachFill <= 0)
        {
            Destroy(gameObject);
        }

        if(transform.position.y < SimulationManager.instance.yLimit)
        {

            yVel = 0;

            yVel += flapY;
            xVel += flapX;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        yVel = 0;
        xVel = 0;
        grounded = true;
        landedOn = collision.gameObject;
        Debug.Log(landedOn.name);
    }

    void OnCollisionExit(Collision collision)
    {
        grounded = false;
        landedOn = null;
    }

}
