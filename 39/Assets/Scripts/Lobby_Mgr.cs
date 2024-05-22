using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby_Mgr : MonoBehaviour
{
    public Button Store_Btn;
    public Button MyRoom_Btn;
    public Button Exit_Btn;
    public Button m_GameStartBtn;

    [Header("Google Play Service")]
    public Button m_GoogleBtn;

    [Header("Config")]
    public Button m_ConfigBtn;


    //## 골드
    [HideInInspector]
    public Text m_GoldText;


    // Start is called before the first frame update
    void Start()
    {
        if (Store_Btn != null)
            Store_Btn.onClick.AddListener(StoreBtnClick);

        if (MyRoom_Btn != null)
            MyRoom_Btn.onClick.AddListener(MyRoomBtnClick);

        if (Exit_Btn != null)
            Exit_Btn.onClick.AddListener(ExitBtnClick);

        if (m_GameStartBtn != null)
            m_GameStartBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameScene");
            });


        //## 구글버튼
        if (m_GoogleBtn != null)
            m_GoogleBtn.onClick.AddListener(() =>
            {
                
            });
        if (m_ConfigBtn != null)
            m_ConfigBtn.onClick.AddListener(() =>
            {
                //Debug.Log("설정 버튼 클릭");
               
            });

        //## 골드 불러오기
        m_GoldText.text = GlobalUserData.g_UserGold.ToString();

    }

    private void StoreBtnClick()
    {
        //Debug.Log("상점으로 가기 버튼 클릭");
        SceneManager.LoadScene("StoreScene");
    }

    private void MyRoomBtnClick()
    {
        //Debug.Log("꾸미기 방 가기 버튼 클릭");
        SceneManager.LoadScene("MyRoomScene");
    }

    private void ExitBtnClick()
    {
        Debug.Log("타이틀 씬으로 나가기 버튼 클릭");
        SceneManager.LoadScene("TitleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
