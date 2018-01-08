using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttack : MonoBehaviour {
    //call other class
  

    //Animator
    private Animator animator;
    public int HumanAtk = 10;//敵人攻擊力
    public Collider weaponCollider;
    public Collider myselfCollider;

    //Audio
    private AudioSource audioSource;
    public AudioClip attack;

    private float timer = 0;//攻擊時間
    void Awake()
    {
        //set class var
        //set Animator
        animator = GetComponent<Animator>();
       
        audioSource = GetComponent<AudioSource>();
        //set WeaponCollider
        weaponCollider.enabled = false;
        Physics.IgnoreCollision(myselfCollider, weaponCollider);//讓兩個物體不會產生碰撞
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && Time.time - timer >1)//Attack
        {
            StartCoroutine(DamageTime());



        }
    }

    int AttackRender()
    {
        int AttackCount = Random.Range(0, 2);
        return AttackCount;
    }

    IEnumerator DamageTime()//攻擊只傷害一次，之後要問老師怎麼改比較好
    {
        yield return new WaitForSeconds(0.2f);
        weaponCollider.enabled = true;
        animator.SetTrigger("Attack");
        animator.SetInteger("Render", AttackRender());
        audioSource.PlayOneShot(attack);
        timer = Time.time;
        yield return new WaitForSeconds(1f);
        weaponCollider.enabled = false;
        StopCoroutine(DamageTime());
    }
   //public void OnAttackTrigger()//避免走路時碰到武器，觸發事件，所以只有攻擊時，才開啟觸發
    //{
    //    weaponCollider.enabled = true;
    //}

    //public void OnDisableAttackTrigger()
    //{
    //    weaponCollider.enabled = false;

    //}
}
