using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace PA
{
    public class CharacterManager : NetworkBehaviour
    {

        public CharacterController characterController;

        CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            characterController = GetComponent<CharacterController>();
            characterNetworkManager  = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
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
