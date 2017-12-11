using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnaround : MonoBehaviour {
    
    public float x;
    public float xAdd;
    public float xSpeed = 120;//x靈敏度
    public Vector3  playerDir;



    private Quaternion rotationEuler;
    
    

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        //讀取滑鼠的X、Y軸移動訊息  
        xAdd = Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        ;
        x += xAdd;
      
        //保證X在360度以內
        if (x > 360)
        {
            x -= 360;
        }
        else if (x < 0)
        {
            x += 360;
        }

        playerDir = this.gameObject.transform.forward;



        //應用
        rotationEuler = Quaternion.Euler(0 , x, 0);

        transform.rotation = rotationEuler;

    }
}
