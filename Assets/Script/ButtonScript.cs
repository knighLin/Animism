using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    
    public GameObject LoadingCanvas, ChooseSaveData;
    private AsyncOperation _async;
    public AudioSource audioSource;
    public Slider LoadingSlider;
    public Image FadeOut;
    public float time;
    private bool Fade;

    void Update()
    {
        if (Fade && time < 1)
        {
            time += Time.deltaTime*0.8f;
            if (time >= 1)
                time = 1;
            FadeOut.color = new Color(0, 0, 0, time);
            


        }
        else if (time >= 1)
        {
            //AudioFadeOut(audioSource, time);
            time = 0;
            Fade = false;
            Debug.Log("LoadSence");
            Application.LoadLevelAsync("Game");
           // LoadSence();
        }
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
    public void Click()
    {
        switch (this.name)
        {
            //以下按鈕為Homepage場景內
            case "NewGame":
                ChooseSaveData.GetComponent<ChooseSaveData>().SelectedData = "NewGame";
                Fade = true;
                break;
            case "Data1":
                if (this.tag == "LoadData")
                {
                    if (File.Exists(@"C:\Users\user\AppData\LocalLow\Animism\Soul\Save\" + "Data1" + ".sav"))
                    {
                        ChooseSaveData.GetComponent<ChooseSaveData>().SelectedData = "Data1";
                        Fade = true;
                    }
                }
                else if(this.tag == "SaveData")
                {
                    GameObject.Find("PlayerManager").GetComponent<SaveData>().SaveDataValue("Data1");
                }
                break;
            case "Data2":
                if (this.tag == "LoadData")
                {
                    if (File.Exists(@"C:\Users\user\AppData\LocalLow\Animism\Soul\Save\" + "Data2" + ".sav"))
                    {
                        ChooseSaveData.GetComponent<ChooseSaveData>().SelectedData = "Data2";
                        Fade = true;
                    }
                }
                else if (this.tag == "SaveData")
                {
                    GameObject.Find("PlayerManager").GetComponent<SaveData>().SaveDataValue("Data2");
                }
                break;
            case "Data3":
                if (this.tag == "LoadData")
                {
                    if (File.Exists(@"C:\Users\user\AppData\LocalLow\Animism\Soul\Save\" + "Data3" + ".sav"))
                    {
                        ChooseSaveData.GetComponent<ChooseSaveData>().SelectedData = "Data3";
                        Fade = true;
                    }
                }
                else if (this.tag == "SaveData")
                {
                    GameObject.Find("PlayerManager").GetComponent<SaveData>().SaveDataValue("Data3");
                }
                break;
            case "Exit":
                //離開遊戲
                break;
            case "ReturnHomepage":
                if (Time.timeScale == 0)//如果暫停狀態下回到主畫面則讓時間恢復
                    Time.timeScale = 1;
                GameObject.Find("ChooseSaveData").GetComponent<ChooseSaveData>().DestroyThis();
                Application.LoadLevelAsync("HomePage");
                break;
        }
    }
    public static IEnumerator AudioFadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}


