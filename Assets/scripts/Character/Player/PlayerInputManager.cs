using System;
using System.Collections;
using System.Collections.Generic;
using PA;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace PA
{
    public class PlayerInputManager : MonoBehaviour
    {
        //建立单例 
        public static PlayerInputManager instance;

        public PlayerManager player;
        PlayerControls playerControls;

        [Header("Camera MoveMent Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVertialInput;
        public float cameraHorizontalInput;


        [Header("Position MoveMent Input")]
        [SerializeField] Vector2 movementInput;
        public float vertialInput;
        public float horizontalInput;
        public float moveAmount; // 移动输入的总量 

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool RB_Input = false;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // 订阅 activeSceneChanged 事件 , 事件发生时, 调用 OnSceneChange  
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }

        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                //如果当前场景是 世界场景 ,那么激活这里的 inputmanager 
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                // 这里设置是为了不让角色在 world scene 以外从场景,例如开始时的界面,因为wasd 的输入而到处跑 ,  
                instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                //这里从 playerControls 设置好在内容中 读取输入的值,然后写入上面设置的变量 movement中 
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;

                //这里 sprint 是按下设置true，抬起设置false
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            }
            playerControls.Enable();
        }

        private void OnDestroy()
        {
            //取消对事件的注册 
            SceneManager.activeSceneChanged -= OnSceneChange;
        }



        private void OnApplicationFocus(bool focus)
        {
            // 这里 focus 就是游戏窗口被选中时才会调用的 函数 
            // 这样设置以后,就不能同时移动两个窗口中的游戏对象了 
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }

        }
        private void Update()
        {
            HandleAllInput();
        }

        private void HandleAllInput()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            //HandleSprintInput();
            HandleJumpInput();
            HandRBInput();
        }


        //MOVEMENT 

        private void HandlePlayerMovementInput()
        {
            vertialInput = movementInput.y;
            horizontalInput = movementInput.x;
            // 但 上下键和 左右键 同时按下时,通过 clamp01 , 将两者的输入值 限制在 0 到 1 上 ,避免45度方向的移动快于 x y 方向 
            moveAmount = Mathf.Clamp01(Mathf.Abs(vertialInput) + Mathf.Abs(horizontalInput));


            // 将移动 的 总量 限制在 0.5 和 1.0 两个数值上, 以此来 表示 跑 和 走 
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }


            if (player == null)
                return;
            //这里直接传 0 和 moveAmount 是因为正常情况下,角色不会 向左先后 那样走,只有在 锁定某个目标的时候,才会出现那样的动作
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }


        private void HandleCameraMovementInput()
        {
            cameraVertialInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        //ACTION 

        private void HandleDodgeInput() //Dodge 躲闪 , 移动中按下就是翻滚，原地按下就是后退 
        {
            if (dodgeInput) // 如果按下了dodge 键
            {
                dodgeInput = false; // 按下一次就执行一次 
                                    //执行dodge 任务 （在localmotion中完成）
                player.playerLocalmotionManager.AttemptToPerfomDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                //处理sprint 相关操作
                player.playerLocalmotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                player.playerLocalmotionManager.AttemptToPerfomJump();
            }
        }

        private void HandRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;
                // TODO: 如果我们打开了一个用户界面窗口，返回并什么都不做
                player.playerNetworkManager.SetCharacterActionHand(true);
                // TODO: 如果我们双手持握武器，使用双手动作
                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

    }

}