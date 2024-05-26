using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mon_Ctrl : MonoBehaviour
{

    float m_MaxHp = 100.0f;
    float m_CurHp = 100.0f;
    public Image HpBar = null;

    public float speed = -1.0f;
    // �̵� �ӵ�, ������ �����Ͽ� �������� �̵�
    public float amplitude = 1.0f;
    // ���� ��� ����

    float initialY;
    // �ʱ� y ��ġ
    float time;
    // ��� �ð�


    public int m_SpawnIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
        time = Mathf.PI / 2;
        // ���� ��� ���� ��ġ�� ����



    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        float x = transform.position.x + speed * Time.deltaTime;
        float y = initialY + amplitude * Mathf.Sin(time);

        transform.position = new Vector3(x, y, transform.position.z);

        #region ���Ͱ� ȭ������� ��������
        //## ���Ͱ� ȭ������� ��������
        if (transform.position.x > 8.63f)
        {
            Destroy(gameObject);

        }
        if (transform.position.x < -8.63f)
        {

            Destroy(gameObject);

        }

        if (transform.position.y > 4.5f)
        {

            Destroy(gameObject);

        }
        if (transform.position.y < -4.5f)
        {

            Destroy(gameObject);

        }

        #endregion

    }

    void OnTriggerEnter2D(Collider2D Coll)
    {
        if (Coll.gameObject.CompareTag("Bullet"))
        {
            Bulllet_Ctrl a_BL_Ctrl = Coll.gameObject.GetComponent<Bulllet_Ctrl>();
            TakeDamage(a_BL_Ctrl.m_Damage);
        }
    }

    public void TakeDamage(float a_Value)
    {
        if (m_CurHp <= 0.0f)
            return;

        m_CurHp -= a_Value;
        if (m_CurHp <= 0.0f)
        {
            m_CurHp = 0.0f;
            Destroy(gameObject); // ���͸� �����մϴ�.

            if (Mon_Mgr.inst != null)
                Mon_Mgr.inst.ReSetSpawn(m_SpawnIdx); // �߰�: ���Ͱ� �׾��� �� ���� ��ġ �缳��
        }

        if (HpBar != null)
            HpBar.fillAmount = m_CurHp / m_MaxHp;
    }
}
