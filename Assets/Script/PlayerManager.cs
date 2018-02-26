using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string NowType, PossessType;
    public GameObject NowCharacter;
    private string PreviousType;
    private TypeValue typevalue;

    //GameObject Player;//控制的角色


    void Awake()
    {
        NowType = "Human";//一開始型態為Human
        typevalue = GetComponent<TypeValue>();
    }

    private void Start()
    {
        NowCharacter = GameObject.Find("Pine");
    }
    void Update()
    {
        if (NowType != PreviousType)//如果數值沒有變化就不做數值改變，反之則要
        {
            switch (NowType)
            {//給予回應 //判斷目前形態為何，不同型態應有不同數值
                case "Human":
                    typevalue.HumanVal();
                    break;
                case "Bear":
                    typevalue.BearVal();
                    break;
                case "Wolf":
                    typevalue.WolfVal();
                    break;
            }
        }
    }

    public void TurnType(string TypeTag, string Previous)//收到物件的回饋
    {
        switch (TypeTag)
        {

            case "Human":
                NowType = "Human";
                PreviousType = Previous;
                break;

            case "Bear":
                NowType = "Bear";
                PreviousType = Previous;
                break;

            case "Wolf":
                NowType = "Wolf";
                PreviousType = Previous;
                break;

        }
    }



}
