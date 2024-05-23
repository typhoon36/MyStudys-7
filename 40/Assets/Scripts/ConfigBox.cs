using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigBox : MonoBehaviour
{
    public Button m_Ok_Btn = null;
    public Button m_Close_Btn = null;

    public InputField NickInputField = null;

    public Toggle m_Sound_Toggle = null;
    public Slider m_Sound_Slider = null;

    HeroCtrl m_RefHero = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Ok_Btn != null)
            m_Ok_Btn.onClick.AddListener(OkBtnClick);

        if (m_Close_Btn != null)
            m_Close_Btn.onClick.AddListener(CloseBtnClick);

        if (m_Sound_Toggle != null) 
            m_Sound_Toggle.onValueChanged.AddListener(SoundOnOff);
        //체크 상태가 변경되었을 때 호출되는 함수를 대기하는 코드

        if (m_Sound_Slider != null)
            m_Sound_Slider.onValueChanged.AddListener(SliderChanged);
        //슬라이드 상태가 변경 되었을 때 호출되는 함수 대기하는 코드

        m_RefHero = FindObjectOfType<HeroCtrl>();
        //Hierarchy쪽에서 HeroCtrl 컴포넌트가 붙어있는 게임오브젝트를 찾아서 객체를 찾아오는 방법

        //--- 체크상태, 슬라이드상태, 닉네임 로딩 후 UI 컨트롤에 적용
        int a_SoundOn = PlayerPrefs.GetInt("SoundOn", 1);
        if (m_Sound_Toggle != null)
        {
            //if(a_SoundOn == 1)

            //    m_Sound_Toggle.isOn = true;

            //else

            //    m_Sound_Toggle.isOn = false;

            m_Sound_Toggle.isOn = (a_SoundOn == 1) ? true : false;

        }

        if (m_Sound_Slider != null)

            m_Sound_Slider.value = PlayerPrefs.GetFloat("SoundVolume", 0.1f);


        //Text a_Placehoder = null;
        if (NickInputField != null)
        {
            //Transform a_PLHTr = NickInputField.transform.Find("Placeholder");
            //a_Placehoder = a_PLHTr.GetComponent<Text>();
            //if (a_Placehoder != null)
            //    a_Placehoder.text = PlayerPrefs.GetString("UserNick", "사냥꾼");
            NickInputField.text = PlayerPrefs.GetString("UserNick", "사냥꾼");
        }


       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OkBtnClick()
    {
        //--- 닉네임 주인공 머리위에 적용
        if(NickInputField != null && NickInputField.text.Trim() != "")
        {
            string NickStr = NickInputField.text.Trim();
            if (m_RefHero != null)
                m_RefHero.ChangeNickName(NickStr);

            GlobalUserData.g_NickName = NickStr;
            PlayerPrefs.SetString("UserNick", NickStr);
  
        }//if(NickInputField != null && NickInputField.text.Trim() != "")
        //--- 닉네임 주인공 머리위에 적용

        Time.timeScale = 1.0f;  //일시정지 풀어주기
        Destroy(gameObject);
    }

    private void CloseBtnClick()
    {
        Time.timeScale = 1.0f;  //일시정지 풀어주기
        Destroy(gameObject);
    }

    private void SoundOnOff(bool value)
    {
        //## 체크상태 저장
        //if(value == true)
        //    PlayerPrefs.SetInt("SoundOn", 1);
        //else
        //    PlayerPrefs.SetInt("SoundOn", 0);


        int a_IntV = (value == true) ? 1 : 0;
        PlayerPrefs.SetInt("SoundOn", a_IntV);

        Sound_Mgr.Inst.SoundOn(value);
        
    }




    private void SliderChanged(float value)
    {
        //## 슬라이드 상태 저장
        PlayerPrefs.SetFloat("SoundVolume", value);
        Sound_Mgr.Inst.SoundVolume(value);

    }




}
