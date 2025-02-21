using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{
//这里把 player 和 character 分开是因为 除了玩家以外 游戏中还有许多角色有独立的行为逻辑,不能完全套用玩家player 的 代码 
public class CharacterLocalMotionManager : MonoBehaviour
{
    protected virtual void Awake()
    {
        
    }
}

}