using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPcontroller : MonoBehaviour
{


    private PlayerHealth PlayerHealth;
    private AnimalHealth AnimalHealth;
    private PlayerManager playerManager;
    public Image HpB, HpW, HpR;//腳色血條圖片
    public Image FaceB, FaceW, FaceR;//腳色臉圖片
    public float BlinkTime;
    public float PineHpMax, PineHpNow, WolfHpMax, WolfHpNow, BearHpMax, BearHpNow, DeerHpMax, DeerHpNow;//腳色血量數值
    public bool Blink;

    // Use this for initialization
    void Start()
    {


        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        PlayerHealth = GameObject.Find("Pine").GetComponent<PlayerHealth>();
        Blink = false;
        CharacterSwitch();
    }

    // Update is called once per frame
    void Update()
    {



        if (Blink)//閃爍
            UIBlink();
        else
            BlinkTime = 0;

    }

    public void CharacterSwitch()
    {
        switch (playerManager.PossessType)
        {
            case "Human":
                FaceB.sprite = Resources.Load("UI/左上/Pine/PINE_BUTTOM", typeof(Sprite)) as Sprite;
                FaceW.sprite = Resources.Load("UI/左上/Pine/PINE_WHITE", typeof(Sprite)) as Sprite;
                FaceR.sprite = Resources.Load("UI/左上/Pine/PINE_WHITE", typeof(Sprite)) as Sprite;
                break;
            case "WolfMaster":
                FaceB.sprite = Resources.Load("UI/左上/Wolf/WOLF_BUTTOM", typeof(Sprite)) as Sprite;
                FaceW.sprite = Resources.Load("UI/左上/Wolf/WOLF_WHITE", typeof(Sprite)) as Sprite;
                FaceR.sprite = Resources.Load("UI/左上/Wolf/WOLF_WHITE", typeof(Sprite)) as Sprite;
                break;
            case "BearMaster":
                FaceB.sprite = Resources.Load("UI/左上/Bear/BEAR_BUTTOM", typeof(Sprite)) as Sprite;
                FaceW.sprite = Resources.Load("UI/左上/Bear/BEAR_WHITE", typeof(Sprite)) as Sprite;
                FaceR.sprite = Resources.Load("UI/左上/Bear/BEAR_WHITE", typeof(Sprite)) as Sprite;
                break;
            case "DeerMaster":
                FaceB.sprite = Resources.Load("UI/左上/Deer/DEER_BUTTOM", typeof(Sprite)) as Sprite;
                FaceW.sprite = Resources.Load("UI/左上/Deer/DEER_WHITE", typeof(Sprite)) as Sprite;
                FaceR.sprite = Resources.Load("UI/左上/Deer/DEER_WHITE", typeof(Sprite)) as Sprite;
                break;
        }
    }
    public void UIBlink()
    {
        BlinkTime += Time.deltaTime * 8;
        if (BlinkTime <= 5)
        {
            if (BlinkTime % 2 < 1)
            {
                HpR.sprite = Resources.Load("UI/左上/Hp/HP_WHITE", typeof(Sprite)) as Sprite;
                switch (playerManager.PossessType)
                {
                    case "Human":
                        FaceR.sprite = Resources.Load("UI/左上/Pine/PINE_WHITE", typeof(Sprite)) as Sprite;
                        break;
                    case "Wolf":
                        FaceR.sprite = Resources.Load("UI/左上/Wolf/WOLF_WHITE", typeof(Sprite)) as Sprite;
                        break;
                    case "Bear":
                        FaceR.sprite = Resources.Load("UI/左上/Bear/BEAR_WHITE", typeof(Sprite)) as Sprite;
                        break;
                    case "Deer":
                        FaceR.sprite = Resources.Load("UI/左上/Deer/DEER_WHITE", typeof(Sprite)) as Sprite;
                        break;
                }
            }
            else
            {
                HpR.sprite = Resources.Load("UI/左上/Hp/HP_RED", typeof(Sprite)) as Sprite;
                switch (playerManager.PossessType)
                {
                    case "Human":
                        FaceR.sprite = Resources.Load("UI/左上/Pine/PINE_RED", typeof(Sprite)) as Sprite;
                        break;
                    case "Wolf":
                        FaceR.sprite = Resources.Load("UI/左上/Wolf/WOLF_RED", typeof(Sprite)) as Sprite;
                        break;
                    case "Bear":
                        FaceR.sprite = Resources.Load("UI/左上/Bear/BEAR_RED", typeof(Sprite)) as Sprite;
                        break;
                    case "Deer":
                        FaceR.sprite = Resources.Load("UI/左上/Deer/DEER_RED", typeof(Sprite)) as Sprite;
                        break;
                }
            }
        }
        else Blink = false;
    }
    public void CharacterHpControll()
    {

        switch (playerManager.PossessType)
        {
            case "Human":
                PineHpMax = PlayerHealth.MaxHealth;
                PineHpNow = PlayerHealth.currentHealth;

                HpW.fillAmount = (PineHpNow * 0.75f + PineHpMax * 0.25f) / PineHpMax;//(75%當前血量+25%血量最大值)/血量最大值
                HpR.fillAmount = (PineHpNow * 0.75f + PineHpMax * 0.25f) / PineHpMax;//Ex:當前血20 血量最大值100 為 (20*75%+100*25%)/100 = 0.4
                break;
            case "Wolf":
                if (PossessedSystem.AttachedBody == this.gameObject)
                {
                    AnimalHealth = GameObject.Find("Player/Wolf").GetComponent<AnimalHealth>();
                    WolfHpMax = AnimalHealth.MaxHealth;
                    WolfHpNow = AnimalHealth.currentHealth;

                    HpW.fillAmount = (WolfHpNow * 0.75f + WolfHpMax * 0.25f) / WolfHpMax;
                    HpR.fillAmount = (WolfHpNow * 0.75f + WolfHpMax * 0.25f) / WolfHpMax;
                }
                break;
                //case "Bear":
                //    AnimalHealth = GameObject.Find("Player/Bear").GetComponent<AnimalHealth>();
                //    BearHpMax = AnimalHealth.MaxHealth;
                //    BearHpNow = AnimalHealth.currentHealth;
                //    HpB.fillAmount = (BearHpNow * 0.75f + BearHpMax * 0.25f) / BearHpMax;
                //    HpW.fillAmount = (BearHpNow * 0.75f + BearHpMax * 0.25f) / BearHpMax;
                //    HpR.fillAmount = (BearHpNow * 0.75f + BearHpMax * 0.25f) / BearHpMax;
                //    break;
                //case "Deer":
                //    AnimalHealth = GameObject.Find("Player/Deer").GetComponent<AnimalHealth>();
                //    DeerHpMax = AnimalHealth.MaxHealth;
                //    DeerHpNow = AnimalHealth.currentHealth;
                //    HpB.fillAmount = (DeerHpNow * 0.75f + DeerHpMax * 0.25f) / DeerHpMax;
                //    HpW.fillAmount = (DeerHpNow * 0.75f + DeerHpMax * 0.25f) / DeerHpMax;
                //    HpR.fillAmount = (DeerHpNow * 0.75f + DeerHpMax * 0.25f) / DeerHpMax;
                //    break;
        }
    }
}
