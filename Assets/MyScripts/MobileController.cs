using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource SE;
    public CubeController AudioSourceCube;
    private Gyroscope gyro;
    private Vector3 acce, pointingDirection;
    private float MaximumDistance = 30.0f;
    private Quaternion initialRotation, gyroInitialRotation;
    private bool cubeHit;
    private RaycastHit lastHit;
    private AudioSource SpatialAudioSource;
    public float minimumAngle = 90.0f, maxAlphaAngel;
    public Vector3 initialDownDirection, initialRightDirection;
    private bool canHit = true, cubeHitSEFlag = true, isOverhit = false;

    // ============= EularAngles Settings =============
    private Vector3 cal_rotation;
    private Matrix4x4 T_ini_mat, current_mat;
    public Transform empty;
    Gyroscope m_Gyro;


    void Start()
    {   
        // ======== Sensor Input ========
        Input.gyro.enabled = true;
        gyro = Input.gyro;
        m_Gyro = Input.gyro;
        initialRotation = transform.rotation;
        gyroInitialRotation = gyro.attitude;

        // Initialize Audio Source
        SpatialAudioSource = AudioSourceCube.GetComponent<AudioSource>();
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
        cubeHit = Physics.Raycast(ray, out hit) && hit.transform == AudioSourceCube.transform;
        AudioSourceCube.SetGazedAt(cubeHit);
        // lastHit Initialization
        if (Time.frameCount == 1) {
            lastHit = hit;
            // Debug.Log("lastHit Initialization Complete.");
        }
        

        // initialDownDirection Initialization
        if (Time.frameCount == 120){
            SetInitialDownDirection();
            initialRightDirection = transform.right;
            ResetGyro();
        }

        // Calculate AngleBetween: pointingDirection and AudioSource position
        float AngleBetween = GetAngleBetweenTargetAndPointing(pointingDirection, AudioSourceCube.transform.position);
        // Calculate alphaAngel: pointingDirection and initialDownDirection;
        float alphaAngel = Vector3.Angle(pointingDirection, initialDownDirection);
        // Calculate betaAngle: initialDownDirection and AudioSource Position
        float betaAngle = Vector3.Angle(initialDownDirection, AudioSourceCube.transform.position);
        float phiAngle = Vector3.Angle(pointingDirection, initialRightDirection);
        float betaAngle_OE = CartesianToSpherical(AudioSourceCube.transform.localPosition);
        // Debug.Log(betaAngle_OE);

        Vector3 currentTargetPosition = AudioSourceCube.transform.localPosition - AudioSourceCube.BodyPositionOffSet;

        if (alphaAngel < 30.0f){
            canHit = true;
        }

        // ============= EularAngles Settings =============
        current_mat = Matrix4x4.Rotate(m_Gyro.attitude);
        var after = new Matrix4x4();

        after.m00 = T_ini_mat.m00 * current_mat.m00 + T_ini_mat.m01 * current_mat.m10 + T_ini_mat.m02 * current_mat.m20;
        after.m01 = T_ini_mat.m00 * current_mat.m01 + T_ini_mat.m01 * current_mat.m11 + T_ini_mat.m02 * current_mat.m21;
        after.m02 = T_ini_mat.m00 * current_mat.m02 + T_ini_mat.m01 * current_mat.m12 + T_ini_mat.m02 * current_mat.m22;

        after.m10 = T_ini_mat.m10 * current_mat.m00 + T_ini_mat.m11 * current_mat.m10 + T_ini_mat.m12 * current_mat.m20;
        after.m11 = T_ini_mat.m10 * current_mat.m01 + T_ini_mat.m11 * current_mat.m11 + T_ini_mat.m12 * current_mat.m21;
        after.m12 = T_ini_mat.m10 * current_mat.m02 + T_ini_mat.m11 * current_mat.m12 + T_ini_mat.m12 * current_mat.m22;

        after.m20 = T_ini_mat.m20 * current_mat.m00 + T_ini_mat.m21 * current_mat.m10 + T_ini_mat.m22 * current_mat.m20;
        after.m21 = T_ini_mat.m20 * current_mat.m01 + T_ini_mat.m21 * current_mat.m11 + T_ini_mat.m22 * current_mat.m21;
        after.m22 = T_ini_mat.m20 * current_mat.m02 + T_ini_mat.m21 * current_mat.m12 + T_ini_mat.m22 * current_mat.m22;

        Vector3 forward;
        forward.x = after.m02;
        forward.y = after.m12;
        forward.z = after.m22;

        Vector3 upwards;
        upwards.x = after.m01;
        upwards.y = after.m11;
        upwards.z = after.m21;

        empty.rotation = Quaternion.LookRotation(forward, upwards);
        cal_rotation = empty.eulerAngles;

        // Main Debug
        if (!AudioSourceCube.isMoving){
            // Debug.Log(empty.eulerAngles.y);
            Debug.Log(AngleBetween + ";" + minimumAngle + ";" + alphaAngel + ";" + phiAngle+ ";" + maxAlphaAngel + ";" + betaAngle + ";" + canHit + ";" + cubeHit + ";" + isOverhit + ";" + currentTargetPosition + ";" + empty.eulerAngles.x + ";" + empty.eulerAngles.y + ";" + empty.eulerAngles.z);
        }
        
        // Pitch Guidance
        // if (AngleBetween < 90){
        //     SpatialAudioSource.pitch = 1 + (90 - AngleBetween)/90 * SpatialAudioPitchScale;
        // }

        if (canHit && !AudioSourceCube.isMoving){
            // Cube Hit Event Handler
            if (cubeHit){
                
                // Record Maximum alphaAngle
                if (alphaAngel > maxAlphaAngel){
                    maxAlphaAngel = alphaAngel;
                }

                // Record Minimum AngleBetween
                if (AngleBetween < minimumAngle){
                    minimumAngle = AngleBetween;
                }
                // Debug
                // Debug.Log("CubeHit! Minimum Angle Between [PointingDirection:]" + pointingDirection + "and [AudioSourcePosition:]" + AudioSourceCube.transform.position + " is: " + minimumAngle);

                // Enter HitEvent SE
                if (cubeHitSEFlag){
                    SE.Play();
                    cubeHitSEFlag = false;
                }

                if (maxAlphaAngel > betaAngle + 10 ){
                    isOverhit = true;
                }
                AudioSourceCube.OverhitColor(isOverhit);

                // Add Vibration
                // Handheld.Vibrate();
                
            }else{

                // Debug
                // Debug.Log("CubeOut! Minimum Angle Between is: " + minimumAngle);

                if(lastHit.transform.name == AudioSourceCube.transform.name){
                    // Debug.Log("Exit CubeHit");
                    AudioSourceCube.CubeShrink(minimumAngle, maxAlphaAngel, betaAngle);
                    
                    // Disable Hit After leave
                    canHit = false;

                    // Reset cubeHitSEFlag
                    cubeHitSEFlag = true;

                    // Reset isOverHit
                    isOverhit = false;

                    // Leave HitEvent SE
                    // SE.Play();
                }
            }
        }
        lastHit = hit;
        
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

    public void ResetGyro(){
        // ============= EularAngles Settings =============
        current_mat = Matrix4x4.Rotate(m_Gyro.attitude);
        // ============= EularAngles Settings =============
        T_ini_mat = current_mat.transpose;
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
