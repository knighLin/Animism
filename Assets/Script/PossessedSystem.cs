using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

public class PossessedSystem : MonoBehaviour
{
    //call other class
    private PlayerManager playerManager;
    private PossessEffect possessEffect;
    private AnimalHealth animalHealth;
    private List<Highlighter> highlighter = new List<Highlighter>();
    private List<HighlighterConstant> highlighterConstant = new List<HighlighterConstant>();

    //set possessValue
    public static bool OnPossessed = false;//附身狀態
    public static GameObject AttachedBody;//附身物
    public static SphereCollider PossessedCol;//附身範圍
    private List<Collider> RangeObject = new List<Collider>();//範圍附身物
    private GameObject Player;
    private GameObject Possessor;//人的型態
    private string PreviousTag;//附身前的標籤
    public LayerMask PossessedLayerMask;//可被附身物的階層
    private RaycastHit hit;//點擊的動物物件
    public GameObject bear, deer, wolf;//UI的
    public static int WolfCount;//狼的連續附身次數


    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        possessEffect = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PossessEffect>();
        Possessor = GameObject.Find("Human");
        PossessedCol = GetComponent<SphereCollider>();
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))//打開關閉附身系統
        {
            PossessedCol.enabled = !PossessedCol.enabled;//當附身範圍collider是關閉則打開，反之則打開
            if (PossessedCol.enabled == true)//當附身範圍collider打開時
            {
                possessEffect.OpenPossessedEffectCam();//開啟附身的運鏡
                PlayerMovement.m_Animator.SetTrigger("Surgery");//播放附身動畫
                //PossessedCol.enabled = true;
                Time.timeScale = 0.5f;//遊戲慢動作
                //清掉之前範圍的動物物件和Highlight
                RangeObject.Clear();
                highlighter.Clear();
                highlighterConstant.Clear();
            }
            else//附身範圍collider關閉
            {
                possessEffect.ClosePossessedEffectCam();//關閉附身運鏡
                CloseRangOnLight();//關掉附身物的附身效果shader
                Time.timeScale = 1f;//取消慢動作
            }
        }
        MouseChoosePossessed();//當開啟附身系統，才能點選要附身物


        if (Input.GetKeyUp(KeyCode.Q) && AttachedBody != null)//解除附身
        {
            LifedPossessed();//離開附身物
            animalHealth.CancelLink();//解除與附身物的血條連動
        }
    }

    void MouseChoosePossessed()//滑鼠點擊附身物
    {
        if (Input.GetMouseButtonDown(0) && PossessedCol.enabled == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 10, PossessedLayerMask);
            for (int i = 0; i < RangeObject.Count; i++)
            {
                if (!hit.collider.CompareTag("Player"))//如果是自己本身不執行
                {
                    if (hit.collider == RangeObject[i])//當點擊的物件是附身範圍裡的物件時
                    {
                        EnterPossessed();//執行附身
                        Time.timeScale = 1f;//慢動作回覆
                        PossessedCol.enabled = false;//關掉附身範圍
                        possessEffect.ClosePossessedEffectCam();//關閉附身運鏡
                    }
                }
            }
        }
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
            AttachedBody = hit.collider.gameObject;//讓新的附身物等於AttachedBody
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
                Debug.Log(WolfCount);
            }
            else
            {
                WolfCount = 0;
            }

            //附身後抓取附身動物的HP腳本，將動物血量跟主角血量做連動
            animalHealth = AttachedBody.GetComponent<AnimalHealth>();
            animalHealth.LinkHP();

            //附身後抓取動物的動畫
            PlayerMovement.m_Animator = AttachedBody.GetComponent<Animator>();

            switch (AttachedBody.transform.tag)
            {//將附身物的標籤傳到管理者，方便變換動物數值
                case "Bear":
                    playerManager.TurnType("Bear", PreviousTag);
                    deer.SetActive(false);
                    wolf.SetActive(false);
                    bear.SetActive(true);
                    break;
                case "Wolf":
                    playerManager.TurnType("Wolf", PreviousTag);
                    deer.SetActive(false);
                    wolf.SetActive(true);
                    bear.SetActive(false);
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
        highlighter.Add(Object.GetComponent<Highlighter>());
        highlighterConstant.Add(Object.GetComponent<HighlighterConstant>());

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

        deer.SetActive(false);
        wolf.SetActive(false);
        bear.SetActive(false);
    }

    private void CloseRangOnLight()
    {
        for (int i = 0; i < RangeObject.Count; i++)
        {
            highlighter[i].enabled = false;
            highlighterConstant[i].enabled = false;
        }
    }
}

