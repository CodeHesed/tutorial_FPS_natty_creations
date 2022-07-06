using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : Interactable
{
    private PlayerState playerState;
    private GameObject player;
    private Rigidbody rigidBody;
    private string savedPromptMessage;
    
    // hyper parameters
    [SerializeField] private float minSpeed = 0;
    [SerializeField] private float maxSpeed = 300f;
    [SerializeField] private float maxDistance = 10f;


    // variables
    private GameObject holdParent = null;
    private bool targetHeld = false;
    private float currentDist = 0f;
    private float currentSpeed = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        playerState = player.GetComponent<PlayerState>();
        rigidBody = GetComponent<Rigidbody>();
        savedPromptMessage = promptMessage;
        // I don't like this, since it won't work for multi player games
    }

    // Update is called once per frame
    void Update()
    {
        if (targetHeld)
        {
            // linear motion
            Vector3 displacement = holdParent.transform.position - rigidBody.position;
            currentDist = displacement.magnitude;
            currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, currentDist / maxDistance);
            rigidBody.velocity = 100 * displacement.normalized * currentSpeed * Time.fixedDeltaTime;
            Debug.Log("Current speed = " + currentSpeed);
            Debug.Log("Displacement.normalized = " + displacement.normalized);
            Debug.Log("rigidBody.velocity = " + rigidBody.velocity);
            // rotational motion
            transform.rotation = new Quaternion(0, 0, 0, 0);

            // remove message
            promptMessage = null;
        }
        else
        {
            promptMessage = savedPromptMessage;
        }
    }

    // this function is where we will design our interaction using code!
    protected override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
    protected override void LeftInteract()
    {
        if (!targetHeld) // hold
        {
            if (rigidBody.mass <= playerState.strength)
            {
                holdObject(player, new List<string>{"left"});
                Debug.Log(gameObject.name + " held on left hand");
            }
        }
        else // throw
        {
            throwObject(player, new List<string>{"left"});
            Debug.Log(gameObject.name + " thrown away by left hand");            
        }
    }
    protected override void RightInteract()
    {
        if (!targetHeld) // hold
        {
            if (rigidBody.mass <= playerState.strength)
            {
                holdObject(player, new List<string>{"right"});
                Debug.Log(gameObject.name + " held on right hand");
            }
        }
        else // throw
        {
            throwObject(player, new List<string>{"right"});
            Debug.Log(gameObject.name + " thrown away by right hand");
        }
    }
    protected override void BothInteract()
    {        
        if (rigidBody.mass <= playerState.strength) // for targets that are light enough, use only left hand
        {
            LeftInteract();
        }
        else
        {
            if (!targetHeld) // hold
            {
                if (rigidBody.mass <= 2*playerState.strength)
                {
                    holdObject(player, new List<string>{"left", "right"});
                    Debug.Log(gameObject.name + " held on both hands");
                }
            }
            else // throw
            {
                throwObject(player, new List<string>{"left", "right"});
                Debug.Log(gameObject.name + " thrown away by both hands");
            }
        }                 
    }

    // function for holding an object
    private void holdObject (GameObject player, List<string> hands)
    {
        PlayerState playerState = player.GetComponent<PlayerState>();
        
        // change variables
        targetHeld = true;
        rigidBody.useGravity = false;        
        if (hands.Count == 2) 
        {
            playerState.leftHandObject = this;
            playerState.rightHandObject = this;
            holdParent = playerState.bothHand;
        }
        else
        {
            if (hands.Contains("left"))
            {
                playerState.leftHandObject = this;
                holdParent = playerState.leftHand;
            }
            else if (hands.Contains("right"))
            {
                playerState.rightHandObject = this;
                holdParent = playerState.rightHand;
            }
        }        

        // set initial location
        rigidBody.position = holdParent.transform.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    
    // function for throwing an object
    private void throwObject (GameObject player, List<string> hands)
    {
        PlayerState playerState = player.GetComponent<PlayerState>();

        // change variables
        targetHeld = false;
        rigidBody.useGravity = true;
        if (playerState.leftHandObject == playerState.rightHandObject) // for target on both hands
        {
            playerState.leftHandObject = null;
            playerState.rightHandObject = null;
        }
        if (hands.Contains("left"))
        {
            playerState.leftHandObject = null;
        }
        if (hands.Contains("right"))
        {
            playerState.rightHandObject = null;
        }

        // apply force - propotional to the number of hands used
        transform.parent = null;
        Vector3 forceDirection = player.GetComponent<PlayerLook>().cam.transform.forward;
        rigidBody.AddForce(forceDirection * hands.Count * playerState.strength, ForceMode.Impulse);
    }
}
