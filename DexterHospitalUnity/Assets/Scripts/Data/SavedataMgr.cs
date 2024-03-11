using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace GameDataManagement
{
    public class SaveData
    {
        public int LetterNumber;
        public int RoleHp;
        public int XiaoXiHp;
        public string SceneName;
        public string SaveTime;
        public bool IsUsed;
    }
    public class SavedataMgr : Singleton<SavedataMgr>
    {
        private SaveData[] Data;

        public SavedataMgr()
        {
            Data = new SaveData[3];
            for (int i = 0; i < 3; ++i) Data[i] = new SaveData();
            LoadData();
        }

        // ------------------ Read File -----------------

        private int GetLetterSum(List<Letter> Letters)
        {
            int ans = 0;
            foreach (Letter _letter in Letters)
            {
                if (_letter.isFound == 1)
                {
                    ans += 1;
                }
            }
            return ans;
        }
        private void WriteIn(int i, GameData gameData)
        {
            Data[i].LetterNumber = GetLetterSum(gameData.Letters);
            Data[i].RoleHp = gameData.Hp;
            Data[i].XiaoXiHp = gameData.Npcs[2].Hp;
            Data[i].SceneName = gameData.SaveSceneName;
            Data[i].SaveTime = gameData.SaveTime;
            Data[i].IsUsed = true;
        }
        private void LoadData()
        {
            for (int i = 0; i < 3; ++i)
            {
                // index = i+1
                string filePath = Application.dataPath + "/Resources/Data/GameData" + (i+1).ToString() + ".json";
                if (File.Exists(filePath))
                {
                    string jsonText = File.ReadAllText(filePath);
                    GameData gameData = JsonConvert.DeserializeObject<GameData>(jsonText);

                    WriteIn(i, gameData);
                }
                else
                {
                    Data[i].IsUsed = false;
                }
            }
        }

        public void SaveUpdate(int index, string jsonText) // intex = 1,2,3
        {
            GameData gameData = JsonConvert.DeserializeObject<GameData>(jsonText);
            WriteIn(index - 1, gameData);
        }

        public SaveData GetSave(int index)
        {
            return Data[index];
        }
    }

}
