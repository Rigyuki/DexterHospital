using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameDataManagement
{
    public class BagItem
    {
        public string Name;
        public int Count;
    }

    public class Npc
    {
        public string Name;
        public int Hp;
        public int Favor;
        public int isFound; // 0: not found
    }

    public class SceneContent
    {
        public string Name;
        public List<SceneItem> SceneItems;
    }

    public class SceneItem
    {
        public string Name;
        public int TriggerCnt;
        public int IsAtivate;
    }

    public class Letter
    {
        public string Name;
        public int isFound;
    }

    public class GameData
    {
        // ��ɫ HP
        public int Hp;
        // �Ƿ��յ��ż�
        public List<Letter> Letters;
        // npc״̬��Ѫ�� �Ƿ���� �øжȣ�
        public List<Npc> Npcs;
        // ����
        public List<BagItem> Bag; 
        // ����״̬ ���彻���������Ƿ���ʾĳ����
        public List<SceneContent> Scenes;

        // �浵������
        public string SaveSceneName;
        // ����ʱ��
        public string SaveTime;
    }
    public class UserdataMgr : Singleton<UserdataMgr>
    {
        private GameData Data;

        private string PlayerPlaceCode;

        private int BeCode;

        public UserdataMgr()
        {
            LoadData(0); // init dele TODO
            PlayerPlaceCode = "";
        }

        // ------------------ Hp -----------------
        public int GetHp()
        {
            return Data.Hp;
        }

        public void SetHp(int _hp)
        {
            Data.Hp = _hp;
        }

        // ------------------ Letters -----------------
        public Letter GetLetters(int index)
        {
            return Data.Letters[index];
        }

        public void AddLetter(string LetterName)
        {
            foreach(Letter _letter in Data.Letters)
            {
                if (_letter.Name == LetterName)
                {
                    _letter.isFound = 1;
                }
            }
            // TODO �յ���
        }

        // ------------------ Npc -----------------
        public Npc GetNpcs(int index)
        {
            return Data.Npcs[index];
        }

        // ------------------ Bag -----------------
        public void BagAddItem(string ItemName, int Number)
        {
            // ��������û��
            BagItem flag = null;
            foreach (BagItem bagItem in Data.Bag)
            {
                if (bagItem.Name == ItemName)
                {
                    flag = bagItem;
                    break;
                }
            }
            if (flag != null) flag.Count = flag.Count + Number;
            else Data.Bag.Add(new BagItem() { Name = ItemName, Count = Number });
        }

        public int BagUseItem(string ItemName, int Number) // ����0��ʾ��Ʒ�����޷�ʹ�ã���ʹ�÷���1
        {
            BagItem flag = null;
            foreach (BagItem bagItem in Data.Bag)
            {
                if (bagItem.Name == ItemName)
                {
                    flag = bagItem;
                    break;
                }
            }
            if (flag != null && flag.Count >= Number)
            {
                // use
                if (flag.Count == Number)
                {
                    Data.Bag.Remove(flag);
                }
                else 
                    flag.Count = flag.Count - Number;
                return 1;
            }
            return 0;
        }

        public int BagItemNumber(string ItemName) // �����������
        {
            BagItem flag = null;
            foreach (BagItem bagItem in Data.Bag)
            {
                if (bagItem.Name == ItemName)
                {
                    flag = bagItem;
                    break;
                }
            }
            if (flag != null)
            {
                return flag.Count;
            }
            return 0;
        }

        public List<BagItem> GetBag()
        {
            return Data.Bag;
        }

        // ------------------ Scene item trigger time -----------------
        // ��ȡ���崥������
        public int GetTriggerTime(string SceneName, string ItemName)
        {
            foreach (SceneContent scene in Data.Scenes)
            {
                if (scene.Name == SceneName)
                {
                    foreach (SceneItem item in scene.SceneItems)
                    {
                        if (item.Name == ItemName)
                            return item.TriggerCnt;
                    }
                    return 0;
                }
            }
            return 0;
        }
        // ���崥������+1
        public void TriggerTimeAdd1(string SceneName, string ItemName)
        {
            SceneContent sceneFlag = null;
            SceneItem itemFlag = null;

            foreach (SceneContent scene in Data.Scenes)
            {
                if (scene.Name == SceneName)
                {
                    sceneFlag = scene;
                    foreach (SceneItem item in scene.SceneItems)
                    {
                        if (item.Name == ItemName)
                        {
                            sceneFlag = scene;
                            itemFlag = item;
                            break;
                        } 
                    }
                    break;
                }
            }
            if (itemFlag != null)
            {
                itemFlag.TriggerCnt = itemFlag.TriggerCnt + 1;
                return;
            }

            itemFlag = new SceneItem() { Name = ItemName, TriggerCnt = 1, IsAtivate = 1 };
            if (sceneFlag == null)
            {
                sceneFlag = new SceneContent() { Name = SceneName, SceneItems = new List<SceneItem>() };
                Data.Scenes.Add(sceneFlag);
            }

            sceneFlag.SceneItems.Add(itemFlag);
        }

        public void ItemObjectReplace(string SceneName, string OldItemName, string NewItemName)
        {
            SceneContent sceneFlag = null;
            SceneItem itemFlag = null;

            SceneItem newItem = new SceneItem() { Name = NewItemName, TriggerCnt = 0, IsAtivate = 1 };

            foreach (SceneContent scene in Data.Scenes)
            {
                if (scene.Name == SceneName)
                {
                    sceneFlag = scene;
                    foreach (SceneItem item in scene.SceneItems)
                    {
                        if (item.Name == OldItemName)
                        {
                            sceneFlag = scene;
                            itemFlag = item;
                            break;
                        }
                    }
                    break;
                }
            }

            if (itemFlag != null)
            {
                itemFlag.IsAtivate = 0;
            }
            else
            {
                itemFlag = new SceneItem() { Name = OldItemName, TriggerCnt = 0, IsAtivate = 0 };
                if (sceneFlag != null)
                {
                    sceneFlag = new SceneContent() { Name = SceneName, SceneItems = new List<SceneItem>() };
                    Data.Scenes.Add(sceneFlag);
                }
                sceneFlag.SceneItems.Add(itemFlag);
            }
            sceneFlag.SceneItems.Add(newItem);
        }

        // ------------------ Read File -----------------
        public void LoadData(int index) // 0: init; intex = 1,2,3, filename = SaveData1.json
        {
            // read json
            string filePath;
            if (index == 0)
            {
                filePath = Application.dataPath + "/Resources/Data/GameDataInit.json"; 
            }
            else
            {
                filePath = Application.dataPath + "/Resources/Data/GameData" + index.ToString() + ".json";
            }
            if (File.Exists(filePath))
            {
                string jsonText = File.ReadAllText(filePath);
                Data = JsonConvert.DeserializeObject<GameData>(jsonText);
            }
            else
            {
                Debug.Log(filePath + "�ļ������ڣ�");
            }
            PlayerPlaceCode = "";
        }

        public void SaveData(int index) // intex = 1,2,3, filename = SaveData1.json
        {
            DateTime nowTime = DateTime.Now.ToLocalTime();
            Data.SaveTime = nowTime.ToString("yyyy-MM-dd HH:mm:ss");
            string filePath = Application.dataPath + "/Resources/Data/GameData" + index.ToString() + ".json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            string jsonText = JsonConvert.SerializeObject(Data);
            File.WriteAllText(filePath, jsonText);
            SavedataMgr.Instance.SaveUpdate(index, jsonText);
        }

        // ------------------ Load Scene -----------------
        public List<SceneItem> GetSceneItems(string sceneName)
        {
            foreach (SceneContent _scene in Data.Scenes)
            {
                if (sceneName == _scene.Name)
                {
                    return _scene.SceneItems;
                }
            }
            return null;
        }

        public string GetPlayerPlaceCode()
        {
            return PlayerPlaceCode;
        }
        public void SetPlayerPlaceCode(string code)
        {
            PlayerPlaceCode = code;
        }

        public int GetBeCode()
        {
            return BeCode;
        }
        public void SetBeCode(int code)
        {
            BeCode = code;
        }

    }
}
