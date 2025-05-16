using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{


public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager character;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //动画完成后，进入empty的时候 调用 , 这个脚本将从animator 挂在的object上找到 charatcer 
        if(character == null)
            character = animator.GetComponent<CharacterManager>();
        character.isPerfromingAction = false; //表示现在已经没有在执行动作了 ，重置 isPerfromingAction
        character.canMove = true;
        character.canRotate = true; 
        if (character.IsOwner)
        {
            character.characterNetworkManager.isJumping.Value = false;
        }
        
        //动作结束以后就不应该再使用动画上面的rootmotion了，
        //这里启用rootmotion会导致其他那些不需要rootmotion的动画也会激活rootmotion，并叠加到原来的速度上面
        character.applyRootMotion = false; // 手动实现的rootmotion，用下面被标注的 也行
        //animator.applyRootMotion = false ; 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
}
