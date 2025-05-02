using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string itemName;
        public Sprite itemIcon;
        [TextArea] public string itemDescrition;
        public int itemID;
    }

}
