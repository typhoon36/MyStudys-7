using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyType
{
    BT_Ally,        //�Ʊ�
    BT_Enemy,       //����
}

public class BulletCtrl : MonoBehaviour
{
    [HideInInspector] public AllyType m_AllyType = AllyType.BT_Ally;

    //--- �̵� ���� ������
    Vector3 m_DirVec = Vector3.zero; //���ư� ���� ����
    Vector3 m_StartPos = new Vector3(0, 0, 1);  //���� ��ġ ���� ����

    Vector3 m_MoveStep = Vector3.zero;  //���÷��Ӵ� �̵� ���� ���� ����
    float m_MoveSpeed = 35.0f;          //�̵��ӵ�
    //--- �̵� ���� ������

    float m_ShootRange = 30.0f;  //��Ÿ�
    [HideInInspector] public float m_Damage = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_MoveStep = m_DirVec * (Time.deltaTime * m_MoveSpeed);
        m_MoveStep.y = 0.0f;

        transform.Translate(m_MoveStep, Space.World);

        Vector3 a_Pos = Camera.main.WorldToViewportPoint(transform.position);
        if (a_Pos.x < -0.1f || 1.1f < a_Pos.x || a_Pos.y < -0.1f || 1.1f < a_Pos.y)
        {
            Destroy(gameObject);
        }
        else
        {
            float a_Length = Vector3.Distance(transform.position, m_StartPos);
            //float a_Length = (transform.position - m_StartPos).magnitude;
            if (m_ShootRange < a_Length)  //��Ÿ� ����
                Destroy(gameObject);
        }

        //Vector3 a_SPos = Vector3.zero;
        //a_SPos.x = 1.1f;
        //a_SPos.y = Random.Range(0.1f, 0.9f);
        //Vector3 a_CacPos = Camera.main.ViewportToWorldPoint(a_SPos);

    }//void Update()

    public void BulletSpawn(Vector3 a_OwnPos, Vector3 a_DirVec,
                            float a_ShootRange = 30.0f, float a_Dmg = 10)
    {
        a_DirVec.y = 0.0f;
        m_DirVec = a_DirVec;
        m_DirVec.Normalize();

        m_StartPos = a_OwnPos + (m_DirVec * 2.5f);
        m_StartPos.y = transform.position.y;

        transform.position = new Vector3(m_StartPos.x, transform.position.y, m_StartPos.z);
        //transform.rotation = Quaternion.LookRotation(m_DirVec);
        transform.forward = m_DirVec; //�Ѿ��� ���ư��� ������ �ٶ󺸰� ȸ�� ���� �ִ� �κ�

        m_ShootRange = a_ShootRange;
        m_Damage = a_Dmg;
    }
}
