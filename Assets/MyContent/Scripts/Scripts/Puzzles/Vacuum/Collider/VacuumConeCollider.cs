using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Logger.Debug;

namespace Skills {
    public class VacuumConeCollider : MonoBehaviour {
        private SkillController _skill;

        private void Start() {
            _skill = GetComponentInParent<SkillController>();
        }

        #region Trigger Methods

        private void OnTriggerEnter(Collider c) {
            var obj = c.GetComponent<IVacuumObject>();
            if (obj == null) return;
            if (!_skill.objectsToInteract.Contains(obj))
                _skill.objectsToInteract.Add(obj);
        }

        private void OnTriggerExit(Collider c) {
            var obj = c.GetComponent<IVacuumObject>();
            if (obj == null) return;
#if UNITY_EDITOR
            if (obj.rb == null) {
                Debug.LogError(this, "Object " + DebugObject(c) + " has no rigidbody");
            }
#endif
            obj.rb.isKinematic = false;
            obj.isAbsorved = false;
            obj.Exit();
            _skill.objectsToInteract.Remove(obj);
        }
        
        
#if UNITY_EDITOR
        private string DebugObject(Collider other) {
            return other.gameObject.transform.parent.name;
        }    
#endif

        private void OnTriggerStay(Collider c) {
            var obj = c.GetComponent<IVacuumObject>();
            if (obj == null) return;
            if (!_skill.objectsToInteract.Contains(obj) && (GameInput.instance.blowUpButton)) {
                _skill.objectsToInteract.Add(obj);
            }
        }

        #endregion
    }
}