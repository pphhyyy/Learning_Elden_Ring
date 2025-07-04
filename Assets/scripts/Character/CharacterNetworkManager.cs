using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace PA
{
    public class CharacterNetworkManager : NetworkBehaviour // 这里network behaviour 依然有 monbehavirour 的属性,只是多了很多和 network 相关的 内容 
    {

        CharacterManager character;

        //配置 position 的 网络参数, 所有对象都能访问,但只有拥有者才能写入 
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        // 同步旋转 参数 
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;
        //public Quaternion networkRotationVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;


        //为客户端玩家 同步 动画value 
        [Header("Animator")]
        public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Flags")]
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Resources")]
        public NetworkVariable<int> currentHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> vitality = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void CheckHP(int oldValue, int newValue)
        {
            if (currentHealth.Value <= 0)
            {
                StartCoroutine(character.ProcessDeathEvent());
            }

            // 防止我们过度治疗
            if (character.IsOwner)
            {
                if (currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
            }
        }


        //从任何客户端调用的函数，将信息从客户端到服务器 每当要播放动画的时候，都会请求服务器 RPC
        //所有charatcer 调用 PlayTargetActionAnimtion 的时候 都会调用一次  NotifyTheServerOfActionAnimationServerRpc 
        //以通知服务器，而服务器 用客户端传递过来的参数 调用 PlayActionAnimationForAllClientsClientRpc
        //让所有clinet 都更新对应的动画  
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //通知服务器，这边的动作附带有 动画
            if (IsServer)
            {
                //如果是本机作为服务器，那么就应该通知其他所有客户端，要播放对应的动画
                PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }
        //有客户端调用，并在服务器/主机上 接受 并执行



        //服务器调用 client RPC 通知所有客户端播放服务器中存在的动画信息，
        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //如果得到的 clientID 就是本机的 id 那么就不需要响应这个动画事件了，因为本机本地已经播放了一次，没必要播放第二次
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                //如果这个clientID 不是本机的，表示他们是客户端，需要在本机上播放他们动作的动画
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }
        //服务器调用 客户端执行 

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            //播放动画
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

        //从任何客户端调用的函数，将信息从客户端到服务器 每当要播放动画的时候，都会请求服务器 RPC
        //所有charatcer 调用 PlayTargetActionAnimtion 的时候 都会调用一次  NotifyTheServerOfActionAnimationServerRpc 
        //以通知服务器，而服务器 用客户端传递过来的参数 调用 PlayActionAnimationForAllClientsClientRpc
        //让所有clinet 都更新对应的动画  

        // 攻击动画 

        [ServerRpc]
        public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //通知服务器，这边的动作附带有 动画
            if (IsServer)
            {
                //如果是本机作为服务器，那么就应该通知其他所有客户端，要播放对应的动画
                PlayAttackActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }
        //有客户端调用，并在服务器/主机上 接受 并执行



        //服务器调用 client RPC 通知所有客户端播放服务器中存在的动画信息，
        [ClientRpc]
        public void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            //如果得到的 clientID 就是本机的 id 那么就不需要响应这个动画事件了，因为本机本地已经播放了一次，没必要播放第二次
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                //如果这个clientID 不是本机的，表示他们是客户端，需要在本机上播放他们动作的动画
                PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
            }
        }
        //服务器调用 客户端执行 

        private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            //播放动画
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

        // 伤害 
        // 伤害
        [ServerRpc(RequireOwnership = false)]
        // 0个引用
        public void NotifyTheServerOfCharacterDamageServerRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            if (IsServer)
            {
                NotifyTheServerOfCharacterDamageClientRpc(damagedCharacterID,
                    characterCausingDamageID,
                    physicalDamage,
                    magicDamage,
                    fireDamage,
                    holyDamage,
                    poiseDamage,
                    angleHitFrom,
                    contactPointX,
                    contactPointY,
                    contactPointZ);
            }
        }

        [ClientRpc]
        // 0个引用
        public void NotifyTheServerOfCharacterDamageClientRpc(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            ProcessCharacterDamageFromServer(damagedCharacterID,
                    characterCausingDamageID,
                    physicalDamage,
                    magicDamage,
                    fireDamage,
                    holyDamage,
                    poiseDamage,
                    angleHitFrom,
                    contactPointX,
                    contactPointY,
                    contactPointZ);
        }


        public void ProcessCharacterDamageFromServer(
            ulong damagedCharacterID,
            ulong characterCausingDamageID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectManager.instance.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }





    }
}
