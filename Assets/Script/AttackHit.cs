using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour {

    private EnemyHealth enemyHealth;
    private TypeValue typeValue;
    private float timer = 0;//攻擊時間


    void Awake()
    {
        typeValue = GameObject.FindWithTag("Player").GetComponent<TypeValue>();
    }
    void OnTriggerEnter(Collider other)//當進入範圍的物件為主角///只後要做範圍的距離判斷，改變敵人攻擊方式
    {
        //animator.SetTrigger ("hit");//EnemyBeAtk
        Debug.Log("EnemyDamage");
        enemyHealth = other.GetComponent<EnemyHealth>();
        if (other.tag == "Enemy")
        {

            timer += Time.deltaTime;
            //Debug.Log (timer);
            if (timer >= 1f && enemyHealth.currentHealth > 0)
            {
                var damage = typeValue.PlayerAtk * Random.Range(0.9f, 1.1f);
                damage = Mathf.Round(damage);
                enemyHealth.Hurt(damage);//敵人的攻擊扣掉主角的防禦，然後＊隨機小數點，就是主角要被扣掉的血
                timer = 0;
            }

        }

    }


}
