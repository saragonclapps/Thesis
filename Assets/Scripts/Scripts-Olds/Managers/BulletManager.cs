using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    //Singletone
    private static BulletManager _instance;
    public static BulletManager instance { get { return _instance; } }

    //Dictionaries
    //Item amount
    private Dictionary<Items, int> _itemBag;

    //What GameEvent is triggered when this item Spawns
    private Dictionary<Items, GameEvent> _itemEvents;

    //Speed Dictionary
    private Dictionary<Items, float> _itemSpeed;

    //VacuumHole
    public Transform vacuumHole;

    //Max Amount of bullet of each time
    public int maxBullets;

    #region Pool
    //Bullet Dardo
    public GameObject dardoBulletPrefab;
    private Pool<BulletDardo> _dardoBulletPool;

    //Bullet Scrap
    public GameObject scrapBulletPrefab;
    private Pool<BulletScrap> _scrapBulletPool;

    //Escupitajo Bullet
    public GameObject escupitajoBulletPrefab;
    private Pool<EscupitajoBullet> _escupitajoBulletPool;


    #endregion

    void Awake () {
        if (_instance == null)
            _instance = this;

        //Dictionaries Initialization
        _itemEvents = new Dictionary<Items, GameEvent>();
        _itemEvents.Add(Items.DARDO, GameEvent.BULLET_DARDO_SPAWN);
        _itemEvents.Add(Items.SCRAP, GameEvent.BULLET_SCRAP_SPAWN);

        _itemSpeed = new Dictionary<Items, float>();
        _itemSpeed.Add(Items.DARDO, 30f);
        _itemSpeed.Add(Items.SCRAP, 50f);

        
	}

    void Start()
    {
        //Pool Initialization
        _dardoBulletPool = new Pool<BulletDardo>(10, BulletDardoFactory, BulletDardo.InitializeDardoBullet, BulletDardo.DestroyDardoBullet, true);
        _escupitajoBulletPool = new Pool<EscupitajoBullet>(20, EscupitajoBulletFactory, EscupitajoBullet.InitializeEscupitajoBullet, EscupitajoBullet.DestroyEscupitajoBullet, true);
        _scrapBulletPool = new Pool<BulletScrap>(20, BulletScrapFactory, BulletScrap.InitializeScrapBullet, BulletScrap.DestroyScrapBullet, true);

        //Event Listener for dardo bullet Spawn and Destroy
        EventManager.AddEventListener(GameEvent.BULLET_DARDO_SPAWN, DardoBulletSpawn);
        EventManager.AddEventListener(GameEvent.BULLET_DARDO_DESTROY, DardoBulletDestroy);

        //Event Listener for scrap bullet Spawn and Destroy
        EventManager.AddEventListener(GameEvent.BULLET_SCRAP_SPAWN, ScrapBulletSpawn);
        EventManager.AddEventListener(GameEvent.BULLET_DARDO_DESTROY, ScrapBulletDestroy);

        //EventListener for Escupitajo Bullet Spawn and Destroy
        EventManager.AddEventListener(GameEvent.ESCUPITAJO_SHOOT, EscupitajoBulletSpawn);
        EventManager.AddEventListener(GameEvent.ESCUPITAJO_BULLET_DESTROY, EscupitajoBulletDestroy);
    }

    #region ItemBag Manager
    public void AddItemToBag(Items it)
    {
        if (_itemBag == null)
        {
            _itemBag = new Dictionary<Items, int>();
        }
        if (!_itemBag.ContainsKey(it))
        { 
            _itemBag.Add(it, 0);
        }
        if (_itemBag[it] < maxBullets)
        {
            _itemBag[it] += 1;
            HUDManager.instance.SetChargerState((float)_itemBag[it] / maxBullets);
            Debug.Log("Added: " + it + " Total: " + _itemBag[it]);
        }
        else
        {
            HUDManager.instance.SetChargerState(1);
            Debug.Log("Max Item Reached: " + it);
        }
    }

    public void RemoveItemFromBag(Items it)
    {
        if(_itemBag == null)
        {
            _itemBag = new Dictionary<Items, int>();
        }
        if (!_itemBag.ContainsKey(it) || _itemBag[it] <= 0)
        {
            Debug.Log("No " + it + " remaining");
        }
        else
        {
            _itemBag[it] -= 1;
            EventManager.DispatchEvent(_itemEvents[it], vacuumHole.position, vacuumHole.forward, _itemSpeed[it]);
            HUDManager.instance.SetChargerState((float)_itemBag[it] / maxBullets);
            Debug.Log("Remove: " + it + "Total: " + _itemBag[it]);
        }
    }

    public bool CheckItemFromBag(Items it)
    {
        if (_itemBag == null)
        {
            _itemBag = new Dictionary<Items, int>();
        }
        return _itemBag.ContainsKey(it) && _itemBag[it] > 0;
    }
    #endregion

    #region Pool
    BulletDardo BulletDardoFactory()
    {
        var bd = dardoBulletPrefab.GetComponent<BulletDardo>();
        var pos = new Vector3(0, 500, 0);
        var rot = Quaternion.identity;
        return Instantiate(bd, pos, rot);
    }

    BulletScrap BulletScrapFactory()
    {
        var bs = scrapBulletPrefab.GetComponent<BulletScrap>();
        var pos = new Vector3(0, 500, 0);
        var rot = Quaternion.identity;
        return Instantiate(bs, pos, rot);
    }

    EscupitajoBullet EscupitajoBulletFactory()
    {
        var eb = escupitajoBulletPrefab.GetComponent<EscupitajoBullet>();
        return Instantiate(eb);
    }
    #endregion

    #region ShootSpawn
    private void DardoBulletSpawn(object[] pC)
    {
        var bulDar = _dardoBulletPool.GetObjectFromPool();
        bulDar.transform.position = (Vector3)pC[0];
        bulDar.transform.forward = (Vector3)pC[1];
        bulDar.speed = (float)pC[2];
    }

    private void ScrapBulletSpawn(object[] pC)
    {
        var bulScrap = _scrapBulletPool.GetObjectFromPool();
        bulScrap.transform.position = (Vector3)pC[0];
        bulScrap.transform.forward = (Vector3)pC[1];
        bulScrap.speed = (float)pC[2];
    }

    private void EscupitajoBulletSpawn(object[] pC)
    {
        var escBul = _escupitajoBulletPool.GetObjectFromPool();
        escBul.transform.position = ((Transform)pC[0]).position;
        escBul.GetComponent<Rigidbody>().velocity = ((Transform)pC[0]).forward * (float)pC[1] + Vector3.up * (float)pC[2];
    }
    #endregion

    #region BulletDestroy
    private void DardoBulletDestroy(object[] pC)
    {
        var bullDardo = (BulletDardo)pC[0];
        _dardoBulletPool.DisablePoolObject(bullDardo);
    }

    private void ScrapBulletDestroy(object[] pC)
    {
        var bullScrap = (BulletScrap)pC[0];
        _scrapBulletPool.DisablePoolObject(bullScrap);
    }

    private void EscupitajoBulletDestroy(object[] pC)
    {
        var escBull = ((GameObject)pC[0]).GetComponent<EscupitajoBullet>();
        _escupitajoBulletPool.DisablePoolObject(escBull);
    }
    #endregion
}

public enum Items
{
    NONE,
    DARDO,
    SCRAP,
    LAST
}
