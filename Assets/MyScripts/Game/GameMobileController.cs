using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMobileController : MonoBehaviour
{
    // Start is called before the first frame update
    private Gyroscope gyro;
    public Vector3 pointingDirection;
    private float MaximumDistance = 30.0f;
    public float minimumAngle = 90.0f, maxAlphaAngel;
    public Vector3 initialDownDirection, initialRightDirection;

    void Start()
    {   
        // Gyroscope input
        Input.gyro.enabled = true;
        gyro = Input.gyro;
    }

    // Update is called once per frame
    void Update()
    {   
        // Coordinate transform functions
        Quaternion offsetRotation = gyro.attitude;
        transform.rotation =  GyroToUnity(offsetRotation);

        // Create raycast
        pointingDirection =  transform.up;
        Ray ray = new Ray(transform.position, pointingDirection);
        

        // InitialDownDirection initialization
        if (Time.frameCount == 120){
            SetInitialDownDirection();
            initialRightDirection = transform.right;
        }
        
        // Target detection
        

        // Debug
        Debug.DrawRay(transform.position, pointingDirection * MaximumDistance, Color.green);

    }

    private Quaternion GyroToUnity(Quaternion q){
        return new Quaternion(-q.x, q.y, -q.z, q.w);
    }

    public void SetInitialDownDirection(){
        initialDownDirection = pointingDirection;
        Debug.Log("Initial Down Direction is Set to: " + pointingDirection + ".");
    }

    
}
