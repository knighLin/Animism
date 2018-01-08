using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class PossessEffect : MonoBehaviour
{
    public GameObject possessStart, possessEnd, cam1, cam2, cam3;
    public Transform Target;
    public int time;
    public int stop = 5;
    public bool IscameraMove = false;
    public AnimationCurve camMoveYCurve;//cam2的移動曲線，使用插件DoTween
    // 放在Awake內，在物件執行之前就先封鎖住，避免物件的出現，直到使用者切換攝影機後再創造該物件，若是放在Start內則會造成全部的物件都已經產生且初始化後再封鎖住
    void Awake()
    {
        //預設先開啟第一部攝影機//一定要先暫停不使用的攝影機後，再開啟要使用的攝影機！
        cam3.SetActive(false);
        cam2.SetActive(false);
        cam1.SetActive(true);
    }


    public void OpenPossessedEffectCam()
    {
        cam2.transform.position = cam1.transform.position;//附身鏡頭初始位置在腳色後方
        cam2.transform.LookAt(Target);
        cam1.SetActive(false);//close mainCam
        cam2.SetActive(true);//Open PossessedEffectCam

        var targetPos = possessStart.transform.position;
        var seq = DOTween.Sequence();
        seq.Append(cam2.transform.DOMoveX(targetPos.x, 0.8f))
           .Join(cam2.transform.DOMoveY(targetPos.y, 0.8f).SetEase(camMoveYCurve))
           .Join(cam2.transform.DOMoveZ(targetPos.z, 0.8f).SetEase(camMoveYCurve));//從cam1的位置到possessStart
        
        StartCoroutine("MoveCam2ToEnd");

    }
    public void ClosePossessedEffectCam()
    {
        StopCoroutine("MoveCam2ToEnd");
        cam1.transform.position = possessStart.transform.position;
        cam2.transform.position = possessStart.transform.position;
        IscameraMove = false;
        cam3.SetActive(false);//close firstPersonCam
        cam2.SetActive(false);//close PossessedEffectCam
        cam1.SetActive(true);//open mainCam
        time = 0;
    }


    IEnumerator MoveCam2ToEnd()
    {
        while (Vector3.Distance(cam2.transform.position, possessEnd.transform.position) > 0.01f)
        {
            cam2.transform.LookAt(Target);
            cam2.transform.position = Vector3.MoveTowards(cam2.transform.position, possessEnd.transform.position, 5*Time.deltaTime);
            yield return null;
        }
        cam2.SetActive(false);
        cam3.SetActive(true);
        StopCoroutine("MoveCam2ToEnd");
    }
}
