using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttack : MonoBehaviour {
    //call other class
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private TypeValue value;

    //private GameObject gameManager;


    //Animator
    private Animator animator;
   // public float timeBetweenAttacks = 1f;//敵人攻擊的時間間距
    public int HumanAtk = 10;//敵人攻擊力
    public Collider weaponCollider;

    void Awake()
    {
        //set class var
        //set Animator
        animator = GameObject.Find("Human").GetComponent<Animator>();
      
        //set WeaponCollider
        //weaponCollider.enabled = false;
       // Physics.IgnoreCollision(GetComponent<Collider>(), weaponCollider);//讓兩個物體不會產生碰撞
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Attack");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        //animator.SetTrigger ("hit");//EnemyBeAtk
        //Debug.Log("EnemyDamage");
        if (other.tag == "Enemy")
        {
            enemyHealth = other.GetComponent<EnemyHealth>();
           // Debug.Log("Fuck");
            if (enemyHealth.currentHealth > 0)
            {//當Enemy的還有血量時
                var damage = HumanAtk * Random.Range(0.9f, 1.1f);
                damage = Mathf.Round(damage);
                enemyHealth.Hurt(damage);
            }
        }
    }
    
  /*  public void OnAttackTrigger()//避免走路時碰到武器，觸發事件，所以只有攻擊時，才開啟觸發
    {
        weaponCollider.enabled = true;
    }

    public void OnDisableAttackTrigger()
    {
        weaponCollider.enabled = false;

    }*/
}
