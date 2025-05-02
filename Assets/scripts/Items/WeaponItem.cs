using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PA
{
    public class WeaponItem : Item
    {
        // 动画控制器覆盖 (根据当前使用的武器改变攻击动画)
        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthREQ = 0;  // 力量需求
        public int dexREQ = 0;       // 敏捷需求 
        public int intREQ = 0;       // 智力需求
        public int faithREQ = 0;     // 信仰需求

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;    // 物理伤害
        public int magicDamage = 0;       // 魔法伤害
        public int fireDamage = 0;        // 火焰伤害
        public int holyDamage = 0;        // 神圣伤害
        public int lightningDamage = 0;   // 闪电伤害

        // 武器防御吸收率 (格挡能力)

        [Header("Weapon Poise")]
        public float poiseDamage = 10;
        // 攻击时的进攻性韧性加成

        // 武器修正参数
        // 轻攻击修正
        // 重攻击修正  
        // 暴击伤害修正等

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        // 奔跑攻击耐力消耗修正
        // 轻攻击耐力消耗修正
        // 重攻击耐力消耗修正等

        // 基于物品的动作 (RB, RT, LB, LT按键)

        // 战技系统

        // 格挡音效
    }
}
