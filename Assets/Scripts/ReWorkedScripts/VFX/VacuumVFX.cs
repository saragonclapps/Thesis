using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumVFX : IHandEffect {

    ParticleSystem particle;
    bool isPlaying;


    public VacuumVFX(ParticleSystem particle)
    {
        this.particle = particle;
        
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void StartEffect()
    {
        if (!isPlaying)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
            isPlaying = true;
        }
    }

    public void StopEffect()
    {
        particle.Stop();
        isPlaying = false;
    }

    public void TerminateEffect()
    {
        particle.gameObject.SetActive(false);
        isPlaying = false;
    }

}
