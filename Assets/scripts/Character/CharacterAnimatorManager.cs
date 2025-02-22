using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        float vertical;
        float horizontal;

        protected virtual  void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        //由playerinputmanger 直接调用 
        public void UpdateAnimatorMovementParameters(float horizontalValue , float verticalValue)
        {
            //OPTION 1 
            //按原本的值输入,最后的动画会有混合效果 
            //但在这个项目中,我们本来就在 player input manager 里面 设置了 输入值 的钳制 , 输入值 只有 0 0.5 1 所以实际上这里不需要 option 2 
            character.animator.SetFloat("Horizontal",horizontalValue,0.1f,Time.deltaTime);
            character.animator.SetFloat("Vertical",verticalValue,0.1f,Time.deltaTime);
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
    }
}
