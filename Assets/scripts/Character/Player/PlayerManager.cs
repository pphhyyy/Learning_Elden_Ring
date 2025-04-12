 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
public class PlayerManager : CharacterManager
{
        [HideInInspector] public PlayerLocalmotionManager playerLocalmotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager; 
        [HideInInspector] public PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();
            //只有玩家会做 普通 character 不会做的事
            playerLocalmotionManager = GetComponent<PlayerLocalmotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>(); 
            playerStatsManager = GetComponent<PlayerStatsManager>();

            
        }

        protected override void Update()
        {
            base.Update();
            Debug.Log("Remote Debug from Mac ");
            //如果当前对象不属于本机,就不要对他更新移动 
            if(!IsOwner)
                return;

            playerLocalmotionManager.HandleAllMovement();

        }


        protected override void LateUpdate()
        {

            if(!IsOwner) 
                return; 
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
            // 恢复耐力
            playerStatsManager.RegenarateStamina(); 
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if(IsOwner) 
            {
                PlayerCamera.instance.player = this; //如果这个player object 是 本机的，那么就给它的 playerCamera 的 player 变量复制到 this  
                PlayerInputManager.instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;


                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
            }
        }

         

    }

}