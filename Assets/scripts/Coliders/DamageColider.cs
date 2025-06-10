using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class DamageColider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider;

        [Header("Damage")]
        public float physicalDamage = 0; // （待办）拆分为“标准”、“打击”、“斩击”和“穿刺”
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint; // 碰撞点，用于确定伤害来源的位置

        [Header("Characer Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>(); // 受伤角色列表

        protected virtual void Awake()
        {

        }

        protected virtual  void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            Debug.Log("OnTriggerEnter :" + other.gameObject.name);
            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // 检查是否可以基于友军伤害规则对目标造成伤害

                // 检查目标是否在格挡

                // 检查目标是否处于无敌状态

                // 造成伤害

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
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

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
        

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            // 清空之前记录的 collider 遇见的 目标对象的列表 
            charactersDamaged.Clear();
        }

    }

}
