using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dardo;

public class EnemyManager : MonoBehaviour {

    //private Pool<DardoFSM> _dardoPool;
    private Pool<DardoController> _dardoPool;

    private static EnemyManager _instance;
    public static EnemyManager instance { get { return _instance;  } }

    public Transform player;

    public HiddenVFX vignette;

    public GameObject dardoPrefab;

	void Start () {
        _instance = this;
        //_dardoPool = new Pool<DardoFSM>(10, DardoFactory, DardoFSM.InitializeDardo, DardoFSM.DestroyDardo, true);
        _dardoPool = new Pool<DardoController>(20, DardoFactory, DardoController.InitializeDardo, DardoController.DestroyDardo, true);
        EventManager.AddEventListener(GameEvent.DARDO_SPAWN, DardoSpawn);
	}

    private void DardoSpawn(object[] pC)
    {
        var dar = _dardoPool.GetObjectFromPool();
        dar.Configure(player, (Vector3)pC[0], (Transform)pC[1]);
    }

    DardoController DardoFactory()
    {
        var d = dardoPrefab.GetComponent<DardoController>();
        return Instantiate(d);
    }

    public void ReturnDardoToPool(DardoController dardo)
    {
        _dardoPool.DisablePoolObject(dardo);
    }
}
