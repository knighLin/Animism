using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour {

    private TypeValue typeValue;
    public delegate void callback(float JumpPower);
    public callback aaa;

    private void Awake()
    {
        typeValue = GetComponent<TypeValue>();
    }

    void JumpEvent()
    {
        print("123");
        if(aaa != null){
            print("fgjhkl;");
            aaa(typeValue.JumpPower);
        }
        else{
            print("5678");
        }
            

    }
}
