using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;
    private PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        bool interactTriggered = inputManager.onFoot.Interact.triggered;
        bool leftHandPress = inputManager.onFoot.LeftHandPress.triggered;
        bool rightHandPress = inputManager.onFoot.RightHandPress.triggered;
        bool leftHandRelease = inputManager.onFoot.LeftHandRelease.triggered;
        bool rightHandRelease = inputManager.onFoot.RightHandRelease.triggered;

        // cases for throwing away objects
        if (leftHandRelease && rightHandRelease)
        {
            if (playerState.leftHand == playerState.rightHand) // if one object is held with both hands
            {
                if (playerState.leftHand != null) playerState.leftHand.BaseBothInteract();
            }
            else // if object on each hand is different
            {
                if (playerState.leftHand != null) playerState.leftHand.BaseLeftInteract();
                if (playerState.rightHand != null) playerState.rightHand.BaseRightInteract();   
            }
        }
        else if (leftHandRelease && playerState.leftHand != null)
        {
            playerState.leftHand.BaseLeftInteract();
        }
        else if (rightHandRelease && playerState.rightHand != null)
        {
            playerState.rightHand.BaseRightInteract();
        }

        // create a ray at the center of the camera, shooting outwards.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo; // variable to store our collision information.

        // if ray hits an object
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                
                playerUI.UpdateText(interactable.promptMessage);
                
                // interaction by keypad
                if (interactTriggered)
                {
                    interactable.BaseInteract();
                }

                // interaction by hands
                if (leftHandPress && !rightHandPress && playerState.leftHand == null)
                {
                    interactable.BaseLeftInteract();
                }
                else if (!leftHandPress && rightHandPress && playerState.rightHand == null)
                {
                    interactable.BaseRightInteract();
                }
                else if (leftHandPress && rightHandPress && playerState.leftHand == null && playerState.rightHand == null)
                {
                    interactable.BaseBothInteract();
                }
                
            }
        }

    }
}
