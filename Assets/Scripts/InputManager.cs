using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    // Start is called before the first frame update

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerInteract interact;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>(); 
        look = GetComponent<PlayerLook>();
        
        onFoot.Jump.performed += ctx => motor.Jump(onFoot.Movement.ReadValue<Vector2>());
        look.InitialLook();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // tell the playermotor to move using the value from our movement action.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }


    private void LateUpdate()
    {
        // tell the playerlook to move using the value from our mouse action.
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
