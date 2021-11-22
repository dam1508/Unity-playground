using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] GameObject particlesVFX;
    [SerializeField] float time = 1.0F;
    [SerializeField] Transform landPosition;

    public void LandingVFX()
    {
        GameObject particles = Instantiate(particlesVFX, landPosition.position, Quaternion.identity);
        Destroy(particles, time);
    }

    
}
