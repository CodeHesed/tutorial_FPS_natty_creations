using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Interactable : MonoBehaviour
{
    // message displayed to player when looking at an interactable.
    public string promptMessage; 

    // this function is called from our player script.
    public void BaseInteract()
    {
        Interact();
    }

    public void BaseLeftInteract()
    {
        LeftInteract();
    }

    public void BaseRightInteract()
    {
        RightInteract();
    }

    public void BaseBothInteract()
    {
        BothInteract();
    }

    protected virtual void Interact()
    {
        // template function to be over riden by sub-classes
    }
    protected virtual void LeftInteract()
    {
        // template function to be over riden by sub-classes
    }
    protected virtual void RightInteract()
    {
        // template function to be over riden by sub-classes
    }
    protected virtual void BothInteract()
    {
        // template function to be over riden by sub-classes
    }
}
