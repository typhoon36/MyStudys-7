using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//## SpanwPos
class SpawnPos
{
    public Vector3 m_Pos = Vector3.zero;
    public float m_SpDelay = 0.0f;
    public int m_Level = 1;

    public SpawnPos()
    {
        m_SpDelay = 0.0f;
    }


    public bool Update_spPos(float a_DeltaTime)
    {
        if (0.0f< m_SpDelay)
        {
            m_SpDelay -= a_DeltaTime;
            if (m_SpDelay <= 0.0f)
            {
                m_SpDelay = 0.0f;
                return true;
            }
        }
        return false;
    }


}



   


public class Monster_Mgr : MonoBehaviour
{
    Transform m_Enermygroup = null;
    GameObject m_MonPrefab = null;
    List<SpawnPos> m_SpawnPosList = new List<SpawnPos>();

    public Texture[] m_MonImg = null;



    //## �̱��� ���� ���
    public static Monster_Mgr Inst;


    void Awake()
    {
        Inst = this;
        //���� ���� ��ġ ����Ʈ�� ����
        m_Enermygroup = this.transform;

        m_MonPrefab = Resources.Load("MonsterRoot") as GameObject;


        MonsterCtrl[] a_MonsterList;
       a_MonsterList = m_Enermygroup.GetComponentsInChildren<MonsterCtrl>();
        //���� ��Ʈ�� ��ũ��Ʈ ���� ������Ʈ���� ã���ֱ�

        for(int i = 0; i < a_MonsterList.Length; i++)
        {
            SpawnPos a_Node = new SpawnPos();
            a_Node.m_Pos = a_MonsterList[i].gameObject.transform.position;
            a_MonsterList[i].m_SpawnIdx = i;
            //monseterctrl Ŭ�����ʿ� m_SpawnIdx �ε������� ����

            Destroy(a_MonsterList[i].gameObject);
    
            m_SpawnPosList.Add(a_Node);
        }
    
    }


    // Start is called before the first frame update
    void Start()
    {
        //## ���� ���� ���� �ϰ� �ٲٱ�
        for(int i = 0; m_SpawnPosList.Count > i; i++)
        {
            int a_Rand = Random.Range(0, 10);
            if(9 <= a_Rand)
                m_SpawnPosList[i].m_Level = 3;
            else if(7 <= a_Rand)
                m_SpawnPosList[i].m_Level = 2;
            else
                m_SpawnPosList[i].m_Level = 1;


            m_SpawnPosList[i].m_SpDelay = 0.01f;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; m_SpawnPosList.Count > i; i++)
        {
            if (m_SpawnPosList[i].Update_spPos(Time.deltaTime) == false)
                continue;

            //## ���� ����
            GameObject newObj = Instantiate(m_MonPrefab);
            newObj.transform.SetParent(m_Enermygroup);
            newObj.transform.position = m_SpawnPosList[i].m_Pos;
            newObj.GetComponent<MonsterCtrl>().m_SpawnIdx = i;


            //## ���� ������ ���� �ɷ�ġ ����
            int a_LV = m_SpawnPosList[i].m_Level;
            float a_MaxHp = 100.0f + (m_SpawnPosList[i].m_Level * 50.0f);
            if(1000.0f < a_MaxHp)
                a_MaxHp = 1000.0f; //�ִ� ü�� 1000���� ����

            float a_AttSpd = 0.5f + (m_SpawnPosList[i].m_Level * 0.1f);


            if(0.1f < a_AttSpd)
                a_AttSpd = 0.1f; //���ݼӵ� ����

            float a_MvSpd = 2.0f + (m_SpawnPosList[i].m_Level * 0.5f);

            if(5.0f < a_MvSpd)
                a_MvSpd = 5.0f; //�̵��ӵ� ����
            newObj.GetComponent<MonsterCtrl>().SetSpawnInfo(i, a_LV, a_MaxHp, a_AttSpd, a_MvSpd);

        }
        
    }

    public void ReSetSpawn(int Idx)
    {
        if(Idx < 0 || Idx >= m_SpawnPosList.Count)
        
            return;

        

        //## ���� ���� ���� �ϰ� �ٲٱ�
        for (int i = 0; m_SpawnPosList.Count > i; i++)
        {
            //int a_Rand = Random.Range(0, 10);
            //if (9 <= a_Rand)
            //    m_SpawnPosList[i].m_Level = 3;
            //else if (7 <= a_Rand)
            //    m_SpawnPosList[i].m_Level = 2;
            //else
            //    m_SpawnPosList[i].m_Level = 1;


            m_SpawnPosList[Idx].m_Level++;
            if(3 < m_SpawnPosList[Idx].m_Level)
                m_SpawnPosList[Idx].m_Level = 3;


            m_SpawnPosList[Idx].m_SpDelay = Random.Range(4.0f, 6.0f);

        }



    }


}
