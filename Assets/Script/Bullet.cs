using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private TypeValue typeValue;
	private PlayerHealth PlayerHealth;
	private float BulletAtk = 20;

    //audio
    private AudioSource audioSource;
    public AudioClip FreshHit;


    void Awake()
	{
        typeValue = GameObject.Find ("Player").GetComponent<TypeValue> ();
        PlayerHealth = GameObject.Find("Player").GetComponent <PlayerHealth> ();
        audioSource = GetComponent<AudioSource>();

    }

	void OnCollisionEnter(Collision Target)
	{
		if (Target.transform.tag == "Player")
		{
			if (PlayerHealth.currentHealth > 0) 
			{//當主角的還有血量時
                var damage = (BulletAtk - typeValue.PlayerDef) * Random.Range(0.9f, 1.1f);
                damage = Mathf.RoundToInt(damage);
                audioSource.PlayOneShot(FreshHit);
                PlayerHealth.Hurt(damage);//敵人的攻擊扣掉主角的防禦，然後＊隨機小數點，就是主角要被扣掉的血

			}
		}
	}
}
