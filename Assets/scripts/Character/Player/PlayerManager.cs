 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
public class PlayerManager : CharacterManager
{
        PlayerLocalmotionManager playerLocalmotionManager;
        protected override void Awake()
        {
            base.Awake();
            //只有玩家会做 普通 character 不会做的事
            playerLocalmotionManager = GetComponent<PlayerLocalmotionManager>();
        }

        protected override void Update()
        {
            base.Update();

            //如果当前对象不属于本机,就不要对他更新移动 
            if(!IsOwner)
                return;

            playerLocalmotionManager.HandleAllMovement();
        }
    }

}