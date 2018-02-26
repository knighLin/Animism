using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private PossessedSystem PossessedSystem;
    private PlayerManager playerManager;
    public GameObject PossessTarget;
    public GameObject PossessEffect, Crosshairs;
    public GameObject NowCharacter;
    public GameObject MoveEnd, PlayerView;
    public Transform[] AttachedBodyChildren;
    public Quaternion RotationEuler;
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
    public float FowardStop = 0.2f; //鏡頭前進的秒數
    public float PossessTime; //附身鏡頭計時
    public float PossessStop = 2; //附身鏡頭的秒數
    public bool CanPossess = false;//靈視狀態下才會為true 代表可以附身;
    public bool IsPossessing = false;//附身中為true直到附身結束切回正常狀態才會被重置為false

    // Use this for initialization
    void Start()
    {
        PossessedSystem = GameObject.Find("Pine").GetComponent<PossessedSystem>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        PossessEffect.SetActive(false);//開始時靈視關閉
        Crosshairs.SetActive(false);//開始時準心關閉
        CameraState = "NormalState";//初始狀態為正常狀態
        AttachedBodyChildren = new Transform[3];//只抓前四個物件(包含本身)
        PlayerView = GameObject.Find("FirstPersonCamPoint");
        MoveEnd = GameObject.Find("CamMoveEndPoint");//一開始取正確腳色位置
        NowCharacter = GameObject.Find("Pine");
        //NormalPosition = GameObject.Find("Pine").transform.rotation * new Vector3(0, 1, -3f) + PlayerView.transform.position;//一開始取得讀取角度
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsPossessing) CameraRotate();//如果不是附身模式讓鏡頭可以轉
        NormalPosition = RotationEuler * new Vector3(0, 1, -3f) + PlayerView.transform.position;//每幀確認鏡頭正常的位置 讓前進後退順暢
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

        if (Input.GetKey(KeyCode.E) && !IsPossessing)//持續按著靈視鍵可以進入靈視 但附身過程或附身後還按著無效(IsPossessing為true)
            CameraState = "SoulVision";

    }
    public void ResetValue()//重置一些前進後退中用到的值 以防下次進入其他模式出問題
    {
        CanPossess = false;//不能附身
        IsPossessing = false;//可以進入靈視
        FowardAndBackTime = 0;//前進後退的計時為0
        PossessTime = 0;//前進後退的計時為0
    }
    public void CameraRotate()//攝影機旋轉
    {
        if (Time.timeScale == 1)
        {
            //讀取滑鼠的X、Y軸移動訊息
            rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            rotX -= Input.GetAxis("joy3") * sensitivity * Time.deltaTime * 5;
            rotY -= Input.GetAxis("joy4") * sensitivity * Time.deltaTime * 5;
        }
        else if (Time.timeScale == 0.5f)
        {
            //讀取滑鼠的X、Y軸移動訊息
            rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * 2;
            rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * 2;
            rotX -= Input.GetAxis("joy3") * sensitivity * Time.deltaTime * 10;
            rotY -= Input.GetAxis("joy4") * sensitivity * Time.deltaTime * 10;
        }
        //保證X在360度以內
        if (rotX > 360) rotX -= 360;
        else if (rotX < 0) rotX += 360;
        if (rotY > 45) rotY = 45;
        else if (rotY < -45) rotY = -45;
        //運算攝影機旋轉
        RotationEuler = Quaternion.Euler(rotY, rotX, 0);
        transform.rotation = RotationEuler; //鏡頭轉動
    }
    public void NormalState()
    {
        ResetValue();//只要進入正常模式 就重置一些參數
        //鏡頭穿牆處理
        RaycastHit hit;
        if (Physics.Linecast(PlayerView.transform.position, NormalPosition, out hit))
        {
            int HitTag = hit.collider.gameObject.layer;//撞到的物件的layer
            if (HitTag != 9 && HitTag != 11&&HitTag!=8)//9為player 11為ragdoll 8為CanPossess
            {
                RedressVector = NormalPosition - hit.point;//如果撞到物件 設一個向量為 撞到的位置和原來鏡頭位置之差
                transform.position = NormalPosition - RedressVector;//減掉位置差 讓鏡頭移動到撞到的位置 其實值等於 hit.point即可 只是變數留著可以做變化 先不做優化
                //Debug.DrawLine(PlayerView.transform.position, hit.point, Color.red);
            }
            else
            {
                transform.position = NormalPosition;//如果撞到主角身上物件 座標為正常位置
            }
        }
        else
        {
            transform.position = NormalPosition;//如果沒撞到物件 座標為正常位置
            //Debug.DrawLine(PlayerView.transform.position, transform.position, Color.green);
        }
    }
    public void SoulVision()//鏡頭前進為靈視狀態
    {
        if (!Input.GetKey(KeyCode.E) && !IsPossessing)//只要在靈視狀態下放開靈視鍵則退出靈視
            CameraState = "SoulVisionOver";
        if (FowardAndBackTime < FowardStop)//0.25秒移動到到指定位置
        {
            FowardAndBackTime += Time.deltaTime;
            VectorMoveDistance = MoveEnd.transform.position - NormalPosition;//距離為終點減正常位置
            Move = VectorMoveDistance * Time.deltaTime * 5;
            transform.position += Move;
            CameraNowPosition = transform.position;
        }
        else if (FowardAndBackTime >= FowardStop)//到指定位置後開啟靈視效果和準心 並可以進入附身
        {
            NowCharacter.transform.rotation = Quaternion.Euler(0, rotX, 0);//靈視狀態下腳色轉動
            FowardAndBackTime = FowardStop;
            CameraNowPosition = MoveEnd.transform.position;
            transform.position = MoveEnd.transform.position;
            PossessEffect.SetActive(true);
            Crosshairs.SetActive(true);
            CanPossess = true;
        }
    }
    public void SoulVisionOver()//鏡頭後退為正常狀態
    {
        PossessEffect.SetActive(false);
        Crosshairs.SetActive(false);
        CanPossess = false;
        if (FowardAndBackTime > 0)//從移動到的位置退回正常位置
        {
            FowardAndBackTime -= Time.deltaTime;
            VectorMoveDistance = NormalPosition - CameraNowPosition;//距離為正常位置減當前位置
            Move = VectorMoveDistance * Time.deltaTime * 5;
            transform.position += Move;
        }
        else if (FowardAndBackTime <= 0)
        {
            CameraState = "NormalState";
        }
    }
    public void GettingPossess()
    {
        if (PossessTime < 0.2)//鏡頭回到正常的位置
        {
            PossessTime += Time.deltaTime;
            VectorMoveDistance = NormalPosition - transform.position;
            Move = VectorMoveDistance * Time.deltaTime * 5;
            transform.position += Move;
        }
        else if (PossessTime >= 0.2 && PossessTime < 0.8)
        {

            PossessTime += Time.deltaTime;

            if (PossessTime >= 0.6 && PossessTime < 0.8)
            {
                VectorMoveDistance = PossessTarget.transform.position - NormalPosition;//距離為終點減正常位置
                Move = VectorMoveDistance * Time.deltaTime * 3;
                transform.position += Move;
                CameraNowPosition = transform.position;
            }
        }
        else if (PossessTime >= 0.8)
        {
            PossessedSystem.InToPossess();
            LoadCharacterPosition();//讀取動物鏡頭位置
            CameraState = "NormalState";
        }
    }
    public void LoadCharacterPosition()
    {
        switch (playerManager.PossessType)
        {
            case "Human":
                NowCharacter = GameObject.Find("Pine");
                PlayerView = GameObject.Find("FirstPersonCamPoint");
                MoveEnd = GameObject.Find("CamMoveEndPoint");
                break;
            case "WolfMaster":
                NowCharacter = PossessTarget;
                AttachedBodyChildren = PossessedSystem.AttachedBody.GetComponentsInChildren<Transform>();
                PlayerView = AttachedBodyChildren[2].transform.gameObject;
                MoveEnd = AttachedBodyChildren[1].transform.gameObject;
                break;

        }
    }
}
