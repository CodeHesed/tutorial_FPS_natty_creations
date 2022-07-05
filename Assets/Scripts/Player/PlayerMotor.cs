
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    private Vector3 moveDirection = Vector3.zero;

    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }
    
    // receive the inputs from our InputManager.cs and aplly them to our character controller.
    public void ProcessMove(Vector2 input)
    {
        if(isGrounded)
        {
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            moveDirection = transform.TransformDirection(moveDirection);
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
        else
        {
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
        // falling motion
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0 )
        {
            playerVelocity.y = -0.5f;
        }
        controller.Move(playerVelocity * Time.deltaTime);    
    }

    public void Jump(Vector2 input)
    {
        // setting initial jump velocity
        if (isGrounded)
        {
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            moveDirection = transform.TransformDirection(moveDirection);
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2F * gravity);
        }
    }

}
