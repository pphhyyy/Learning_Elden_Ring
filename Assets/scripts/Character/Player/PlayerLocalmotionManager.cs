using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace PA
{
public class PlayerLocalmotionManager : CharacterLocalMotionManager
{
    PlayerManager player;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float vertialMovement;
    [HideInInspector] public float moveAmount;


    [Header("Movment Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 7 ;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] int sprintingStaminaCost = 8;
    
     
     [Header("Dodge")]
     private Vector3 rollDirection;
     [SerializeField] float dodgeStaminaCost = 25;
     [Header("Jump")]
     [SerializeField] float jumpStaminaCost = 25; 

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = vertialMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            vertialMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0,moveAmount,player.playerNetworkManager.isSprinting.Value);
        }

    }

    public void HandleAllMovement()
    {
        //处理所有移动相关

        //处理地面上的移动 
        HandleGroundMovement();
        
        //处理旋转
        HandleRotation();
    }

    private void GetMovementValues()
    {
        vertialMovement = PlayerInputManager.instance.vertialInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
        //CLAMP
    }


    private void HandleGroundMovement()
    {
        if(!player.canMove)
            return; 
        GetMovementValues();
        //这里玩家移动的方向 将有 player camera 来 决定 
        moveDirection = PlayerCamera.instance.transform.forward * vertialMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement; // 摄像机 移动方向时候，player的方向也要改变
        //这里 direction 只是 方向, 所以需要进行归一化 
        moveDirection.Normalize();
        //unity y 方向 表示 垂直于 "地面"的向上 , 所以这里 y 方向 需要归零 
        moveDirection.y = 0;

        if(player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if(PlayerInputManager.instance.moveAmount > 0.5f  )
            {
                // 跑步 
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }   
            else if (PlayerInputManager.instance.moveAmount <= 0.5f  )
            {   
                // 走 
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }


    }

    private void HandleRotation()
    {
        if(!player.canRotate)
            return;
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * vertialMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0; 

        if(targetRotationDirection == Vector3.zero )
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation , newRotation , rotationSpeed * Time.deltaTime);
        transform.rotation =  targetRotation; 
    }

    public void AttemptToPerfomDodge() // 尝试执行 dodge ，这里尝试，是因为未来耐力或者一些攻击动作 会限制player 在那个时刻不能执行dodge
    {
        if(player.isPerfromingAction)
            return; // 如果player当前正在执行动作 就不应该接受后面这些东西，直接返回
        if(player.playerNetworkManager.currentStamina.Value <=0) // 耐力为零 不能翻滚
            return;

        if(PlayerInputManager.instance.moveAmount > 0) //如果是向前移动的时候 按下 dodge 键，就将让player 转向 camera 的forward 方向
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.vertialInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            rollDirection.y = 0;
            rollDirection.Normalize();

            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;
            
            //之后播放 翻滚动画 （才characterAnimatorManager 中)
            player.playerAnimatorManager.PlayTargetActionAnimtion("Roll_Forward_01",true,true);
        }

        else //说明是向后跳
        {
            //播放向后跳的动画
            player.playerAnimatorManager.PlayTargetActionAnimtion("Back_Step_01",true,true);
        }
        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
    }

    public void HandleSprinting()
    {
        if (player.isPerfromingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        //如果player 没有耐力了，也不应该设置 sprint 为 true 
        if(moveAmount >= 0.5f)
            player.playerNetworkManager.isSprinting.Value = true;
        else
            player.playerNetworkManager.isSprinting.Value = false;
        //只有player 正在 moving 的时候，按下对应的键才能设置 sprint 为 true ， 否则就应该将其设置为 false 

        if(player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }
    }

    public void AttemptToPerfomJump() 
    {
        if(player.isPerfromingAction)
            return; // 如果player当前正在执行动作 就不应该接受后面这些东西，直接返回
        if(player.playerNetworkManager.currentStamina.Value <=0) // 耐力为零 不能翻滚
            return;

        if(player.isJumping)
            return;

        // 如果还未接地，就不应该处理jump 的输入 
        if(player.isGrounded)
            return;

        // 如果当前是双手握持武器，就要处理双手的jump 动画，否则就默认播放 单手武器版的 动画 
        player.playerAnimatorManager.PlayTargetActionAnimtion("Main_Jump_01",false);
        player.isJumping = true;

        player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

    }
}


}
