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
                // change variables
                playerState.leftObject = this;
                targetHeld = true;
                
                // set initial location
                transform.SetParent(player.transform);
                transform.localPosition = Vector3.forward + Vector3.left;
                transform.rotation = new Quaternion(0, 0, 0, 0);

                Debug.Log(gameObject.name + " held on left hand");
            }
        }
        else // throw
        {
            // change variables
            if (playerState.leftObject == playerState.rightObject) // for target on both hands
            {
                playerState.rightObject = null;
            }
            playerState.leftObject = null;
            targetHeld = false;

            // set initial veloctiy
            transform.parent = null;
            Vector3 forceDirection = player.GetComponent<PlayerLook>().cam.transform.forward;
            rigidBody.AddForce(forceDirection * playerState.strength, ForceMode.Impulse);

            Debug.Log(gameObject.name + " thrown away from left hand");            
        }
    }
    protected override void RightInteract()
    {
        if (!targetHeld) // hold
        {
            if (rigidBody.mass <= playerState.strength)
            {
                // change variables
                playerState.rightObject = this;
                targetHeld = true;
                
                // set initial location
                transform.SetParent(player.transform);
                transform.localPosition = Vector3.forward + Vector3.right;
                transform.rotation = new Quaternion(0, 0, 0, 0);

                Debug.Log(gameObject.name + " held on right hand");
            }
        }
        else // throw
        {
            // change variables
            if (playerState.leftObject == playerState.rightObject) // for target on both hands
            {
                playerState.leftObject = null;
            }
            playerState.rightObject = null;
            targetHeld = false;

            // set initial veloctiy
            transform.parent = null;
            Vector3 forceDirection = player.GetComponent<PlayerLook>().cam.transform.forward;
            rigidBody.AddForce(forceDirection * playerState.strength, ForceMode.Impulse);

            Debug.Log(gameObject.name + " thrown away from right hand");
        }
    }
    protected override void BothInteract()
    {        
        if (rigidBody.mass <= playerState.strength) // for targets that are light enough
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
                    // change variables
                    playerState.leftObject = this;
                    playerState.rightObject = this;
                    targetHeld = true;
                    
                    // set initial location
                    transform.SetParent(player.transform);
                    transform.localPosition = Vector3.forward;
                    transform.rotation = new Quaternion(0, 0, 0, 0);

                    Debug.Log(gameObject.name + " held on both hands");
                }
            }
            else
            {
                // change variables
                playerState.leftObject = null;
                playerState.rightObject = null;
                targetHeld = false;

                // set initial veloctiy
                transform.parent = null;
                Vector3 forceDirection = player.GetComponent<PlayerLook>().cam.transform.forward;
                rigidBody.AddForce(forceDirection * 2*playerState.strength, ForceMode.Impulse);

                Debug.Log(gameObject.name + " thrown away from both hands");
            }
        }                 
    }
    
}
