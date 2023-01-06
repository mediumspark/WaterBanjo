using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _Controller;
    private PlayerAnimations _AnimationController;
    public CharacterController Controller => _Controller;
    public Hydro Hydro => GetComponent<Hydro>(); 
    private bool _isGrounded, _isLaunching, _isJumping, _isMoving,
        _isDashing, _isOnWall, _isOnLedge, _isHovering;

    public bool Grounded => _isGrounded;
    public bool isMoving { get => _isMoving; set => _isMoving = value; }
    public bool isJumping { set => _isJumping = value; }
    public bool isDashing { set => _isDashing = value; }
    public bool isOnWall => _isOnWall;
    public bool isOnLedge
    {
        get => _isOnLedge;
        set
        {
            if (_isOnLedge == false && value == true)
            {
                _AnimationController.HitLedge();
            }
            _isOnLedge = value;

        }
    }

    public bool isHovering { get => _isHovering; set => _isHovering = value; }
    
    [Header("Gravity")]
    [SerializeField]
    float gravity;
    [SerializeField]
    float GroundedOffset;
    [SerializeField]
    GameObject GroundPoint;

    private LayerMask GroundLayers;

    [Header("Movement")]
    public Vector3 MoveDirection;

    protected float MovementSpeed = 5.0f;
    protected float VerticleVelocity = -2;
    public float JumpTimer = 1.5f;
    public float JumpForce;

    private float _SprintMultiplier = 1.5f;
    private float _MovementSpeedHolder;

    protected float turnSmoothVelocity;
    private float _turnSmoothTime = 0.1f;

    float _JumpTimeoutDelta = 0.1f;
    float _FallTimeoutDelta = 1.0f;
    private bool _Launching = false;
    public bool Launch { set => _Launching = value; }
    private Vector3 LastMove; 

    [Header("Wall Interactions")]
    [SerializeField]
    private Vector3 ledgeDetectionOffset;
    private Vector3 ledgeDetectionPoint;
    [SerializeField]
    private float ledgeDetectionSize;
    private Vector3 WallJumpDirection;
    [SerializeField]
    private Transform LedgeGetupPoint;
    public Transform GetUpFromLedgePoint => LedgeGetupPoint; 
    [SerializeField]
    private Transform ClosestLedgePoint;

    private void Awake()
    {
        _Controller = GetComponent<CharacterController>();
        _AnimationController = GetComponent<PlayerAnimations>();
        _MovementSpeedHolder = MovementSpeed;
        GroundLayers = 1 << LayerMask.NameToLayer("Ground");
        PlayerControls.PlayerMovement = this;
/*
        gameObject.AddComponent<TesterMovement>();
        GetComponent<PlayerMovement>().enabled = false; */
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        _AnimationController.AniGrounded(_isGrounded);
        _AnimationController.AniMovingSpeed(new Vector2(MoveDirection.x, MoveDirection.z).magnitude);
        _AnimationController.AniVerticleV(VerticleVelocity / 2 - 1);
        _AnimationController.AniOnLedge(_isOnLedge);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        UpdateLedgeDetection();
       // UpdateWallCheckLocation();
    }

    //Needs Update
    private void UpdateLedgeDetection()
    {
        Vector3 Direction = new Vector3(MoveDirection.x * 0.1f, 2, MoveDirection.z * 0.1f); 
        ledgeDetectionPoint = _Controller.bounds.center + Direction + ledgeDetectionOffset;
    }

    private void LedgeCheck(ControllerColliderHit hit)
    {
        if (hit.normal.y < 0.1f && !_isOnLedge)
        {
            //Debug.Log("Wall Hit");
            ledgeDetectionPoint = new Vector3(transform.position.x, hit.collider.bounds.size.y + hit.transform.position.y, transform.position.z);
            ClosestLedgePoint.position = new Vector3(transform.position.x, hit.collider.bounds.size.y + hit.transform.position.y, transform.position.z);
            ClosestLedgePoint.rotation = transform.rotation;
        }
    }

  //  private ControllerColliderHit Wall_Hit;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
     //   Wall_Hit = hit;
        WallJumpDirection = hit.normal;
     //   Debug.Log(WallJumpDirection);
        _isOnWall = !_isGrounded && hit.normal.y < 0.1f;

        ClosestLedgePoint.position = hit.collider.ClosestPoint(ledgeDetectionPoint);

        LedgeCheck(hit);

        float distance = Vector3.Distance(ledgeDetectionPoint, ClosestLedgePoint.position);

        /*        if (distance <= ledgeDetectionSize)
                {
                    _isOnLedge = true;
                }
        */

        isOnLedge = Vector3.Distance(transform.position + Vector3.up * (_Controller.bounds.size.y + 2.0f), ClosestLedgePoint.position) < 1 && !_isGrounded;


        //ClosestLedgePoint = hit.point + Vector3.one * hit.collider.bounds.max.y;
        //new Vector3(hit.point.x, hit., hit.point.z); 
        // _nearClimbableLedge = LedgeCheck(hit, hit.normal.y > 0.5f && !_isGrounded); 
        // ; 
    }
    /*    private void UpdateWallCheckLocation()
        {
            ///Character bounds center is around the center of the character controller
            ///Direction is the MoveDireciton the player set to 1 and is forced up on the y
            ///because that's the current movement velocity 
            ///Then it's placed .5 forward and .5 up to just in front of where the player would normal deem visible
            Vector3 Direction = MoveDirection;
            Direction.Scale(Vector3.one);
            WallDetector = _Controller.bounds.center + Direction.normalized + Vector3.up;

        }*/

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(GroundPoint.transform.position.x, GroundPoint.transform.position.y - GroundedOffset, GroundPoint.transform.position.z);
        _isGrounded = Physics.CheckSphere(spherePosition, GroundedOffset, GroundLayers);
    }

    public void WallJump()
    {
        VerticleVelocity = JumpForce;
        MoveDirection = WallJumpDirection * MovementSpeed;
        LastMove = MoveDirection;
    }

    public void Hover()
    {
        _isHovering = true; 
    }

    public void NormalJump()
    {
        _isJumping = true;
    }

    public void NormalJumpEnd()
    {
        _isJumping = false;
        _isHovering = false;
    }

    public void CalculateGravityAndJump()
    {
        if (_isGrounded)
        {
            _FallTimeoutDelta = 5.0f;
            VerticleVelocity = -2.0f;//2.0f;
            _isHovering = false;

            // Jump
            if (_isJumping)
                VerticleVelocity = JumpForce;

            
            if (_isJumping && _JumpTimeoutDelta <= 0.0f)
            {
                /*     MoveDirection.y += (VerticleVelocity * JumpForce * 0.2f +
                                                      (gravity / 2) * Mathf.Pow(0.1f, 2)) / MovementSpeed;
                */
                 //VerticleVelocity += gravity * 0.2f * JumpForce * Time.deltaTime;
            }
            

            else if (_Launching)
            {
                VerticleVelocity += Mathf.SmoothDamp(2.0f, transform.position.y + 3.5f, ref VerticleVelocity, 2.0f);
                _Launching = false;
            }

            // jump timeout
            if (_JumpTimeoutDelta >= 0.0f)
            {
                _JumpTimeoutDelta -= Time.deltaTime;
            }
        }

        else if (_isOnLedge)
        {
            MoveDirection = Vector3.zero; 
        }

        //Falling
        else
        {
            // reset the jump timeout timer
            _JumpTimeoutDelta = 0.10f;
            //MoveDirection.y -= gravity * Time.deltaTime;
            VerticleVelocity = !_isHovering ? VerticleVelocity - gravity * Time.deltaTime * JumpForce : 0; 

            // fall timeout
            if (_FallTimeoutDelta >= 0.0f)
            {
                _FallTimeoutDelta -= Time.deltaTime;
            }

            Hydro.HoverWalk(_isHovering);

            // if we are not grounded, do not jump
            _isJumping = false;
            if (!isHovering)
            {
                MoveDirection = LastMove;
            }
        }
    }

    private void CalculateMovement()
    {
        MoveDirection = Vector3.zero;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && !_isOnLedge)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, _turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, Angle, 0f);
            Vector3 movedir = Quaternion.Euler(0, targetangle, 0) * Vector3.forward;
            MoveDirection += movedir;
        }

        CalculateGravityAndJump();

        MoveDirection.y = 0;
        MoveDirection.Normalize();
        MoveDirection *= MovementSpeed;
        MoveDirection.y = VerticleVelocity;

        //Dashing Movement 
        MovementSpeed = !_isDashing ? 5.0f : 10.5f;
        _AnimationController.AniDashing(_isDashing);
        PlayerCanvas.PlayerLocationCacheObject.LocationCache = transform.position;

        Controller.Move(MoveDirection * Time.deltaTime);
        LastMove = MoveDirection;
    }

/*    public void CalculateMovement(Vector3 InputForMovementAngle)
    {
        targetAngle = Mathf.Atan2(MovementAngle.x, MovementAngle.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        //Calculating the forward angle by using the tangent of the Camera y-axis 
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, _turnSmoothTime);
        //Smoothing the angle 
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //Rotates the character towards the smoothed angle 
        MoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        MovementAngle = new Vector3(InputForMovementAngle.x, 0, InputForMovementAngle.y);
        Vector3 NormalizedMovement = MovementAngle.normalized;
        MovementAngle = NormalizedMovement;
    }*/

    public void GetUpFromLedge()
    {
        if (_isOnLedge)
        {
            _AnimationController.GetUpTrigger();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Ground Gizmo
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(new Vector3(GroundPoint.transform.position.x,
            GroundPoint.transform.position.y - GroundedOffset, GroundPoint.transform.position.z),
            GroundedOffset);

        //Draw Wall Detection
        Gizmos.color = Color.cyan;
      //  Gizmos.DrawWireCube(WallDetector, HalfVector.Half);
      
        //Ledge Detection
        //Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ledgeDetectionPoint, 0.25f);
        Gizmos.DrawWireSphere(ClosestLedgePoint.position, 0.25f);
        Gizmos.DrawWireSphere(LedgeGetupPoint.position, 0.25f); 
    }

#endif
}


public class TesterMovement : MonoBehaviour
{
    Vector3 moveVector;
    Vector3 lastMove;
    float speed = 8f;
    float jf = 8;
    float grav = 25;
    float VV;
    CharacterController CC;


    private void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.z = Input.GetAxisRaw("Vertical");

        if (CC.isGrounded)
        {
            VV = -1;

            if (Input.GetKeyDown(KeyCode.H))
            {
                VV = jf;
            }
        }
        else
        {
            VV -= grav * Time.deltaTime;
            moveVector = lastMove;
        }

        moveVector.y = 0;
        moveVector.Normalize();
        moveVector *= speed;
        moveVector.y = VV;

        CC.Move(moveVector * Time.deltaTime);
        lastMove = moveVector;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!CC.isGrounded && hit.normal.y < 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                VV = jf;
                moveVector = hit.normal * speed;
            }
        }
    }
}