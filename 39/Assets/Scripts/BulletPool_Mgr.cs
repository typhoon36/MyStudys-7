using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool_Mgr : MonoBehaviour
{
    [Header("--- Bullet Pool ---")]
    public GameObject BulletPrefab;
    //�Ѿ��� �̸� ������ ������ ����Ʈ �ڷ���
    [HideInInspector] public List<BulletCtrl> m_BulletPool = new List<BulletCtrl>();

    //## �̱��� ����
    public static BulletPool_Mgr Inst = null;

    void Awake()
    {
        Inst = this;
    }
   

    // Start is called before the first frame update
    void Start()
    {
        //## �Ѿ��� ������ ������Ʈ Ǯ�� ����

        for (int i = 0; i < 50; i++)
        {
            //�Ѿ� �������� ����
            GameObject a_Bullet = Instantiate(BulletPrefab);
            //������ �Ѿ��� Bullet_Mgr ������ ���ϵ�ȭ �ϱ�
            a_Bullet.transform.SetParent(this.transform);
            //������ �Ѿ��� ��Ȱ��ȭ
            a_Bullet.SetActive(false);
            //������ �Ѿ��� ������Ʈ Ǯ�� �߰�
            BulletCtrl a_BL_Ctrl = a_Bullet.GetComponent<BulletCtrl>();
            a_BL_Ctrl.m_IsPool = true;
            m_BulletPool.Add(a_BL_Ctrl);
        }
       
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    //## ������Ʈ Ǯ�� ó������ ������ ��ȸ
    public BulletCtrl GetBulletPool()
    {
       

        foreach (BulletCtrl a_BNode in m_BulletPool)
        {
            //��Ȱ��ȭ ���η� ��� ������ Bullet�� �Ǵ�
            if (a_BNode.gameObject.activeSelf == false)
            {
                a_BNode.gameObject.SetActive(true);
                return a_BNode;
            }
        }

        //##����ϰ� �ִ� �Ѿ��� ������ �Ѿ��� ���� �ϳ� �� �߰�

        //�Ѿ� �������� ����
        GameObject a_Bullet = Instantiate(BulletPrefab);
        //������ �Ѿ��� Bullet_Mgr ������ ���ϵ�ȭ �ϱ�
        a_Bullet.transform.SetParent(this.transform);
        //������ �Ѿ��� ��Ȱ��ȭ
        //a_Bullet.SetActive(false);
        //������ �Ѿ��� ������Ʈ Ǯ�� �߰�
        BulletCtrl a_BL_Ctrl = a_Bullet.GetComponent<BulletCtrl>();
        a_BL_Ctrl.m_IsPool = true;
        m_BulletPool.Add(a_BL_Ctrl);

        //������ �Ѿ��� Ȱ��ȭ
        a_Bullet.SetActive(true);
        return a_BL_Ctrl;

    }
}
