using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace PA
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(
            false, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController characterController;

        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;

        [Header("Flag")]
        public bool isPerfromingAction = false; // 是否正在执行动作，如果是，就不应该接受其他动作的触发 
        public bool canRotate = true;
        public bool canMove = true;
        public bool applyRootMotion = false;
        // public bool isJumping = false;
        public bool isGrounded = true;



        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            animator = GetComponent<Animator>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        }

        protected virtual void Start()
        {
            Debug.Log("IgnoreMyOwnColliders : Start");
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", isGrounded);
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
                transform.position = Vector3.SmoothDamp(transform.position,
                                                        characterNetworkManager.networkPosition.Value,
                                                        ref characterNetworkManager.networkPositionVelocity,
                                                        characterNetworkManager.networkPositionSmoothTime);

                //rotation 
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        characterNetworkManager.networkRotation.Value,
                                                        characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // 重置需要重置的任何标志
                // 目前没有需要重置的内容

                // 如果我们不在地面上，播放空中死亡动画
                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimtion("Dead_01", true);
                }

                // 播放一些死亡音效
                yield return new WaitForSeconds(5);

                // 奖励玩家符文
                // 禁用角色
            }
        }

        public virtual void ReviveCharacter()
        {

        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            // 将所有可被伤害的角色碰撞器添加到将用于忽略碰撞的列表中
            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            // 将角色控制器的碰撞器添加到将用于忽略碰撞的列表中
            ignoreColliders.Add(characterControllerCollider);

            // 遍历列表中的每个碰撞器，并忽略它们之间的碰撞
            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }

    }
}
