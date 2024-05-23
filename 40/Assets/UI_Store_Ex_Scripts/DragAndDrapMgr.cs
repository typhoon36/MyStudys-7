using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrapMgr : MonoBehaviour
{
    public SlotScript[] m_SlotSc;
    public RawImage m_MsObj = null;

    int m_SaveIndex = -1;
    int m_DrtIndex = -1;        //Direction Index
    //bool m_IsPick = false;

    //--- ������ �����ϰ� ������� �ϱ� ����� ����
    float AniDuring  = 0.8f; //���̵� �ƿ� ���� �ð� ����
    float m_CacTime  = 0.0f;
    float m_AddTimer = 0.0f;
    Color m_Color;
    //--- ������ �����ϰ� ������� �ϱ� ����� ����

    [Header("--- Display TextUI ---")]
    public Text m_GoldText;
    public Text m_SkillText;

    [Header("--- Help TextUI ---")]
    public Text m_HelpText;
    float m_HelpDuring = 1.5f;  //���̵�ƿ� ���� �ð�
    float m_HelpTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GlobalUserData.LoadGameInfo();

        if(m_GoldText != null)
        {
            if (GlobalUserData.g_UserGold <= 0)
                m_GoldText.text = "x 00";
            else
                m_GoldText.text = "x " + GlobalUserData.g_UserGold.ToString();
        }//if(m_GoldText != null)

        if (m_SkillText != null)
        {
            if (GlobalUserData.g_BombCount <= 0)
                m_SkillText.text = "x 00";
            else
                m_SkillText.text = "x " + GlobalUserData.g_BombCount.ToString();
        }//if (m_SkillText != null)
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) == true)  //���� ���콺 ��ư�� Ŭ���� ����
        {
            BuyMouseBtnDown();
        }

        if(Input.GetMouseButton(0) == true)     //���� ���콺�� ������ �ִ� ����
        {
            BuyMouseBtnPress();
        }

        if(Input.GetMouseButtonUp(0) == true)  //���� ���콺�� ������ �ִٰ� �� ����
        {
            BuyMouseBtnUp();
        }

        BuyDirection();
    }

    bool IsCollSlot(GameObject a_CkObj)  //���콺�� UI ���� ������Ʈ ���� ������? �Ǵ��ϴ� �Լ�
    {
        Vector3[] v = new Vector3[4];
        a_CkObj.GetComponent<RectTransform>().GetWorldCorners(v);

        if (v[0].x <= Input.mousePosition.x && Input.mousePosition.x <= v[2].x &&
            v[0].y <= Input.mousePosition.y && Input.mousePosition.y <= v[2].y)
        {
            return true;
        }

        return false;
    }

    private void BuyMouseBtnDown()
    {
        m_SaveIndex = -1;

        for(int i = 0; i < m_SlotSc.Length; i++)
        {
            //���� Ȯ�� ���Կ������� ������ ���ϰڴٴ� �ǹ̷� ���� Ȯ�� ����(������ ����)�� ��� ��ŵ
            if (i == 1)
                continue;

            if (m_SlotSc[i].ItemImg.gameObject.activeSelf == true &&
                IsCollSlot(m_SlotSc[i].gameObject) == true)
            {
                m_SaveIndex = i;
                m_SlotSc[i].ItemImg.gameObject.SetActive(false);
                m_MsObj.gameObject.SetActive(true);
                m_MsObj.transform.position = Input.mousePosition;
                break;
            }
        }//for(int i = 0; i < m_SlotSc.Length; i++)
    }//private void BuyMouseBtnDown()

    private void BuyMouseBtnPress()
    {
        if(0 <= m_SaveIndex)
        {
            m_MsObj.transform.position = Input.mousePosition;
        }
    }

    //private void BuyMouseBtnUp()  //������ �ڵ�
    //{
    //    if (m_SaveIndex < 0)
    //        return;

    //    for(int i = 0; i < m_SlotSc.Length; i++)
    //    {
    //        if (m_SlotSc[i].ItemImg.gameObject.activeSelf == false &&
    //            IsCollSlot(m_SlotSc[i].gameObject) == true)
    //        {
    //            m_SlotSc[i].ItemImg.gameObject.SetActive(true);
    //            m_SlotSc[i].ItemImg.color = Color.white;
    //            m_DrtIndex = i;
    //            m_SaveIndex = -1;
    //            m_MsObj.gameObject.SetActive(false);
    //            break;
    //        }
    //    }

    //    if(0 <= m_SaveIndex)
    //    {
    //        m_SlotSc[m_SaveIndex].ItemImg.gameObject.SetActive(true);
    //        m_SaveIndex = -1;
    //        m_MsObj.gameObject.SetActive(false);
    //    }

    //}//private void BuyMouseBtnUp()

    private void BuyMouseBtnUp()
    {
        if (m_SaveIndex < 0)
           return;

        for(int i = 0; i < m_SlotSc.Length; i++)
        {
            if(m_SaveIndex == i)  //�ڱ� �ڸ��� ���� ��� ���� �Ұ�
                continue;

            if (m_SlotSc[i].ItemImg.gameObject.activeSelf == false &&
                IsCollSlot(m_SlotSc[i].gameObject) == true)
            {
                //--- ���� �㰡
                if(2000 <= GlobalUserData.g_UserGold)
                {
                    m_SlotSc[i].ItemImg.gameObject.SetActive(true);
                    m_SlotSc[i].ItemImg.color = Color.white;
                    m_DrtIndex = i;
                    m_AddTimer = AniDuring; 
                    //m_SaveIndex = -1;
                    m_MsObj.gameObject.SetActive(false);

                    GlobalUserData.g_UserGold -= 2000;
                    m_GoldText.text = "x " + GlobalUserData.g_UserGold.ToString("N0");
                    PlayerPrefs.SetInt("GoldCount", GlobalUserData.g_UserGold); //�� ����

                    GlobalUserData.g_BombCount += 1;
                    m_SkillText.text = "x " + GlobalUserData.g_BombCount.ToString();
                    PlayerPrefs.SetInt("BombCount", GlobalUserData.g_BombCount); //�� ����

                }//if(100 <= GlobalUserData.g_UserGold)
                else  //���� �Ұ�
                {
                    m_HelpText.gameObject.SetActive(true);
                    m_HelpText.color = Color.white;
                    m_HelpTimer = m_HelpDuring;
                }

                break;
            }//if (m_SlotSc[i].ItemImg.gameObject.activeSelf == false &&
        }//for(int i = 0; i < m_SlotSc.Length; i++)

        //if(0 <= m_SaveIndex)
        {
            m_SlotSc[m_SaveIndex].ItemImg.gameObject.SetActive(true);
            m_SaveIndex = -1;
            m_MsObj.gameObject.SetActive(false);
        }

    }//private void BuyMouseBtnUp()

    void BuyDirection() //���� ���� �Լ�
    {
        //--- ������ �������� ������ ������� ó���ϴ� ����
        if(0.0f < m_AddTimer)
        {
            m_AddTimer -= Time.deltaTime;
            m_CacTime = m_AddTimer / AniDuring;
            m_Color = m_SlotSc[m_DrtIndex].ItemImg.color;
            m_Color.a = m_CacTime;
            m_SlotSc[m_DrtIndex].ItemImg.color = m_Color;

            if(m_AddTimer <= 0.0f)
            {
                m_SlotSc[m_DrtIndex].ItemImg.gameObject.SetActive(false);
            }
        }
        //--- ������ �������� ������ ������� ó���ϴ� ����

    }//void BuyDirection() //���� ���� �Լ�
}
