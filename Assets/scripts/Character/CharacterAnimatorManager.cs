using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace PA
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            //用string to hash 的方式，在下面 Setfloat 的时候可以直接穿入 hash 值，相比穿入 string 更加节省内存
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        //由playerinputmanger 直接调用 
        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValue;
            if (isSprinting)
            {
                verticalAmount = 2;
            }

            //OPTION 1 
            //按原本的值输入,最后的动画会有混合效果 
            //但在这个项目中,我们本来就在 player input manager 里面 设置了 输入值 的钳制 , 输入值 只有 0 0.5 1 所以实际上这里不需要 option 2 
            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
            // 这里设置 0.1f,Time.deltaTime 可以让动画 从 idle 到 walk 的过渡更平滑一些 

            //OPTION 2 
            // // 将 水平方向输入的值 限制在 -1 -0.5 0 0.5 1 上, 让动画保持原本的样子, 动画没有混合效果
            // // 主要用于 那些 动画混合效果不够好的动画组 
            // float snappedHorizontal = 0;
            // float snappedVertical = 0;

            // #region Horizontal


            // if (horizontalValue > 0 && horizontalValue <= 0.5f)
            // {
            //     snappedHorizontal = 0.5f;
            // } 
            // else if (horizontalValue > 0.5f && horizontalValue <= 1)
            // {
            //     snappedHorizontal = 1;
            // } 
            // else if (horizontalValue < 0 && horizontalValue >= -0.5f)
            // {
            //     snappedHorizontal = -0.5f;
            // } 
            // else if (horizontalValue < -0.5f && horizontalValue >= -1)
            // {
            //     snappedHorizontal = -1;
            // } 
            // else
            // {
            //     snappedHorizontal = 0;
            // }

            // #endregion

            // #region Vertical

            // if (verticalValue > 0 && verticalValue <= 0.5f)
            // {
            //     snappedVertical = 0.5f;
            // } 
            // else if (verticalValue > 0.5f && verticalValue <= 1)
            // {
            //     snappedVertical = 1;
            // } 
            // else if (verticalValue < 0 && verticalValue >= -0.5f)
            // {
            //     snappedVertical = -0.5f;
            // } 
            // else if (verticalValue < -0.5f && verticalValue >= -1)
            // {
            //     snappedVertical = -1;
            // } 
            // else
            // {
            //     snappedVertical = 0;
            // }

            // #endregion

            // character.animator.SetFloat("Horizontal",snappedHorizontal);
            // character.animator.SetFloat("Vertical",snappedVertical);

        }

        public virtual void PlayTargetActionAnimtion
            (string targteAnimation,
             bool isPermormingAction,
             bool applyRootMotion = true,
             bool canRotate = false,
             bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion; //要为目标动作播放的动画，是否要启动root motion 功能，这里默认为true ；
            character.animator.CrossFade(targteAnimation, 0.2f);

            //这里 isPermormingAction 表示当前对象正在执行动画，其他动作不应该打断他的动画播放 
            //也就是是否允许停止当前动画的播放，转而去执行下一个动画
            character.isPerfromingAction = isPermormingAction;
            //控制在 播放动作动画的时候，不能旋转也不能移动
            character.canMove = canMove;
            character.canRotate = canRotate;

            //还需要告诉 网络端(服务器 )，我们这边的角色在他们哪里要播放动画，不然角色动画效果就只有本地能看见  
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targteAnimation, applyRootMotion);
        }


        public virtual void PlayTargetAttackActionAnimtion
            (string targteAnimation,
             bool isPermormingAction,
             bool applyRootMotion = true,
             bool canRotate = false,
             bool canMove = false)
        {
            // 跟踪上次执行的攻击（用于连招）
            // 跟踪当前攻击类型（轻击、重击等）
            // 更新动画集为当前武器的动画
            // 判定我们的攻击是否能被格挡
            // 告知网络我们的“正在攻击”标志已激活（用于反击伤害等）

            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targteAnimation, 0.2f);
            character.isPerfromingAction = isPermormingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // 告知服务器/主机我们播放了一个动画，并让其他在场的人也播放该动画
            character.characterNetworkManager.NotifyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targteAnimation, applyRootMotion);
        }



    }
}
