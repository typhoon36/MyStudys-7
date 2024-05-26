using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Ctrl : MonoBehaviour
{
    Rigidbody2D rb;

    float maxspeed = 4.0f;

    public GameObject Bullet_Prefab;
    float m_MaxHp = 100.0f;
    float m_CurHp = 100.0f;
    public Image HpBar = null;

    public float damage = 50.0f;

    public SpriteRenderer shieldSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //## 이동 로직
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");



        //## 속도제한
        if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
        {
            rb.velocity = new Vector2(moveHorizontal * maxspeed, moveVertical * maxspeed);
        }



        //## 플레이어가 화면밖으로 나갔을때
        if (transform.position.x > 8.63f)
        {
            transform.position = new Vector2(8.63f, transform.position.y);
        }
        if (transform.position.x < -8.63f)
        {
            transform.position = new Vector2(-8.63f, transform.position.y);
        }

        if (transform.position.y > 4.5f)
        {
            transform.position = new Vector2(transform.position.x, 4.5f);
        }
        if (transform.position.y < -4.5f)
        {
            transform.position = new Vector2(transform.position.x, -4.5f);
        }

        //## 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        //## 방패 사용
        if (Input.GetKeyDown(KeyCode.N))
        {
            Shield();
        }

        // 방어막 스프라이트의 위치를 플레이어의 위치로 업데이트합니다.
        shieldSprite.transform.position = transform.position;
    }

    void Fire()
    {
        // 마우스 위치를 가져옵니다.
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 마우스 위치와 플레이어 위치 사이의 방향을 계산합니다.
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        direction.Normalize();

        // 총알 Prefab의 인스턴스를 생성하고, 이 인스턴스의 위치와 방향을 설정
        GameObject bullet = Instantiate(Bullet_Prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)));
        bullet.GetComponent<Bulllet_Ctrl>().SetDirection(direction);



    }

    void Shield()
    {
        // 방어막 스프라이트의 활성 상태를 토글합니다.
        shieldSprite.enabled = !shieldSprite.enabled;
    }


    void OnTriggerEnter2D(Collider2D Coll)
    {
        if (Coll.gameObject.CompareTag("Mob"))
        {
            // 방어막이 활성화되어 있지 않을 때만 체력이 닳습니다.
            if (!shieldSprite.enabled)
            {
                TakeDamage(damage); // 체력 감소
            }
            else
            {
                shieldSprite.enabled = false; // 방어막 비활성화
            }
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
            Destroy(gameObject); 

            GameMgr.Inst.GameOverPanel.SetActive(true);
            Time.timeScale = 0.0f;

        }

        if (HpBar != null)
            HpBar.fillAmount = m_CurHp / m_MaxHp;
    }

  
 
}
