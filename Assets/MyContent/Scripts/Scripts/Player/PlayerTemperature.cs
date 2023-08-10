using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;
using Player;

public class PlayerTemperature : MonoBehaviour, IHeat {
    [SerializeField] 
    private float _temperature;
    public float temperature => _temperature;

    public Transform Transform => transform;

    public float life;
    private float _currentLife;

    private bool _setToDieByLaser;
    private PlayerController _player;
    private SkillController _skills;

    private Animator _blackOut;

    private void Start() {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        _player = GetComponentInParent<PlayerController>();
        _skills = GetComponentInParent<SkillController>();
        _blackOut = GameObject.Find("BlackIn").GetComponent<Animator>();
        _currentLife = life;
    }

    public void Restart() {
        _setToDieByLaser = false;
        _currentLife = life;
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
    }

    void Execute() {
        if (!_setToDieByLaser) return;
        Die();
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }

    private void Die() {
        _player.enabled = false;
        _skills.enabled = false;
        _blackOut.SetTrigger("FadeOutLose");
    }

    public void Hit(float damage) {
        if (_setToDieByLaser) return;
        _currentLife -= damage * Time.deltaTime;
        if (_currentLife < 0) {
            _setToDieByLaser = true;
        }
    }

    private void OnDestroy() {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}