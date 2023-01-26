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
    }

    //Needs Update
    private void UpdateLedgeDetection()
    {
        Vector3 Direction = new Vector3(MoveDirection.x * 0.1f, 2, MoveDirection.z * 0.1f); 
        ledgeDetectionPoint = _Controller.bounds.center + Direction + ledgeDetectionOffset;

        isOnLedge = Vector3.Distance(transform.position + HalfVector.HalfUp + HalfVector.HalfForward
            //* (_Controller.bounds.size.y + 2.0f)
            , ClosestLedgePoint.position) < 1 && !_isGrounded;

    }

    private void UpdateLedgeCheck(ControllerColliderHit hit)
    {
        if (hit.normal.y < 0.1f && !_isOnLedge)
        {
            //Debug.Log("Wall Hit");
            ledgeDetectionPoint = new Vector3(transform.position.x,
                hit.collider.bounds.size.y + hit.transform.position.y, 
                transform.position.z);
            ClosestLedgePoint.position = new Vector3(transform.position.x, 
                hit.collider.bounds.max.y, 
                transform.position.z);
            ClosestLedgePoint.rotation = transform.rotation;
            UpdateLedgeDetection();
            LedgeGetupPoint.localPosition = HalfVector.HalfUp + HalfVector.SesquiForward;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        WallJumpDirection = hit.normal;
        _isOnWall = !_isGrounded && hit.normal.y < 0.1f;

        if (!(hit.collider is MeshCollider))
        {
            UpdateLedgeCheck(hit);

           // float distance = Vector3.Distance(ledgeDetectionPoint, ClosestLedgePoint.position);
        }
    }

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

            else if (_Launching)
            {
                VerticleVelocity = JumpForce * 2.0f;
                _Launching = false;
            }

            // jump timeout
            if (_JumpTimeoutDelta >= 0.0f)
            {
                _JumpTimeoutDelta -= Time.deltaTime;
            }
        }

        //Falling
        else
        {
            // reset the jump timeout timer
            _JumpTimeoutDelta = 0.10f;
            if(isOnLedge)
            {
                VerticleVelocity = 0;
                return; 
            }

            VerticleVelocity = !_isHovering ?
                VerticleVelocity - gravity * Time.deltaTime * JumpForce : 0;

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


        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(ledgeDetectionPoint, 0.25f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ClosestLedgePoint.position, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(LedgeGetupPoint.position, 0.5f); 
    }
#endif
}
