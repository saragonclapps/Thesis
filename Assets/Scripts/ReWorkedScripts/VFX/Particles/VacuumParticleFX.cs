using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumParticleFX : MonoBehaviour {

    public ParticleSystem ps;
    public Transform target;
    public float t;
	
	void Update ()
    {
        var particles = new ParticleSystem.Particle[ps.particleCount];
        var count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            particles[i].position = Vector3.Lerp(particles[i].position, target.position, t);
        }

        ps.SetParticles(particles, count);
	}
}
