using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PA
{
public class PlayerCamera : MonoBehaviour
{

    public Camera cameraObject;

    public PlayerManager player; 
    public static PlayerCamera instance;
    
    // 这个transform 独立于 playercamer 自己的 transform , 是 camera 子物体,up and down 上的 tansform ,
    //这样设置是因为 在 绕y 旋转以后, 如果还是继续用之前的transform 来旋转 ,就会出现错误
    [SerializeField] Transform cameraPivotTransform;  
    [Header("Camera Settings ")]
    private float cameraSmoothSpeed =1;

    // 这里因为用的是鼠标 所以speed 设置的是 10 , 后面用rog ally 调试时,可以设置为220
    [SerializeField] float leftAndRightRotationSpeed = 10;
    [SerializeField] float upAndDownRotationSpeed = 10;

    //这里 pivot 是可以看的 最低点和最高点,这样设置可以防止摄像机在最高最低处 突然反向 
    [SerializeField] float minimumPivot = -30;
    [SerializeField] float maxmumPivot = 60;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; //发送碰撞后,相机就移动到这个地方 
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    [SerializeField] float cameraCollisionRadius = 0.2f;

    [SerializeField] LayerMask collideWithLayer; //那些层的物体和 相机射线碰撞后,需要处理相机的位置 

    //这两个值是用来处理 camer collision 时候 相机位置移动的 
    private float CameraZPosition;
    private float targetCameraZPosition;

    protected void Awake()
    {

        
        if(instance == null)
        instance = this;
        else 
        Destroy(gameObject);
    }

    protected void Start()
    {
        DontDestroyOnLoad(gameObject);
        // 这里相机z轴是指向 目标点的, 相机碰到 场景物体,就让相机沿着和z 轴移动到 碰不到的地方就好了 
        CameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if(player != null)
        {
            //跟随玩家
            HandleFollowTarget();
            //绕着玩家旋转
            HandleRotation();
            //与场景“边界”的碰撞 
            HandleCollisions();
        }
    
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position,player.transform.position,ref cameraVelocity,cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition; 
    }

    private void HandleRotation()
    {
        // 如果有锁定,就加上 look 否则就正常旋转 

        //正常旋转 
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        upAndDownLookAngle -= (PlayerInputManager.instance.cameraVertialInput * upAndDownRotationSpeed) * Time.deltaTime;
        //上下的镜头移动需要钳制在一个范围内,否则会在90°的临界区 出现镜头的大幅度移动 
        upAndDownLookAngle  = Mathf.Clamp(upAndDownLookAngle,minimumPivot,maxmumPivot);


        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        //在左右方向进行旋转 (绕 摄像机 y 轴)
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        
        //上下方向上的旋转,(绕摄像机 x 轴)
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition  = CameraZPosition;
        RaycastHit hit;
        //相机位置 到 pivot 的位置 直线,相当于是相机射向 pivot 的一条射线  根据这个射线找到"碰撞点",再用碰撞点和 pivot 的距离 减去 radius 就是 相机应该去的位置 
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();
        if (Physics.SphereCast(cameraPivotTransform.position , cameraCollisionRadius , direction, out hit , Mathf.Abs(targetCameraZPosition),collideWithLayer))
        {
            Debug.Log("摄像机射线碰撞");
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position,hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        //如果已经太近了,甚至超过了 cameraCollisionRadius 就把 targetCameraZPosition 设置为 cameraCollisionRadius
        if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z , targetCameraZPosition,0.2f);
        cameraObject.transform.localPosition =  cameraObjectPosition;
    }
}
}
