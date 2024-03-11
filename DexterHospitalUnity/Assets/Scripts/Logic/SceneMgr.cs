using GameDataManagement;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    private GameObject _backgroundObject;
    // 挂载空物体，放在场景文件下
    void Awake()
    {
        _backgroundObject = transform.parent.Find("BackGround").gameObject;

        LoadSceneItems(SceneManager.GetActiveScene().name);
        // load player place
        string PlayerPlaceCode = UserdataMgr.Instance.GetPlayerPlaceCode();
        if (PlayerPlaceCode != "")
        {
            GameObject Player = transform.parent.Find("Player").gameObject;

            GameObject Door = transform.parent.Find("BackGround/" + PlayerPlaceCode).gameObject;
            Vector3 offset = new Vector3(0.17f, 0.44f, 0);
            Player.transform.position = Door.transform.position + offset;
        }
    }


    private void LoadSceneItems(string sceneName)
    {
        List<SceneItem> _sceneItems = UserdataMgr.Instance.GetSceneItems(sceneName);
        if (_sceneItems == null)
            return;
        foreach (SceneItem item in _sceneItems)
        {
            _backgroundObject.transform.Find(item.Name).gameObject.SetActive(item.IsAtivate == 1);
        }
    }
}
