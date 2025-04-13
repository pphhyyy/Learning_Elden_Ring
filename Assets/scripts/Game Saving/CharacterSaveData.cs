using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA {
  [System.Serializable]
  // 这个类只是作为数据保存和加载时的一个模版数据结构而已，不会附加到实际的游戏对象上
  public class CharacterSaveData 
  {
    [Header("Character Name")]
    public string characterName;

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Coordinates")] // 为了方便序列化，这里直接用 三个 flaot  表示坐标，而不是用 vector ,因为unity 的序列化只支持基本的数据类型 
    public float xPosition;
    public float yPosition;
    public float zPosition;

  }
}
