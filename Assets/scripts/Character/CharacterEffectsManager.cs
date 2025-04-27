using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // 处理及时（instant） 效果 （受伤 防御 治愈）

        // 处理时间效果 （中毒 

        // 处理静态效果 （当玩家装备 戒指盔甲等装备时 增加一个buff ，脱掉时移除）

        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // 展示一个特效
            effect.ProcessEffect(character);
            // 处理特效相关的计算
            
        }

    }
}

