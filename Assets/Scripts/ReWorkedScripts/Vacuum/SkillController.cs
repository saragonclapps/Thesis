using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Player;

namespace Skills
{
    public class SkillController : MonoBehaviour
    {

        #region Strategy
        ISkill actualAction;
        Dictionary<Skills, ISkill> _skills;
        Dictionary<Skills, IEnumerable> _lists;
        Skills skillAction;

        // BulletShoot _bulletShoot;
        Attractor _attractor;
        FlameThrower _flameThrower;
        WaterLauncher _waterLauncher;
        Electricity _electricity;
        #endregion

        #region Atractor Variables
        //Atractor Variables
        public List<IVacuumObject> objectsToInteract;

        [Header("Attractor Variables")]
        public float atractForce;
        public float shootSpeed;
        public Transform vacuumHoleTransform;


        bool _isStuck;
        IHandEffect aspireVFX;
        IHandEffect blowVFX;

        WindZone wind;

        #endregion

        #region FireVariables
        IHandEffect fireVFX;
        public List<IFlamableObjects> flamableObjectsToInteract;
        #endregion

        #region IceVariables
        IHandEffect iceVFX;
        public List<IFrozenObject> frozenObjectsToInteract;
        #endregion

        #region WaterVariables
        IHandEffect waterVFX;
        public List<IWaterObject> wetObjectsToInteract;
        #endregion

        #region ElectricityVariables
        IHandEffect electricityVFX;
        public List<IElectricObject> electricObjectsToInteract;
        #endregion

        #region HudVariables
        //Dictionary<Skills, typeSkill> hudSkill;
        #endregion

        #region  Visual Effect
        [Header("VFX References")]
        public ParticleSystem aspireParticle;
        public ParticleSystem blowParticle;
        public ParticleSystem fireParticle;
        public ParticleSystem waterParticle;
        public GameObject electricityPrefab;
        #endregion

        public Skills currentSkill;
        PlayerController2 _pC;

        void Awake()
        {

            //hudSkill = new Dictionary<Skills, typeSkill>();
            //hudSkill.Add(Skills.VACCUM, typeSkill.BlowAndAspire);

            skillAction = new Skills();

            wind = GetComponentInChildren<WindZone>();
            _pC = GetComponent<PlayerController2>();

            //Initializing List Dictionary
            _lists = new Dictionary<Skills, IEnumerable>();
            _lists.Add(Skills.VACCUM, objectsToInteract);
            _lists.Add(Skills.FIRE, flamableObjectsToInteract);
            _lists.Add(Skills.WATER, wetObjectsToInteract);

            //Lists Initializing
            objectsToInteract = new List<IVacuumObject>();
            flamableObjectsToInteract = new List<IFlamableObjects>();
            wetObjectsToInteract = new List<IWaterObject>();
            electricObjectsToInteract = new List<IElectricObject>();

            //Hand VFX Initializing
            aspireVFX = new VacuumVFX(aspireParticle);
            blowVFX = new VacuumVFX(blowParticle);
            fireVFX = new VacuumVFX(fireParticle);
            waterVFX = new VacuumVFX(waterParticle);
            electricityVFX = new ElectricityVFX(electricityPrefab, electricObjectsToInteract, vacuumHoleTransform);

            //Strategy Initializing
            _attractor = new Attractor(atractForce, shootSpeed, vacuumHoleTransform, aspireVFX, blowVFX, objectsToInteract, wind);
            _flameThrower= new FlameThrower(fireVFX, flamableObjectsToInteract);
            _waterLauncher = new WaterLauncher(waterVFX, wetObjectsToInteract);
            _electricity = new Electricity(electricityVFX, electricObjectsToInteract);


            _skills = new Dictionary<Skills, ISkill>();
            _skills.Add(Skills.VACCUM, _attractor);
            _skills.Add(Skills.FIRE, _flameThrower);
            _skills.Add(Skills.WATER, _waterLauncher);
            _skills.Add(Skills.ELECTRICITY, _electricity);

            actualAction = _skills[skillAction];
            actualAction.Enter();

        }

        void Start()
        {
            UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
        }

        void Execute()
        {
            if (GameInput.instance.skillUp)
            {

                if (skillAction + 1 != Skills.LAST)
                {
                    skillAction++;
                    RecuCheckAmount(skillAction, true);
                    if (skillAction == Skills.LAST) skillAction = Skills.VACCUM;
                }
                else skillAction = Skills.VACCUM;

                SkillSet();

            }
            else if (GameInput.instance.skillDown)
            {
                if (skillAction > 0)
                {
                    skillAction--;
                    RecuCheckAmount(skillAction, false);
                }
                else
                {
                    skillAction = Skills.LAST;
                    skillAction--;
                    RecuCheckAmount(skillAction, false);
                }

                SkillSet();

            }

            

            if (!(GameInput.instance.crouchButton || GameInput.instance.sprintButton) && !_pC.isSkillLocked)
            {
                actualAction.Execute();
            }
            else
            {
                actualAction.Exit();
            }
        }

        private void SkillSet()
        {
            currentSkill = skillAction;
            actualAction.Exit();
            actualAction = _skills[skillAction];
            actualAction.Enter();
        }

        void RecuCheckAmount(Skills skill, bool sign)
        {
            if (skill != Skills.VACCUM && skill != Skills.LAST)
            {
                if (SkillManager.instance.CheckSkillAmount(skill))
                {
                    skillAction = skill;
                }
                else
                {
                    if (sign)
                    {
                        skillAction++;
                        RecuCheckAmount(skillAction, true);
                    }
                    else
                    {
                        skillAction--;
                        RecuCheckAmount(skillAction, false);
                    }
                }
            }
        }

        public void NoMoreSkillAmount()
        {
            skillAction = Skills.VACCUM;
            SkillSet();
        }
    }

    public enum Skills
    {
        VACCUM,
        FIRE,
        WATER,
        ICE,
        ELECTRICITY,
        LAST
    }
}

