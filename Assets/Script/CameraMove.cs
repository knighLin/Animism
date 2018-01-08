using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CameraMove : MonoBehaviour
{

    public GameObject Blood;
    public GameObject Crosshairs;
    public GameObject possessStart, possessEnd, PossessEffect;
    // private PossessedSystem PossessedSystem;
    private PlayerManager PlayerManager;
    private Quaternion rotationEuler;
    public Transform target;//跟隨目標
    public Transform ray;//射線起始位置(FirstPersonCamPoint)
    private Vector3 cameraPosition;
    public Vector3 raydistance;
    public Vector3 startPoint;
    public Vector3 effectdistance;
    public Vector3 move;
    public Vector3 possessingCamera;
    public float rotX;
    public float rotY;
    public float sensitivity = 30f;//靈敏度
    public float distance;//當前攝影機與主角的距離;
    public float disSpeed = 20f;//滾輪靈敏度
    public float minDistence = 0;//攝影機與主角的最小距離
    public float maxDistence = 5;//攝影機與主角的最大距離
    public float shakePower;
    public float shakeDelay = 0.05f;
    public int time;//附身鏡頭前進後退時間
    public int stopFoward;//附身鏡頭前進停止時間
    public int stopBack = 0;//附身鏡頭後退停止時間
    public bool isHit = false;
    public bool pressingE = false;//判斷是否按著E
    public bool resetCameraMove = true;//重置鏡頭移動設定
    public bool canCameraMove = true;//判斷鏡頭是否可移動 附身後為false 防止按著附身鍵不放鏡頭出問題
    public bool isBacking = false;//鏡頭正在後退
    public bool AnimationOver = false;//動畫播完


    void Awake()
    {

        target = GameObject.Find("Player").transform;
        PlayerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        //PossessedSystem = GetComponent<PossessedSystem>();
        possessStart = GameObject.Find("CamStartPoint");
        possessEnd = GameObject.Find("CamEndPoint");
        ray = GameObject.Find("FirstPersonCamPoint").transform;
        PossessEffect = GameObject.Find("PossessCamera");

    }

    private void Start()
    {
        startPoint = possessStart.transform.localPosition;

        PossessEffect.SetActive(false);
    }

    // LateUpdate is called once per frame after other Update
    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (shakePower == 0)
                shakePower = 0.1f;
            shakePower *= -1;
        }
        else
            shakePower = 0;




        if (!isBacking)//如果不是在後退則鏡頭可以轉動
            cameraRotate();



        if (Input.GetKey(KeyCode.E))
        {
            Foward();
        }
        else
        {

            if (pressingE)//pressingE在進入附身模式後會持續為true 退出後也不會false
            {
                resetCameraMove = true;//重讀取鏡頭移動距離
                isBacking = true;//正在後退
                pressingE = false;
                PossessEffect.SetActive(false);
                Crosshairs.SetActive(false);
            }
            if (isBacking && canCameraMove)//退出附身模式後將鏡頭拉回 若為附身之後canCameraMove會等於false不執行
            {
                if (resetCameraMove)
                {
                    effectdistance = rotationEuler * (new Vector3(0, 0, -distance) + startPoint) + target.position - possessingCamera;//一次性讀取附身鏡頭與移動終點之間距離
                    resetCameraMove = false;
                }
                if (0 < time && time <= stopBack)
                {
                    move = effectdistance / stopFoward;
                    transform.position += move;
                    time -= 1;

                }
                else if (time <= 0)//鏡頭後退完畢
                {
                    stopBack = 0;
                    time = 0;
                    canCameraMove = false;//跳出鏡頭後退
                }
            }
            else
            {
                Crosshairs.SetActive(false);
                PossessEffect.SetActive(false);
                time = 0;
                AnimationOver = false;
                isBacking = false;
                pressingE = false;
                canCameraMove = true;//可以進入附身模式 若為附身之後canCameraMove會等於false
                move = Vector3.zero;
                //讀取滑鼠滾輪的數值
                distance -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;
                cameraFollow();
            }
        }
    }

    public void Foward()
    {
        pressingE = true;
        if (isBacking || time == 0)//設為正在後退時或是剛開始拉近(time=0)都要重置鏡頭移動設定
        {
            resetCameraMove = true;
            isBacking = false;
        }
        if (canCameraMove && AnimationOver)
        {
            target.rotation = Quaternion.Euler(0, rotX, 0);//鏡頭旋轉
            if (resetCameraMove)//重置鏡頭移動設定
            {
                stopFoward = 5;//預設停止時間
                effectdistance = possessEnd.transform.position - transform.position;//一次性讀取附身鏡頭與移動終點之間距離
                resetCameraMove = false;
            }
            if (time < stopFoward)
            {
                move = effectdistance / stopFoward;
                transform.position += move + rotationEuler * (new Vector3(0, 0, 0));
                possessingCamera = transform.position;
                time += 1;
            }
            else if (time >= stopFoward)
            {
                move = effectdistance;
                possessingCamera = possessEnd.transform.position;
                transform.position = possessEnd.transform.position;
                PossessEffect.SetActive(true);
                Crosshairs.SetActive(true);
            }
            stopBack = time;
        }
        else
        {
            Crosshairs.SetActive(false);
            PossessEffect.SetActive(false);
            cameraFollow();//附身之後canCameraMove為false 執行cameraFollow重置鏡頭至主角身上
        }
    }

    public void LoadCharacter()
    {
        if (PlayerManager.NowType == "Wolf")
        {
            possessStart = GameObject.Find("WolfCamStartPoint");
            possessEnd = GameObject.Find("WolfCamEndPoint");
            ray = GameObject.Find("WolfFirstPersonCamPoint").transform;
        }
        else if (PlayerManager.NowType == "Human")
        {
            possessStart = GameObject.Find("CamStartPoint");
            possessEnd = GameObject.Find("CamEndPoint");
            ray = GameObject.Find("FirstPersonCamPoint").transform;
        }

    }




    public void cameraRotate()
    {
        //攝影機旋轉
        //讀取滑鼠的X、Y軸移動訊息
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        //保證X在360度以內
        if (rotX > 360)
        {
            rotX -= 360;
        }
        else if (rotX < 0)
        {
            rotX += 360;
        }
        if (rotY > 20)
        {
            rotY = 20;
        }
        else if (rotY < -20)
        {
            rotY = -20;
        }
        //運算攝影機旋轉
        rotationEuler = Quaternion.Euler(rotY, rotX, 0);
        transform.rotation = rotationEuler;
    }

    public void cameraFollow()
    {

        //鏡頭穿牆處裡
        RaycastHit hit;
        if (Physics.Linecast(ray.position, rotationEuler * (new Vector3(0, 0, -distance) + startPoint) + target.position, out hit))
        {
            string name = hit.collider.gameObject.tag;
            if (name != "MainCamera")
            {
                //  Debug.Log(hit.point);
                Debug.DrawLine(ray.position, hit.point, Color.red);
                //if (Vector3.Distance(hit.point, target.position) < Vector3.Distance(rayend.transform.position, target.position))
                raydistance = rotationEuler * (new Vector3(0, 0, -distance) + startPoint) + target.position - hit.point;
                isHit = true;
            }
        }
        else
        {
            raydistance = Vector3.zero;
            isHit = false;
            Debug.DrawLine(ray.position, transform.position, Color.green);
        }
        //攝影機移動
        //限制距離
        distance = Mathf.Clamp(distance, minDistence, maxDistence);
        //運算攝影機座標
        cameraPosition = rotationEuler * (new Vector3(0, 0, -distance) + startPoint) + target.position - raydistance + new Vector3(-shakePower, shakePower, 0);
        //應用
        //target.transform.localRotation = Quaternion.AngleAxis(rotX, target.transform.up);//人物轉向 但ASD不能用
        transform.position = cameraPosition;
        // UpdateCursorLock();
    }

}
