using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace PA
{
    public class PlayerUIManager : MonoBehaviour
    {   
        //作为单例来构建
        public static PlayerUIManager instance;  

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

        private void Awake()
        {
            if(instance == null)
            {
                instance =this;
            }
            else
            {
                Destroy(gameObject);
            }

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
        }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            if(startGameAsClient)
            {
                startGameAsClient = false;
                //为了作为客户端接入服务器，这里需要先关闭 （玩家端）的网络主机
                NetworkManager.Singleton.Shutdown();
                Debug.Log("Close NetWork Client !");
                //之后再作为客户端 打开 
                NetworkManager.Singleton.StartClient();
            }

        }
    }
}
