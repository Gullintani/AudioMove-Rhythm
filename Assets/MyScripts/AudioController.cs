using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Music;
    public MobileController MobileController;
    public List<Vector3> PositionList;
    public float speed, tolerantAngle;
    public bool isMoving = false;
    public Vector3 BodyPositionOffSet = new Vector3(-1.33f, 0.0f, 0.0f);

    void Start() {

    }

    private void Update() {
        
    }

    public Vector3 SphericalToCartesian(float radius, float polar, float elevation){
        float a = radius * Mathf.Cos(elevation);
        float x = a * Mathf.Cos(polar);
        float y = radius * Mathf.Sin(elevation);
        float z = a * Mathf.Sin(polar);
        return new Vector3(x, y, z);
    }
}
