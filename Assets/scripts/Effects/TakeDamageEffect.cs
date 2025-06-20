using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    // 继承自InstantCharacterEffect的公共类TakeDamageEffect
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        // 标题：造成伤害的角色
        [Header("Character Causing Damage")]
        // 如果伤害是由另一个角色的攻击造成的，该角色管理器将存储在这里
        public CharacterManager characterCausingDamage;

        // 标题：伤害
        [Header("Damage")]
        // 物理伤害（未来将细分为“标准”、“打击”、“斩击”和“穿刺”）
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        // 标题：最终伤害int
        [Header("Final Damage")]
        // 所有计算完成后角色受到的伤害
        private int finalDamageDealt = 0;

        // 标题：架势
        [Header("Poise")]
        public float poiseDamage = 0;
        // 如果角色的架势被打破，他们将“眩晕”并播放受击动画
        public bool poiseIsBroken = false;

        // （待办）积累效果数值
        // （TO DO）BUILD UPS
        // build up effect amounts

        // 标题：动画
        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        // 标题：音效
        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        // 如果存在元素伤害，在常规音效之上使用的音效
        public AudioClip elementalDamageSoundFX;

        // 标题：受到伤害的方向
        [Header("Direction Damage Taken From")]
        // 用于确定播放什么受击动画（向后移动、向左移动、向右移动等）
        public float angleHitFrom;
        // 用于确定血迹特效生成的位置
        public Vector3 contactPoint;

        // 重写ProcessEffect方法，接收角色管理器实例作为参数
        public override void ProcessEffect(CharacterManager character)
        {
            // 调用基类的ProcessEffect方法
            base.ProcessEffect(character);

            // 如果角色已死亡，则不应处理任何额外的伤害效果
            if (character.isDead.Value)
                return;

            // 检查“无敌”状态

            // 计算伤害
            CalculateDamage(character);
            // 检查伤害来自哪个方向
            PlayDirectionalBasedDamageAnimation(character);
            // 播放受击动画 
            // 检查积累效果（中毒、流血等）
            // 播放受击音效
            PlayDamageSFX(character);
            // 播放受击视觉效果（血迹）
            PlayDamageVFX(character);


            // 如果角色是AI，检查是否存在造成伤害的角色并寻找新目标
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
            {
                return;
            }

            if (characterCausingDamage != null)
            {
                // 检查伤害修正并修改基础伤害（物理/元素伤害增益）
            }

            // 检查角色的固定防御值并从伤害中减去
            // 检查角色的护甲吸收，并从伤害中减去百分比
            // 将所有伤害类型加在一起，并应用最终伤害

            // 将所有伤害类型相加，并应用最终伤害
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }
            Debug.Log("Final Damage Dealt: " + finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            // 计算姿态伤害以确定角色是否会被击晕
            // CALCULATE POISE DAMAGE TO DETERMINE IF THE CHARACTER WILL BE STUNNED

        }

        private void PlayDamageVFX(CharacterManager character)
        {
            Debug.Log("PlayDamageVFX");
            // 如果我们有火焰伤害，播放火焰粒子
            // 闪电伤害，闪电粒子 等等
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            // 若火焰伤害大于 0，播放灼烧音效
            // 若闪电伤害大于 0，播放电击音效
        }
        private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            //当前poise 被打破 
                poiseIsBroken = true;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                // 根据命中角度，设置向前中等伤害 01 动画
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                // 根据命中角度，设置向前中等伤害 01 动画
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // 根据命中角度，设置向后中等伤害 01 动画
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                // 根据命中角度，设置向左中等伤害 01 动画
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                // 根据命中角度，设置向右中等伤害 01 动画
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
            }

            // 如果韧性被打破，播放一个 stagger（摇晃、踉跄 ）的受击动画
            if (poiseIsBroken)
            {
                character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
                character.characterAnimatorManager.PlayTargetActionAnimtion(damageAnimation, true);
            }
        }
    }
}

