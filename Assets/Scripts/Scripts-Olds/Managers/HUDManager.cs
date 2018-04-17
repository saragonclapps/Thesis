using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//using XInputDotNetPure;

public class HUDManager : MonoBehaviour{

    public static HUDManager instance;

    public GameObject objectChargerMaterial;
    public GameObject currentSkill;
    public ParticleSystem raysProjectionLife;
    public ParticleSystem raysProjectionSkill;
    public Image[] lifeBar;
    public Image[] damageImages;
    public GameObject[] ArmsCharger;

    private PlayerController _player;
    private float _rangeObjectsVacuum = 5;
    private float _totalLife;
    private float _timeRefreshVacuumObjects = 0.5f;
    private Material _materialIcon;
    private List<Material> _materialChargerState;
    private List<Material> _vacuumMaterials;
    private Dictionary<string, Texture> _objects;
    private Dictionary<string, Sprite> _skills;

    const string OBJECTS_path = "Objects/";//Path for set texture in the backpack.
    const string SKILLS_path = "Skills/";//Path for set texture in the backpack.


    [HideInInspector]
    public typeIcon currentIcon;

    public HUDManager()
    {
        //Singleton.
        if (instance == null){
            instance = this;
        }
        else{
            instance = null;
            instance = this;
        }
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        if (!_player) throw new Exception ("this state dont have PlayerController");

        //Assignment of materials by ID search.
        var m1 = ArmsCharger.SelectMany(x => x.GetComponent<Renderer>().materials)
                            .Where(y => y.shader.name == "Created/ArmsScreens")
                            .ToList();

        var m2 = objectChargerMaterial.GetComponent<Renderer>()
                                      .materials.Where(x => x.shader.name == "Created/BagScreen")
                                      .ToArray();

        _materialChargerState = m1;
        _materialIcon = m2[0];

        //Add only the materials on scene without repeat.(Vacuum objects)
        _vacuumMaterials = FindObjectsOfType<IVacuumObjects>()
                                        .Where(x => x.GetComponent<Renderer>())
                                        .Select(x => x.GetComponent<Renderer>().material)
                                        .Where(x => x.shader.name == "Created/Vacuum object")
                                        .ToList()
                                        ;

        //Load state player
        _totalLife = _player.life;
        SetLifeState(1f);
        SetChargerState(0f);
        StartLoadIcons();
        StartLoadSkills();
        EventManager.AddEventListener(GameEvent.PLAYER_TAKE_DAMAGE, Damage);
        EventManager.AddEventListener(GameEvent.PLAYER_DIE, Reset);

        StartCoroutine(UpdateStateVaccumObjects());
    }

    private void Reset(object[] parameterContainer)
    {
        SetLifeState(1f);
        SetChargerState(0f);
    }

    private void Damage(object[] parameterContainer)
    {

        SetLifeState(_player.life/_totalLife);

        if((float)parameterContainer[0] > 0)
            StartCoroutine(DamageImage());

        StartCoroutine(Vibration());//put this in the GameManager.
    }

    private IEnumerator DamageImage()
    {
        float t = 0;
        while (t < 1)
        {
            t += 0.1f;
            foreach (var item in damageImages)
                item.color = new Color(1, 0, 0, Mathf.Lerp(0.2f,0, t));
            yield return null;
        }
        while (t > 0)
        {
            t -= 0.1f;
            foreach (var item in damageImages)
                item.color = new Color(1, 0, 0, Mathf.Lerp(0,0.2f, t));
            yield return null;
        }
    }

    //----------------------put this in the GameManager.
    private IEnumerator Vibration()
    {
        //GamePad.SetVibration(0, 1, 1);
        yield return new WaitForSeconds(0.3f);
        //GamePad.SetVibration(0, 0, 0);
    }
    
    private IEnumerator UpdateStateVaccumObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeRefreshVacuumObjects);
            foreach (var item in _vacuumMaterials)
                item.SetVector("_PlayerPos", transform.position);
        }
    }

    /// <summary> Only values of 0 to 1. </summary>
    public void SetLifeState(float value)
    {
        if (value > 1 || value < 0) throw new Exception("Out of range, ChargerState HUDManager.");

        var c = Color.Lerp(Color.red, Color.green, value);
        _materialIcon.SetColor("_VFX", c);
        var p = raysProjectionLife.main;
        p.startColor = c;

        //----------------------------LOADING CHANGE. 
        foreach (var item in lifeBar)
        {
            item.color = c;
            item.fillAmount = value;
        }

    }

    /// <summary> Only values of 0 to 1. </summary>
    public void SetChargerState(float value)
    {
        if (value > 1 || value < 0) throw new Exception("Out of range, ChargerState HUDManager.");

        var c = Color.Lerp(Color.green, Color.red, value);

        foreach (var item in _materialChargerState)
        {
            item.SetColor("_VFX", c);
            item.SetFloat("_Animation", value);
        }
    }

    #region SKILLS
    private void StartLoadSkills()
    {
        _skills = new Dictionary<string, Sprite>();
        _skills.Add(typeSkill.BlowAndAspire.ToString(), GetSkillByName(typeSkill.BlowAndAspire.ToString()));
        _skills.Add(typeSkill.Shoot.ToString(), GetSkillByName(typeSkill.Shoot.ToString()));
        _skills.Add(typeSkill.Scrap.ToString(), GetSkillByName(typeSkill.Scrap.ToString()));
        _skills.Add(typeSkill.Skill_4.ToString(), GetSkillByName(typeSkill.Skill_4.ToString()));
        _skills.Add(typeSkill.Skill_5.ToString(), GetSkillByName(typeSkill.Skill_5.ToString()));
        SetSkillByType(typeSkill.BlowAndAspire);
    }

    public void SetSkillByType(typeSkill type)
    {
        SetSkill(_skills[type.ToString()]);
    }

    /// <param name="name">Name file equal.</param>
    private Sprite GetSkillByName(string name)
    {
        Sprite image = Resources.Load(SKILLS_path + name, typeof(Sprite)) as Sprite;
        if (image == null) throw new Exception("The name does not exist or the path is failure:" + SKILLS_path + name);
        return image;
    }

    /// <summary>Change the texture for skill projection</summary>
    /// <param value="value">Only texture icon for backpack.</param>
    public void SetSkill(Sprite value)
    {
        if (value == null) throw new Exception("Material Icon is NULL, ChargerIconSkill HUDManager.");
        currentSkill.GetComponent<Image>().sprite = value;
    }
    #endregion

    #region ICON
    private void StartLoadIcons()
    {
        _objects = new Dictionary<string, Texture>();
        _objects.Add(typeIcon.None.ToString(), GetIconByName(typeIcon.None.ToString()));
        _objects.Add(typeIcon.Dardo.ToString(), GetIconByName(typeIcon.Dardo.ToString()));
        _objects.Add(typeIcon.Rock.ToString(), GetIconByName(typeIcon.Dardo.ToString()));
        _objects.Add(typeIcon.Wood.ToString(), GetIconByName(typeIcon.Dardo.ToString()));
        SetIconByType(typeIcon.None);
    }

    public void SetIconByType(typeIcon type)
    {
        SetIcon(_objects[type.ToString()]);
    }

    /// <param name="name">Name file equal.</param>
    private Texture GetIconByName(string name)
    {
        Texture tex = Resources.Load(OBJECTS_path+name) as Texture;
        if (tex == null) throw new Exception("The name does not exist or the path is failure:" + OBJECTS_path + name);
        return tex;
    }

    /// <summary>Change the texture for backpack</summary>
    /// <param value="value">Only texture icon for backpack.</param>
    public void SetIcon(Texture value)
    {
        if (value == null) throw new Exception("Material Icon is NULL, ChargerIcon HUDManager.");
        _materialIcon.SetTexture("_Icon", value);
    }
    #endregion

}

public enum typeIcon { None, Dardo, Rock, Wood }
public enum typeSkill { BlowAndAspire, Shoot, Scrap, Skill_4, Skill_5 }