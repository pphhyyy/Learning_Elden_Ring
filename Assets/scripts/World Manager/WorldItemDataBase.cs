using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PA
{
    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;
        public WeaponItem unarmedWeapon;
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        // 游戏中所有物品的列表
        [Header("Items")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // 将所有武器添加到物品列表中
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }

            // 为所有物品分配唯一的物品ID
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }
    
        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
    
    }
}