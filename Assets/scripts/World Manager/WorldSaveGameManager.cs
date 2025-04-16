using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace PA
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        public PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFIleDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;

        private void Awake()
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
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "characterSlot_10";
                    break;
                default:
                    fileName = "invalidSlot";
                    break;
            }

            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            // 初始化存档文件读写器
            saveFileDataWriter = new SaveFIleDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            // 根据当前使用的角色槽位决定文件名
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //  如果当前槽位没有数据，就新建一个
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;

            }
            // 根据当前使用的角色槽位决定文件名
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //  如果当前槽位没有数据，就新建一个
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene());
                return;

            }

            // 如果没有足够的空间 , 弹出一个窗口  提醒玩家 没有空位了 
            TitleScreenManager.Instance.DisplayNoFreeCharacterSlotPopUp();

        }


        public void LoadGame()
        {
            // 根据当前使用的角色槽位决定文件名
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            // 初始化存档文件读写器
            saveFileDataWriter = new SaveFIleDataWriter();
            // 使用跨平台的持久化数据路径
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            // 加载存档数据
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            // 异步加载世界场景
            StartCoroutine(LoadWorldScene());
        }

        // 0 references
        public void SaveGame()
        {
            // 根据当前使用的角色槽位决定文件名
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            // 初始化存档文件读写器
            saveFileDataWriter = new SaveFIleDataWriter();
            // 使用跨平台的持久化数据路径
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            //  传递角色信息
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // 创建新角色存档文件
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFIleDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

