using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>(); 
    }

    public void AniMovingSpeed(float MoveSpeed)
    {
        Animator.SetFloat("Moving", MoveSpeed);
    }

    public void AniVerticleV(float VerticalSpeed)
    {
        Animator.SetFloat("Vertical", VerticalSpeed); 
    }

    public void AniGrounded(bool Grounded)
    {
        Animator.SetBool("Grounded", Grounded); 
    }
}
