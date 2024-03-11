using GameDataManagement;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiagalMgr : MonoBehaviour
{
    public static DiagalMgr Instance { get; private set; }
    public List<ScriptData> scriptDatas;
    private int _scriptIndex;
    private Scene _scene;
    private Button _btnNext;
    private Button _btnOption;

    string _contentTotal;

    private void Awake()
    {
        Instance = this;
        _scene = SceneManager.GetActiveScene();

        scriptDatas = new List<ScriptData>();
        HanldeCSV();//��ȡcsv�籾����

        HandleScript();//����ִ��ÿһ�����������
        _scriptIndex = 0;

        for (int i = 0; i < scriptDatas.Count; i++)
        {
            scriptDatas[i].scriptIndex = i;
        }

        _btnNext = DiagalUIMgr.Instance.getButton();
        _btnNext.onClick.AddListener(OnBtnNextClick);

        _btnOption = DiagalUIMgr.Instance.getOptionButton();
        _btnOption.onClick.AddListener(OnBtnOptionClick);
    }
    private void OnBtnNextClick()
    {
        LoadNextScript();
    }

    private void OnBtnOptionClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void HanldeCSV()
    {
        string path = Application.dataPath + "/Resources/txt/gal/" + _scene.name + ".csv";
        //Debug.Log(path);
        Debug.Log("��ȡ�籾csv" + "= " + path);
        _contentTotal = getCsvfile(path);
        string[] _content = _contentTotal.Split("/n");

        for (int i = 1; i < _content.Length - 1; i++)
        {
            //Debug.Log("_content = " + i + " " + _content[i]);
            string[] rows = _content[i].Split(",");

            if (rows[0] == "start")
            {
                //Debug.Log("bg = " + rows[2]);
                //Debug.Log("_content = " + rows[4]);
                scriptDatas.Add(new ScriptData()
                {
                    loadType = 1,
                    _spriteBGName = rows[3],
                    _soundType = 3,
                    _content = rows[6],
                    _soundName = rows[9],

                });
            }
            else if (rows[0] == "dialog")
            {
                if (rows[4] != "" || rows[5] != "")
                {
                    scriptDatas.Add(new ScriptData()
                    {
                        loadType = 2,
                        _npcTextName = rows[2],
                        _spriteBGName = rows[3],
                        _npcName1 = rows[4],
                        _npcName2 = rows[5],
                        _content = rows[6],
                    });
                }


                if (rows[4] == "" && rows[5] == "")
                {
                    scriptDatas.Add(new ScriptData()
                    {
                        loadType = 1,
                        _spriteBGName = rows[3],
                        _content = rows[6],
                    });
                }
            }
            else if (rows[0] == "optionMgr")
            {
                Debug.Log("option content ***" + rows[6]);
                scriptDatas.Add(new ScriptData()
                {
                    loadType = 3,
                    _spriteBGName = rows[3],
                    _eventID = int.Parse(rows[1]),
                    _eventMark = int.Parse(rows[2]),
                });;
            }

            else if (rows[0] == "option")
            {
                //Debug.Log("option content ***" + rows[6]);
                scriptDatas.Add(new ScriptData()
                {
                    loadType = 3,
                    _eventID = int.Parse(rows[1]),
                    _eventMark = int.Parse(rows[2]),
                    _spriteBGName = rows[2],
                    _content = rows[6],
                });
            }

            else if (rows[0] == "image")
            {
                Debug.Log("image content ***" + rows[6]);
                scriptDatas.Add(new ScriptData()
                {
                    loadType = 4,
                    _spriteBGName = rows[3],
                    _filename = rows[4],
                    //_content = rows[6],
                });
            }

            else if (rows[0] == "AnimaRole")
            {
                Debug.Log("animarole content ***" + rows[3]);
                scriptDatas.Add(new ScriptData()
                {
                    loadType = 5,
                    _filename = rows[4],
                    _spriteBGName = rows[3],
                    //_content = rows[6],
                });
            }

            else if (rows[0] == "Anima")
            {
                Debug.Log("anima content ***" + rows[3]);
                scriptDatas.Add(new ScriptData()
                {
                    loadType = 6,
                    _filename = rows[3],
                    //_content = rows[6],
                });
            }
            else if (rows[0] == "end")
            {
                scriptDatas.Add(new ScriptData()
                {
                    loadType = 999,
                });
                Debug.Log("���������������");
            }
        }
    }

    //handle script
    private void HandleScript()
    {
        if (_scriptIndex >= scriptDatas.Count)
        {
            Debug.Log("scene end");
            return;
        }

        PlaySound(scriptDatas[_scriptIndex]._soundType);

        if (scriptDatas[_scriptIndex].loadType == 1)
        {
            //load bg 
            Debug.Log("only bg" + scriptDatas[_scriptIndex]._content);
            setBGImageSpriteOnly(scriptDatas[_scriptIndex]._spriteBGName);
            updateText(scriptDatas[_scriptIndex]._content);
        }
        else if (scriptDatas[_scriptIndex].loadType == 2)
        {
            Debug.Log("handle npc");
            //show sprites 
            //show content
            HandleNpc();
        }
        else if (scriptDatas[_scriptIndex].loadType == 3)
        {
            Debug.Log("handle option");
            switch (scriptDatas[_scriptIndex]._eventID)
            {
        //        //��ʾѡ����
                case 1:
                    ShowChoiceUI(scriptDatas[_scriptIndex]._eventMark, GetChoiceContent
                        (scriptDatas[_scriptIndex]._eventMark));
                    break;
                //��ת�����λ�þ籾
                case 2:
                    SetScriptIndex();
                    break;
                default:
                    break;
            }
        }

        else if (scriptDatas[_scriptIndex].loadType == 4)
        {
            Debug.Log("handle image");
            setBgImageAndFile(scriptDatas[_scriptIndex]._spriteBGName, scriptDatas[_scriptIndex]._filename);

        }

        else if (scriptDatas[_scriptIndex].loadType == 5)
        {
            Debug.Log("handle role image");
            setRoleAnimate(scriptDatas[_scriptIndex]._spriteBGName, scriptDatas[_scriptIndex]._filename);

        }

        else if (scriptDatas[_scriptIndex].loadType == 6)
        {
            Debug.Log("handle Anima video");
            setVideo(scriptDatas[_scriptIndex]._filename);

        }
        else if (scriptDatas[_scriptIndex].loadType == 999)
        {
            Debug.Log("��ת����");
           
            //StartCoroutine(sceneFade.LoadSceneNext());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void HandleNpc()
    {
        ShowCharacter(scriptDatas[_scriptIndex]._spriteBGName, scriptDatas[_scriptIndex]._npcName1, scriptDatas[_scriptIndex]._npcName2, scriptDatas[_scriptIndex]._npcTextName);
        updateText(scriptDatas[_scriptIndex]._content);
    }

    private void setBGImageSpriteOnly(string spriteBgName)
    {
        DiagalUIMgr.Instance.setBGImageSpriteOnly(spriteBgName);
    }

    private void setVideo(string _videoName)
    {
        DiagalUIMgr.Instance.setvideo(_videoName);
    }

    private void setRoleAnimate(string spriteBgName, string _imageName)
    {
        DiagalUIMgr.Instance.setRoleAnimate(spriteBgName, _imageName);
    }

    private void setFileImage(string _imageName)
    {
        DiagalUIMgr.Instance.setFileImage(_imageName);
    }

    private void setBgImageAndFile(string spriteBgName, string imageName)
    {
        DiagalUIMgr.Instance.setBgImageAndFile(spriteBgName, imageName);
    }


    public void SetScriptIndex(int index = 0)
    {
        for (int i = 0; i < scriptDatas.Count; i++)
        {
            if (scriptDatas[_scriptIndex + index]._eventMark == scriptDatas[i].scriptID)
            {
                _scriptIndex = scriptDatas[i].scriptIndex;
                break;
            }
        }
        HandleScript();
    }

    public void LoadNextScript()
    {
        //Debug.Log("load next script");
        _scriptIndex++;
        HandleScript();
    }

    private void ShowCharacter(string spriteBgName, string _NPCname1, string _NPCname2,string _npcTextName)
    {
        DiagalUIMgr.Instance.setCharacterUI(spriteBgName, _NPCname1, _NPCname2, _npcTextName);
    }

    private void updateText(string content)
    {
        DiagalUIMgr.Instance.UpdateDialogContent(content);
    }
    public void PlaySound(int soundType)
    {
        switch (soundType)
        {
            case 1:
                AudioMgr.Instance.playDialogue(
                    scriptDatas[_scriptIndex]._npcName1 + "/" + scriptDatas[_scriptIndex]._soundName);
                break;
            case 2:
                AudioMgr.Instance.PlayEffect(
                    scriptDatas[_scriptIndex]._soundName);
                break;
            case 3:
                AudioMgr.Instance.PlayMusic(
                    scriptDatas[_scriptIndex]._soundName);
                break;
            default:
                break;
        }
    }


    public void ShowChoiceUI(int choiceNum, string[] choiceContent)
    {
        Debug.Log("ShowchoiceUI");
        DiagalUIMgr.Instance.ShowChoiceUI(choiceNum, choiceContent);
    }

    public string[] GetChoiceContent(int num)
    {
        string[] choiceContent = new string[num];
        for (int i = 0; i < num; i++)
        {
            choiceContent[i] = scriptDatas[_scriptIndex + 1 + i]._content;
        }
        return choiceContent;
    }

    public string getCsvfile(string fullpath)
    {
        string readedLines = "";
        Debug.Log("getcsv function");
        Debug.Log(fullpath);
        Encoding utf = Encoding.GetEncoding("UTF-8");

        if (!File.Exists(fullpath))
        {
            Debug.Log("File non-existant...");
        }
        else
        {
            Debug.Log("It Exists...");
            StreamReader reader = new StreamReader(fullpath, utf);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                //Debug.Log(line);
                readedLines += line;
                readedLines += "/n";
            }
            reader.Close();
        }
        return readedLines;

    }
}

//�������ݣ�������������֣���ַ���Ի�����
public class ScriptData
{
    public int loadType;//1.background 2.character 3.option


    public string _npcTextName;//npc text name
    public string _npcName1;//npc sprite nama
    public string _npcName2;//npc sprite nama
    public int _characterID;//

    public string _spriteBGName; //path 
    public string _content;//dialog content
    public string _filename;


    public int _soundType;//1.�Ի� 2.��Ч 3.����
    public string _soundName;

    //1.��ʾѡ���� 2.��ת��ָ���籾λ��
    public int _eventID;
    //�¼�����
    //1.����ѡ�� 2.����Ҫ��ת���ı��λ 
    public int _eventMark;
    //�籾���λ��������ת
    public int scriptID;
    //�籾����
    public int scriptIndex;
}
