using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace SG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;
        
        [SerializeField] int worldSceneIndex = 1;

        private  void Awake()
        {

            //单利模式  
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject); 
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject );
        }

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null; 
        }
    }
}

