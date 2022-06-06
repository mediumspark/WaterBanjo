using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttack : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (gameObject.TryGetComponent<Well>(out Well well))
        {
            well.WaterInWell += 0.5f;
        }
    }
}
