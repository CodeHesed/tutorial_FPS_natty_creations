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
        bool leftHandTriggered = inputManager.onFoot.LeftHand.triggered;
        bool rightHandTriggered = inputManager.onFoot.RightHand.triggered;
        

        // cases for throwing away objects
        if (leftHandTriggered && playerState.leftObject != null)
        {
            playerState.leftObject.BaseLeftInteract();
        }
        if (rightHandTriggered && playerState.rightObject != null)
        {
            playerState.rightObject.BaseRightInteract();
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
                if (leftHandTriggered && !rightHandTriggered && playerState.leftObject == null)
                {
                    interactable.BaseLeftInteract();
                }
                else if (!leftHandTriggered && rightHandTriggered && playerState.rightObject == null)
                {
                    interactable.BaseRightInteract();
                }
                else if (leftHandTriggered && rightHandTriggered && playerState.leftObject == null && playerState.rightObject == null)
                {
                    interactable.BaseBothInteract();
                }
            }
        }
        
    }
}
