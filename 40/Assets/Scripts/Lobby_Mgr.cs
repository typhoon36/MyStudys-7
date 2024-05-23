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

    public Text GoldText;

    [Header("google play service")]
    public Button m_GooglePlayLoginBtn;

    [Header("Config")]
    public Button config_Btn;
    public GameObject m_ConfigBoxObj;
    public GameObject Config_Canvas = null;


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

        if (config_Btn != null)
            config_Btn.onClick.AddListener(() =>
            {
                if (m_ConfigBoxObj == null)
                    m_ConfigBoxObj = Resources.Load("ConfigBox") as GameObject;

                GameObject a_CfgBoxObj = Instantiate(m_ConfigBoxObj);
                a_CfgBoxObj.transform.SetParent(GameObject.Find("Config_Canvas").transform, false);
                Time.timeScale = 0.0f;  //일시정지 효과
            });


        Sound_Mgr.Inst.PlayBGM("sound_bgm_village_001", 0.2f);

        GoldText.text = GlobalUserData.g_UserGold.ToString();


    }

    private void StoreBtnClick()
    {
        //Debug.Log("상점으로 가기 버튼 클릭");
        SceneManager.LoadScene("StoreScene");
        Sound_Mgr.Inst.PlayGUISound("Pop", 0.4f);
    }

    private void MyRoomBtnClick()
    {
        //Debug.Log("꾸미기 방 가기 버튼 클릭");
        SceneManager.LoadScene("MyRoomScene");
        Sound_Mgr.Inst.PlayGUISound("Pop", 0.4f);
    }

    private void ExitBtnClick()
    {
        Debug.Log("타이틀 씬으로 나가기 버튼 클릭");
        SceneManager.LoadScene("TitleScene");
        Sound_Mgr.Inst.PlayGUISound("Pop", 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
