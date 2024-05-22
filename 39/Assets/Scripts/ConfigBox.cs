using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigBox : MonoBehaviour
{
    public Button m_OK_Btn = null;
    public Button m_Close_Btn = null;


    public InputField Nick_InputField = null;

    public Toggle m_Sound_Toggle = null;
    public Slider m_Sound_Slider = null;

    [HideInInspector] public HeroCtrl m_RefHero = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_OK_Btn != null)

            m_OK_Btn.onClick.AddListener(OKBtnClick);

        if (m_Close_Btn != null)

            m_Close_Btn.onClick.AddListener(CloseBtnClick);

        if (m_Sound_Toggle != null)

            m_Sound_Toggle.onValueChanged.AddListener(SoundOnOff);

        //OnValueChanged : 슬라이더/토글값이 변경되었을때
        //호출되는 함수를 대기시키는 함수

        if (m_Sound_Slider != null)

            m_Sound_Slider.onValueChanged.AddListener(SoundSliderChange);

        m_RefHero = FindObjectOfType<HeroCtrl>();

        //## 체크상태 & 슬라이드 상태 & 닉네임 로딩후 UI컨트롤에 적용
        Text a_Placeholder = null;
        if(Nick_InputField != null)
        {
            Transform a_PLHTr = Nick_InputField .transform.Find("Placeholder");
            a_Placeholder = a_PLHTr.GetComponent<Text>();
            if(a_Placeholder != null)
                a_Placeholder.text = PlayerPrefs.GetString("UserNick", "유니티양");

           // Nick_InputField.text = PlayerPrefs.GetString("UserNick", "유니티양");
        }



    }

    private void SoundSliderChange(float arg0)
    {

    }

    private void SoundOnOff(bool arg0)
    {

    }

    private void CloseBtnClick()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    private void OKBtnClick()
    {
        //## 닉네임 주인공 머리위에 적용
        if (Nick_InputField != null && Nick_InputField.text.Trim() != "")
        {
            string NickStr = Nick_InputField.text.Trim();
            if (m_RefHero != null)
            
                m_RefHero.ChangeNickName(NickStr);

            
            
            
                GlobalUserData.g_NickName = NickStr;
                PlayerPrefs.SetString("UserNick", NickStr);

            
        }

        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
