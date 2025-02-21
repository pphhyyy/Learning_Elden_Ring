using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;

namespace PA
{
    public class CharacterNetworkManager : NetworkBehaviour // 这里network behaviour 依然有 monbehavirour 的属性,只是多了很多和 network 相关的 内容 
    {
        //配置 position 的 网络参数, 所有对象都能访问,但只有拥有者才能写入 
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero,NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);
        
        // 同步旋转 参数 

        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);

        
        public Vector3 networkPositionVelocity;
        //public Quaternion networkRotationVelocity;

        public float networkPositionSmoothTime  = 0.1f;
        public float networkRotationSmoothTime  = 0.1f;
    }
}
