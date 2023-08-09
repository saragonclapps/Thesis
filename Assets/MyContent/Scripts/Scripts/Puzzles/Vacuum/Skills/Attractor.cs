using System.Collections.Generic;
using UnityEngine;

public class Attractor : ISkill {
    public List<IVacuumObject> _objectsToInteract;

    private float _attractForce;
    private float _shootSpeed;
    private Transform _vacuumHoleTransform;
    private IHandEffect _aspireParticle;
    private IHandEffect _blowParticle;

    private SkinnedMeshRenderer _targetMesh;
    private Mesh _atractorMesh;

    private bool _isStuck;

    public bool isStuck => _isStuck;

    private PathCalculate _pathCalculate;
    private AudioPlayer _audioPlayer;
    private bool _inputBefore;

    public Attractor SetAttractForce(float attractForce) {
        _attractForce = attractForce;
        return this;
    }

    public Attractor SetShootSpeed(float shootSpeed) {
        _shootSpeed = shootSpeed;
        return this;
    }

    public Attractor SetVacuumHole(Transform vacuumHole) {
        _vacuumHoleTransform = vacuumHole;
        return this;
    }

    public Attractor SetAspireParticle(IHandEffect aspireParticle) {
        _aspireParticle = aspireParticle;
        _aspireParticle.StopEffect();
        return this;
    }

    public Attractor SetBlowParticle(IHandEffect blowParticle) {
        _blowParticle = blowParticle;
        _blowParticle.StopEffect();
        return this;
    }

    public Attractor SetObjectsToInteract(List<IVacuumObject> objectsToInteract) {
        _objectsToInteract = objectsToInteract;
        return this;
    }

    public Attractor SetAudioPlayer(AudioPlayer audioPlayer) {
        _audioPlayer = audioPlayer;
        return this;
    }


    public void Enter() {
    }

    public void Execute() {
        if (!_isStuck) {
            InputChecker();
            return;
        }

        if (_inputBefore) {
            _aspireParticle.StopEffect();
            _blowParticle.StopEffect();
            _audioPlayer.StopPowerAudio();
            _isStuck = false;
        }

        if (GameInput.instance.blowUpButton) {
            _inputBefore = true;
            if (_objectsToInteract.Count > 0) {
                _objectsToInteract[0].ViewFX(false);
                _objectsToInteract[0].Shoot(_shootSpeed, _vacuumHoleTransform.forward);
            }

            //_pc.DeactivatePath();
            _isStuck = false;
            return;
        }
        if (GameInput.instance.absorbButton) {
            _inputBefore = true;
            Attract();
            if (_objectsToInteract.Count > 0)
                _objectsToInteract[0].ViewFX(true);
            return;
        }
        _inputBefore = false;
        //_pc.DeactivatePath();
        _isStuck = false;
        foreach (var obj in _objectsToInteract) {
            obj.Exit();
        }
        
    }

    private void InputChecker() {
        if (GameInput.instance.blowUpButton) {
            _inputBefore = true;
            _aspireParticle.StopEffect();
            _isStuck = false;
            Reject();
            if (_blowParticle.IsPlaying()) return;
            _blowParticle.StartEffect();
            _audioPlayer.PlayVacuumAudio();
            return;
        }
        
        if (GameInput.instance.absorbButton) {
            _inputBefore = true;
            _blowParticle.StopEffect();
            if (!_aspireParticle.IsPlaying() && !_isStuck) {
                _audioPlayer.PlayVacuumAudio();
                _aspireParticle.StartEffect();
            }
            Attract();
            return;
        }

        if (!_inputBefore) return;
        _audioPlayer.StopPowerAudio();
        _aspireParticle.StopEffect();
        _blowParticle.StopEffect();
        _inputBefore = false;
        foreach (var obj in _objectsToInteract) {
            obj.Exit();
        }
    }

    public void Exit() {
        _audioPlayer.StopPowerAudio();
        _aspireParticle.StopEffect();
        //_aspireParticle.TerminateEffect();
        _blowParticle.StopEffect();
        //_blowParticle.TerminateEffect();
        //_pc.DeactivatePath();
        _isStuck = false;
        foreach (var obj in _objectsToInteract) {
            obj.Exit();
        }
    }

    private void Attract() {
        for (int i = 0; i < _objectsToInteract.Count; i++) {
            if (!_isStuck) {
                _objectsToInteract[i].SuckIn(_vacuumHoleTransform, _attractForce);
                _objectsToInteract[i].isBeeingAbsorved = true;
            }

            if (_objectsToInteract[i].isAbsorved && _objectsToInteract[i].isAbsorvable) {
                _objectsToInteract[i].ReachedVacuum();
                _objectsToInteract.Remove(_objectsToInteract[i]);
            }
            else if (_objectsToInteract[i].isAbsorved && !_objectsToInteract[i].isAbsorvable) {
                var aux = _objectsToInteract[i];
                _objectsToInteract.RemoveAll(x => x != null);
                _objectsToInteract.Add(aux);
                _isStuck = true;
                _aspireParticle.StopEffect();
            }
        }
    }

    private void Reject() {
        _isStuck = false;
        if (_objectsToInteract.Count <= 0) return;
        foreach (var obj in _objectsToInteract) {
            obj.BlowUp(_vacuumHoleTransform, _attractForce, _vacuumHoleTransform.forward);
            obj.isBeeingAbsorved = true;
        }
    }
}