using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private TypeValue typeValue;
	private PlayerHealth playerHealth;
	private float BulletAtk = 20;
    
    void Awake()
	{
		typeValue = GameObject.Find ("GameManager").GetComponent<TypeValue> ();
		playerHealth = GameObject.Find("Player").GetComponent <PlayerHealth> ();
       

    }

	void OnCollisionEnter(Collision Target)
	{
		if (Target.transform.tag == "Player")
		{
			if (PlayerHealth.currentHealth > 0) 
			{//當主角的還有血量時
                var damage = (BulletAtk - typeValue.PlayerDef) * Random.Range(0.9f, 1.1f);
                damage = Mathf.RoundToInt(damage);
                playerHealth.Hurt(damage, AttackName: "Shoot");//敵人的攻擊扣掉主角的防禦，然後＊隨機小數點，就是主角要被扣掉的血
			}
		}
	}
}
