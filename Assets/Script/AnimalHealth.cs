using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHealth : MonoBehaviour {

    private PlayerHealth PlayerHealth;
    private PossessedSystem possessedSystem;
    private HPcontroller HPcontroller;
    public float MaxHealth = 100; //最大HP
    public float currentHealth; //當前HP
    private float StorePlayerHealth;
    public static bool CanPossessed;
    //private bool BeLifePossessed = false;
    //private Animator animator;
   
    void Start()
    {
        HPcontroller= GameObject.Find("GameManager").GetComponent<HPcontroller>();
        possessedSystem = GameObject.Find("Pine").GetComponent<PossessedSystem>();
        PlayerHealth = GameObject.Find("Pine").GetComponent<PlayerHealth>();
        currentHealth = MaxHealth;//開始時，當前ＨＰ回最大ＨＰ
       // animator = GetComponent<Animator>();
    }
    void Update()
    {

        if (PossessedSystem.OnPossessed && PossessedSystem.AttachedBody == this.gameObject)
        {
            if (currentHealth < MaxHealth * 0.3f)//當動物血量小於30%，分離主角，並扣出主角原本血量的一半
            {
              
                possessedSystem.LifedPossessed();
                PlayerHealth.currentHealth = StorePlayerHealth * 0.5f;
                //Debug.Log(StorePlayerHealth);
                CanPossessed = false;
                enabled = false;
                HPcontroller.CharacterHpControll();
            }
        }
    }

    public void LinkHP()
    {
        Debug.Log(PossessedSystem.AttachedBody + ":" + currentHealth);
        StorePlayerHealth = PlayerHealth.currentHealth;//當附身後將角色的生命先儲存起來
        PlayerHealth.currentHealth = currentHealth;//讓動物的血條變成角色的血量
        HPcontroller.CharacterHpControll();
        //Debug.Log("PlayerStore:" +StorePlayerHealth);
    }

    public void CancelLink()
    {
        PlayerHealth.currentHealth = StorePlayerHealth;
        //Debug.Log(PossessedSystem.AttachedBody + ":"  + currentHealth);
    }

}
