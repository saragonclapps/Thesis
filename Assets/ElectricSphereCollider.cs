using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class ElectricSphereCollider : MonoBehaviour {

        SkillController _skillController;

        void Start()
        {
            _skillController = GetComponentInParent<SkillController>(); 
        }

        private void OnTriggerEnter(Collider c)
        {
            IElectricObject obj;
            obj = c.GetComponent<IElectricObject>();
            if (obj != null)
                if (!_skillController.electricObjectsToInteract.Contains(obj))
                    _skillController.electricObjectsToInteract.Add(obj);
        }

        private void OnTriggerExit(Collider c)
        {
            IElectricObject obj;
            obj = c.GetComponent<IElectricObject>();
            if (obj != null)
            {
                _skillController.electricObjectsToInteract.Remove(obj);
            }
        }

        private void OnTriggerStay(Collider c)
        {
            var obj = c.GetComponent<IElectricObject>();
            if (obj != null)
                if (!_skillController.electricObjectsToInteract.Contains(obj) && (GameInput.instance.blowUpButton))
                    _skillController.electricObjectsToInteract.Add(obj);
        }
    }

}
