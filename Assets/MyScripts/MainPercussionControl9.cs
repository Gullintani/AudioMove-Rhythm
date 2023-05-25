using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPercussionControl9 : MonoBehaviour
{
    public PercussionControl91 p1;
    public PercussionControl92 p2;
    public PercussionControl93 p3;
    public PercussionControl94 p4;
    public PercussionControl95 p5;
    public PercussionControl96 p6;
    public PercussionControl97 p7;
    public PercussionControl98 p8;
    public PercussionControl99 p9;

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
