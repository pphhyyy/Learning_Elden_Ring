using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class MeleeWeaponDamageCollider : DamageColider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // （在计算伤害时，用于检查攻击者的伤害修正、效果等）
    }
}

