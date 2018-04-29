using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VacuumController : MonoBehaviour {

    #region Strategy
    private IVacuumAction actualAction;
    private Dictionary<Items, Action> _actionEnter;
    private Items itemAction;

    private BulletShoot _bulletShoot;
    private Attractor _attractor;

    #endregion

    #region Atractor Variables
    //Atractor Variables
    public List<IVacuumObject> objectsToInteract;

    [Header("Attractor Variables")]
    public float atractForce;
    public float shootSpeed;
    public Transform vacuumHoleTransform;

    //Visual Effect
    [Header("VFX References")]
    public ParticleSystem aspireParticle;
    public ParticleSystem blowParticle;

    IHandEffect aspireVFX;
    IHandEffect blowVFX;

    private bool _isStuck;

    #endregion

    #region ShootVariables
    public Items currentItem;
    [HideInInspector]public PlayerController playerController;
    #endregion

    #region HudVariables
    Dictionary<Items, typeSkill> hudSkill;
    #endregion

    private PathCalculate _pc;
    private ArmRotator _arm;

    void Awake()
    {
        _actionEnter = new Dictionary<Items, Action>();
        _actionEnter.Add(Items.NONE, AttractorEnter);
        _actionEnter.Add(Items.DARDO, DardoShootEnter);
        _actionEnter.Add(Items.SCRAP, DardoShootEnter);

        hudSkill = new Dictionary<Items, typeSkill>();
        hudSkill.Add(Items.NONE, typeSkill.BlowAndAspire);
        hudSkill.Add(Items.DARDO, typeSkill.Shoot);
        hudSkill.Add(Items.SCRAP, typeSkill.Scrap);

        itemAction = new Items();

        _arm = GetComponent<ArmRotator>();
        _pc = GetComponent<PathCalculate>();
        playerController = GetComponentInParent<PlayerController>();
        _bulletShoot = new BulletShoot(_arm, _pc, vacuumHoleTransform, this);

        //Hand VFX Initializing
        aspireVFX = new VacuumVFX(aspireParticle);
        blowVFX = new VacuumVFX(blowParticle);

        objectsToInteract = new List<IVacuumObject>();
        _attractor = new Attractor(atractForce, shootSpeed, vacuumHoleTransform, aspireVFX, blowVFX, _pc, objectsToInteract);
        AttractorEnter();

    }
	
	void Start () {
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);

	}
	
	
	void Execute ()
    {
        if (GameInput.instance.skillUp)
        {

            if (itemAction + 1 != Items.LAST)
            {
                itemAction++;
                RecuCheckAmount(itemAction, true);
                if (itemAction == Items.LAST) itemAction = Items.NONE;
            }
            else itemAction = Items.NONE;

            currentItem = itemAction;
            actualAction.Exit();
            _actionEnter[itemAction]();
            HUDManager.instance.SetSkillByType(hudSkill[itemAction]);
        }
        else if (GameInput.instance.skillDown)
        {
            if (itemAction > 0)
            {
                itemAction--;
                RecuCheckAmount(itemAction, false);
            }
            else
            {
                itemAction = Items.LAST;
                itemAction--;
                RecuCheckAmount(itemAction, false);
            }
            currentItem = itemAction;
            actualAction.Exit();
            _actionEnter[itemAction]();
            HUDManager.instance.SetSkillByType(hudSkill[itemAction]);
        }
        if(!(GameInput.instance.crouchButton || GameInput.instance.sprintButton))
        {
            actualAction.Execute(objectsToInteract);
        }else
        {
            actualAction.Exit();
        }
	}

    public void OutOfAmmo()
    {
        itemAction = Items.NONE;
        currentItem = itemAction;
        actualAction.Exit();
        _actionEnter[itemAction]();
        HUDManager.instance.SetSkillByType(hudSkill[itemAction]);
    }

    #region ActionEnters
    void DardoShootEnter()
    {
        actualAction = _bulletShoot;
        actualAction.Initialize();
    }

    void AttractorEnter()
    {
        actualAction = _attractor;
        actualAction.Initialize();
    }


    #endregion

    void RecuCheckAmount(Items item, bool sign)
    {
        if(item != Items.NONE && item != Items.LAST)
        {
            if (BulletManager.instance.CheckItemFromBag(item))
            {
                itemAction = item;
            }
            else
            {
                if (sign)
                {
                    itemAction++;
                    RecuCheckAmount(itemAction, true);
                }
                else
                {
                    itemAction--;
                    RecuCheckAmount(itemAction, false);
                }
            }
        }
    }

    //#region Trigger Methods
    //private void OnTriggerEnter(Collider c)
    //{
    //    VacuumInteractive obj;
    //    if ((obj = c.GetComponent<VacuumInteractive>()))
    //        if(!objectsToInteract.Contains(obj))
    //            objectsToInteract.Add(obj);
    //}

    //private void OnTriggerExit(Collider c)
    //{
    //    VacuumInteractive obj;
    //    if (obj = c.GetComponent<VacuumInteractive>())
    //    {
    //        obj.rb.isKinematic = false;
    //        obj.isAbsorved = false;
    //        obj.Exit();
    //        objectsToInteract.Remove(obj);
    //    }
    //}

    //private void OnTriggerStay(Collider c)
    //{
    //    var aux = GetComponent<VacuumInteractive>();
    //    if (aux != null)
    //        if (!objectsToInteract.Contains(aux) && (GameInput.instance.absorbButton || GameInput.instance.blowUpButton)) objectsToInteract.Add(aux);
    //}
    //#endregion
}
