using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PA
{
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        // 是左手槽还是右手槽? 
        public WeaponModelSlot weaponSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeapon(GameObject weaponModel)
        {
            Debug.Log("LoadWeapon : "+weaponModel.name);
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }
    }
}

