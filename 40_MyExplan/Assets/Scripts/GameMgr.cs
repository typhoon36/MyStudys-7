using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    public static GameObject Bullet_Prefab = null;

    public GameObject GameOverPanel = null;
    public  Button Replay_Btn = null;



    public static GameMgr Inst;

    void Awake()
    {
        Inst = this;

    }

    void Start()
    {
        Time.timeScale = 1.0f;
        Bullet_Prefab = Resources.Load("Bullet") as GameObject;


        Replay_Btn.onClick.AddListener(() =>
        {
           SceneManager.LoadScene("GameScene");
        });


    }





}
