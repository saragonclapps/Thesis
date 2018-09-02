using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;

public class FireWall : MonoBehaviour, IVacuumObject
{
    public bool isAbsorved { get { return _isAbsorved; } set { _isAbsorved = value; } }
    public bool isAbsorvable { get { return _isAbsorvable; } }
    public bool isBeeingAbsorved { get { return _isBeeingAbsorved; } set { _isBeeingAbsorved = value; } }
    public Rigidbody rb { get { return _rb; } set { _rb = value; } }

    bool _isAbsorved;
    bool _isAbsorvable;
    bool _isBeeingAbsorved;
    Rigidbody _rb;

    ParticleSystem[] _ps;
    float t = 0.05f;
    float att = 0.1f;
    float blowAtt = 0.05f;

    public Transform vacuum;
    public AnimationCurve curve;

    public float fireAmount;
    public float fireRefillSpeed;
    BoxCollider _box;

    private void Start()
    {
        _ps = GetComponentsInChildren<ParticleSystem>();
        _rb = GetComponent<Rigidbody>();
        _box = GetComponent<BoxCollider>();
    }

    private void LateUpdate()
    {
        _rb.isKinematic = true;
    }

    public void BlowUp(Transform origin, float atractForce, Vector3 direction)
    {
        for (int i = 0; i < _ps.Length; i++)
        {
            var particles = new ParticleSystem.Particle[_ps[i].particleCount];
            var count = _ps[i].GetParticles(particles);

            for (int j = 0; j < count; j++)
            {
                var dir = -(vacuum.position - particles[j].position).normalized;
                particles[j].position += direction * atractForce * blowAtt * Time.deltaTime;
            }

            _ps[i].SetParticles(particles, count);
        }
    }

    public void SuckIn(Transform origin, float atractForce)
    {
        if(fireAmount > 0)
        {
            for (int i = 0; i < _ps.Length; i++)
            {
                var particles = new ParticleSystem.Particle[_ps[i].particleCount];
                var count = _ps[i].GetParticles(particles);

                for (int j = 0; j < count; j++)
                {
                    particles[j].position = Vector3.Lerp(particles[j].position, vacuum.position, t);
                }

                _ps[i].SetParticles(particles, count);
                var solt = _ps[i].sizeOverLifetime;
                solt.size = new ParticleSystem.MinMaxCurve(1.5f, curve);
            }

            origin.GetComponentInParent<SkillManager>().AddAmountToSkill(fireRefillSpeed * Time.deltaTime, Skills.Skills.FIRE);
            fireAmount -= fireRefillSpeed * Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < _ps.Length; i++)
            {
                _ps[i].Stop();
                _box.size = new Vector3(2, 0.25f, 0.15f);
                _box.center = Vector3.zero;
            }
        }

    }

    #region Unused IvacuumObjectMethods
    public void Shoot(float shootForce, Vector3 direction){}
    public void ReachedVacuum(){}
    public void ViewFX(bool active){}
    public void Exit(){}
    #endregion
}
