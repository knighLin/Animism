using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Animator Anim;

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && PossessedSystem.AttachedBody == this.gameObject)
        {
            Anim.SetTrigger("Attack");
        }   
    }

    void SpecialAttack()
    {
        if (Input.GetMouseButtonDown(1) && PossessedSystem.AttachedBody == this.gameObject)
        {
            
        }
    }
}
