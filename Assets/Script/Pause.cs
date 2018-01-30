using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public GameObject PauseCanvas;
    public Image FadeIn;
    public float time=1;
    private bool Fade;
    private bool IsPause=false;
    // Use this for initialization
    void Start () {
        Fade = true;
	}
	
	// Update is called once per frame
	void Update () {
 
        if (Fade && time <=1&& time>0)
        {
            FadeIn.color = new Color(0, 0, 0, time);
            time -= Time.deltaTime * 0.8f;
        }
        else if (time < 0)
        {
            time = 0;
            Fade = false;
        }

        if (IsPause)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;//鎖滑鼠標
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetButtonDown("joy17"))
        {
            if (!IsPause)
            {
                CreatPauseCanvas();
            }
            else
            {
                DestroyPauseCanvas();
            }
        }
    }
    public void CreatPauseCanvas()
    {
        //Instantiate(canvasPrefab, Vector2.zero, Quaternion.identity).name= "PauseCanvas";
        PauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        IsPause = true;
    }
    public void DestroyPauseCanvas()
    {
        //Destroy(GameObject.Find("PauseCanvas"));
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        IsPause = false;
    }
}
