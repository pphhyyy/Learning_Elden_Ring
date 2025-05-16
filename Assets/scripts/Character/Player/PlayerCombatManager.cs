using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace PA
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;
        public WeaponItem currentWeaponBeingUsed;


        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }


        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                // 执行动作
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

                // 通知服务器我们已经执行了动作，以便从服务器端视角也执行该动作
                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
            }

        }
    }
}
