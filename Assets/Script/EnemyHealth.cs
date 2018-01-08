using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyAI enemyAI;
	public float MaxHealth = 100; //最大HP
	public float currentHealth; //當前HP

	bool isDead;//是否死亡
                //bool damaged;//受到攻擊

    private Animator Anim;

    //audio
    private AudioSource audioSource;
    public AudioClip hurt;

	void Awake()
	{
        enemyAI = GetComponent<EnemyAI>();
        currentHealth = MaxHealth;//開始時，當前ＨＰ回最大ＨＰ
        audioSource = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
	}

	public void Hurt(float Amount)
	{
		if(isDead)// ... no need to take damage so exit the function.
			return;
		//damaged = true;
        audioSource.PlayOneShot(hurt);
        Anim.SetTrigger("Hurt");
		currentHealth -= Amount;//扣血
		if(currentHealth <= 0)
		{
			Death ();
		}
	}

	void Death()
	{
		isDead = true;
        enemyAI.enabled = false;
        Anim.SetBool("Die", isDead);
		Destroy (gameObject, 4f);
	}

}
