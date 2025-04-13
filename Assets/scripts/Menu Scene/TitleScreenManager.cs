using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

namespace PA
{
   public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            Debug.Log("Start Host!"); 
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
        }
    } 

}

