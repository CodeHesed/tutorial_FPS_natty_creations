using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : Interactable
{
    private PlayerState playerState;
    private GameObject player;
    private Rigidbody rigidBody;
    private string savedPromptMessage;

    //variables
    private bool targetHeld = false;

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
            rigidBody.useGravity = false;
            rigidBody.velocity = Vector3.zero;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            promptMessage = null;
        }
        else
        {
            rigidBody.useGravity = true;
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
                holdObject(player, new List<string>{"left"}, Vector3.forward + Vector3.left);
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
                holdObject(player, new List<string>{"right"}, Vector3.forward + Vector3.right);
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
        if (rigidBody.mass <= playerState.strength) // for targets that are light enough, use only one hand
        {
            if (playerState.leftObject == null)
            {
                LeftInteract();
            }
            else if (playerState.rightObject == null)
            {
                RightInteract();
            }
        }
        else
        {
            if (!targetHeld) // hold
            {
                if (rigidBody.mass <= 2*playerState.strength)
                {
                    holdObject(player, new List<string>{"left", "right"}, Vector3.forward);
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
    private void holdObject (GameObject player, List<string> hands, Vector3 localPosition)
    {
        PlayerState playerState = player.GetComponent<PlayerState>();

        // change variables
        if (hands.Contains("left"))
        {
            playerState.leftObject = this;
        }
        if (hands.Contains("right"))
        {
            playerState.rightObject = this;
        }
        targetHeld = true;

        // set initial location
        transform.SetParent(player.transform);
        transform.localPosition = localPosition;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    
    // function for throwing an object
    private void throwObject (GameObject player, List<string> hands)
    {
        PlayerState playerState = player.GetComponent<PlayerState>();

        // change variables
        if (playerState.leftObject == playerState.rightObject) // for target on both hands
        {
            playerState.leftObject = null;
            playerState.rightObject = null;
        }
        if (hands.Contains("left"))
        {
            playerState.leftObject = null;
        }
        if (hands.Contains("right"))
        {
            playerState.rightObject = null;
        }
        targetHeld = true;

        // apply force - propotional to the number of hands used
        transform.parent = null;
        Vector3 forceDirection = player.GetComponent<PlayerLook>().cam.transform.forward;
        rigidBody.AddForce(forceDirection * hands.Capacity * playerState.strength, ForceMode.Impulse);
    }
}
