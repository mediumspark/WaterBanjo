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
                PlayerMovement.GetUpFromLedge();
                // PlayerMovement.CalculateMovement(ctx.ReadValue<Vector2>());
            }
        };

      //  Inputs.Movement.Move.canceled += ctx => PlayerMovement.Stop(); 

        Inputs.Action.Interact.performed += ctx =>
        {
            PlayerMovement.GetUpFromLedge();
            PlayerInteract.Interact();
        };

        Inputs.Action.ShiftNozzle.started += ctx => 
        {
            if (ctx.ReadValue<float>() > 0) PlayerHydro.NozzleIndexUp();

            else PlayerHydro.NozzleIndexDown(); 
                
        };

        Inputs.Action.Spray.performed += ctx => PlayerHydro.NozzleOn = true; 
        Inputs.Action.Spray.canceled += ctx => PlayerHydro.NozzleOn = false; 

        Inputs.Movement.Jump.performed += ctx =>
        {
            PlayerMovement.GetUpFromLedge();

            if (!PlayerMovement.Grounded && !PlayerMovement.isOnWall)
            {
                PlayerMovement.Hover();
                return;
            }

                //PlayerMovement.isJumping = true;
            if (PlayerMovement.isOnWall) PlayerMovement.WallJump();
            else PlayerMovement.NormalJump();
        };

        Inputs.Movement.Jump.canceled += ctx => PlayerMovement.NormalJumpEnd();


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
