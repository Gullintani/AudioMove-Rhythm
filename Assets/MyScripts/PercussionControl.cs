using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercussionControl : MonoBehaviour
{
    public PercussionControl1 p1;
    public PercussionControl2 p2;
    public PercussionControl3 p3;
    public PercussionControl4 p4;
    public PercussionControl5 p5;
    public PercussionControl6 p6;
    public PercussionControl7 p7;
    public PercussionControl8 p8;
    public PercussionControl9 p9;

    // Start is called before the first frame update
    void Start()
    {
        p1.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f, -Mathf.PI/4.0f);
        p2.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f, 0.0f);
        p3.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f, Mathf.PI/4.0f);
        
        p4.transform.position = SphericalToCartesian(2.0f, Mathf.PI/2.0f, -Mathf.PI/4.0f);
        p5.transform.position = SphericalToCartesian(2.0f, Mathf.PI/2.0f, 0.0f);
        p6.transform.position = SphericalToCartesian(2.0f, Mathf.PI/2.0f, Mathf.PI/4.0f);
        
        p7.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f * 2.0f, -Mathf.PI/4.0f);
        p8.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f * 2.0f, 0.0f);
        p9.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f * 2.0f, Mathf.PI/4.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 SphericalToCartesian(float radius, float polar, float elevation){
        float a = radius * Mathf.Cos(elevation);
        float x = a * Mathf.Cos(polar);
        float y = radius * Mathf.Sin(elevation);
        float z = a * Mathf.Sin(polar);
        return new Vector3(x, y, z);
    }
}
