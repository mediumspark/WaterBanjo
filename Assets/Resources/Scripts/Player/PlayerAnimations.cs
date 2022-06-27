using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator Animator;
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
    private CinemachineFreeLook CFL; 

    private void Awake()
    {
        CFL = GetComponentInChildren<CinemachineFreeLook>(); 
        Animator = GetComponent<Animator>();
        IdleTimerS = TimeToIdle;
        FallTimerS = TimeToGroundPound; 
    }

    private void Update()
    {
        
        IdleTimer -= Time.deltaTime; 
        
        if(IdleTimer <= 0  && !Idling)        
            IdleTrigger(); 
        

        if (!Grounded && !GroundPounding)
            FallTimer -= Time.deltaTime;


        if (FallTimer <= 0 && !Grounded && !GroundPounding
            && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Collected Coin"))
            PlayGroundPound();
    }

    private void IdleTrigger()
    {
        Animator.SetTrigger("Idle");
        Idling = true; 
        CFL.m_RecenterToTargetHeading.m_enabled = true; 
    }

    public void AniHurt()
    {
        Animator.SetTrigger("Hurt");

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            Animator.ResetTrigger("Hurt");

        IdleTimerS = TimeToIdle;
    }

    public void Invol(bool Invol)
    {
        Animator.SetBool("Invol", Invol);
    }

    public void AniMovingSpeed(float MoveSpeed)
    {
        Animator.SetFloat("Moving", MoveSpeed);

        if (MoveSpeed > 0)
            IdleTimerS = TimeToIdle;
    }

    public void AniVerticleV(float VerticalSpeed)
    {
        Animator.SetFloat("Vertical", VerticalSpeed); 

        if(VerticalSpeed > 0)
            IdleTimerS = TimeToIdle;
    }

    public void AniGrounded(bool Grounded)
    {
        Animator.SetBool("Grounded", Grounded);

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
        Animator.Play("Collected Coin");
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
        Animator.Play("GroundPound");
    }

    public void AniDashing(bool dash)
    {
        Animator.SetBool("Dashing", dash); 
    }
}
