using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PA
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;


        [Header("Action Sound")]
        public AudioClip rollSFX;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            // 生成一个随机索引，范围是 0 到数组长度（不包含数组长度值  ）
            int index = Random.Range(0, array.Length);
            // 返回数组中对应索引的音频片段
            return array[index];
        }

    }
}

