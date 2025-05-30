using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


namespace PA
{
  public class PlayerStatsManager : CharacterStatsManager
  {
    PlayerManager player;


    protected override void Awake()
    {
      base.Awake();
      player = GetComponent<PlayerManager>();
    }


    protected override void Start()
    {
      base.Start();
      // 为什么在这里计算这些？
      // 当我们制作角色创建菜单时，并根据职业设置属性，这些计算将在那里完成
      // 在此之前，属性不会被计算，因此我们在Start中处理。如果存在存档文件，加载场景时这些值会被覆盖
      if (player.IsServer && player.IsOwner ) // 确保只有服务器端执行修改
      {
        player.playerNetworkManager.maxHealth.Value = CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
        player.playerNetworkManager.maxStamina.Value = CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
      }
      }
    }

  }


