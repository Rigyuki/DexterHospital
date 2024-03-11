using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class changeTextColor : MonoBehaviour
{
    public TMP_Text text;
    public GameObject emissImg;
    //void Update()
    //{
    //   OnMouseOver();
    //}

    private void Start()
    {
        emissImg.SetActive(false);
    }

    public void OnMouseOver()
    {
        Debug.Log("当鼠标在物体上的时候，才会触发该事件函数");
        text.color = Color.red;
        emissImg.SetActive(true);
    }

    //public void OnMouseEnter()
    //{
    //    Debug.Log("当鼠标移动到该物体上的时候（瞬间），才会触发该事件函数");
    //}

    public void OnMouseExit()
    {
        Debug.Log("当鼠标移出该物体的时候（瞬间），才会触发该事件函数");
        text.color = Color.white;
        emissImg.SetActive(false);
    }

}
