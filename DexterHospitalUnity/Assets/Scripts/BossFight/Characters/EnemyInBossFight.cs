using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyInBossFight : Character
{
    //public Image hp;
   // new float health;

    [SerializeField] Button attack;
    [SerializeField] Button delay;
    [SerializeField] Button mail;


   // public Text AmountText;
   // public int defaultAmount;//实际是你之前获取的道具的数量（后面再改
    //int amount;
    //public Image cooldownImage;

   // public float cooldownTime = 1f;
    //bool isReady = true;
    private void Update()
    {
        //AmountText.text = defaultAmount.ToString();
        //hp.fillAmount -= health / 100;
    }

    private void Awake()
    {
        //amount = defaultAmount;
    }
    private void Start()
    {
       // hp.fillAmount = 1;
        //UpdateAmountText(amount);
        //AmountText.text = defaultAmount.ToString();

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
       // health = damage;
        
    }

    
   /* public void OnAttack()
    {
        TakeDamage(50f);
        if (amount == 0||!isReady) return;
        isReady = false;


        amount--;
        UpdateAmountText(amount);//在这里调用的
        Debug.Log("defaultAmount:" + amount);
        if (amount == 0)
        {
            UpdateCooldownImage(1f);
        }
        else
        {
            StartCoroutine(CooldownCoroutine());
        }
        //attackLogic();
        

    }*/

    /*IEnumerator CooldownCoroutine()
    {
        var cooldownValue = cooldownTime;
        while(cooldownTime > 0f)
        {
            UpdateCooldownImage(cooldownValue / cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime,0f);
            yield return null;
        }

       // yield return new WaitForSeconds(cooldownTime);
        isReady = true;
    }*/

    /*void UpdateAmountText(int amount)
    {
        AmountText.text = amount.ToString();
    }*/
    //void UpdateAmountText(int amount) => AmountText.text = amount.ToString();
    //void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
    /*void UpdateCooldownImage(float fillAmount) 
    {
        cooldownImage.fillAmount = fillAmount;
        Debug.Log(fillAmount);
    } */ 

    /*public void attackLogic()
    {
        TakeDamage(50f);
        if (defaultAmount == 0 || !isre)
        {
            cooldownImage.fillAmount = 0f;
        }
        Debug.Log("you click this button");
    }*/

}
