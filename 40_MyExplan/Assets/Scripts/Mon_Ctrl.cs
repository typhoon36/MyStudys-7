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
    // 이동 속도, 음수로 설정하여 왼쪽으로 이동
    public float amplitude = 1.0f;
    // 사인 곡선의 진폭

    float initialY;
    // 초기 y 위치
    float time;
    // 경과 시간


    public int m_SpawnIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
        time = Mathf.PI / 2;
        // 사인 곡선의 시작 위치를 조정



    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        float x = transform.position.x + speed * Time.deltaTime;
        float y = initialY + amplitude * Mathf.Sin(time);

        transform.position = new Vector3(x, y, transform.position.z);

        #region 몬스터가 화면밖으로 나갔을때
        //## 몬스터가 화면밖으로 나갔을때
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
            Destroy(gameObject); // 몬스터를 제거합니다.

            if (Mon_Mgr.inst != null)
                Mon_Mgr.inst.ReSetSpawn(m_SpawnIdx); // 추가: 몬스터가 죽었을 때 스폰 위치 재설정
        }

        if (HpBar != null)
            HpBar.fillAmount = m_CurHp / m_MaxHp;
    }
}
