using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    PlayerInput Inputs;
    public static PlayerMovement PlayerMovement;
    public static Hydro PlayerHydro; 

    private void Awake()
    {
        Inputs = new PlayerInput();

        Inputs.Movement.Move.performed += ctx =>
        {
            if (Time.timeScale >= 1)
            {
                PlayerMovement.isMoving = true;
                PlayerMovement.CalculateMovement(ctx.ReadValue<Vector2>());
            }
        };

        Inputs.Movement.Move.canceled += ctx => PlayerMovement.Stop(); 

        Inputs.Action.Interact.performed += ctx =>
        {
            PlayerInteract.Interact();
        };

        Inputs.Action.ShiftNozzle.started += ctx => 
        {
            if (ctx.ReadValue<float>() > 0) PlayerHydro.NozzleIndexUp();

            else PlayerHydro.NozzleIndexDown(); 
                
        };

        Inputs.Action.Spray.performed += ctx => PlayerHydro.NozzleOn = true; 
        Inputs.Action.Spray.canceled += ctx => PlayerHydro.NozzleOn = false; 

        Inputs.Movement.Jump.performed += ctx => PlayerMovement.isJumping = true; 

        Inputs.Movement.Jump.canceled += ctx => PlayerMovement.isJumping = false; 

        Inputs.Action.Pause.started += ctx => PauseMenu.OnPause();
    }

    public static void TeleportPlayer(Vector3 position)
    {
        PlayerMovement.Controller.enabled = false;
        PlayerMovement.transform.position = position;
        PlayerMovement.Controller.enabled = true;
    }

    public static void TeleportPlayer(Transform position)
    {
        TeleportPlayer(position.position); 
    }

    private void OnEnable() => Inputs.Enable();

    private void OnDisable() => Inputs.Disable();
}
