using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour {

    private PlayerMovement playerMovement;//角色的移動
   // private PossessedSystem possessedSystem;
	public float MaxHealth = 100; //最大HP
    [SerializeField]
	public static float currentHealth; //當前HP
	
	private Animator animator;
	private bool isDead;//是否死亡
    [SerializeField]
    private HPUI HP;

    void Awake()
	{
        playerMovement = GetComponent<PlayerMovement>();
       // possessedSystem = GetComponent<PossessedSystem>();
		currentHealth = MaxHealth;//開始時，當前ＨＰ回最大ＨＰ
        
        animator = GameObject.FindWithTag ("Human").GetComponent<Animator> ();
    }
    void Start()
    {
        HP = GameObject.FindGameObjectWithTag("HP").GetComponent<HPUI>();
        HP.SetHumanHP(currentHealth);
    }



    public void Hurt(float Amount, String AttackName)
	{
        if (AttackName == "Shoot")
        {
            animator.SetTrigger("ShootHit");
        }
        else
        {
        }

       if(PossessedSystem.OnPossessed)//如果附身，扣動物血量
        {
            PossessedSystem.AttachedBody.GetComponent<AnimalHealth>().currentHealth -= Amount;
        }
        else
        {
            currentHealth -= Amount;//扣血
        }

       // Debug.Log(currentHealth+","+Amount);
		//damaged = true;

		if(currentHealth <= 0 && !isDead)
		{
			Death ();
		}
       // Debug.Log(currentHealth);
        HP.SetHumanHP(currentHealth);
	}

	void Death()
	{
		isDead = true;
		playerMovement.enabled = false;
		animator.enabled = false;//After Change DeadAnimation
		Destroy(gameObject,4f);
	}

}
