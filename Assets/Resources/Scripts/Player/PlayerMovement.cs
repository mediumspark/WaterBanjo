using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging; 

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _Controller;
    private PlayerAnimations _AnimationController;

    private bool _isGrounded, _isLaunching, _isJumping, _isMoving, _isDashing;

    public bool Grounded => _isGrounded;
    public bool isMoving { get => _isMoving; set => _isMoving = value; }
    public bool isJumping { set => _isJumping = value; }
    public bool isDashing { set => _isDashing = value; }
    public CharacterController Controller => _Controller; 

    [SerializeField]
    float gravity, GroundedOffset;
    [SerializeField]
    GameObject GroundPoint; 

    private LayerMask GroundLayers;

    public Vector3 MoveDirection;

    protected float MovementSpeed = 5.0f;
    protected float VerticleVelocity = 2;
    public float JumpTimer = 1.5f;
    public float JumpForce;

    private float _SprintMultiplier = 1.5f;
    private float _MovementSpeedHolder; 

    protected float angle, targetAngle, turnSmoothVelocity;
    private Vector3 MovementAngle;
    private float turnSmoothTime = 0.1f;

    private bool sprint;

    public bool Sprint { set => sprint = value;  }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        GravityAndJump();
        Move();
        _AnimationController.AniGrounded(_isGrounded);
        _AnimationController.AniMovingSpeed(new Vector2(MoveDirection.x, MoveDirection.z).magnitude);
        _AnimationController.AniVerticleV(VerticleVelocity / 2 -1);
    }

    private void Awake()
    {
        _Controller = GetComponent<CharacterController>();
        _AnimationController = GetComponent<PlayerAnimations>();
        _MovementSpeedHolder = MovementSpeed;
        GroundLayers = 1 << LayerMask.NameToLayer("Ground");
        PlayerControls.PlayerMovement = this; 
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(GroundPoint.transform.position.x, GroundPoint.transform.position.y - GroundedOffset, GroundPoint.transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, GroundedOffset, GroundLayers); 
    }

    public void Stop()
    {
        MoveDirection = new Vector3(0, MoveDirection.y, 0);
        isMoving = false;
        Sprint = false;
    }

    float _JumpTimeoutDelta = 0.1f;
    float _FallTimeoutDelta = 1.0f;
    private bool _Launching = false;
    public bool Launch { set => _Launching = value;  }


    public void GravityAndJump()
    {
        if (_isGrounded)
        {
            MovementSpeed = sprint ? MovementSpeed * _SprintMultiplier : MovementSpeed;
            _FallTimeoutDelta = 5.0f;
            VerticleVelocity = 2.0f;

            // Jump
            if (_isJumping && _JumpTimeoutDelta <= 0.0f)
            {
                MoveDirection.y += (VerticleVelocity * JumpForce * 0.2f +
                                                 (gravity / 2) * Mathf.Pow(0.1f, 2)) / MovementSpeed;
                VerticleVelocity += gravity * 0.2f * JumpForce * Time.deltaTime;
            }
            else if (_Launching)
            {
                VerticleVelocity += Mathf.SmoothDamp(2.0f, transform.position.y + 3.5f, ref VerticleVelocity, 2.0f);
                MoveDirection.y += VerticleVelocity;
                _Launching = false; 
            }
            // jump timeout
            if (_JumpTimeoutDelta >= 0.0f)
            {
                _JumpTimeoutDelta -= Time.deltaTime;
            }
        }

        else
        {
            // reset the jump timeout timer
            _JumpTimeoutDelta = 0.10f;
            MoveDirection.y -= gravity * Time.deltaTime;

            // fall timeout
            if (_FallTimeoutDelta >= 0.0f)
            {
                _FallTimeoutDelta -= Time.deltaTime;
            }

            // if we are not grounded, do not jump
            isJumping = false;

        }

    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, Angle, 0f);
            Vector3 movedir = Quaternion.Euler(0, targetangle, 0) * Vector3.forward;
            MoveDirection = movedir + new Vector3(0, MoveDirection.y);
        }
        
        //Dashing Movement 
        MovementSpeed = !_isDashing ? 5.0f : 10.5f;
        _AnimationController.AniDashing(_isDashing); 
        PlayerCanvas.PlayerLocationCacheObject.LocationCache = transform.position; 
        _Controller.Move(MoveDirection * MovementSpeed * Time.deltaTime);
    }

    public void CalculateMovement(Vector3 InputForMovementAngle)
    {
        targetAngle = Mathf.Atan2(MovementAngle.x, MovementAngle.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        //Calculating the forward angle by using the tangent of the Camera y-axis 
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //Smoothing the angle 
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //Rotates the character towards the smoothed angle 
        MoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        MovementAngle = new Vector3(InputForMovementAngle.x, 0, InputForMovementAngle.y);
        Vector3 NormalizedMovement = MovementAngle.normalized;
        MovementAngle = NormalizedMovement;
    }

    private void OnDrawGizmos()=>
        Gizmos.DrawWireSphere(new Vector3(GroundPoint.transform.position.x,
            GroundPoint.transform.position.y - GroundedOffset, GroundPoint.transform.position.z), 
            GroundedOffset);
    
}
