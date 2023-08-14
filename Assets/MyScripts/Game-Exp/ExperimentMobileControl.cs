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
    public int HitCountAdpative;
    private bool isMotionReset;
    public float ray_initialDown, ray_target, target_initialDown, maxElevationAngel, minBetweenAngle;
    private GameObject HitObject, LastHitObject;

    void Start()
    {   
        // Gyroscope initialize
        GyroEnabled = EnableGyro();
        Input.gyro.enabled = true;

        // Variable initialize
        HitCount = 0;
        isMotionReset = true;
        maxElevationAngel = 90;
        minBetweenAngle = 50;
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


            // Create raycast and important angles
            pointingDirection = transform.up;
            Ray ray = new Ray(transform.position, pointingDirection);
            RaycastHit hit;
            ray_initialDown = Vector3.Angle(pointingDirection, initialDownDirection);
            ray_target = Vector3.Angle(pointingDirection, GameAudioControl.transform.position);
            target_initialDown = Vector3.Angle(GameAudioControl.transform.position, initialDownDirection);
            
            // Set flags
            if(ray_initialDown < 15.0f){
                isMotionReset = true;
            }
            // Just directly set true when doing task 3
            if(PlayerPrefs.GetInt("ExperimentTask")==2){
                isMotionReset = true;
            }

            // Target detection
            if (Physics.Raycast(ray, out hit) && PlayerPrefs.GetInt("ExperimentTask")!=0){
                HitObject = hit.collider.gameObject;
                if (HitObject.name.Contains("Sphere") && GameAudioControl.isMoving == false && isMotionReset == true){
                    // SetColor
                    GameAudioControl.ColorHit(true);
                    
                    if(PlayerPrefs.GetInt("ExperimentAdaptiveShrink")==0 && PlayerPrefs.GetInt("ExperimentTask")==2){
                        if (ray_target < 20.0f && GameAudioControl.isMoving == false){
                            Debug.Log(ray_target);
                            if (ray_target < minBetweenAngle){
                                minBetweenAngle = ray_target;
                            }
                            GameAudioControl.MoveToNextPosition();
                            GameUIControl.Verbal.PlayOneShot(GameUIControl.Clip3_SuccessHit);
                            GameUIControl.Verbal.PlayOneShot(GameUIControl.Clip7_AudioMove);
                            GameAudioControl.ErrorAngles.Add(minBetweenAngle);
                        }
                    }

                    if(PlayerPrefs.GetInt("ExperimentAdaptiveShrink")==0 && PlayerPrefs.GetInt("ExperimentTask")!=2){                        
                        // if activated adpative shrink, 
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
                    }else if(PlayerPrefs.GetInt("ExperimentAdaptiveShrink")==1 && PlayerPrefs.GetInt("ExperimentTask")!=2){
                        // if not activated adpative shrink
                        GameAudioControl.MoveToNextPosition();
                        GameUIControl.Verbal.PlayOneShot(GameUIControl.Clip3_SuccessHit);
                        HitCount += 1;
                    }
                }else if(PlayerPrefs.GetInt("ExperimentAdaptiveShrink")==0 && LastHitObject != null && GameAudioControl.isMoving == false && isMotionReset == true){
                    // Leaving the AudioSource Sphere
                    if(LastHitObject.name.Contains("Sphere")){
                        GameAudioControl.AdpativeShrink(minBetweenAngle, maxElevationAngel, target_initialDown);
                        isMotionReset = false;
                        GameAudioControl.ColorHit(false);
                        GameAudioControl.OverhitColor(false);
                    }
                }else if(PlayerPrefs.GetInt("ExperimentAdaptiveShrink")==1 && LastHitObject != null && GameAudioControl.isMoving == false && isMotionReset == true){
                    if(LastHitObject.name.Contains("Sphere")){
                        GameAudioControl.ColorHit(false);
                    }
                }
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
