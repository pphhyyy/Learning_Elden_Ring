using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace PA
{
    public class TitleScreenManager : MonoBehaviour
    {

        public static TitleScreenManager Instance;
        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button MainMenunewGameButton;

        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkeyButton;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNetworkAsHost()
        {
            Debug.Log("Start Host!");
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
            //
        }

        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            // 要注意 setactive 为false 的顺序 
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);
            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkeyButton.Select();

        }

        public void CloseNoFreeCharacterSlotPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            MainMenunewGameButton.Select();

        }
    }

}

