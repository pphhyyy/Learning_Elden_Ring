using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA
{


    public class WorldCharacterEffectManager : MonoBehaviour
    {
        public static WorldCharacterEffectManager instance;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;

        [Header("VFX")]
        public  GameObject bloodSplatterVFX;

        [SerializeField] List<InstantCharacterEffect> instantEffects;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            GenerateEffectIDs();
        }
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }

        
    }
}
