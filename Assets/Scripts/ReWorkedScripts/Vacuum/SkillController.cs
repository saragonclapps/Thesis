using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        #region  Visual Effect
        [Header("VFX References")]
        public ParticleSystem aspireParticle;
        public ParticleSystem blowParticle;
        public ParticleSystem fireParticle;

        #endregion

        public Skills currentSkill;

        #region FireVariables
        IHandEffect fireVFX;
        public List<IFlamableObjects> flamableObjectsToInteract;  
        #endregion

        #region IceVariables
        #endregion

        #region WaterVariables
        #endregion

        #region ElectricityVariables
        #endregion

        #region HudVariables
        //Dictionary<Skills, typeSkill> hudSkill;
        #endregion

        void Awake()
        {

            //hudSkill = new Dictionary<Skills, typeSkill>();
            //hudSkill.Add(Skills.VACCUM, typeSkill.BlowAndAspire);

            skillAction = new Skills();

            wind = GetComponentInChildren<WindZone>();

            //Initializing List Dictionary
            _lists = new Dictionary<Skills, IEnumerable>();
            _lists.Add(Skills.VACCUM, objectsToInteract);
            _lists.Add(Skills.FIRE, flamableObjectsToInteract);

            //Hand VFX Initializing
            aspireVFX = new VacuumVFX(aspireParticle);
            blowVFX = new VacuumVFX(blowParticle);
            fireVFX = new VacuumVFX(fireParticle);

            //Lists Initializing
            objectsToInteract = new List<IVacuumObject>();
            flamableObjectsToInteract = new List<IFlamableObjects>();

            //Strategy Initializing
            _attractor = new Attractor(atractForce, shootSpeed, vacuumHoleTransform, aspireVFX, blowVFX, /*_pc,*/ objectsToInteract, wind);
            _flameThrower= new FlameThrower(fireVFX, flamableObjectsToInteract);
            _skills = new Dictionary<Skills, ISkill>();
            _skills.Add(Skills.VACCUM, _attractor);
            _skills.Add(Skills.FIRE, _flameThrower);


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

                currentSkill = skillAction;
                actualAction.Exit();
                actualAction = _skills[skillAction];
                actualAction.Enter();
                //HUDManager.instance.SetSkillByType(hudSkill[skillAction]);
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
                currentSkill = skillAction;
                actualAction.Exit();
                actualAction = _skills[skillAction];
                actualAction.Enter();
                //HUDManager.instance.SetSkillByType(hudSkill[skillAction]);
            }
            if (!(GameInput.instance.crouchButton || GameInput.instance.sprintButton))
            {
                actualAction.Execute(/*_lists[skillAction]*/);
            }
            else
            {
                actualAction.Exit();
            }
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

