using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

enum JoyStickType
{
    Fixed = 0,
    Flexible = 1,
    FlexibleOnOff = 2
}

//Terrain, NaviMesh <-- X, Z ��鿡�� �����Ǵ� ����Ƽ ���

public class Game_Mgr : MonoBehaviour
{
    public static GameObject m_BulletPrefab = null;

    public Button m_BackBtn;

    //--- UserInfo UI ���� ����
    bool m_UInfoOnOff = false;
    [Header("--- UserInfo UI ---")]
    [SerializeField] private Button m_UserInfo_Btn = null;
    public GameObject m_UserInfoPanel = null;
    public Text m_UserHpText;
    public Text m_SkillText;
    public Text m_MonKillText;
    public Text m_GoldText;

    int m_CurGold = 0;          //�̹� ������������ ���� ��尪
    int m_MonKillCount = 0;     //���� ų �� ����
    //--- UserInfo UI ���� ����

    //--- Fixed JoyStick ó�� �κ�
    JoyStickType m_JoyStickType = JoyStickType.Fixed;

    [Header("--- JoyStick ---")]
    public GameObject m_JoySBackObj = null;
    public Image m_JoyStickImg = null;
    float m_Radius = 0.0f;
    Vector3 m_OriginPos = Vector3.zero;
    Vector3 m_Axis = Vector3.zero;
    Vector3 m_JsCacVec = Vector3.zero;
    float m_JsCacDist = 0.0f;
    //--- Fixed JoyStick ó�� �κ�

    //--- Flexible JoyStick ó�� �κ�
    public GameObject m_JoystickPickPanel = null;
    Vector3 posJoyBack;
    Vector3 dirStick;
    //--- Flexible JoyStick ó�� �κ�

    //--- �Ӹ����� ������ ����� ���� ����
    Vector3 m_StCacPos = Vector3.zero;
    [Header("--- Damage Text ---")]
    public Transform m_Damage_Canvas = null;
    public GameObject m_DamageTxtRoot = null;
    //--- �Ӹ����� ������ ����� ���� ����

    //--- Inventory ScrollView
    [Header("--- Inventory ScrollView OnOff ---")]
    public Button m_Inven_Btn = null;
    public Transform m_InvenScrollTr = null;
    bool  m_Inven_ScOnOff = false;
    float m_ScSpeed = 3800.0f;
    Vector3 m_ScOnPos = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 m_ScOffPos = new Vector3(320.0f, 0.0f, 0.0f);

    public Transform  m_MkInvenContent = null;
    public GameObject m_MkItemNode = null;
    public Button m_ItemSell_Btn = null;
    //--- Inventory ScrollView

  
    //## ȯ�漳�� ���� ����
    [Header("Config Setting")]
    public Button m_CfgBtn = null;
    public GameObject Canvas_Dialog = null;
    GameObject m_ConfigBoxObj = null;


    //## ���ӿ��� �г� ���� ����
    [Header("----GameOver Panel----")]
    public GameObject m_GameOverPanel = null;
    public Button m_LobbyBtn = null;
    public Button m_RetryBtn = null;
    public Text m_GameOverText = null;
    public Text m_GameOverGold = null;
    public Text m_GameOverKill = null;
    public Text m_GameOverTime = null;
  
    //���ӿ����� �ִϸ��̼� ó��.
    float time;
    float PlayTime = 0.0f;




    [HideInInspector] public HeroCtrl m_RefHero = null;

    //--- �̱��� ���� ������ ���� �ڵ�
    public static Game_Mgr Inst;

    private void Awake()
    {
        Inst = this;    
    }
    //--- �̱��� ���� ������ ���� �ڵ�

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; //���� ������ �ӵ� 60���������� ���� ��Ű��.. �ڵ�
        QualitySettings.vSyncCount = 0;

        Time.timeScale = 1.0f;
        GlobalUserData.LoadGameInfo();
        ReflashUserInfoUI();
        ReflashInGameItemScV();

        m_RefHero = FindObjectOfType<HeroCtrl>();

        m_BulletPrefab = Resources.Load("BulletPrefab") as GameObject;

        if (m_BackBtn != null)
            m_BackBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbyScene");
            });

#region --- Fixed Joystick ó�� �κ�
        if(m_JoySBackObj != null && m_JoyStickImg != null &&
           m_JoySBackObj.activeSelf == true &&
           m_JoystickPickPanel.activeSelf == false)
        {
            m_JoyStickType = JoyStickType.Fixed;

            Vector3[] v = new Vector3[4];
            m_JoySBackObj.GetComponent<RectTransform>().GetWorldCorners(v);
            //v[0] : �����ϴ�  v[1] : �������   v[2] : �������   v[3] : �����ϴ�
            //v[0] �����ϴ��� 0, 0 ��ǥ�� ��ũ�� ��ǥ(Screen.width, Screen.height)��
            //�������� 
            m_Radius = v[2].y - v[0].y;
            m_Radius = m_Radius / 3.0f;

            m_OriginPos = m_JoyStickImg.transform.position;

            //using UnityEngine.EventSystems;
            EventTrigger trigger = m_JoySBackObj.GetComponent<EventTrigger>();
            //m_JoySBackObj �� AddComponent --> EventTrigger �� �߰� �Ǿ� �־�� �Ѵ�.
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) =>
            {
                OnDragJoyStick((PointerEventData)data);
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) =>
            {
                OnEndDragJoyStick((PointerEventData)data);
            });
            trigger.triggers.Add(entry);    
        }
        #endregion

#region --- Flexible Joystick ó�� �κ�

        if(m_JoystickPickPanel != null && m_JoySBackObj != null &&
            m_JoyStickImg != null &&
            m_JoystickPickPanel.activeSelf == true)
        {
            if (m_JoySBackObj.activeSelf == true)
                m_JoyStickType = JoyStickType.Flexible;
            else
                m_JoyStickType = JoyStickType.FlexibleOnOff;

            Vector3[] v = new Vector3[4];
            m_JoySBackObj.GetComponent<RectTransform>().GetWorldCorners(v);
            m_Radius = v[2].y - v[0].y;
            m_Radius = m_Radius / 3.0f;

            m_OriginPos = m_JoyStickImg.transform.position;
            m_JoySBackObj.GetComponent<Image>().raycastTarget = false;
            m_JoyStickImg.raycastTarget = false;

            EventTrigger trigger = m_JoystickPickPanel.GetComponent<EventTrigger>();
            EventTrigger.Entry  entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) =>
            {
                OnPointerDown_Flx((PointerEventData)data);
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) =>
            {
                OnPointerUp_Flx((PointerEventData)data);
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) =>
            {
                OnDragJoyStick_Flx((PointerEventData)data);
            });
            trigger.triggers.Add(entry);

        }//if(m_JoystickPickPanel != null && m_JoySBackObj != null &&

        #endregion

        //--- UserInfoPanel On/Off  ���� �ڵ�
        m_UInfoOnOff = m_UserInfoPanel.activeSelf;

        if (m_UserInfo_Btn != null)
            m_UserInfo_Btn.onClick.AddListener(() =>
            {
                m_UInfoOnOff = !m_UInfoOnOff;
                if (m_UserInfoPanel != null)
                    m_UserInfoPanel.SetActive(m_UInfoOnOff);
            });
        //--- UserInfoPanel On/Off  ���� �ڵ�

        //--- �κ��丮 �ǳ� OnOff
        if (m_Inven_Btn != null)
            m_Inven_Btn.onClick.AddListener(() =>
            {
                m_Inven_ScOnOff = !m_Inven_ScOnOff;
                if (m_ItemSell_Btn != null)
                    m_ItemSell_Btn.gameObject.SetActive(m_Inven_ScOnOff);
            });

        if (m_ItemSell_Btn != null)
            m_ItemSell_Btn.onClick.AddListener(ItemSelMethod);
        //--- �κ��丮 �ǳ� OnOff

        //## ȯ�漳�� ��ư ó��
        if(m_CfgBtn != null)
            m_CfgBtn.onClick.AddListener(() =>
            {
                if (m_ConfigBoxObj == null)
                {
                    m_ConfigBoxObj = Resources.Load("ConfigBox") as GameObject;

                }
                GameObject a_CfgBoxObj = Instantiate(m_ConfigBoxObj);
                a_CfgBoxObj.transform.SetParent(Canvas_Dialog.transform, false);
                //false : ���� ������ ������ ������ ä(�����ϵ� ����) ���ϵ�ȭ �ȴ�.
                Time.timeScale = 0.0f;


            });

        //## ���ӿ��� �г� ��ư ó��
        


    }// void Start()

    // Update is called once per frame
    void Update()
    {
        if (m_UserHpText != null && m_RefHero != null)
            m_UserHpText.text = "HP : " + m_RefHero.m_CurHp + " / " + m_RefHero.m_MaxHp;

        InvenScOnOffUpdate();


        PlayTime += Time.deltaTime;


        // ## ���� ���� �г��� Ȱ��ȭ�Ǿ� ���� ���� �ִϸ��̼� ����
        if (m_GameOverPanel.activeSelf)
        {
            m_GameOverText.transform.localScale = Vector3.one * (1 + Mathf.PingPong(time, 1f) - 0.5f);
            time += Time.deltaTime;
        }

    }

#region --- Fixed Joystick ó�� �κ�

    private void OnDragJoyStick(PointerEventData a_data)
    {
        //(Vector3)a_data.position : ���콺 ��ǥ
        if (m_JoyStickImg == null)
            return;

        m_JsCacVec = (Vector3)a_data.position - m_OriginPos;
        m_JsCacVec.z = 0.0f;
        m_JsCacDist = m_JsCacVec.magnitude;
        m_Axis = m_JsCacVec.normalized;

        //���̽�ƽ ��׶��带 ����� ���ϰ� ���� �κ�
        if(m_Radius < m_JsCacDist)
        {
            m_JoyStickImg.transform.position = 
                            m_OriginPos + m_Axis * m_Radius; 
        }
        else
        {
            m_JoyStickImg.transform.position = 
                            m_OriginPos + m_Axis * m_JsCacDist;
        }

        if (1.0f < m_JsCacDist)
            m_JsCacDist = 1.0f;

        //ĳ���� �̵�ó��
        if (m_RefHero != null)
            m_RefHero.SetJoyStickMv(m_JsCacDist, m_Axis);
    }

    private void OnEndDragJoyStick(PointerEventData data)
    {
        if (m_JoyStickImg == null)
            return;

        m_Axis = Vector3.zero;
        m_JoyStickImg.transform.position = m_OriginPos;
        m_JsCacDist = 0.0f;

        //ĳ���� �̵�����
        if (m_RefHero != null)
            m_RefHero.SetJoyStickMv(0.0f, Vector3.zero);
    }

    #endregion


#region --- Flexible  Joystick ó���κ�
    private void OnPointerDown_Flx(PointerEventData data) //���콺 Ŭ����
    {
        if (data.button != PointerEventData.InputButton.Left)
            return;  //�����Ϳ��� ���콺 ���� ��ư Ŭ���� �ƴϸ� ����

        if(m_JoySBackObj == null)
            return;

        if (m_JoyStickImg == null)
            return;

        m_JoySBackObj.transform.position = data.position;
        m_JoyStickImg.transform.position = data.position;

        m_JoySBackObj.SetActive(true);
    }

    private void OnPointerUp_Flx(PointerEventData data) //���콺 Ŭ�� ������
    {
        if (data.button != PointerEventData.InputButton.Left)
            return;

        if(m_JoySBackObj == null)
            return;

        if(m_JoyStickImg == null)
            return;

        m_JoySBackObj.transform.position = m_OriginPos;
        m_JoyStickImg.transform.position = m_OriginPos;

        if(m_JoyStickType == JoyStickType.FlexibleOnOff)
        {
            m_JoySBackObj.SetActive(false); //<-- ���� ���·� �����ϴ� ����� ���� ��Ȱ��ȭ �ʿ�
        }

        m_Axis = Vector3.zero;
        m_JsCacDist = 0.0f;

        //ĳ���� ���� ó��
        if (m_RefHero != null)
            m_RefHero.SetJoyStickMv(0.0f, Vector3.zero);
    }

    private void OnDragJoyStick_Flx(PointerEventData data)
    {
        if(data.button != PointerEventData.InputButton.Left)
            return;

        if(m_JoyStickImg == null)
            return;

        posJoyBack = m_JoySBackObj.transform.position;
        //���̽�ƽ �� �׶��� ���� ��ġ ����
        m_JsCacVec = data.position - (Vector2)posJoyBack;
        m_JsCacVec.z = 0.0f;
        m_JsCacDist = m_JsCacVec.magnitude; //�Ÿ�
        m_Axis = m_JsCacVec.normalized;   //����

        //���̽�ƽ ��׶��带 ����� ���ϰ� ���� �κ�
        if (m_Radius < m_JsCacDist)
        {
            m_JsCacDist = m_Radius;
            m_JoyStickImg.transform.position = 
                                    posJoyBack + m_Axis * m_Radius;
        }
        else
        {
            m_JoyStickImg.transform.position = data.position;
        }

        if (1.0f < m_JsCacDist)
            m_JsCacDist = 1.0f;

        //ĳ���� �̵�ó��
        if (m_RefHero != null)
            m_RefHero.SetJoyStickMv(m_JsCacDist, m_Axis);

    }
#endregion

    public void DamageText(int a_Value, Vector3 a_OwnerPos)
    {
        GameObject a_DmgClone = Instantiate(m_DamageTxtRoot);
        if(a_DmgClone != null && m_Damage_Canvas != null)
        {
            Vector3 a_StCacPos = new Vector3(a_OwnerPos.x, 0.8f, a_OwnerPos.z + 4.0f);
            a_DmgClone.transform.SetParent(m_Damage_Canvas);
            DamageText a_DamageTx = a_DmgClone.GetComponent<DamageText>();
            a_DamageTx.DamageTxtSpawn(a_Value, new Color32(200, 0, 0, 255));
            a_DmgClone.transform.position = a_StCacPos;
        }
    }//public void DamageText(int a_Value, Vector3 a_OwnerPos)

    public static bool IsPointerOverUIObject() //UGUI�� UI���� ���� ��ŷ�Ǵ��� Ȯ���ϴ� �Լ�
    {
        PointerEventData a_EDCurPos = new PointerEventData(EventSystem.current);

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)

			List<RaycastResult> results = new List<RaycastResult>();
			for (int i = 0; i < Input.touchCount; ++i)
			{
				a_EDCurPos.position = Input.GetTouch(i).position;  
				results.Clear();
				EventSystem.current.RaycastAll(a_EDCurPos, results);
                if (0 < results.Count)
                    return true;
			}

			return false;
#else
        a_EDCurPos.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(a_EDCurPos, results);
        return (0 < results.Count);
#endif
    }//public bool IsPointerOverUIObject() 

    void InvenScOnOffUpdate()
    {   //�κ��丮 �ǳ� OnOff ���� �Լ�

        if (m_InvenScrollTr == null)
            return;

        if(m_Inven_ScOnOff == false)
        {
            if(m_InvenScrollTr.localPosition.x < m_ScOffPos.x)
            {
                m_InvenScrollTr.localPosition =
                        Vector3.MoveTowards(m_InvenScrollTr.localPosition,
                                         m_ScOffPos, m_ScSpeed * Time.deltaTime);
            }
        }
        else //if(m_Inven_ScOnOff == true)
        {
            if (m_ScOnPos.x < m_InvenScrollTr.localPosition.x )
            {
                m_InvenScrollTr.localPosition =
                        Vector3.MoveTowards(m_InvenScrollTr.localPosition,
                                        m_ScOnPos, m_ScSpeed * Time.deltaTime);
            }

        }//else //if(m_Inven_ScOnOff == true)
    }//void InvenScOnOffUpdate()

    public void InvenAddItem(GameObject a_Obj)
    {
        ItemObjInfo a_RefItemInfo = a_Obj.GetComponent<ItemObjInfo>();
        if(a_RefItemInfo != null)
        {
            ItemValue a_Node = new ItemValue();
            a_Node.UniqueID    = a_RefItemInfo.m_ItemValue.UniqueID;
            a_Node.m_Item_Type = a_RefItemInfo.m_ItemValue.m_Item_Type;
            a_Node.m_ItemName  = a_RefItemInfo.m_ItemValue.m_ItemName;
            a_Node.m_ItemLevel = a_RefItemInfo.m_ItemValue.m_ItemLevel;
            a_Node.m_ItemStar  = a_RefItemInfo.m_ItemValue.m_ItemStar;
            GlobalUserData.g_ItemList.Add(a_Node);

            AddNodeScrollView(a_Node);  //��ũ�� �信 �߰�
            GlobalUserData.ReflashItemSave(); //���� ����
        }
    }

    void AddNodeScrollView(ItemValue a_Node)
    {
        GameObject a_ItemObj = Instantiate(m_MkItemNode);
        a_ItemObj.transform.SetParent(m_MkInvenContent, false);
        // false �� ��� : ���� ������ ������ ������ ä ���ϵ�ȭ �ȴ�.

        ItemNode a_MyItemInfo = a_ItemObj.GetComponent<ItemNode>();
        if (a_MyItemInfo != null)
            a_MyItemInfo.SetItemRsc(a_Node);
    }

    private void ReflashInGameItemScV()  //<--- InGame ScrollView ����
    { //GlobalUserData.g_ItemList ����� ���� scrollView �� ������ �ִ� �Լ�
        ItemNode[] a_MyNodeList = 
                    m_MkInvenContent.GetComponentsInChildren<ItemNode>(true);
        for(int i = 0; i < a_MyNodeList.Length; i++)
        {
            Destroy(a_MyNodeList[i].gameObject);
        }

        for(int i = 0; i < GlobalUserData.g_ItemList.Count; i++)
        {
            AddNodeScrollView(GlobalUserData.g_ItemList[i]);    
        }
    }//private void ReflashInGameItemScV()  //<--- InGame ScrollView ����

    private void ItemSelMethod()
    {
        //������ �ϳ��� 100������ �Ǹ�

        //��ũ�Ѻ��� ��带 ��� ���鼭 ���õǾ� �ִ� �͵鸸 �Ǹ��ϰ� 
        //�ش� ����ũ ID�� g_ItemList���� ã�Ƽ� ������ �ش�.
        ItemNode[] a_MyNodeList =
                    m_MkInvenContent.GetComponentsInChildren<ItemNode>(true);
        //true : Active�� ���� �ִ� ������Ʈ���� ��� �������� �ɼ�
        for(int i = 0; i < a_MyNodeList.Length; i++)
        {
            if (a_MyNodeList[i].m_SelOnOff == false)
                continue;

            //--- �۷ι� ����Ʈ���� �Ǹ��Ϸ��� �������� ������ȣ�� ã�Ƽ� ����Ʈ������ ������ ��� �Ѵ�.
            for (int b = 0; b < GlobalUserData.g_ItemList.Count; b++)
            {
                if (a_MyNodeList[i].m_UniqueID == GlobalUserData.g_ItemList[b].UniqueID)
                {
                    GlobalUserData.g_ItemList.RemoveAt(b);
                    break;
                }
            }//for(int b = 0; b < GlobalUserData.g_ItemList.Count; b++)
            //--- �۷ι� ����Ʈ���� �Ǹ��Ϸ��� �������� ������ȣ�� ã�Ƽ� ����Ʈ������ ������ ��� �Ѵ�.

            Destroy(a_MyNodeList[i].gameObject);
            AddGold(100);  //��� ����

        }//for(int i = 0; i < a_MyNodeList.Length; i++)

        GlobalUserData.ReflashItemSave();   //����Ʈ �ٽ� ����

    }//private void ItemSelMethod()

    public void AddMonKill(int a_Val = 1)
    {
        m_MonKillCount += a_Val;
        m_MonKillText.text = "x " + m_MonKillCount.ToString();
    }

    public void AddGold(int a_Val = 5)
    {
        m_CurGold += a_Val; //�̹� ������������ ���� ��尪

        if (GlobalUserData.g_UserGold <= int.MaxValue - a_Val)
            GlobalUserData.g_UserGold += a_Val;
        else
            GlobalUserData.g_UserGold = int.MaxValue;

        m_GoldText.text = "x " + GlobalUserData.g_UserGold.ToString("N0");
        //"N0" õ�������� ��ǥ ǥ��

        PlayerPrefs.SetInt("GoldCount", GlobalUserData.g_UserGold);  //�� ����
    }

    public void AddBombSkill(int a_Val = 1)
    {
        GlobalUserData.g_BombCount += a_Val;

        if (GlobalUserData.g_BombCount <= 0)
            m_SkillText.text = "x 00";
        else
            m_SkillText.text = "x " + GlobalUserData.g_BombCount.ToString();

        PlayerPrefs.SetInt("BombCount", GlobalUserData.g_BombCount); //�� ����
    }

    public void ReflashUserInfoUI()
    {
        if(m_GoldText != null)
        {
            if (GlobalUserData.g_UserGold <= 0)
                m_GoldText.text = "x 00";
            else
                m_GoldText.text = "x " + GlobalUserData.g_UserGold.ToString("N0");
        }

        if(m_SkillText != null)
        {
            if (GlobalUserData.g_BombCount <= 0)
                m_SkillText.text = "x 00";
            else
                m_SkillText.text = "x " + GlobalUserData.g_BombCount.ToString();
        }
    }

    public void GameOver()
    {
        //�г� Ȱ��ȭ
        m_GameOverPanel.SetActive(true);


        //�ִϸ��̼� �ʱ�ȭ
        ResetAnim();


        //��ư Ȱ��ȭ
        m_LobbyBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LobbyScene");
        });

        m_RetryBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });


        m_GameOverText.text = "Game Over";
        m_GameOverGold.text = "ȹ���� : " + m_CurGold.ToString();
        m_GameOverKill.text = "ų�� : " + m_MonKillCount.ToString();
        m_GameOverTime.text = "�÷��̽ð� : " + TimeSpan.FromSeconds(PlayTime).ToString(@"hh\:mm\:ss");



        
    }


    public void ResetAnim()
    {
        time = 0;
      
        m_GameOverText.transform.localScale = Vector3.one;
    }


}//public class Game_Mgr : MonoBehaviour