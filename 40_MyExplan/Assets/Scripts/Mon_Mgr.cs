using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Mon_Mgr : MonoBehaviour
{
    public static Mon_Mgr inst { get; private set; }


    public GameObject Enermy_1 = null;
    public List<Transform> spawnPoints = new List<Transform>(); // 추가: 스폰 위치 리스트
    public float spawnInterval = 5.0f; // 추가: 스폰 간격

    public class SpawnPos // 추가: 스폰 위치 클래스
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
        StartCoroutine(SpawnMonster()); // 추가: 코루틴 시작
    }

    public int numMonsters = 1; // 추가: 몬스터 수

    IEnumerator SpawnMonster() // 추가: 몬스터 스폰 코루틴
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                for (int i = 0; i < numMonsters; i++) // 추가: 한 번에 여러 몬스터 스폰
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

        m_SpawnPosList[idx].m_SpDelay = spawnInterval; // 수정: 스폰 딜레이 재설정

    }
}

