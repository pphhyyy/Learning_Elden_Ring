using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            if(player.applyRootMotion)
            {
                //手动实现 rootmotion 功能呢
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }
    }

}
