using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMobileController : MonoBehaviour
{
    private bool GyroEnabled;
    private Gyroscope gyro;
    private Quaternion OffSetRotation, InitialPosture;
    public Vector3 pointingDirection;
    public Vector3 initialDownDirection;
    public GameAudioController GameAudioControl;
    public GameUIControl GameUIControl;
    public int HitCount;
    public float minBetweenAngle, maxElevationAngel;
    private float ray_initialDown, ray_target, target_initialDown;
    private bool isMotionReset;
    private GameObject LastHitObject, HitObject;
    void Start()
    {   
        // Gyroscope initialize
        GyroEnabled = EnableGyro();
        Input.gyro.enabled = true;

        // Variable initialize
        HitCount = 0;
        isMotionReset = true;
        maxElevationAngel = 0f;
        minBetweenAngle = 90f;
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
            // Debug.Log("GyroRotation" + gyroRotation + "|" + "TransformRotation" + transform.rotation);

            // Create raycast
            pointingDirection = transform.up;
            Ray ray = new Ray(transform.position, pointingDirection);
            RaycastHit hit;
            ray_initialDown = Vector3.Angle(pointingDirection, initialDownDirection);
            ray_target = Vector3.Angle(pointingDirection, GameAudioControl.transform.position);
            target_initialDown = Vector3.Angle(GameAudioControl.transform.position, initialDownDirection);
            
            // Set flags
            if(ray_initialDown < 20.0f){
                isMotionReset = true;
            }

            // Target detection
            if (Physics.Raycast(ray, out hit)){
                HitObject = hit.collider.gameObject;
                if (HitObject.name.Contains("Sphere") && GameAudioControl.isMoving == false && isMotionReset == true){
                    // Record maximum elevation angle, only record when ray inside the target area, and active adaptive shrink
                    if (ray_initialDown > maxElevationAngel){
                        maxElevationAngel = ray_initialDown;
                    }
                    // Record minimum AngleBetween, only record when ray inside the target area, and active adaptive shrink
                    if (ray_target < minBetweenAngle){
                        minBetweenAngle = ray_target;
                    }
                    if (maxElevationAngel > target_initialDown + 10){
                        GameAudioControl.OverhitColor(true);
                    }
                    GameAudioControl.ColorHit(true);
                }else if(LastHitObject != null && GameAudioControl.isMoving == false && isMotionReset == true){
                    // Leaving the AudioSource Sphere
                    if(LastHitObject.name.Contains("Sphere")){
                        GameAudioControl.AdpativeShrink(minBetweenAngle, maxElevationAngel, target_initialDown);
                        isMotionReset = false;
                        GameAudioControl.ColorHit(false);
                        GameAudioControl.OverhitColor(false);
                    }
                }
                // if (HitObject.name.Contains("Sphere") && GameAudioControl.isMoving == false){
                //     GameAudioControl.MoveToNextPosition();
                //     GameUIControl.Verbal.PlayOneShot(GameUIControl.Clip3_SuccessHit);
                //     HitCount += 1;
                // }
            }
            // Setting LastHitObject
            LastHitObject = HitObject;
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
