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



    //## 싱글톤 패턴 사용
    public static Monster_Mgr Inst;


    void Awake()
    {
        Inst = this;
        //몬스터 스폰 위치 리스트로 저장
        m_Enermygroup = this.transform;

        m_MonPrefab = Resources.Load("MonsterRoot") as GameObject;


        MonsterCtrl[] a_MonsterList;
       a_MonsterList = m_Enermygroup.GetComponentsInChildren<MonsterCtrl>();
        //몬스터 컨트롤 스크립트 붙은 오브젝트들을 찾아주기

        for(int i = 0; i < a_MonsterList.Length; i++)
        {
            SpawnPos a_Node = new SpawnPos();
            a_Node.m_Pos = a_MonsterList[i].gameObject.transform.position;
            a_MonsterList[i].m_SpawnIdx = i;
            //monseterctrl 클래스쪽에 m_SpawnIdx 인덱스변수 생성

            Destroy(a_MonsterList[i].gameObject);
    
            m_SpawnPosList.Add(a_Node);
        }
    
    }


    // Start is called before the first frame update
    void Start()
    {
        //## 몬스터 스폰 랜덤 하게 바꾸기
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

            //## 새로 스폰
            GameObject newObj = Instantiate(m_MonPrefab);
            newObj.transform.SetParent(m_Enermygroup);
            newObj.transform.position = m_SpawnPosList[i].m_Pos;
            newObj.GetComponent<MonsterCtrl>().m_SpawnIdx = i;


            //## 몬스터 레벨에 따라 능력치 세팅
            int a_LV = m_SpawnPosList[i].m_Level;
            float a_MaxHp = 100.0f + (m_SpawnPosList[i].m_Level * 50.0f);
            if(1000.0f < a_MaxHp)
                a_MaxHp = 1000.0f; //최대 체력 1000으로 제한

            float a_AttSpd = 0.5f + (m_SpawnPosList[i].m_Level * 0.1f);


            if(0.1f < a_AttSpd)
                a_AttSpd = 0.1f; //공격속도 제한

            float a_MvSpd = 2.0f + (m_SpawnPosList[i].m_Level * 0.5f);

            if(5.0f < a_MvSpd)
                a_MvSpd = 5.0f; //이동속도 제한
            newObj.GetComponent<MonsterCtrl>().SetSpawnInfo(i, a_LV, a_MaxHp, a_AttSpd, a_MvSpd);

        }
        
    }

    public void ReSetSpawn(int Idx)
    {
        if(Idx < 0 || Idx >= m_SpawnPosList.Count)
        
            return;

        

        //## 몬스터 스폰 랜덤 하게 바꾸기
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
