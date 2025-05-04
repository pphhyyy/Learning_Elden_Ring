using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PA
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWaepon;

        [Header("Quick Slots")]
        public WeaponItem[] weaponInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;
        
    }
}
