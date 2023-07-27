using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainController : MonoBehaviour
{   
    public GameMobileController Phone;
    public GameAudioController GameAudioController;
    public GameUIControl GameUIControl;
    
    private void OnEnable() {

    }
    void Start()
    {

    }

    void Update()
    {   

    }

    public void WorldCalibration(){
        transform.rotation = Phone.transform.rotation;
        transform.rotation *= Quaternion.Euler(0,0,180f);
        Phone.SetInitialDownDirection();
    }

    public static float CartesianToSpherical(Vector3 cartCoords){
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        float outRadius = Mathf.Sqrt((cartCoords.x * cartCoords.x)
                        + (cartCoords.y * cartCoords.y)
                        + (cartCoords.z * cartCoords.z));
        float outPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            outPolar += Mathf.PI;
        float outElevation = Mathf.Asin(cartCoords.y / outRadius);
        return outPolar;
    }
}
