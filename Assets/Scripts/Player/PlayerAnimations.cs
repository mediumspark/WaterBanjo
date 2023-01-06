using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging; 

public class PlayerAnimations : MonoBehaviour
{
    private Animator PlayerAnimator;
    private float TimeToIdle = 1.5f;
    private float IdleTimer;
    private float IdleTimerS { 
        get 
        {
            return IdleTimer;
        }
        set
        {
            IdleTimer = value;
            Idling = false; 
            CFL.m_RecenterToTargetHeading.m_enabled = false; 
        }
    }

    private float TimeToGroundPound = 1.75f; 
    private float FallTimer; 
    private float FallTimerS { get => FallTimer;
        set => FallTimer = value;
    }

    private bool Grounded = false; 
    private bool GroundPounding = false; 
    private bool Idling = false;
    private bool HangingFromLedge = false;
    private CinemachineFreeLook CFL;

    [Header("Rigging")]
    [SerializeField]
    private MultiAimConstraint LeftRrigging;
    [SerializeField]
    private MultiAimConstraint RightRigging;

    Hydro Hydro; 

    private void Awake()
    {
        CFL = GetComponentInChildren<CinemachineFreeLook>();
        PlayerAnimator = GetComponent<Animator>();
        IdleTimerS = TimeToIdle;
        FallTimerS = TimeToGroundPound;
        Hydro = GetComponent<Hydro>();
    }

    private void SetRiggingWeights(float weight)
    {
        LeftRrigging.weight = weight;
        RightRigging.weight = weight; 
    }

    private void Update()
    {        
        IdleTimer -= Time.deltaTime; 
        
        if(IdleTimer <= 0  && !Idling)        
            IdleTrigger(); 
        

        if (!Grounded && !GroundPounding)
            FallTimer -= Time.deltaTime;


        if (FallTimer <= 0 && !Grounded && !GroundPounding
            && !PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Collected Coin"))
            PlayGroundPound();

    }

    private void IdleTrigger()
    {
        PlayerAnimator.SetTrigger("Idle");
        Idling = true; 
        CFL.m_RecenterToTargetHeading.m_enabled = true; 
    }

    public void AniHurt()
    {
        PlayerAnimator.SetTrigger("Hurt");

        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            PlayerAnimator.ResetTrigger("Hurt");

        IdleTimerS = TimeToIdle;
    }

    public void GetUpTrigger()
    {
        if(HangingFromLedge)
            PlayerAnimator.SetTrigger("GetUpFromLedge");
    }

    public void OffLedge()
    {
        PlayerMovement PM = GetComponent<PlayerMovement>(); 
        PlayerControls.TeleportPlayer(PM.GetUpFromLedgePoint);
        //PM.VerticleVelocity = -2;
        AniOnLedge(false); 
    }

    public void HitLedge()
    {
        PlayerAnimator.SetTrigger("HitLedge");
    }

    public void AniOnLedge(bool onLedge)
    {
        PlayerAnimator.SetBool("OnLedge", onLedge);
        HangingFromLedge = onLedge; 
    }

    public void Invol(bool Invol)
    {
        PlayerAnimator.SetBool("Invol", Invol);
    }

    public void AniMovingSpeed(float MoveSpeed)
    {
        PlayerAnimator.SetFloat("Moving", MoveSpeed);

        if (MoveSpeed > 0)
            IdleTimerS = TimeToIdle;
    }

    public void AniVerticleV(float VerticalSpeed)
    {
        PlayerAnimator.SetFloat("Vertical", VerticalSpeed); 

        if(VerticalSpeed > 0)
            IdleTimerS = TimeToIdle;
    }

    public void AniGrounded(bool Grounded)
    {
        PlayerAnimator.SetBool("Grounded", Grounded);

        if (!Grounded)
        {
            IdleTimerS = TimeToIdle;
        }
        else
        {
            GroundPounding = false; 
            FallTimerS = TimeToGroundPound; 
        }
    }
    
    public void PlayCoinCollected()
    {
        PlayerAnimator.SetTrigger("CollectCoin");
    }

    public void StopPlayer()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    public void ContinuePlayer()
    {
        GetComponent<PlayerMovement>().enabled = true; 
    }

    public void PlayGroundPound()
    {
        GroundPounding = true;
        PlayerAnimator.Play("GroundPound");
    }

    public void AniDashing(bool dash)
    {
        PlayerAnimator.SetBool("Dashing", dash); 
    }

    public void Spray(bool Spraying)
    {
        if (Spraying)
        {
            Hydro.NozzleEffect();    
            SetRiggingWeights(1);
        }
        else
            SetRiggingWeights(0);

        PlayerAnimator.SetBool("SprayOn", Spraying);
    }
}
