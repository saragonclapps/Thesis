using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooleableOBJ<T> {

    private bool _isActive;
    private T _obj;
    public delegate void PoolCallBack(T obj);
    private PoolCallBack _initializationCallBack;
    private PoolCallBack _finalizationCallBack;

    public PooleableOBJ(T obj, PoolCallBack initialization, PoolCallBack finalization)
    {
        _obj = obj;
        _initializationCallBack = initialization;
        _finalizationCallBack = finalization;
        _isActive = false;
    }
	

    public T GetObject
    {
        get
        {
            return _obj;
        }
    }

    public bool isActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
            if (_isActive)
            {
                if (_initializationCallBack != null)
                    _initializationCallBack(_obj);
            }
            else
                if (_finalizationCallBack != null)
                _finalizationCallBack(_obj);
        }
    }
}
