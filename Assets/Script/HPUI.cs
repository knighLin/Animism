using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUI : MonoBehaviour {

    [SerializeField]
    private Animator animator;


    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void SetHumanHP(float hp)
    {
  
        animator.SetFloat("HP", hp/20);
      

    }
}
