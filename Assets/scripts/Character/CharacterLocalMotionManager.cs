using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    //这里把 player 和 character 分开是因为 除了玩家以外 游戏中还有许多角色有独立的行为逻辑,不能完全套用玩家player 的 代码 
    public class CharacterLocalMotionManager : MonoBehaviour
    {
        CharacterManager characterManager;
        //public bool gamestart = false;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 0.3f;
        //[SerializeField] float groundCheckDistance = 0.1f; // 直线检测距离
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20; //角色在地面上受到的力
        [SerializeField] protected float fallStartYVelocity = -5; //角色下落时的力
        [SerializeField] float originDiff = 0.1f;
        //[SerializeField] float CheckSphere_diff = 0f;
        protected bool fallingVelocityHAsBeenset = false;
        protected float inAirTimer = 0;



        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {

            
            HandleGroundCheck();
            if (true)
            {
                if (characterManager.isGrounded)
                {
                    // 如果我们没有尝试跳跃或向上移动
                    if (yVelocity.y < 0)
                    {
                        inAirTimer = 0;
                        fallingVelocityHAsBeenset = false;
                        yVelocity.y = groundedYVelocity;
                    }
                }
                else
                {
                    // 如果我们没有跳跃，并且下落速度尚未设置
                    if (!characterManager.characterNetworkManager.isJumping.Value && !fallingVelocityHAsBeenset)
                    {
                        fallingVelocityHAsBeenset = true;
                        yVelocity.y = fallStartYVelocity;
                    }

                    inAirTimer = inAirTimer + Time.deltaTime;
                    Debug.Log("InAirTimer :" + inAirTimer);
                    characterManager.animator.SetFloat("InAirTimer", inAirTimer);

                    yVelocity.y += gravityForce * Time.deltaTime;
                }

                // 不管当前处于什么状态，都应该有一股y方向的力，把角色拽向地面
                characterManager.characterController.Move(yVelocity * Time.deltaTime);
            }
        }

        protected void HandleGroundCheck()
        {
            //RaycastHit hit;
            //Vector3 origin = characterManager.transform.position + Vector3.up *originDiff;
            // 直线检测
            //characterManager.isGrounded = Physics.Raycast(characterManager.transform.position,Vector3.down, out hit,groundCheckDistance,groundLayer);
            //Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

            // 球形检查 这里应该等角色滞空一段时间以后才检查，不然如果刚刚跳起然后胶囊体膜到了地面，就会判定接地，然后掉下来
            Vector3 orignal_check_position = characterManager.transform.position + Vector3.up * originDiff;
            characterManager.isGrounded = Physics.CheckSphere(orignal_check_position, groundCheckSphereRadius, groundLayer);

            // 向下的球形检查
            // characterManager.isGrounded = Physics.SphereCast(origin ,groundCheckSphereRadius,Vector3.down,out hit,groundCheckDistance,groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            //Vector3 origin = characterManager.transform.position + Vector3.up * originDiff;
            // 在gizmos 上 画出 地面检测半径的 球体 
            //Vector3 orignal_check_position = characterManager.transform.position + Vector3.up * originDiff;
            //  Gizmos.DrawSphere(orignal_check_position, groundCheckSphereRadius);
        }
    }

}