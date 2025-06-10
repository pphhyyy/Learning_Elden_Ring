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

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;


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

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // 如果我们手动在这个模型上放置了血液飞溅特效，播放它的版本
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // 否则，使用我们在其他地方有的通用（默认版本）
            else
            {
                Debug.Log("General Vesion !");
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }

    }
}

