using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Mgr : G_Singleton<Sound_Mgr>
{
    [HideInInspector] public AudioSource m_AudioSrc = null;
    Dictionary<string, AudioClip> m_ADClipList = new Dictionary<string, AudioClip>();

    float m_bgmVolume = 0.2f;
    [HideInInspector] public bool m_SoundOn = true;
    [HideInInspector] public float m_SoundVolume = 1.0f;


    //## 효과음 최적화를 위한 버퍼 변수
    int m_EffSdCount = 5; //필요에따라 레이어 조절
    int m_SoundCount = 0; //최대 5개까지 재생되게 제어
    GameObject[] m_SndObjList = new GameObject[10];
    AudioSource[] m_SndSrcList = new AudioSource[10];
    float[] m_EffVoulme = new float[10];



    protected override void Init() //awake 대신 사용
    {
        base.Init();
        //부모 클래스의 Init()을 호출하고 그 다음에 아래 코드를 실행

        LoadChildGameObj();
    }



    // Start is called before the first frame update
    void Start()
    {
        //## 사운드 리소스 로딩
        AudioClip a_GAudioClip = null;
        object[] temp = Resources.LoadAll("Sounds");
        for (int i = 0; i < temp.Length; i++)
        {
            a_GAudioClip = temp[i] as AudioClip;

            if (m_ADClipList.ContainsKey(a_GAudioClip.name) == true)
                continue;

            m_ADClipList.Add(a_GAudioClip.name, a_GAudioClip);

        }


    }



    // Update is called once per frame
    void Update()
    {
       
    }



    void LoadChildGameObj()
    {
        m_AudioSrc = gameObject.AddComponent<AudioSource>();

        //# 게임 효과음을 위한 5개의 레이어 생성 로직
        for (int i = 0; i < m_EffSdCount; i++)
        {
            GameObject newSndObj = new GameObject();
            newSndObj.transform.SetParent(this.transform);
            newSndObj.transform.localPosition = Vector3.zero;

            AudioSource a_AudioSrc = newSndObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;
            a_AudioSrc.name = "SoundEffObj";

            m_SndSrcList[i] = a_AudioSrc;
            m_SndObjList[i] = newSndObj;
        }

        //## 게임 시작시 사운드 로컬 로딩 후 적용
        int a_SoundOn = PlayerPrefs.GetInt("SoundOn", 1);
        if(a_SoundOn == 1)
            SoundOn(true);
        else
            SoundOn(false);

        float a_Value = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
        SoundVolume(a_Value);



    }

    public void PlayBGM(string a_FileName, float fVolume = 0.2f)
    {
        AudioClip a_GAudioClip = null;

        if (m_ADClipList.ContainsKey(a_FileName)== true)
        {
            a_GAudioClip = m_ADClipList[a_FileName];
        }
        else
        {
            a_GAudioClip = Resources.Load("Sounds/" + a_FileName) as AudioClip; // "/"는 파일을 불러와야하기에 사용
            m_ADClipList.Add(a_FileName, a_GAudioClip);

        }

        if (m_AudioSrc == null)
            return;
        if (m_AudioSrc.clip != null && m_AudioSrc.clip.name == a_FileName)
            return;

        m_AudioSrc.clip = a_GAudioClip;
        m_AudioSrc.volume = fVolume * m_SoundVolume;
        m_bgmVolume = fVolume;
        m_AudioSrc.loop = true;
        m_AudioSrc.Play();


    }

    //## GUI 사운드 재생
    public void PlayGUISound(string a_FileName, float fVolume = 0.2f)
    {
        if (m_SoundOn == false)
            return;

        AudioClip a_GAudioClip = null;

        if (m_ADClipList.ContainsKey(a_FileName) == true)
        {
            a_GAudioClip = m_ADClipList[a_FileName];
        }
        else
        {
            a_GAudioClip = Resources.Load("Sounds/" + a_FileName) as AudioClip; // "/"는 파일을 불러와야하기에 사용
            m_ADClipList.Add(a_FileName, a_GAudioClip);

        }

        if (m_AudioSrc == null)
            return;

        m_AudioSrc.PlayOneShot(a_GAudioClip, fVolume * m_SoundVolume);
    }

    public void PlayEffectSound(string a_FIleName, float fVolume = 0.2f)
    {
        if (m_SoundOn == false)
            return;
        AudioClip a_GAudioClip = null;
        if (m_ADClipList.ContainsKey(a_FIleName) == true)
        {
            a_GAudioClip = m_ADClipList[a_FIleName];
        }
        else
        {
            a_GAudioClip = Resources.Load("Sounds/" + a_FIleName) as AudioClip;
            m_ADClipList.Add(a_FIleName, a_GAudioClip);
        }

        if (a_GAudioClip == null)
            return;


        if (m_SndSrcList[m_SoundCount] != null)
        {
            m_SndSrcList[m_SoundCount].volume = 1.0f;
            m_SndSrcList[m_SoundCount].PlayOneShot(a_GAudioClip, fVolume * m_SoundVolume);
            m_EffVoulme[m_SoundCount] = fVolume;

            m_SoundCount++;
            if (m_SoundCount >= m_EffSdCount)
                m_SoundCount = 0;
        }

    }



    public void SoundOn(bool a_On = true)
    {
        bool a_MuteOn = !a_On;

        if (m_AudioSrc != null)
        {
            m_AudioSrc.mute = a_MuteOn;
            //## 리플레이 효과
            //if(a_MuteOn == false)
            //{
            //    m_AudioSrc.time = 0;
            //}
        }

        for (int i = 0; i < m_EffSdCount; i++)
        {
            if (m_SndSrcList[i] != null)
            {
                m_SndSrcList[i].mute = a_MuteOn;

                //## 리플레이 효과
                if (a_MuteOn == false)

                    m_SndSrcList[i].time = 0;

            }
        }


        m_SoundOn = a_On;


    }



    public void SoundVolume(float fVoulme)
    {
        if (m_AudioSrc != null)
        {
            m_AudioSrc.volume = m_bgmVolume * fVoulme;
        }


        //for (int i = 0; i < m_EffSdCount; i++)
        //{
        //    if (m_SndSrcList[i] != null)

        //        m_SndSrcList[i].volume = m_EffVoulme[i] * fVoulme;

        //}

        m_SoundVolume = fVoulme;




    }


}
