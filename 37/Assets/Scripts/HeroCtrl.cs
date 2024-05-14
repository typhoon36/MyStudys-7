using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrl : MonoBehaviour
{
    [HideInInspector] public float m_MaxHp = 200.0f;
    [HideInInspector] public float m_CurHp = 200.0f;
    public Image m_HpBar = null;

    //--- Ű���� �̵� ���� ���� ����
    float h, v;                 //Ű���� �Է°��� �ޱ� ���� ����
    float m_MoveSpeed = 10.0f;  //�ʴ� 10m �̵��ӵ�

    Vector3 m_DirVec;           //�̵��Ϸ��� ���� ���� ����
    //--- Ű���� �̵� ���� ���� ����

    //--- ��ǥ ���� ������...
    Vector3 m_CurPos;
    Vector3 m_CacEndVec;
    //--- ��ǥ ���� ������...

    //--- �Ѿ� �߻� ���� ���� ����
    float m_AttSpeed   = 0.1f;  //���ݼӵ�(����)
    float m_CacAtTick  = 0.0f;  //����� �߻� �ֱ� �����..
    float m_ShootRange = 30.0f; //��Ÿ�
    //--- �Ѿ� �߻� ���� ���� ����

    //--- JoyStick �̵� ó�� ����
    float m_JoyMvLen = 0.0f;
    Vector3 m_JoyMvDir = Vector3.zero;
    //--- JoyStick �̵� ó�� ����

    //--- ���콺 Ŭ�� �̵� ���� ���� (Mouse Picking Move)
    [HideInInspector] public bool m_bMoveOnOff = false; //���� ���콺 ��ŷ���� �̵� ������? �� ����
    Vector3 m_TargetPos;    //���콺 ��ŷ ��ǥ��
    float m_CacStep;        //�ѽ��� ���� ����

    Vector3 m_PickVec = Vector3.zero;
    public ClickMark m_ClickMark = null;
    //--- ���콺 Ŭ�� �̵� ���� ���� (Mouse Picking Move)

    //--- �ִϸ��̼� ���� ����
    AnimSequence m_AnimSeq;
    Quaternion m_CacRot;
    //--- �ִϸ��̼� ���� ����


    //## �Ӹ��� �г���
    public Text m_NickName = null;


    // Start is called before the first frame update
    void Start()
    {
        m_AnimSeq = gameObject.GetComponentInChildren<AnimSequence>();
        //���ϵ� �� ù��°�� ������ AnimSequence.cs ���� ã�ƿ���

        if(m_NickName != null)
        
            m_NickName.text = PlayerPrefs.GetString("NickName", "����Ƽ��");
        

    }

    // Update is called once per frame
    void Update()
    {
        MousePickCtrl();

        KeyBDUpdate();
        JoyStickMvUpdate();
        MousePickUpdate();

        LimitMove(); //���ΰ� ĳ���Ͱ� ������ ����� ���ϰ� ����

        //--- �Ѿ� �߻� �ڵ�
        if (0.0f < m_CacAtTick)
            m_CacAtTick -= Time.deltaTime;

        if(Input.GetMouseButton(1) == true) //���콺 ������ ��ư Ŭ����...
        {
            if(m_CacAtTick <= 0.0f)
            {
                Shoot_Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));    

                m_CacAtTick = m_AttSpeed;
            }
        }
        //--- �Ѿ� �߻� �ڵ�

        //--- Bomb ��ų
        if(Input.GetKeyDown(KeyCode.R) == true)
        {
            UseBombSkill();
        }
        //--- Bomb ��ų

        //--- �ִϸ��̼� ���� �κ�
        //���̽�ƽ���� �����ӵ� ���� //Ű���� �����ӵ� ���� //���콺 �̵��� ���� ��
        if (m_JoyMvLen <= 0.0f && (0.0f == h && 0.0f == v) && m_bMoveOnOff == false)
        {
            m_AnimSeq.ChangeAniState(UnitState.Idle);
        }
        else
        {
            if(m_DirVec.magnitude <= 0.0f)
                m_AnimSeq.ChangeAniState(UnitState.Idle);
            else
            {
                //���⿡ ���� �ִϸ��̼� �����ϴ� �κ�
                m_CacRot = Quaternion.LookRotation(m_DirVec);
                m_AnimSeq.CheckAnimDir(m_CacRot.eulerAngles.y);
            }
        }//else
        //--- �ִϸ��̼� ���� �κ�

    }//void Update()

#region ---- Ű���� �̵�
    void KeyBDUpdate()  //Ű���� �̵�ó��
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if(h != 0.0f || v != 0.0f)  //�̵� Ű���带 �����ϰ� ������...
        {
            m_DirVec = (Vector3.right * h) + (Vector3.forward * v);
            if (1.0f < m_DirVec.magnitude)
                m_DirVec.Normalize();

            transform.Translate(m_DirVec * m_MoveSpeed * Time.deltaTime);
        }
    }//void KeyBDUpdate()  //Ű���� �̵�ó��

#endregion

#region ---- ���̽�ƽ �̵�

    public void SetJoyStickMv(float a_JoyMvLen, Vector3 a_JoyMvDir)
    {
        m_JoyMvLen = a_JoyMvLen;
        if(0.0f < a_JoyMvLen)
        {
            m_JoyMvDir = new Vector3(a_JoyMvDir.x, 0.0f, a_JoyMvDir.y);
        }
    }

    public void JoyStickMvUpdate()
    {
        if (h != 0.0f || v != 0.0f)
            return;

        //--- ���̽�ƽ �̵� �ڵ�
        if(0.0f < m_JoyMvLen)
        {
            m_DirVec = m_JoyMvDir;
            float a_MvStep = m_MoveSpeed * Time.deltaTime;
            transform.Translate(m_JoyMvDir * m_JoyMvLen * a_MvStep, Space.Self);
        }
    }

#endregion

#region ---- ���콺 Ŭ�� �̵�

    float m_Tick = 0.0f;

    void MousePickCtrl()
    {
        //--- ������ �ִ� ��ġ�� ��� �̵� ��Ű��...
        //if (0.0f < m_Tick)
        //    m_Tick -= Time.deltaTime;

        //if (m_Tick <= 0.0f)
        //{
        //    if (Input.GetMouseButton(0) == true)  //���콺 ���� ��ư Ŭ����
        //    {
        //        m_PickVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        SetMsPicking(m_PickVec);
        //        m_Tick = 0.1f;
        //    }
        //}
        //--- ������ �ִ� ��ġ�� ��� �̵� ��Ű��...

        if (Input.GetMouseButtonDown(0) == true &&
            Game_Mgr.IsPointerOverUIObject() == false)  //���콺 ���� ��ư Ŭ����
        {
            m_PickVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetMsPicking(m_PickVec);

            if (m_ClickMark != null)
                m_ClickMark.PlayEff(m_PickVec, this);
        }

    }// void MousePickCtrl()


    void SetMsPicking(Vector3 a_Pos)
    {
        Vector3 a_CacVec = a_Pos - this.transform.position;
        a_CacVec.y = 0.0f;
        if (a_CacVec.magnitude < 1.0f)
            return;

        m_bMoveOnOff = true;

        m_DirVec = a_CacVec;
        m_DirVec.Normalize();
        m_TargetPos = new Vector3(a_Pos.x, transform.position.y, a_Pos.z);
    }

    void MousePickUpdate()
    {
        if( 0.0f < m_JoyMvLen || (h != 0.0f || v != 0.0f) ) //���̽�ƽ, Ű����� �����̴� ���̸�
            m_bMoveOnOff = false;   //��� ���콺 �̵� ���

        if(m_bMoveOnOff == true)
        {
            m_CacStep = Time.deltaTime * m_MoveSpeed;  //�̹��� �Ѱ��� ����(����)
            Vector3 a_CacEndVec = m_TargetPos - transform.position;
            a_CacEndVec.y = 0.0f;

            if(a_CacEndVec.magnitude <= m_CacStep)
            { //��ǥ�������� �Ÿ����� ������ ũ�ų� ������ �������� ����.
                //transform.position = m_TargetPos;
                m_bMoveOnOff = false;
            }
            else
            {
                m_DirVec = a_CacEndVec;
                m_DirVec.Normalize();
                transform.Translate(m_DirVec * m_CacStep, Space.World);
            }
        }//if(m_bMoveOnOff == true)
    }// void MousePickUpdate()

#endregion

    void LimitMove()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x < 0.03f) pos.x = 0.03f;
        if (pos.x > 0.97f) pos.x = 0.97f;
        if (pos.y < 0.07f) pos.y = 0.07f;
        if (pos.y > 0.89f) pos.y = 0.89f;

        //pos.x = Mathf.Clamp(pos.x, 0.03f, 0.97f);
        //pos.y = Mathf.Clamp(pos.y, 0.07f, 0.89f);

        Vector3 a_CacPos = Camera.main.ViewportToWorldPoint(pos);
        a_CacPos.y = transform.position.y;
        transform.position = a_CacPos;
    }

    public void Shoot_Fire(Vector3 a_Pos) //�Ű������� ��ǥ ������ �޴´�.
    {  // Ŭ�� �̺�Ʈ�� �߻����� �� �� �Լ��� ȣ���մϴ�.

        GameObject a_Obj = Instantiate(Game_Mgr.m_BulletPrefab);
        //������Ʈ�� Ŭ��(����ü) ���� 

        m_CacEndVec = a_Pos - transform.position;
        m_CacEndVec.y = 0.0f;

        BulletCtrl a_BulletSc = a_Obj.GetComponent<BulletCtrl>();
        a_BulletSc.BulletSpawn(transform.position, m_CacEndVec.normalized, m_ShootRange);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("BulletPrefab") == true)
        {
            //if (other.gameObject.GetComponent<BulletCtrl>().m_AllyType == AllyType.AT_Ally)
            if (other.gameObject.CompareTag(AllyType.BT_Ally.ToString()) == true)
                    return;  //���Ͱ� �� �Ѿ��̸� ����

            TakeDamage(2.0f);

            Destroy(other.gameObject);
        }
        else if(other.gameObject.name.Contains("coin_") == true)
        {
            Game_Mgr.Inst.AddGold(10);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.name.Contains("bomb_") == true)
        {
            //Debug.Log("��ų ��ź ����");
            Game_Mgr.Inst.AddBombSkill();  //��ų ����
            Destroy(other.gameObject);  
        }
        else if(other.gameObject.name.Contains("Item_Obj") == true)
        {
            Game_Mgr.Inst.InvenAddItem(other.gameObject);
            Destroy(other.gameObject);
        }
    }

    void TakeDamage(float a_Value)
    {
        if(m_CurHp <= 0) 
            return;    

        m_CurHp -= a_Value;
        if(m_CurHp < 0.0f)
           m_CurHp = 0.0f;

        if (m_HpBar != null)
            m_HpBar.fillAmount = m_CurHp / m_MaxHp;

        Game_Mgr.Inst.DamageText((int)a_Value, transform.position);

        if(m_CurHp <= 0.0f)  //���ó��
        {
            m_CurHp = 0.0f;

            //���ӿ���
        }
    }

    void UseBombSkill()
    {
        if(GlobalUserData.g_BombCount <= 0)
            return;

        //--- 360�� �߻�
        Vector3 a_TargetV = Vector3.zero;
        GameObject a_NewBObj = null;
        BulletCtrl a_BL_sc = null;
        for(float Angle = 0.0f; Angle < 360.0f; Angle += 15.0f)
        {
            a_TargetV.x = Mathf.Sin(Angle * Mathf.Deg2Rad);
            a_TargetV.y = 0.0f;
            a_TargetV.z = Mathf.Cos(Angle * Mathf.Deg2Rad);
            a_TargetV.Normalize();

            a_NewBObj = Instantiate(Game_Mgr.m_BulletPrefab);
            a_BL_sc = a_NewBObj.GetComponent<BulletCtrl>();
            a_BL_sc.BulletSpawn(transform.position, a_TargetV, 30.0f, 120.0f);
        }
        //--- 360�� �߻�

        Game_Mgr.Inst.AddBombSkill(-1);
    }

    public  void ChangeNickName(string nickStr)
    {
        if(m_NickName != null)
            m_NickName.text = nickStr;


        



    }
}
