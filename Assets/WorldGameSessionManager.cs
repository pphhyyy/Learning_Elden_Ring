using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;

        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private void Awake()
        {
            // 如果 instance 为空，将其赋值为当前实例
            if (instance == null)
            {
                instance = this;
            }
            // 否则，销毁当前游戏对象
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddPlayerToActivePlayersList(PlayerManager player)
        {
            // 检查列表，如果列表中还没有该玩家，就添加他们
            if (!players.Contains(player))
            {
                players.Add(player);
            }

            // 检查列表中的空槽位，并移除空槽位
            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            // 检查列表，如果列表中包含该玩家，就移除他们
            if (players.Contains(player))
            {
                players.Remove(player);
            }

            // 检查列表中的空槽位，并移除空槽位
            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }
    }
}