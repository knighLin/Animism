using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHealth : MonoBehaviour {

   
    private PossessedSystem possessedSystem;

    public float MaxHealth = 100; //最大HP
    public float currentHealth; //當前HP
    private float StorePlayerHealth;
    public static bool CanPossessed;
    //private bool BeLifePossessed = false;
    //private Animator animator;
    private lightSwitch lightSwitch;
    void Awake()
    {
        possessedSystem = GameObject.Find("Player").GetComponent<PossessedSystem>();
        currentHealth = MaxHealth;//開始時，當前ＨＰ回最大ＨＰ
       // animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (PossessedSystem.OnPossessed && PossessedSystem.AttachedBody == this.gameObject)
        {
            lightSwitch = GameObject.FindGameObjectWithTag("light").GetComponent<lightSwitch>();

            if (currentHealth <= MaxHealth && currentHealth >= MaxHealth * 0.7f)//green
            {
                lightSwitch.SetHP(1);           
            }
            else if (currentHealth < MaxHealth * 0.7f && currentHealth >= MaxHealth * 0.4f)//yellow
            {
                lightSwitch.SetHP(0.5f);
            }
            else if (currentHealth < MaxHealth * 0.4f && currentHealth >= MaxHealth * 0.3f)//red
            {
                lightSwitch.SetHP(0);
            }
            else if (currentHealth < MaxHealth * 0.3f)//當動物血量小於30%，分離主角，並扣出主角原本血量的一半
            {
                possessedSystem.LifedPossessed();
                PlayerHealth.currentHealth = StorePlayerHealth * 0.5f;
                //Debug.Log(StorePlayerHealth);
                CanPossessed = false;
                enabled = false;
            }
        }
    }

    public void LinkHP()
    {
        Debug.Log(PossessedSystem.AttachedBody + ":" + currentHealth);
        StorePlayerHealth = PlayerHealth.currentHealth;//當附身後將角色的生命先儲存起來
        PlayerHealth.currentHealth = currentHealth;//讓動物的血條變成角色的血量
        Debug.Log("PlayerStore:" +StorePlayerHealth);
    }

    public void CancelLink()
    {
        PlayerHealth.currentHealth = StorePlayerHealth;
        Debug.Log(PossessedSystem.AttachedBody + ":"  + currentHealth);
    }

}
