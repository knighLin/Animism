using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessCameraMove : MonoBehaviour {

    public Transform target;//跟隨目標
    public float rotX;
    public float rotY;
    public float sensitivity = 50f;//靈敏度
    private Quaternion rotationEuler;
    private Vector3 cameraPosition;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //讀取滑鼠的X、Y軸移動訊息
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime*2;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime*2;

        rotX += Input.GetAxis("joy3") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("joy4") * sensitivity * Time.deltaTime;


        //保證X在360度以內
        if (rotX > 360)
        {
            rotX -= 360;
        }
        else if (rotX < 0)
        {
            rotX += 360;
        }
        if (rotY > 35)
        {
            rotY = 35;
        }
        else if (rotY < -20)
        {
            rotY = -20;
        }

       
        //限制距離
       

        //運算攝影機座標、旋轉
        rotationEuler = Quaternion.Euler(rotY, rotX, 0) * Quaternion.Euler(23, 0, 0);
        cameraPosition = rotationEuler * new Vector3(0, 0,0)+target.position; ;

        //應用
        transform.rotation = rotationEuler;
       
        //target.transform.localRotation = Quaternion.AngleAxis(rotX, target.transform.up);//人物轉向 但ASD不能用
        transform.position = cameraPosition;
      
        }
 }

