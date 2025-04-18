using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


namespace PA
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFIleDataWriter saveFileWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timedPlayed;

        // Unity Message | 0 references
        private void OnEnable()
        {
            LoadSaveSlot();
        }

        // 1 reference
        private void LoadSaveSlot()
        {
            saveFileWriter = new SaveFIleDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

            // SAVE SLOT 01
            if(characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }

            else if(characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                // 如果当前角色存档文件存在，就从其中获取数据，以显示在slot 上
                if(saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                }
                else // 如果不存在，就不要显示这个slot 
                {
                    gameObject.SetActive(false);
                }
            }
        }
    
        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharacterSLot(characterSlot);
        }
    
    }
}

