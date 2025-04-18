 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace PA
{
    public class CharacterManager : NetworkBehaviour
    {

        [HideInInspector] public CharacterController characterController;

        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;


        [Header("Flag")]
        public bool isPerfromingAction = false; // 是否正在执行动作，如果是，就不应该接受其他动作的触发 
        public bool canRotate = true;  
        public bool canMove = true;
        public bool applyRootMotion = false; 
        public bool isJumping = false;
        public bool isGrounded = true;

        
        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject); 
             
            characterController = GetComponent<CharacterController>();
            characterNetworkManager  = GetComponent<CharacterNetworkManager>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            animator.SetBool("IsGrounded",isGrounded);
            if (IsOwner)
            {
                // 如果是该对象的主机 ,那么该对象 的位置 就应该和主机位置同步 
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }

            else
            {
                //如果对象不属于该主机,就用他的网络数据 平滑后 赋值给该主机 以显示其他玩家 

                //position
                transform.position = Vector3.SmoothDamp(transform.position , 
                                                        characterNetworkManager.networkPosition.Value , 
                                                        ref characterNetworkManager.networkPositionVelocity,
                                                        characterNetworkManager.networkPositionSmoothTime);

                //rotation 
                transform.rotation = Quaternion.Slerp(transform.rotation , 
                                                        characterNetworkManager.networkRotation.Value , 
                                                        characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {
            
        }

        
    }
}
