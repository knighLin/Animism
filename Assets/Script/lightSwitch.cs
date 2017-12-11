using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSwitch : MonoBehaviour {
    private Animator animator;
   

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        
    }
    public void SetHP(float hp)
    {
        animator.SetFloat("hp", hp);
    }
   
}
