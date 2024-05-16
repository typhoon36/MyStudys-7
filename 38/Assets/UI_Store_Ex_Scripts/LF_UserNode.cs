using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LF_UserNode : MonoBehaviour
{
    [HideInInspector] public int m_UniqueID = -1;        //������ ������ȣ
    [HideInInspector] public string m_UserName = "";     //������ �г���
    [HideInInspector] public int m_ULevel = -1;          //������ ����
    [HideInInspector] public bool m_SelectOnOff = false; //���� ���� ����
    public RawImage m_SelectImg;
    public Text m_InfoText;

    // Start is called before the first frame update
    void Start()
    {
        m_SelectOnOff = false;
        this.GetComponent<Button>().onClick.AddListener(OnClickMethod);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void InitInfo(int a_UniqueID, string a_Name, int a_Level)
    {
        m_UniqueID = a_UniqueID;
        m_UserName = a_Name;
        m_ULevel = a_Level;
        m_InfoText.text = a_Name + " : Lv(" + a_Level.ToString() + ")";
    }

    private void OnClickMethod()  //�� ��ư ���ý� ���� ���� ǥ�� �Լ�
    {
        m_SelectOnOff = !m_SelectOnOff;
        if(m_SelectImg != null)
            m_SelectImg.gameObject.SetActive(m_SelectOnOff);
    }
}
