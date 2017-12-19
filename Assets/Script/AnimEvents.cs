using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    private PlayerMovement playerMovent;
    private TypeValue typeValue;
    public delegate void callback(float JumpPower);
    public callback Jump;

    private void Awake()
    {
        typeValue = GetComponent<TypeValue>();
    }

    public void JumpEvent()
    {
        
        //Jump(typeValue.JumpPower);
        Debug.Log("5678");
    }
}
