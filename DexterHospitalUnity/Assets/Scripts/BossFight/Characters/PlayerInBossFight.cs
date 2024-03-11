using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameDataManagement;
using UnityEngine.SceneManagement;

public class PlayerInBossFight : Character
{
    [SerializeField] Image heart1;
    [SerializeField] Image heart2;
    [SerializeField] Image heart3;
    [SerializeField] Image heart4;
    [SerializeField] Image heart5;   

    private SceneFadeMgr _secneFadeMgr;

    [SerializeField] bool regenerateHealth = false;//开关闭，方便启用或者禁用玩家血量再生的功能
    [SerializeField] float healthRegenerateTime;
    [SerializeField, Range(0f, 1f)] float healthRegeneratePercent;
    WaitForSeconds waitHealthRegenerateTime;//等待生命值再生时间
    Coroutine healthRegenerateCouroutine;
    private void Start()
    {
        //玩家受伤之后开启生命值再生
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);

        _secneFadeMgr = transform.parent.Find("SceneFadeMgr").GetComponent<SceneFadeMgr>();
    }

    private void Update()
    {
         
    }

    private void Awake()
    {
         
    }



    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //playerDamageAudio.Play();

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (healthRegenerateCouroutine != null)
                {
                    StopCoroutine(healthRegenerateCouroutine);
                }

                healthRegenerateCouroutine = StartCoroutine(HealthRegenerateCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }

        switch (health)
        {
            case 5:
                heart5.gameObject.SetActive(true);
                heart4.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                break;
            case 4:
                heart5.gameObject.SetActive(false);
                heart4.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                break;
            case 3:
                heart5.gameObject.SetActive(false);
                heart4.gameObject.SetActive(false);
                heart3.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                break;
            case 2:
                heart5.gameObject.SetActive(false);
                heart4.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                heart2.gameObject.SetActive(true);
                heart1.gameObject.SetActive(true);
                break;
            case 1:
                heart5.gameObject.SetActive(false);
                heart4.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart1.gameObject.SetActive(true);
                break;
            case 0:
                //StartCoroutine(_secneFadeMgr.LoadScene("BE"));
                UserdataMgr.Instance.SetBeCode(2);
                SceneManager.LoadScene("BE");

                break;
            default:
                break;
        }
    }

    public override void Die()
    {
        base.Die();

        heart1.gameObject.SetActive(false);
              
    }
  
}
