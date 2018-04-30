using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class VacuumConeCollider : MonoBehaviour {

        SkillController vacuum;
	    // Use this for initialization
	    void Start () {
            vacuum = GetComponentInParent<SkillController>();
	    }

        #region Trigger Methods
        private void OnTriggerEnter(Collider c)
        {
            IVacuumObject obj;
            obj = c.GetComponent<IVacuumObject>();
            if (obj != null)
                if (!vacuum.objectsToInteract.Contains(obj))
                    vacuum.objectsToInteract.Add(obj);

            IFlamableObjects flameObj;
            flameObj = c.GetComponent<IFlamableObjects>();
            if (flameObj != null)
                if (!vacuum.flamableObjectsToInteract.Contains(flameObj))
                    vacuum.flamableObjectsToInteract.Add(flameObj);
        }

        private void OnTriggerExit(Collider c)
        {
            IVacuumObject obj;
            obj = c.GetComponent<IVacuumObject>();
            if(obj != null)
            {
                obj.rb.isKinematic = false;
                obj.isAbsorved = false;
                obj.Exit();
                vacuum.objectsToInteract.Remove(obj);
            }

            IFlamableObjects flameObj;
            flameObj = c.GetComponent<IFlamableObjects>();
            if (flameObj != null)
            {
                vacuum.flamableObjectsToInteract.Remove(flameObj);
            }
                
                    


        }

        private void OnTriggerStay(Collider c)
        {
            var obj = GetComponent<IVacuumObject>();
            if (obj != null)
                if (!vacuum.objectsToInteract.Contains(obj) && (GameInput.instance.absorbButton || GameInput.instance.blowUpButton))
                    vacuum.objectsToInteract.Add(obj);

            IFlamableObjects flameObj;
            flameObj = c.GetComponent<IFlamableObjects>();
            if (flameObj != null)
                if (!vacuum.flamableObjectsToInteract.Contains(flameObj) && (GameInput.instance.absorbButton || GameInput.instance.blowUpButton))
                    vacuum.flamableObjectsToInteract.Add(flameObj);
        }
        #endregion
    }

}
