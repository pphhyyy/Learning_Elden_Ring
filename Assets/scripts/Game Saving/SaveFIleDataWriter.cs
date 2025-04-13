using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


namespace PA
{
  public class SaveFIleDataWriter
  {
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";


    /// <summary>
    /// 在创建新的保存文件之前要先检查当前角色保存文件 是否已经存在（最大10个）
    /// </summary>
    /// <returns></returns>
    public bool CheckToSeeIfFileExists()
    {
      if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
      {
        return true;
      }
      else
      {
        return false;
      }
    }



    /// <summary>
    /// 删除角色的保存文件
    /// </summary>
    public void DeletSaveFile()
    {
      File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }


    /// <summary>
    /// 开始新游戏时，用于创建新的角色存档
    /// </summary>
    /// <param name="characterData"></param>
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
      // 创建一个路径来保存当前玩家的角色文件
      string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

      try
      {
        // 如果目标目录不存在，则创建该目录
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

        // 如果目标目录不存在，则创建该目录
        string dataToStore = JsonUtility.ToJson(characterData, true);

        // 将数据写入文件系统
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
          using (StreamWriter fileWriter = new StreamWriter(stream))
          {
            fileWriter.Write(dataToStore);
          }
        }
      }
      catch (Exception ex)
      {
        Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + ex);
      }
    }

    /// <summary>
    /// 用于在加载游戏时读取存档文件
    /// </summary>
    /// <returns> </returns>
    public CharacterSaveData LoadSaveFile()
    {
      CharacterSaveData characterData = null;

      // 构建加载文件的完整路径（机器上的存储位置）
      string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

      if (File.Exists(loadPath))
      {
        try
        {
          string dataToLoad = "";
          using (FileStream stream = new FileStream(loadPath, FileMode.Open))
          {
            using (StreamReader reader = new StreamReader(stream))
            {
              dataToLoad = reader.ReadToEnd();
            }
          }

          // 将 JSON 数据反序列化为 Unity 可用的对象
          characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
        }
        catch (Exception ex)
        {

        }


      }

      return characterData;
    }
  }
}
