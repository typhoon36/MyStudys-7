using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    Camera RefCam = null;
    public CinemachineVirtualCamera m_vCam;

    //--- ī�޶� �� �ξƿ�
    float maxDist = 20.0f;  //�ܾƿ��� �ִ밪
    float minDist = 5.0f;   //������ �ּҰ�
    float zoomSpeed = 1.0f; //���ξƿ��� �ӵ�
    float distance = 15.0f;
    //--- ī�޶� �� �ξƿ�

    // Start is called before the first frame update
    void Start()
    {
        RefCam = GetComponent<Camera>();
        //if (RefCam != null)
        //    distance = RefCam.orthographicSize;   //�ó׸ӽ��� ������� �ʰ� �⺻ ī�޶� ����

        if(m_vCam != null)
            distance = m_vCam.m_Lens.OrthographicSize; //�ó׸ӽ��� ����ϰ� �ִ� ���
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //--- PC������ �۵��Ǵ� ���� �ܾƿ� ���
        //Input.GetAxis("Mouse ScrollWheel")
        //���콺 �� ��� �� -0.1f(size�� Ŀ����)
        //���콺 �� �ж� +0.1f(size�� ��������)
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && distance < maxDist)
        {
            distance += zoomSpeed;
            //RefCam.orthographicSize = distance; //Main Camera�� ���
            m_vCam.m_Lens.OrthographicSize = distance; //�ó׸ӽ��� ����ϴ� ���
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && distance > minDist)
        {
            distance -= zoomSpeed;
            //RefCam.orthographicSize = distance; //Main Camera�� ���
            m_vCam.m_Lens.OrthographicSize = distance; //�ó׸ӽ��� ����ϴ� ���
        }
        //--- PC������ �۵��Ǵ� ���� �ܾƿ� ���
    }
}
