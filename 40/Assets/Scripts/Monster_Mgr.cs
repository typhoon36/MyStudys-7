using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//--- SpawnPos
class SpawnPos //����Ʈ�� ���� �����̰� ����Ʈ�̱� ������ �ε����� ���� �ȴ�.
{
    public Vector3 m_Pos = Vector3.zero;
    public float m_SpDelay = 0.0f;
    public int m_Level = 1; //������ ������ ������ ������ ���̴�.

    public SpawnPos() //������ �Լ�
    {
        m_SpDelay = 0.0f;
    }

    public bool Update_SpPos(float a_DeltaTime)
    {
        if(0.0f < m_SpDelay)
        {
            m_SpDelay -= a_DeltaTime;
            if(m_SpDelay <= 0.0f)
            {
                m_SpDelay = 0.0f;
                return true;
            }
        }

        return false;
    }//public bool Update_SpPos(float a_DeltaTime)
}
//-------SpawnPos

public class Monster_Mgr : MonoBehaviour
{
    Transform m_EnemyGroup = null;
    GameObject m_MonPrefab = null;
    List<SpawnPos> m_SpawnPosList = new List<SpawnPos>();

    public Texture[] m_MonImg = null;

    //--- �̱��� ����
    public static Monster_Mgr Inst;

    void Awake()
    {
        Inst = this;

        //--- ���� ���� ��ġ ����Ʈ�� ������ ����...
        m_EnemyGroup = this.transform;
        m_MonPrefab = Resources.Load("MonsterRoot") as GameObject;

        MonsterCtrl[] a_MonSterList;
        a_MonSterList = transform.GetComponentsInChildren<MonsterCtrl>();
        // MonsterCtrl ������Ʈ�� �پ��ִ� �ֵ鸸 ã�ƿ���...
        for(int i = 0; i < a_MonSterList.Length; i++)
        {
            SpawnPos a_Node = new SpawnPos();
            a_Node.m_Pos = a_MonSterList[i].gameObject.transform.position;
            a_MonSterList[i].m_SpawnIdx = i;  //MonsterCtrl class �ʿ� m_SpawnIdx �ε��������� ����� �ش�.
            Destroy(a_MonSterList[i].gameObject);  //������ ��ġ�Ǿ� �ִ� ���ʹ� ��� ������ �ش�.
            m_SpawnPosList.Add(a_Node);
        }
        //--- ���� ���� ��ġ ����Ʈ�� ������ ����...
    }
    //--- �̱��� ����

    // Start is called before the first frame update
    void Start()
    {
        //--- ���� ���� �����ϰ� �ٲٱ�...
        for(int i = 0; i < m_SpawnPosList.Count; i++)
        {
            int a_Rnd = Random.Range(0, 10);
            if (9 <= a_Rnd)
                m_SpawnPosList[i].m_Level = 3;
            else if (7 <= a_Rnd)
                m_SpawnPosList[i].m_Level = 2;
            else
                m_SpawnPosList[i].m_Level = 1;

            m_SpawnPosList[i].m_SpDelay = 0.01f;
        }
        //--- ���� ���� �����ϰ� �ٲٱ�...
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < m_SpawnPosList.Count; i++)
        {
            if (m_SpawnPosList[i].Update_SpPos(Time.deltaTime) == false)
                continue;

            //���� ���� ��Ų��.
            GameObject newObj = Instantiate(m_MonPrefab);
            newObj.transform.SetParent(m_EnemyGroup);
            newObj.transform.position = m_SpawnPosList[i].m_Pos;
            //newObj.GetComponent<MonsterCtrl>().m_SpawnIdx = i;  //m_SpawnPos �� �ε����� ������ �ش�.

            //--- ������ ���� ���� �ɷ�ġ ����
            int a_Lv = m_SpawnPosList[i].m_Level;
            float a_MaxHp = 100.0f + (m_SpawnPosList[i].m_Level * 50.0f);
            if (1000.0f < a_MaxHp)
                a_MaxHp = 1000.0f;  //�ִ�ü�� 100.0f ~ 1000.0f ����
            float a_AttSpeed = 0.5f - (m_SpawnPosList[i].m_Level * 0.1f);
            if (a_AttSpeed < 0.1f)
                a_AttSpeed = 0.1f;  //���� 0.5f ~ 0.1f ����
            float a_MvSpeed = 2.0f + (m_SpawnPosList[i].m_Level * 0.5f);
            if (5.0f < a_MvSpeed)
                a_MvSpeed = 5.0f;   //�̼� 2.0f ~ 5.0f ����
            newObj.GetComponent<MonsterCtrl>().SetSpawnInfo(i, a_Lv, a_MaxHp, a_AttSpeed, a_MvSpeed);
            //--- ������ ���� ���� �ɷ�ġ ����
        }
    }// void Update()

    public void ReSetSpawn(int idx)
    {
        if (idx < 0 || m_SpawnPosList.Count <= idx)
            return;

        //int a_Rnd = Random.Range(0, 10);
        //if (9 <= a_Rnd)
        //    m_SpawnPosList[idx].m_Level = 3;
        //else if (7 <= a_Rnd)
        //    m_SpawnPosList[idx].m_Level = 2;
        //else
        //    m_SpawnPosList[idx].m_Level = 1;

        m_SpawnPosList[idx].m_Level++;
        if (3 < m_SpawnPosList[idx].m_Level)
            m_SpawnPosList[idx].m_Level = 3;

        m_SpawnPosList[idx].m_SpDelay = Random.Range(4.0f, 6.0f);
    }
}
