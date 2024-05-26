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
        //## �̵� ����
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");



        //## �ӵ�����
        if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
        {
            rb.velocity = new Vector2(moveHorizontal * maxspeed, moveVertical * maxspeed);
        }



        //## �÷��̾ ȭ������� ��������
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

        //## �Ѿ� �߻�
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        //## ���� ���
        if (Input.GetKeyDown(KeyCode.N))
        {
            Shield();
        }

        // �� ��������Ʈ�� ��ġ�� �÷��̾��� ��ġ�� ������Ʈ�մϴ�.
        shieldSprite.transform.position = transform.position;
    }

    void Fire()
    {
        // ���콺 ��ġ�� �����ɴϴ�.
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // ���콺 ��ġ�� �÷��̾� ��ġ ������ ������ ����մϴ�.
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        direction.Normalize();

        // �Ѿ� Prefab�� �ν��Ͻ��� �����ϰ�, �� �ν��Ͻ��� ��ġ�� ������ ����
        GameObject bullet = Instantiate(Bullet_Prefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)));
        bullet.GetComponent<Bulllet_Ctrl>().SetDirection(direction);



    }

    void Shield()
    {
        // �� ��������Ʈ�� Ȱ�� ���¸� ����մϴ�.
        shieldSprite.enabled = !shieldSprite.enabled;
    }


    void OnTriggerEnter2D(Collider2D Coll)
    {
        if (Coll.gameObject.CompareTag("Mob"))
        {
            // ���� Ȱ��ȭ�Ǿ� ���� ���� ���� ü���� ����ϴ�.
            if (!shieldSprite.enabled)
            {
                TakeDamage(damage); // ü�� ����
            }
            else
            {
                shieldSprite.enabled = false; // �� ��Ȱ��ȭ
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
