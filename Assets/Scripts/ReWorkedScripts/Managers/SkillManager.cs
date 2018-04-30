using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class SkillManager : MonoBehaviour
    { 

        private static SkillManager _instance;
        public static SkillManager instance { get { return _instance; } }

        Dictionary<Skills, float> _skillAmount;
        Dictionary<Skills, float> _maxSkillAmount;
        int fireMaxAmount = 300;

        public float initialFlameAmount;
        float initialVacuumAmount = 1;

        public float fireAmount;

        void Awake()
        {
            if (_instance == null)
                _instance = this;

            _skillAmount = new Dictionary<Skills, float>();
            _skillAmount[Skills.VACCUM] = initialVacuumAmount;
            _skillAmount[Skills.FIRE] = initialFlameAmount;

            //For HUD
            _maxSkillAmount = new Dictionary<Skills, float>();
            _maxSkillAmount[Skills.VACCUM] = 1;
            _maxSkillAmount[Skills.FIRE] = fireMaxAmount;


        }

        public bool CheckSkillAmount(Skills sk)
        {
            if (_skillAmount == null)
            {
                _skillAmount = new Dictionary<Skills, float>();
            }
            return _skillAmount.ContainsKey(sk) && _skillAmount[sk] > 0;
        }

        public void AddAmountToSkill(float amount, Skills sk)
        {
            if (_skillAmount == null)
            {
                _skillAmount = new Dictionary<Skills, float>();
            }
            if (!_skillAmount.ContainsKey(sk))
            {
                _skillAmount.Add(sk, 0);
            }
            if (_skillAmount[sk] < _maxSkillAmount[sk])
            {
                _skillAmount[sk] += amount;
            }
            else
            {
                Debug.Log("Max Amount Reached: " + sk);
            }
        }

        public void RemoveAmountToSkill(float amount, Skills sk)
        {
            if (_skillAmount == null)
            {
                _skillAmount = new Dictionary<Skills, float>();
            }
            if (!_skillAmount.ContainsKey(sk) || _skillAmount[sk] <= 0)
            {
                Debug.Log("No " + sk + " remaining");
            }
            else
            {
                _skillAmount[sk] -= amount;
            }
        }

        public float SkillActualAmount(Skills sk)
        {
            return _skillAmount[sk]/_maxSkillAmount[sk];
        }

        //For Debuging only (to see amounts of fire)
        void Update()
        {
            fireAmount = _skillAmount[Skills.FIRE];
        }
    }


}


