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
    PlayerControls playerControls;

    [Header("Position MoveMent Input")]
    [SerializeField] Vector2 movementInput;
    public float vertialInput;
    public float horizontalInput;
    public float moveAmount; // 移动输入的总量 

    [Header("Camera MoveMent Input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVertialInput;
    public float cameraHorizontalInput;
    private void Awake()
    {
        if(instance == null)
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
    }

    private void OnSceneChange(Scene oldScene  , Scene newScene)
    {
        if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            //如果当前场景是 世界场景 ,那么激活这里的 inputmanager 
            instance.enabled = true;
        }
        else 
        {
            // 这里设置是为了不让角色在 world scene 以外从场景,例如开始时的界面,因为wasd 的输入而到处跑 ,  
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            //这里从 playerControls 设置好在内容中 读取输入的值,然后写入上面设置的变量 movement中 
            playerControls.PlayerMovement.Movement.performed += i  => movementInput = i.ReadValue<Vector2>();

            playerControls.PlayerCamera.Movement.performed += i  => cameraInput  = i.ReadValue<Vector2>();
        }
        playerControls.Enable(); 
    }

    private  void OnDestroy()
    {
        //取消对事件的注册 
        SceneManager.activeSceneChanged -= OnSceneChange;
    }



    private void OnApplicationFocus(bool focus)
    {
        // 这里 focus 就是游戏窗口被选中时才会调用的 函数 
        // 这样设置以后,就不能同时移动两个窗口中的游戏对象了 
        if(enabled)
        {
            if(focus)
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
        HandlePositionMovementInput();
        HandleCameraMovementInput();
    }


    private void HandlePositionMovementInput()
    {
        vertialInput = movementInput.y;
        horizontalInput = movementInput.x;
        // 但 上下键和 左右键 同时按下时,通过 clamp01 , 将两者的输入值 限制在 0 到 1 上 ,避免45度方向的移动快于 x y 方向 
        moveAmount = Mathf.Clamp01(Mathf.Abs(vertialInput) + Mathf.Abs(horizontalInput));


        // 将移动 的 总量 限制在 0.5 和 1.0 两个数值上, 以此来 表示 跑 和 走 
        if(moveAmount <= 0.5 && moveAmount >0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <=1 )
        {
            moveAmount = 1; 
        }
    }


    private void HandleCameraMovementInput()
    {
        cameraVertialInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }
}

}