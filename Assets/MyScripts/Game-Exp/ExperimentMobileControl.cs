using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentGameMobileControl : MonoBehaviour
{
    private bool GyroEnabled;
    private Gyroscope gyro;
    private Quaternion OffSetRotation, InitialPosture;
    public Vector3 pointingDirection;
    public Vector3 initialDownDirection;
    public ExperimentGameAudioControl GameAudioControl;
    public ExperimentGameUIControl GameUIControl;
    public ExperimentGameListenerControl ExperimentGameListenerControl;
    public int HitCount;

    void Start()
    {   
        // Gyroscope initialize
        GyroEnabled = EnableGyro();
        Input.gyro.enabled = true;

        // Variable initialize
        HitCount = 0;
    }

    void Update()
    {   
        // InitialDownDirection initialization
        // if (Time.frameCount == 120){
            // SetInitialDownDirection();
        // }
        if (GyroEnabled){
            Quaternion gyroRotation = Input.gyro.attitude;
            transform.rotation = GyroToUnity(gyroRotation);
            // PlayerPrefs settings: when Motion is selected to Phone (1)
            if(PlayerPrefs.GetInt("ExperimentMotion") == 1){
                ExperimentGameListenerControl.transform.rotation = transform.rotation;
            }


            // Create raycast
            pointingDirection = transform.up;
            Ray ray = new Ray(transform.position, pointingDirection);
            RaycastHit hit;
            
            // Target detection
            if (Physics.Raycast(ray, out hit)){
                GameObject HitObject = hit.collider.gameObject;
                if (HitObject.name.Contains("Sphere") && GameAudioControl.isMoving == false){
                    GameAudioControl.MoveToNextPosition();
                    GameUIControl.Verbal.PlayOneShot(GameUIControl.Clip3_SuccessHit);
                    HitCount += 1;
                }
            }

            // Debug
            Debug.DrawRay(transform.position, pointingDirection * 30.0f, Color.green);
        }
    }

    private bool EnableGyro(){
        if(SystemInfo.supportsGyroscope){
            gyro = Input.gyro;
            gyro.enabled = true;

            Debug.Log(this.transform.rotation);
            
            InitialPosture = Quaternion.Euler(0f, 0f, 0f);
            OffSetRotation = new Quaternion(0,0,-1,0);
            // rotation = ;

            return true;
        }
        return false;
    }

    private Quaternion GyroToUnity(Quaternion q){
        return new Quaternion(-q.x, q.y, -q.z, q.w);
    }

    public void SetInitialDownDirection(){
        initialDownDirection = pointingDirection;
        Debug.Log("Initial Down Direction is Set to: " + pointingDirection + ".");
    }

    
}
