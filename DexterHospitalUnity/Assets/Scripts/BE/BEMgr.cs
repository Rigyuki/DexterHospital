using GameDataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BEMgr : MonoBehaviour
{
    public static BEMgr Instance { get; private set; }
    public BEData BEDatas;
    public int _BEIndex;
    string _contentTotal;
    public Image _imageBg;
    public Button _buttonBack;

    public AudioSource _musicAudio;

    private void Awake()
    {
        Instance = this;

        _BEIndex = UserdataMgr.Instance.GetBeCode();

        BEDatas = new BEData();
        HanldeCSV(_BEIndex);//获取csv剧本数据

        HandleScript();
        _buttonBack.onClick.AddListener(onBtnBack);
    }

    public void onBtnBack()
    {
        SceneManager.LoadScene(0);
    }

    public void HanldeCSV(int _BEIndex)
    {
        string path = Application.dataPath + "/txt/BE.csv";
        Debug.Log("读取BEcsv");
        _contentTotal = getCsvfile(path);
        string[] _content = _contentTotal.Split("/n");

        if (_BEIndex < _content.Length - 1)
        {
            string[] _BErows = _content[_BEIndex].Split(",");
            BEDatas._spriteBGName = _BErows[1];
            BEDatas._soundName = _BErows[3];
        }

    }

    //handle script
    private void HandleScript()
    {
        Debug.Log("soudname = " + BEDatas._soundName);
        PlayMusic(BEDatas._soundName);

        Debug.Log("bg = " + BEDatas._spriteBGName);
        setBGImage(BEDatas._spriteBGName);
    }

    public void setBGImage(string _spriteBGName)
    {
        _imageBg.sprite = Resources.Load<Sprite>("Sprites/BE/" + _spriteBGName);
    }


    public void PlayMusic(string _musicName, bool loop = true)
    {
        Debug.Log("BGM path = " + "Audio/BGM" + _musicName);
        _musicAudio.loop = loop;
        _musicAudio.clip = Resources.Load<AudioClip>("Audio/BGM/" + _musicName);

        _musicAudio.Play();
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

//场景数据，背景，人物，名字，地址，对话内容
public class BEData
{
    public string _spriteBGName; //path 
    public string _filename;
    public int BE_index;

    public int _soundType;//1.对话 2.音效 3.音乐
    public string _soundName;

}
