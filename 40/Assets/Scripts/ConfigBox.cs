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
        //üũ ���°� ����Ǿ��� �� ȣ��Ǵ� �Լ��� ����ϴ� �ڵ�

        if (m_Sound_Slider != null)
            m_Sound_Slider.onValueChanged.AddListener(SliderChanged);
        //�����̵� ���°� ���� �Ǿ��� �� ȣ��Ǵ� �Լ� ����ϴ� �ڵ�

        m_RefHero = FindObjectOfType<HeroCtrl>();
        //Hierarchy�ʿ��� HeroCtrl ������Ʈ�� �پ��ִ� ���ӿ�����Ʈ�� ã�Ƽ� ��ü�� ã�ƿ��� ���

        //--- üũ����, �����̵����, �г��� �ε� �� UI ��Ʈ�ѿ� ����
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
            //    a_Placehoder.text = PlayerPrefs.GetString("UserNick", "��ɲ�");
            NickInputField.text = PlayerPrefs.GetString("UserNick", "��ɲ�");
        }


       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OkBtnClick()
    {
        //--- �г��� ���ΰ� �Ӹ����� ����
        if(NickInputField != null && NickInputField.text.Trim() != "")
        {
            string NickStr = NickInputField.text.Trim();
            if (m_RefHero != null)
                m_RefHero.ChangeNickName(NickStr);

            GlobalUserData.g_NickName = NickStr;
            PlayerPrefs.SetString("UserNick", NickStr);
  
        }//if(NickInputField != null && NickInputField.text.Trim() != "")
        //--- �г��� ���ΰ� �Ӹ����� ����

        Time.timeScale = 1.0f;  //�Ͻ����� Ǯ���ֱ�
        Destroy(gameObject);
    }

    private void CloseBtnClick()
    {
        Time.timeScale = 1.0f;  //�Ͻ����� Ǯ���ֱ�
        Destroy(gameObject);
    }

    private void SoundOnOff(bool value)
    {
        //## üũ���� ����
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
        //## �����̵� ���� ����
        PlayerPrefs.SetFloat("SoundVolume", value);
        Sound_Mgr.Inst.SoundVolume(value);

    }




}
