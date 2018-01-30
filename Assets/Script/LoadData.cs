using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadData : MonoBehaviour {
    public GameObject LoadingCanvas;
    public GameObject PlayerPrefab, WolfPrefab, EnemyPrefab;
    public GameObject Wolf;
    public List<int> AnimalState;
    public List<Vector3> AnimalVector3;
    public List<Quaternion> AnimalQuaternion;
    public List<int> EnemyState;
    public List<Vector3> EnemyVector3;
    public List<Quaternion> EnemyQuaternion;
    public string PlayerState;
    public Vector3 PlayerVector3;
    public Quaternion PlayerQuaternion;
    public Slider LoadingSlider;
    private AsyncOperation _async;
    public int LoadNumber;

    // Use this for initialization
    void Awake () {
        SpawnAllObject();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void LoadSence()
    {
        Instantiate(LoadingCanvas, Vector2.zero, Quaternion.identity).name = "LoadingCanvas";
        StartCoroutine(LoadLevelWithBar("Game"));
    }

    IEnumerator LoadLevelWithBar(string level)
    {
        _async = Application.LoadLevelAsync(level);
        while (!_async.isDone)
        {
            LoadingSlider.value = _async.progress;
            yield return null;
        }            
    }
    public  void SpawnAllObject()
    {
        LoadNumber = 1;
        SaveData.Data Load = (SaveData.Data)IOHelper.GetData(Application.persistentDataPath+"/Save/GameData"+ LoadNumber + ".sav", typeof(SaveData.Data));
        Debug.Log("讀取了GameData" + LoadNumber);
        for (int A =0 ; A< Load.AnimalState.Count; A++)     //讀取動物數據
        {
            AnimalState.Add(Load.AnimalState[A]);           //讀取動物狀態
            AnimalVector3.Add(Load.AnimalVector3[A]);       //讀取動物座標
            AnimalQuaternion.Add(Load.AnimalQuaternion[A]); //讀取動物旋轉角度
            if (AnimalState[A]==1)                          //如果動物活著(AnimalState=1)才生成
            {
                Instantiate(WolfPrefab, AnimalVector3[A], AnimalQuaternion[A]).name="Wolf"+A;
                Debug.Log("讀取了第" + (A + 1) + "隻狼," + "狀態為" + AnimalState[A] + ",座標為" + AnimalVector3[A]);
            }
            if (AnimalState[A] == 2)                        //如果動物被附身(AnimalState=2)生成後掛在主角身上
            {
                Wolf = Instantiate(WolfPrefab, GameObject.Find("Player").transform.position, Quaternion.identity);
                Wolf.transform.parent = GameObject.Find("Player").transform;
            }
            //Debug.Log(D1.AnimalVector3[A]);
            //Debug.Log("讀" + AnimalVector3[A]);
        }

        for (int E = 0; E < Load.EnemyState.Count; E++)   //讀取敵人數據
        {
            EnemyState.Add(Load.EnemyState[E]);           //讀取敵人狀態
            EnemyVector3.Add(Load.EnemyVector3[E]);       //讀取敵人座標
            EnemyQuaternion.Add(Load.EnemyQuaternion[E]); //讀取敵人旋轉角度
            if (EnemyState[E] == 1)                       //如果敵人活著(EnemyState=1)才生成
            {
                Instantiate(EnemyPrefab, EnemyVector3[E], EnemyQuaternion[E]).name = "Enemy" + E;
                Debug.Log("讀取了第" + (E + 1) + "個敵人," + "狀態為" + EnemyState[E] + ",座標為" + EnemyVector3[E]);
            }
        }
         PlayerState = Load.PlayerState;
         PlayerVector3 = Load.PlayerVector3;
         PlayerQuaternion = Load.PlayerQuaternion;
         Instantiate(PlayerPrefab, PlayerVector3,PlayerQuaternion).name = "Pine";
         Debug.Log("讀取了派恩的位置,座標為" + PlayerVector3);


    }

}
