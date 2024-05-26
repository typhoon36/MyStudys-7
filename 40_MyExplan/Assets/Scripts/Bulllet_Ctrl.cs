using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public enum AllyType
{
    BT_Ally,        //�Ʊ�
    BT_Enemy,       //����
}



public class Bulllet_Ctrl : MonoBehaviour
{
    [HideInInspector] public AllyType m_AllyType = AllyType.BT_Ally;

    //--- �̵� ���� ������
    Vector3 m_DirVec = Vector3.zero; //���ư� ���� ����
    Vector3 m_StartPos = new Vector3(0, 0, 1);  //���� ��ġ ���� ����

    Vector3 m_MoveStep = Vector3.zero;  //���÷��Ӵ� �̵� ���� ���� ����
    float m_MoveSpeed = 20.0f;          //�̵��ӵ�
                                        //--- �̵� ���� ������

    float m_ShootRange = 30.0f;  //��Ÿ�
    [HideInInspector] public float m_Damage = 10.0f;



    private Vector2 direction;

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_MoveStep = m_DirVec * (Time.deltaTime * m_MoveSpeed);
        m_MoveStep.y = 0.0f;

        transform.position += (Vector3)direction * m_MoveSpeed * Time.deltaTime;


        transform.Translate(m_MoveStep, Space.World);

        Vector3 a_Pos = Camera.main.WorldToViewportPoint(transform.position);
        if (a_Pos.x < -0.1f || 1.1f < a_Pos.x || a_Pos.y < -0.1f || 1.1f < a_Pos.y)
        {
            Destroy(gameObject);
        }
      
      

    

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
        
        transform.forward = m_DirVec; //�Ѿ��� ���ư��� ������ �ٶ󺸰� ȸ�� ���� �ִ� �κ�

        m_ShootRange = a_ShootRange;
        m_Damage = a_Dmg;
    }
}


