using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

public class PossessedSystem : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerManager playerManager;
    private HPcontroller HPcontroller;
    private CameraScript CameraScript;
    public static bool OnPossessed = false;//附身狀態
    public static GameObject AttachedBody;//附身物
    public static SphereCollider PossessedCol;//附身範圍
    public LayerMask PossessedLayerMask;//可被附身物的階層
    public static int WolfCount;//狼的連續附身次數
    private List<Collider> RangeObject = new List<Collider>();//範圍附身物
    public GameObject Possessor;//人的型態
    private RaycastHit hit;//點擊的動物物件
    private string PreviousTag;//附身前的標籤
    public bool ChooseRightObject;
    private bool clear = true;

    private void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        CameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        HPcontroller = GameObject.Find("PlayerManager").GetComponent<HPcontroller>();

    }
    private void Start()
    {
        if (!Possessor)
        Possessor = GameObject.Find("Pine");
    }
    private void OnEnable()//當打開程式，呼叫當前正開啟的物件的附身範圍和移動
    {
        playerMovement = GetComponent<PlayerMovement>();
        PossessedCol = GetComponent<SphereCollider>();
    }
    //private void OnDisable()
    //{
    //    playerMovement = null;
    //    PossessedCol = null;
    //}
    private void Update()
    {
         if (CameraScript.CanPossess)//打開附身系統
         {
             if (clear)//開啟附身系統只清一次，播放一次動畫
             {
                 //PlayerMovement.m_Animator.SetTrigger("Surgery");//播放附身動畫
                 playerMovement.enabled = false;
                 //清掉之前範圍的動物物件和Highlight
                 RangeObject.Clear();
                 clear = false;
             }


             PossessedCol.enabled = true;
             if (Time.timeScale == 1f)
                 Time.timeScale = 0.5f;//如果時間正常則遊戲慢動作
             MouseChoosePossessed();


         }
         else
         {
             if (!CameraScript.IsPossessing)//附身不能動
                playerMovement.enabled = true;
             clear = true;//讓下次開啟附身清理範圍物件
             PossessedCol.enabled = false;//附身範圍collider關閉
             if (Time.timeScale == 0.5f)
                 Time.timeScale = 1f;//如果有變慢 才取消慢動作
             //joycontroller.joypossessed = false; //搖桿
         }
        /*if (Input.GetKeyUp(KeyCode.E))//開啟附身系統只清一次，播放一次動畫
        {
            PossessedCol.enabled = !PossessedCol.enabled;
            if (PossessedCol.enabled == true)
            {
                RangeObject.Clear();//清掉之前範圍的動物物件
                playerMovement.enabled = false;
                PossessedCol.enabled = true;
                Time.timeScale = 0.5f;//如果時間正常則遊戲慢動作
            }
            else
            {
                playerMovement.enabled = true;
                Time.timeScale = 1f;//如果有變慢 才取消慢動作

            }
        }
        MouseChoosePossessed();
        */


        if ((Input.GetKeyUp(KeyCode.Q) || joycontroller.leftpossessed == true) && AttachedBody != null&& !CameraScript.IsPossessing)//解除附身
        {
            LifedPossessed();//離開附身物
            //animalHealth.CancelLink();//解除與附身物的血條連動
        }
    }

    public void MouseChoosePossessed()//滑鼠點擊附身物
    {

         if (ChooseRightObject)//如果點到可附身物件
         {
             playerMovement.enabled = false;//附身不能動
             CameraScript.PossessTarget = hit.collider.gameObject.transform.parent.gameObject;
             ChooseRightObject = false;//重置
             CameraScript.CanPossess = false;//重置
             CameraScript.PossessEffect.SetActive(false);//重置
             CameraScript.Crosshairs.SetActive(false);//重置
             CameraScript.IsPossessing = true;//正在附身模式
             CameraScript.CameraState = "GettingPossess";//轉為附身模式
         }
         else if (((Input.GetMouseButtonDown(1) || Input.GetButtonDown("joy12")) && PossessedSystem.PossessedCol.enabled == true))
         {
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             Physics.Raycast(ray, out hit, 10, PossessedLayerMask);
             for (int i = 0; i < RangeObject.Count; i++)
             {
                //if (!hit.collider.CompareTag("Player"))//如果是自己本身不執行
                //{
                if (hit.collider == RangeObject[i])//當點擊的物件是附身範圍裡的物件時
                {
                    ChooseRightObject = true;
                    break;
                }
                else
                    ChooseRightObject = false;
                 //}
             }
         }
         /*
        if (((Input.GetMouseButtonUp(1) || Input.GetButtonDown("joy12")) && PossessedCol.enabled == true))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 10, PossessedLayerMask);
            Debug.DrawLine(ray.origin, hit.transform.position, Color.green, 0.1f, true);
            for (int i = 0; i < RangeObject.Count; i++)
            {
                if (!hit.collider.gameObject != this.gameObject)//如果是自己本身不執行
                {
                    if (hit.collider == RangeObject[i])//當點擊的物件是附身範圍裡的物件時
                    {
                        Debug.DrawLine(ray.origin, hit.transform.position, Color.red, 0.1f, true);
                        EnterPossessed();
                        Time.timeScale = 1f;//如果有變慢 才取消慢動作
                    }
                }
            }
        }*/

    }

    public void InToPossess()
    {
        EnterPossessed();//執行附身
        HPcontroller.CharacterSwitch();//換圖片
        Time.timeScale = 1f;//慢動作回覆
        PossessedCol.enabled = false;//關掉附身範圍
    }

    public void EnterPossessed()//附身
    {
        if (hit.collider.gameObject != this.gameObject)//當下一個物件不是目前物件時，可以繼續附身
        {
            if (AttachedBody != null && OnPossessed == true)//如果先前有附身物，而且正在附身
            {
                AttachedBody.tag = Possessor.tag;
                Possessor.transform.parent = null;//將玩家物件分離出現在的被附身物
                AttachedBody.GetComponent<PlayerMovement>().enabled = false;
                AttachedBody.GetComponent<PossessedSystem>().enabled = false;
            }
            PreviousTag = Possessor.tag;//附身後將先前附身的tag存起來
            Possessor.tag = hit.collider.tag;//將目前人的tag轉為附身後動物的
            AttachedBody = hit.collider.gameObject.transform.parent.gameObject;//讓新的附身物等於AttachedBody
            playerManager.NowCharacter = AttachedBody;
            // AttachedBody = hit.collider.gameObject;//點擊到的附身物等於AttachedBody
            playerManager.PossessType = AttachedBody.tag;
            AttachedBody.tag = "Player";
            //附身者的位置到新被附身物的位置
            Possessor.transform.position = new Vector3(AttachedBody.transform.position.x,
                                                    AttachedBody.transform.position.y,
                                                    AttachedBody.transform.position.z);
            
            PossessedCol.enabled = false;//關掉當前附身範圍
            Possessor.transform.parent = AttachedBody.transform;//將附身者變為被附身物的子物件
            Possessor.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            AttachedBody.GetComponent<PlayerMovement>().enabled = true;//打開附身者的移動和附身
            AttachedBody.GetComponent<PossessedSystem>().enabled = true;
            AttachedBody.GetComponent<PossessedSystem>().Possessor = Possessor;
            Possessor.SetActive(false);//關掉人型態的任何事
            OnPossessed = true;//已附身


            if (AttachedBody.tag == "Wolf")
            {
                WolfCount++;
            }
            else
            {
                WolfCount = 0;
            }

            //附身後抓取附身動物的HP腳本，將動物血量跟主角血量做連動
            //animalHealth = AttachedBody.GetComponent<AnimalHealth>();
            //animalHealth.LinkHP();

            switch (AttachedBody.transform.tag)
            {//將附身物的標籤傳到管理者，方便變換動物數值
                case "Bear":
                    playerManager.TurnType("Bear", PreviousTag);
                    break;
                case "Wolf":
                    playerManager.TurnType("Wolf", PreviousTag);
                    break;
            }
            Debug.Log("5");
        }
    }

    void OnTriggerEnter(Collider Object)//送出訊息
    {
        switch (Object.transform.tag)
        {//判斷是不是可以附身的物件
         //case "Human":
            case "Bear":
            case "Wolf":
            case "Deer":
                break;
            default:
                return;
        }
        RangeObject.Add(Object);
  }

    public void LifedPossessed()//解除變身
    {
        
        Possessor.transform.parent = null;//將玩家物件分離出被附身物
        Possessor.transform.position = new Vector3(AttachedBody.transform.position.x + 1.5f, transform.position.y + 0.5f, AttachedBody.transform.position.z + 1.5f);
                                          //將被附身物與人的位置分離
        AttachedBody.GetComponent<PlayerMovement>().enabled = false;
        AttachedBody.GetComponent<PossessedSystem>().enabled = false;
        Possessor.GetComponent< PlayerMovement>().enabled = true;
        Possessor.tag = "Player";//將型態變回Human
        Possessor.SetActive(true);//打開人型態的任何事
        playerManager.TurnType("Human", AttachedBody.tag);//將標籤傳至管理者，變換數值
        AttachedBody.tag = playerManager.PossessType;//還tag給動物
        AttachedBody = null;//解除附身後清除附身物，防止解除附身後按Ｑ還有反應
        OnPossessed = false;//取消附身
        playerManager.NowCharacter = Possessor;
        playerManager.PossessType = "Human";
        CameraScript.LoadCharacterPosition();
        HPcontroller.CharacterSwitch(); 


    }
}


/*public class PossessedSystem : MonoBehaviour
{
    //call other class
    private PlayerMovement playerMovement;
    private PlayerManager playerManager;
    private HPcontroller HPcontroller;
    private CameraScript CameraScript;
    private AnimalHealth animalHealth;
    private List<Highlighter> highlighter = new List<Highlighter>();
    private List<HighlighterConstant> highlighterConstant = new List<HighlighterConstant>();

    //set possessValue
    public static bool OnPossessed = false;//附身狀態
    public static GameObject AttachedBody;//附身物
    public static SphereCollider PossessedCol;//附身範圍
    public static int WolfCount;//狼的連續附身次數
    private List<Collider> RangeObject = new List<Collider>();//範圍附身物
    private GameObject Player;
    private GameObject Possessor;//人的型態
    public LayerMask PossessedLayerMask;//可被附身物的階層
    private RaycastHit hit;//點擊的動物物件
    private string PreviousTag;//附身前的標籤
    public float AnimationTime;
    public bool ChooseRightObject;
    private bool clear = true;

    //audio
    private AudioSource audioSource;
    public AudioClip HumanSurgery;
    public AudioClip WolfSurgery;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        HPcontroller = GameObject.Find("GameManager").GetComponent<HPcontroller>();
        playerManager = GetComponent<PlayerManager>();
        CameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        Possessor = GameObject.FindWithTag("Human");
        PossessedCol = GetComponent<SphereCollider>();
        Player = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (CameraScript.CanPossess)//打開附身系統
        {
            if (clear)//開啟附身系統只清一次，播放一次動畫
            {
                //PlayerMovement.m_Animator.SetTrigger("Surgery");//播放附身動畫
                playerMovement.enabled = false;
                //清掉之前範圍的動物物件和Highlight
                RangeObject.Clear();
                highlighter.Clear();
                highlighterConstant.Clear();

                if (Possessor.tag == "Human")//判斷目前形體，播放不同附身的音效
                {
                    audioSource.PlayOneShot(HumanSurgery);
                }
                else if (Possessor.tag == "Wolf")
                {
                    audioSource.PlayOneShot(WolfSurgery);
                   
                }
                clear = false;
            }


            PossessedCol.enabled = true;
            if (Time.timeScale == 1f)
            Time.timeScale = 0.5f;//如果時間正常則遊戲慢動作
            MouseChoosePossessed();


        }
        else
        {
            playerMovement.enabled = true;
            AnimationTime = 0;
            clear = true;//讓下次開啟附身清理範圍物件
            PossessedCol.enabled = false;//附身範圍collider關閉
            CloseRangOnLight();//關掉附身物的附身效果shader
            if (Time.timeScale == 0.5f)
            Time.timeScale = 1f;//如果有變慢 才取消慢動作
            //joycontroller.joypossessed = false; //搖桿
        }


        if (Input.GetKeyUp(KeyCode.Q) && AttachedBody != null || joycontroller.leftpossessed == true)//解除附身
        {
          
            LifedPossessed();//離開附身物
            animalHealth.CancelLink();//解除與附身物的血條連動
        }
    }

    public void MouseChoosePossessed()//滑鼠點擊附身物
    {
        if (ChooseRightObject)//如果點到可附身物件
        {
            ChooseRightObject = false;//重置
            CameraScript.CanPossess = false;//重置
            CameraScript.PossessEffect.SetActive(false);//重置
            CameraScript.Crosshairs.SetActive(false);//重置
            CameraScript.IsPossessing = true;//正在附身模式
            CameraScript.CameraState = "GettingPossess";//轉為附身模式
        }
        else if(((Input.GetMouseButtonUp(0) || Input.GetButtonDown("joy12")) && PossessedSystem.PossessedCol.enabled == true))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 10, PossessedLayerMask);
            for (int i = 0; i < RangeObject.Count; i++)
            {
                //if (!hit.collider.CompareTag("Player"))//如果是自己本身不執行
                //{
                if (hit.collider == RangeObject[i])//當點擊的物件是附身範圍裡的物件時
                {
                    ChooseRightObject = true;
                }
                else
                {
                    ChooseRightObject = false;
                }
                //}
            }
        }
        
    }

    public void InToPossess()
    {
            EnterPossessed();//執行附身
            HPcontroller.CharacterSwitch();//換圖片
            Time.timeScale = 1f;//慢動作回覆
            PossessedCol.enabled = false;//關掉附身範圍
    }


    public void EnterPossessed()//附身
    {
        if (hit.collider.gameObject != AttachedBody)//當下一個物件不是目前物件時，可以繼續附身
        {
            if (AttachedBody != null && OnPossessed == true)//如果先前有附身物，而且正在附身
            {
                AttachedBody.transform.parent = null;//將玩家物件分離出現在的被附身物
            }
            PreviousTag = Possessor.tag;//附身後將先前附身的tag存起來
            Possessor.tag = hit.collider.tag;//將目前人的tag轉為附身後動物的
            AttachedBody = hit.collider.gameObject.transform.parent.gameObject;//讓新的附身物等於AttachedBody
            //附身者的位置到新被附身物的位置
            Player.transform.position = new Vector3(AttachedBody.transform.position.x,
                                                    AttachedBody.transform.position.y,
                                                    AttachedBody.transform.position.z);

            AttachedBody.transform.parent = gameObject.transform;//將新被附身物變為附身者的子物件
            //- (AttachedBody.transform.localScale.y / 2f)
            //AttachedBody.transform.localPosition = Vector3.zero;
            AttachedBody.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Possessor.SetActive(false);//關掉人型態的任何事
            OnPossessed = true;//已附身


            if (AttachedBody.tag == "Wolf")
            {
                WolfCount++;
                //Debug.Log(WolfCount);
            }
            else
            {
                WolfCount = 0;
            }
            //附身後抓取動物的動畫
            PlayerMovement.m_Animator = AttachedBody.GetComponent<Animator>();

            //附身後抓取附身動物的HP腳本，將動物血量跟主角血量做連動
            animalHealth = AttachedBody.GetComponent<AnimalHealth>();
            animalHealth.LinkHP();



            switch (AttachedBody.transform.tag)
            {//將附身物的標籤傳到管理者，方便變換動物數值
                case "Bear":
                    playerManager.TurnType("Bear", PreviousTag);
                    //bear.SetActive(true);
                    //deer.SetActive(false);
                    //wolf.SetActive(false);

                    break;
                case "Wolf":
                    playerManager.TurnType("Wolf", PreviousTag);
                    //deer.SetActive(false);
                    //wolf.SetActive(true);
                    //bear.SetActive(false);
                    break;
            }
        }
       CloseRangOnLight();//附身結束關掉Highlight

    }

    void OnTriggerEnter(Collider Object)//送出訊息
    {

        switch (Object.transform.tag)
        {//判斷是不是可以附身的物件
         //case "Human":
            case "Bear":
            case "Wolf":
            case "Deer":
                break;
            default:
                return;
        }
        RangeObject.Add(Object);
        //將範圍內可以被附身動物的Highlight打開
        highlighter.Add(Object.transform.parent.GetComponent<Highlighter>());
        highlighterConstant.Add(Object.transform.parent.GetComponent<HighlighterConstant>());

        if (highlighter != null && highlighterConstant != null)
        {
            for (int i = 0; i < RangeObject.Count; i++)
            {
                highlighterConstant[i].enabled = true;
                highlighter[i].enabled = true;
            }
        }

    }

    public void LifedPossessed()//解除變身
    {

        AttachedBody.transform.parent = null;//將玩家物件分離出被附身物
        Player.transform.position = new Vector3(AttachedBody.transform.position.x + 1.5f, transform.position.y + 0.5f, AttachedBody.transform.position.z + 1.5f);
        //將被附身物與人的位置分離
        PlayerMovement.m_Animator = Possessor.GetComponent<Animator>();//重新抓人的動畫
        Possessor.tag = "Human";//將型態變回Human
        Possessor.SetActive(true);//打開人型態的任何事
        playerManager.TurnType("Human", AttachedBody.tag);//將標籤傳至管理者，變換數值
        AttachedBody = null;//解除附身後清除附身物，防止解除附身後按Ｑ還有反應
        OnPossessed = false;//取消附身
        HPcontroller.CharacterSwitch();//換圖片
        CameraScript.LoadCharacterPosition();//重置鏡頭位置
    }

    private void CloseRangOnLight()
    {
        for (int i = 0; i < RangeObject.Count; i++)
        {
            highlighter[i].enabled = false;
            highlighterConstant[i].enabled = false;
        }
    }
}*/

