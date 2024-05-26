using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Mon_Mgr : MonoBehaviour
{
    public static Mon_Mgr inst { get; private set; }


    public GameObject Enermy_1 = null;
    public List<Transform> spawnPoints = new List<Transform>(); // �߰�: ���� ��ġ ����Ʈ
    public float spawnInterval = 5.0f; // �߰�: ���� ����

    public class SpawnPos // �߰�: ���� ��ġ Ŭ����
    {
        public Transform transform;
        public float m_SpDelay;
        public int m_Level;

        public SpawnPos(Transform transform)
        {
            this.transform = transform;
            this.m_SpDelay = 0f;
            this.m_Level = 1;
        }
    }

    public List<SpawnPos> m_SpawnPosList = new List<SpawnPos>();

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        StartCoroutine(SpawnMonster()); // �߰�: �ڷ�ƾ ����
    }

    public int numMonsters = 1; // �߰�: ���� ��

    IEnumerator SpawnMonster() // �߰�: ���� ���� �ڷ�ƾ
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                for (int i = 0; i < numMonsters; i++) // �߰�: �� ���� ���� ���� ����
                {
                    Instantiate(Enermy_1, spawnPoint.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }

    }


    public void ReSetSpawn(int idx)
    {
        if (idx < 0 || m_SpawnPosList.Count <= idx)
            return;

        m_SpawnPosList[idx].m_SpDelay = spawnInterval; // ����: ���� ������ �缳��

    }
}

