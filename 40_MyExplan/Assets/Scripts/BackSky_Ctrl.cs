using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSky_Ctrl : MonoBehaviour
{
   
    public MeshRenderer SkyRenderer;

    public float speed;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        SkyRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        offset = Time.deltaTime * speed;
        SkyRenderer.material.mainTextureOffset += new Vector2(offset, 0);
    }
}
