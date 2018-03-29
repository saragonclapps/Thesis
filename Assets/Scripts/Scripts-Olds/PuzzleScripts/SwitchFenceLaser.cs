using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Rigidbody))]
public class SwitchFenceLaser : MonoBehaviour {

    private List<List<PointLaser>> groupsOfFences = new List<List<PointLaser>>();
    private int _currentGroupActive = 0;
    private int _maxGroupActive = 0;

    private bool isRandom = false;
    private bool allDisable = false;
    private bool allEnable = false;

    public event Action ChangeActive = delegate { };

    private void Start()
    {
        var groups = transform.Cast<Transform>().Select(t => t.gameObject).ToList();

        foreach (var item in groups){
            var temp = item.GetComponentsInChildren<PointLaser>()
                           .Select(t => t.GetComponent<PointLaser>())
                           .ToList()
                           ;

            //print("Group have: " + temp.Count);
            groupsOfFences.Add(temp);
        }
        _maxGroupActive = groupsOfFences.Count();

        //print("Group of groups have: " + groupsOfFences.Count);
        BehaviourActive();
    }

    private void BehaviourActive()
    {
        if (_maxGroupActive == 1)
        {
            ChangeActive += () => {
                foreach (var item in groupsOfFences[0])//Enable all group.
                    item.IsActive(!item.GetState());
            };
        }
        else
        {
            ChangeActive += () => {

                foreach (var item in groupsOfFences[_currentGroupActive])//Enable all group.
                    item.IsActive(true);

                _currentGroupActive = (_currentGroupActive + 1 < _maxGroupActive) ? _currentGroupActive + 1 : 0;

                foreach (var item in groupsOfFences[_currentGroupActive])//Disable all group.
                    item.IsActive(false);
            };
        }
    }

    private void AllActive(bool value)
    {
        foreach (var Groups in groupsOfFences)
            foreach (var item in Groups)
                item.IsActive(false);
    }


    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == 16)
        {
            ChangeActive();
        }
    }
}