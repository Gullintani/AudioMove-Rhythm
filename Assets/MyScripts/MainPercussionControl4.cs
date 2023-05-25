using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPercussionControl4 : MonoBehaviour
{
    public PercussionControl41 p1;
    public PercussionControl42 p2;
    public PercussionControl43 p3;
    public PercussionControl44 p4;

    // Start is called before the first frame update
    void Start()
    {
        p1.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f, Mathf.PI/6.0f);
        p2.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f, -Mathf.PI/4.0f);
        p3.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f * 2.0f, Mathf.PI/6.0f);
        p4.transform.position = SphericalToCartesian(2.0f, Mathf.PI/3.0f * 2.0f, -Mathf.PI/4.0f);
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
