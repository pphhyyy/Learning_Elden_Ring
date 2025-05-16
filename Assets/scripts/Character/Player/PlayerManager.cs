using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PA
{
    public class PlayerManager : CharacterManager
    {


        [Header("Debug MENU")]
        [SerializeField] bool respawnCharacter = false; // 角色重生 
        [SerializeField] bool switchRightWeapon = false;

        [HideInInspector] public PlayerLocalmotionManager playerLocalmotionManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        protected override void Awake()
        {
            base.Awake();
            //只有玩家会做 普通 character 不会做的事
            playerLocalmotionManager = GetComponent<PlayerLocalmotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
        }

        protected override void Update()
        {
            base.Update();
            //Debug.Log("Remote Debug from Mac ");
            //如果当前对象不属于本机,就不要对他更新移动 
            if (!IsOwner)
                return;

            playerLocalmotionManager.HandleAllMovement();

            playerStatsManager.RegenarateStamina();

            DebugMenu();

        }


        protected override void LateUpdate()
        {

            if (!IsOwner)
                return;
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
            // 恢复耐力
            playerStatsManager.RegenarateStamina();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                PlayerCamera.instance.player = this; //如果这个player object 是 本机的，那么就给它的 playerCamera 的 player 变量复制到 this  
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;

                // “升级” 时，更新最大血量和最大耐力值
                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                // 更新ui状态栏显示 （体力条和耐力条）
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
            }

            // 状态 
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

            // 装备 
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            // 连接时，如果我们是这个角色的所有者，但我们不是服务器，会把角色删除，然后将我们的角色数据重新加载到一个新实例化的角色上
            // 如果我们是服务器，我们不运行这个操作，因为作为主机，它们已经加载好了，不需要重新加载数据
            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
            }

        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            Debug.Log("ProcessDeathEvent");
            if (IsOwner)
            {
                // 调用 PlayerUIManager 实例的弹出窗口管理器，显示 "YOU DIED" 弹出窗口
                PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
            }

            // 调用基类的 ProcessDeathEvent 方法
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        public override void ReviveCharacter()
        {
            Debug.Log("Revive !");
            base.ReviveCharacter();

            if (IsOwner)
            {
                // 恢复当前生命值和耐力值为最大值
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

                // 恢复专注点（注释中提到，但未实现具体逻辑）

                // 播放重生效果
                playerAnimatorManager.PlayTargetActionAnimtion("Empty", false);
            }
        }

        // 将当前游戏数据保存到角色存档结构体（通过引用修改）
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {

            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            // 从网络管理器获取角色名并保存
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();

            // 分别存储角色位置的XYZ坐标
             currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;

            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
        }

        // 从角色存档结构体加载游戏数据（通过引用修改）
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            Debug.Log("加载！位置");
            // 将存档的角色名同步到网络管理器
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;

            // 从存档数据重建角色位置
            Vector3 myPosition = new Vector3(
                currentCharacterData.xPosition,
                currentCharacterData.yPosition,
                currentCharacterData.zPosition
            );
            transform.position = myPosition;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;


            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.maxHealth.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

        }

        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }

            if (switchRightWeapon)
            {
                switchRightWeapon = false;
                playerEquipmentManager.SwitchRightWeapon();
            }
        }
    }

}