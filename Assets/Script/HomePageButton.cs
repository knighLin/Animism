using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePageButton : MonoBehaviour
{
    public GameObject LoadingCanvas;
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
            FadeOut.color = new Color(0, 0, 0, time);
            time += Time.deltaTime * 0.8f;
        }
        else if (time >= 1)
        {
            //AudioFadeOut(audioSource, time);
            time = 0;
            Fade = false;
            Debug.Log("LoadSence");
            //Application.LoadLevelAsync("Game");
            LoadSence();
        }
    }
    public void Click()
    {
        switch (this.name)
        {
            case "Start":
                Fade = true;
                
                break;
            case "Exit":
                if (Time.timeScale == 0)//如果暫停狀態下回到主畫面則讓時間恢復
                    Time.timeScale = 1;
                Application.LoadLevelAsync("HomePage");
                break;
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


