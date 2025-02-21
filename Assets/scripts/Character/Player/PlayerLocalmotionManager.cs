using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace PA
{
public class PlayerLocalmotionManager : CharacterLocalMotionManager
{
    PlayerManager player;
    public float horizontalMovement;
    public float vertialMovement;
    public float moveAmount;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;

    [SerializeField] float rotationSpeed = 15;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    public void HandleAllMovement()
    {
        //处理所有移动相关

        //处理地面上的移动 
        HandleGroundMovement();
        
        //处理旋转
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        vertialMovement = PlayerInputManager.instance.vertialInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;

        //CLAMP
    }


    private void HandleGroundMovement()
    {

        GetVerticalAndHorizontalInputs();
        //这里玩家移动的方向 将有 player camera 来 决定 
        moveDirection = PlayerCamera.instance.transform.forward * vertialMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        //这里 direction 只是 方向, 所以需要进行归一化 
        moveDirection.Normalize();
        //unity y 方向 表示 垂直于 "地面"的向上 , 所以这里 y 方向 需要归零 
        moveDirection.y = 0;

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

    private void HandleRotation()
    {
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
}

}
