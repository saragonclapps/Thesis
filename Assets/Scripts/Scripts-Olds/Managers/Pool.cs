using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>{

    private List<PooleableOBJ<T>> _poolList;
    public delegate T FactoryCreate();

    private int _count;
    private bool _isDinamic = true;
    private PooleableOBJ<T>.PoolCallBack _init;
    private PooleableOBJ<T>.PoolCallBack _finalize;
    private FactoryCreate _factoryMethod;

    public Pool(int initialStock, FactoryCreate factoryMethod, PooleableOBJ<T>.PoolCallBack initialize, PooleableOBJ<T>.PoolCallBack finalize, bool isDinamic)
    {
        _poolList = new List<PooleableOBJ<T>>();

        _factoryMethod = factoryMethod;
        _isDinamic = isDinamic;
        _count = initialStock;
        _init = initialize;
        _finalize = finalize;

        for(int i = 0; i < _count; i++)
        {
            _poolList.Add(new PooleableOBJ<T>(_factoryMethod(), _init, _finalize));
            _poolList[i].isActive = false;
        }
    }
    
    public PooleableOBJ<T> GetPoolObject()
    {
        for(int i = 0; i< _count; i++)
        {
            if(!_poolList[i].isActive)
            {
                _poolList[i].isActive = true;
                return _poolList[i];
            }
        }
        if(_isDinamic)
        {
            PooleableOBJ<T> po = new PooleableOBJ<T>(_factoryMethod(), _init, _finalize);
            po.isActive = true;
            _poolList.Add(po);
            _count++;
            return po;
        }
        return null;
    }

    public T GetObjectFromPool()
    {
        for(int i = 0; i<_count; i++)
        {
            if(!_poolList[i].isActive)
            {
                _poolList[i].isActive = true;
                return _poolList[i].GetObject;
            }
        }

        if(_isDinamic)
        {
            PooleableOBJ<T> po = new PooleableOBJ<T>(_factoryMethod(), _init, _finalize);
            po.isActive = true;
            _poolList.Add(po);
            _count++;
            return po.GetObject;
        }
        return default(T);
    }

    public void DisablePoolObject(T obj)
    {
        foreach (var poolObj in _poolList)
        {
            if(poolObj.GetObject.Equals(obj))
            {
                poolObj.isActive = false;
                return;
            }
        }
    }
}
