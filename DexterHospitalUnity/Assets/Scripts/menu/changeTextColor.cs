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
        Debug.Log("������������ϵ�ʱ�򣬲Żᴥ�����¼�����");
        text.color = Color.red;
        emissImg.SetActive(true);
    }

    //public void OnMouseEnter()
    //{
    //    Debug.Log("������ƶ����������ϵ�ʱ��˲�䣩���Żᴥ�����¼�����");
    //}

    public void OnMouseExit()
    {
        Debug.Log("������Ƴ��������ʱ��˲�䣩���Żᴥ�����¼�����");
        text.color = Color.white;
        emissImg.SetActive(false);
    }

}
