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
        //  public CharacterSaveData characterSlot02;
        //  public CharacterSaveData characterSlot03;
        //  public CharacterSaveData characterSlot04;
        //  public CharacterSaveData characterSlot05;
        //  public CharacterSaveData characterSlot06;
        //  public CharacterSaveData characterSlot07;
        //  public CharacterSaveData characterSlot08;
        //  public CharacterSaveData characterSlot09;
        //  public CharacterSaveData characterSlot10;

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
        }

        private void Update()
        {
            if(saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if(loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    saveFileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    saveFileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    saveFileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    saveFileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    saveFileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    saveFileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    saveFileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    saveFileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    saveFileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    saveFileName = "characterSlot_10";
                    break;
                default:
                    saveFileName = "invalidSlot";
                    break;
            }
        }

        public void CreateNewGame()
        {
            // 根据当前使用的角色槽位决定文件名
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            Debug.Log("CreateNewGame");
            // 创建新的角色数据对象
            currentCharacterData = new CharacterSaveData();

            // 可在此添加初始化默认数据的逻辑
            // 例如：currentCharacterData.health = 100;
        }


        public void LoadGame()
        {
            // 根据当前使用的角色槽位决定文件名
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

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
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

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

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

