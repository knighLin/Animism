using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttack : MonoBehaviour
{
    //call other class
    private EnemyHealth enemyHealth;
    private TypeValue typeValue;

    //Animator
    private Animator animator;
    public int HumanAtk = 10;//攻擊力
    public Collider weaponCollider;
    public Collider myselfCollider;

    //Audio
    private AudioSource audioSource;
    public AudioClip attack;

   

    void Awake()
    {
        //set Animator
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //set WeaponCollider
        //weaponCollider.enabled = false;
        Physics.IgnoreCollision(myselfCollider, weaponCollider);//讓兩個物體不會產生碰撞
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")))//Attack
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("Render", AttackRender());
            audioSource.PlayOneShot(attack);
        }
    }

    int AttackRender()
    {
        int AttackCount = Random.Range(0, 2);
        return AttackCount;
    }
     //void OnAttackTrigger()//避免走路時碰到武器，觸發事件，所以只有攻擊時，才開啟觸發
    //{
    //    weaponCollider.enabled = true;
    //}

    //public void OnDisableAttackTrigger()
    //{
    //    weaponCollider.enabled = false;

    //}

}
