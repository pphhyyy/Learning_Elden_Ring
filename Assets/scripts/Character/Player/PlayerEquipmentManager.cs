using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace PA
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;


        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();
            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }


        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }


        // 右手武器 

        public void SwitchRightWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimatorManager.PlayTargetActionAnimtion("Swap_Right_Weapon_01", false, true, true, true);

            // 艾尔登法环武器切换
            // 1. 检查是否有主武器以外的其他武器，如果有，永远不要切换到空手状态，在武器1和武器2之间轮换
            // 2. 如果没有，则切换到空手状态，然后跳过另一个空槽并切换回来。在返回主武器之前不要处理两个空槽

            WeaponItem selectedWeapon = null;

            // 如果我们正在双持，则禁用双持
            // 检查我们的武器索引（我们有3个槽位，所以有3个可能的数字）
            // 禁用双持模式（如果我们正在双持）

            // 将索引加1以切换到下一个潜在武器
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            // 如果索引超出范围，将其重置为位置#1（0）
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                // 我们检查是否持有多于一把武器
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponInRightHandSlots[i].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = Instantiate(WorldItemDataBase.Instance.unarmedWeapon);
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponInRightHandSlots)
            {
                // 检查下一个武器是不是“空手”武器
                if (player.playerInventoryManager.weaponInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    // 这里武器的切换也要在 网络端 进行，让其他玩家也能看见这里的武器切换 
                    player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }


        }


        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                //移除上一个武器的模型
                rightHandSlot.UnloadWeapon();

                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                //指定武器的 damage  到这个碰撞体上
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            }
        }


        //左手武器 

        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner)
                return;

            player.playerAnimatorManager.PlayTargetActionAnimtion("Swap_Left_Weapon_01", false, true, true, true);

            // 艾尔登法环武器切换
            // 1. 检查是否有主武器以外的其他武器，如果有，永远不要切换到空手状态，在武器1和武器2之间轮换
            // 2. 如果没有，则切换到空手状态，然后跳过另一个空槽并切换回来。在返回主武器之前不要处理两个空槽

            WeaponItem selectedWeapon = null;

            // 如果我们正在双持，则禁用双持
            // 检查我们的武器索引（我们有3个槽位，所以有3个可能的数字）
            // 禁用双持模式（如果我们正在双持）

            // 将索引加1以切换到下一个潜在武器
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            // 如果索引超出范围，将其重置为位置#1（0）
            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                // 我们检查是否持有多于一把武器
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponInLeftHandSlots[i].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = Instantiate(WorldItemDataBase.Instance.unarmedWeapon);
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }
                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponInLeftHandSlots)
            {
                // 检查下一个武器是不是“空手”武器
                if (player.playerInventoryManager.weaponInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    // 这里武器的切换也要在 网络端 进行，让其他玩家也能看见这里的武器切换 
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;
                }

            }

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }

        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWaepon != null)
            {
                leftHandSlot.UnloadWeapon();
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWaepon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                //指定武器的 damage  到这个碰撞体上
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWaepon);

            }
        }

        // 伤害碰撞体
        public void OpenDamageCollider()
        {
            Debug.Log("OpenDamageCollider ! ");
            // 开启右手武器伤害碰撞体
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }
            // 开启左手武器伤害碰撞体
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }
            // 播放嗖的音效
        }

        public void CloseDamageCollider()
        {
            Debug.Log("CloseDamageCollider ! ");
            // 关闭右手武器伤害碰撞体
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            // 关闭左手武器伤害碰撞体
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
        }
    }
}
