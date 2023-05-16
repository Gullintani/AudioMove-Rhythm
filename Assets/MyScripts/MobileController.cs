using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    // Start is called before the first frame update
    private Gyroscope gyro;
    private Vector3 acce, pointingDirection;
    private float MaximumDistance = 30.0f;
    private Quaternion initialRotation, gyroInitialRotation;
    public float minimumAngle = 90.0f, maxAlphaAngel;
    public Vector3 initialDownDirection, initialRightDirection;
    private bool canHit = true, cubeHitSEFlag = true, isOverhit = false;

    void Start()
    {   
        // ======== Sensor Input ========
        Input.gyro.enabled = true;
        gyro = Input.gyro;
        initialRotation = transform.rotation;
        gyroInitialRotation = gyro.attitude;

    }

    // Update is called once per frame
    void Update()
    {   
        // ======== Transform Functions ========
        Quaternion offsetRotation = gyro.attitude;
        transform.rotation =  GyroToUnity(offsetRotation);
        // Debug.Log(transform.rotation);

        // // Alternative Way
        // Quaternion offsetRotation = Quaternion.Inverse(gyroInitialRotation) * Input.gyro.attitude;
        // transform.rotation = initialRotation * offsetRotation;

        // ======== RayCast ========
        pointingDirection =  transform.up;
        Ray ray = new Ray(transform.position, pointingDirection);
        RaycastHit hit;
        // lastHit Initialization
        

        // initialDownDirection Initialization
        if (Time.frameCount == 120){
            SetInitialDownDirection();
            initialRightDirection = transform.right;
            ResetGyro();
        }
        
        // ======== Debug Log ========
        Debug.DrawRay(transform.position, pointingDirection * MaximumDistance, Color.green);
        // Debug.Log("The Gyro's current rotation: " + gyro.attitude);
        // Debug.Log("The Gyro's offset rotation: " + transform.rotation);
    }

    // Input.gyro.attitude returns a quaternion for a right-handed coordinate system, while unity has a left-handed coordinate system.
    private Quaternion GyroToUnity(Quaternion q){
        return new Quaternion(-q.x, q.y, -q.z, q.w);
    }

    private float GetAngleBetweenTargetAndPointing(Vector3 targetPosition, Vector3 relative_pointingDirection){
        return Vector3.Angle(targetPosition, relative_pointingDirection);
    }

    public void SetInitialDownDirection(){
        initialDownDirection = pointingDirection;
        // Debug.Log("Initial Down Direction is Set!");
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
