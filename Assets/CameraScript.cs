using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject PlayerView, PossessTarget;
    public GameObject MoveEnd, PossessEffect,Crosshairs;
    private Quaternion RotationEuler;
    public Vector3 NormalPosition;//鏡頭正常位置
    public Vector3 RedressVector = Vector3.zero;
    public Vector3 Move;//鏡頭"每次"前進/後退的距離
    public Vector3 VectorMoveDistance;//鏡頭總共要前進的距離
    public Vector3 CameraNowPosition;//鏡頭前進完要後退的位置 用來測量要後退多少距離
    public string CameraState;//鏡頭狀態
    public float rotX;
    public float rotY;
    public float sensitivity = 30f;//靈敏度
    public float FowardAndBackTime;//鏡頭前進/後退計時
    public float FowardStop=0.2f; //鏡頭前進的秒數
    public int Frame50 = 50;//FixedUpdate一秒60幀

    // Use this for initialization
    void Start () {
        PossessEffect.SetActive(false);
        Crosshairs.SetActive(false);
        CameraState = "NormalState";
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        CameraRotate();
        NormalPosition = RotationEuler * new Vector3(0, 1, -3f) + PlayerView.transform.position;//鏡頭的位置
        switch (CameraState)
        {
            case "NormalState":
                NormalState();
                    break;
            case "SoulVision":
                SoulVision();
                break;
            case "SoulVisionOver":
                SoulVisionOver();
                break;
            case "GettingPossess":
                GettingPossess();
                break;
        }
        if (Input.GetKey(KeyCode.E))
            CameraState = "SoulVision";
        else if (Input.GetKeyUp(KeyCode.E))
            CameraState = "SoulVisionOver";
    }
    public void CameraRotate()//攝影機旋轉
    {
        //讀取滑鼠的X、Y軸移動訊息
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotX -= Input.GetAxis("joy3") * sensitivity * Time.deltaTime * 5;
        rotY -= Input.GetAxis("joy4") * sensitivity * Time.deltaTime * 5;
        //保證X在360度以內
        if      (rotX > 360)rotX -= 360;
        else if (rotX < 0)  rotX += 360;
        if      (rotY > 45) rotY = 45;
        else if (rotY < -45)rotY =-45;
        //運算攝影機旋轉
        RotationEuler = Quaternion.Euler(rotY, rotX, 0);
        transform.rotation = RotationEuler; //鏡頭轉動
    }
    public void NormalState()
    {
        //鏡頭穿牆處理
        RaycastHit hit;
        if (Physics.Linecast(PlayerView.transform.position, NormalPosition, out hit))
        {
            string HitTag = hit.collider.gameObject.tag;//撞到的物件的tag
            if (HitTag != "MainCamera" && name != "Wolf" && name != "Human" && name != "Player" )
            {
                RedressVector = NormalPosition - hit.point;//如果撞到物件 設一個向量為 撞到的位置和原來鏡頭位置之差
                transform.position = NormalPosition - RedressVector;//減掉位置差 讓鏡頭移動到撞到的位置
                //Debug.DrawLine(PlayerView.transform.position, hit.point, Color.red);
            }
        }
        else
        {
            transform.position = NormalPosition;
            //Debug.DrawLine(PlayerView.transform.position, transform.position, Color.green);
        }
    }
    public void SoulVision()
    {
        if (FowardAndBackTime < FowardStop)
        {
            FowardAndBackTime += Time.deltaTime;
            VectorMoveDistance = (MoveEnd.transform.position - NormalPosition);//距離為終點減正常位置
            Move = VectorMoveDistance / (Frame50* FowardStop);//
            transform.position += Move;
            CameraNowPosition = transform.position;
        }
        else if (FowardAndBackTime >= FowardStop)
        {
            FowardAndBackTime = FowardStop;
            CameraNowPosition = MoveEnd.transform.position;
            transform.position = MoveEnd.transform.position;
            PossessEffect.SetActive(true);
            Crosshairs.SetActive(true);
        }
    }
    public void SoulVisionOver()
    {
        PossessEffect.SetActive(false);
        Crosshairs.SetActive(false);
        if (FowardAndBackTime > 0)
        {
            FowardAndBackTime -= Time.deltaTime;
            VectorMoveDistance = NormalPosition - CameraNowPosition;//距離為正常位置減當前位置
            Move = VectorMoveDistance / (Frame50 * FowardStop);
            transform.position += Move;
        }
        else if (FowardAndBackTime <= 0)
        {
            FowardAndBackTime = 0;
            CameraState = "NormalState";
        } 
    }
    public void GettingPossess()
    {

    }
}
