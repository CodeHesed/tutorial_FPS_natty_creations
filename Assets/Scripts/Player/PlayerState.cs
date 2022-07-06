using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Camera cam;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject bothHand;
    
    // variables
    public Interactable leftHandObject;
    public Interactable rightHandObject;
    
    // hyper-parameters
    private Vector3 leftHandLocalPosition = new Vector3(-1f, 0f, 1.5f);
    private Vector3 rightHandLocalPosition = new Vector3(1f, 0f, 1.5f);
    public float strength = 5f;

    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;

        leftHand = new GameObject();
        leftHand.name = "leftHand";
        rightHand = new GameObject();
        rightHand.name = "rightHand";
        bothHand = new GameObject();
        bothHand.name = "bothHand";
                
        leftHand.transform.SetParent(cam.transform);
        leftHand.transform.localPosition = leftHandLocalPosition;
        rightHand.transform.SetParent(cam.transform);
        rightHand.transform.localPosition = rightHandLocalPosition;
        bothHand.transform.SetParent(cam.transform);
        bothHand.transform.localPosition = (leftHandLocalPosition + rightHandLocalPosition)/2;
    }
}
