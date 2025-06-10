using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class MeleeWeaponDamageCollider : DamageColider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // （在计算伤害时，用于检查攻击者的伤害修正、效果等）
        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
            damageCollider.enabled = false; // 只有在发动攻击动作的适合，才打开武器上面的伤害碰撞器
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();


            if (damageTarget != null)
            {
                if (damageTarget == characterCausingDamage) //不伤害自己 
                    return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // 检查是否可以基于友军伤害规则对目标造成伤害

                // 检查目标是否在格挡

                // 检查目标是否处于无敌状态

                // 造成伤害

                DamageTarget(damageTarget);
            }

        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // 我们不希望在一次攻击中对同一个目标造成多次伤害
            // 因此我们将其添加到一个列表中，在应用伤害之前进行检查
            if (charactersDamaged.Contains(damageTarget))
                return;
            Debug.Log("Damage Target: " + damageTarget.gameObject.name);
            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            if (characterCausingDamage.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z
                );
            }


            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.poiseDamage *= modifier;

            // 如果攻击是完全充能的重击，在计算完普通伤害修正值后，再乘以完全充能修正值
        }

    }
}

