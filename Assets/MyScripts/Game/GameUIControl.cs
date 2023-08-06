using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using HearXR;
public class GameUIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera[] CameraList;
    public float TargetSize = 2f;
    public GameMainController GameMainController;
    public GameAudioController GameAudioController;
    public HeadphoneMotionExample HeadphoneMotionExample;
    public AudioSource Verbal;
    public AudioClip Clip1_ExerciseStart;
    public AudioClip Clip2_Calibration;
    public AudioClip Clip3_SuccessHit;
    public UIDocument UI;
    private VisualElement UIRoot;
    private Button UIButtonCalibration;
    private Button UIButtonBack;


    private void OnEnable() {
        
    }
    void Start()
    {   
        // Initialize variables
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButtonCalibration = UIRoot.Q<Button>("ButtonCalibration");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");

        // OnStart displays and verbal prompt
        Verbal.PlayOneShot(Clip1_ExerciseStart);

        // Set Button Event
        UIButtonCalibration.clicked += delegate(){
            Verbal.PlayOneShot(Clip2_Calibration);
            // Calibrate headphone and smartphone at the same time
            // HeadphoneMotionExample.CalibrateStartingRotation();
            // GameMainController.WorldCalibration();
        };
        UIButtonBack.clicked += delegate(){
            SceneManager.LoadScene("AudioMoveSettings");
        };
    }

    // Update is called once per frame
    void Update(){

    }

    private Vector3 CartesianToSpherical(Vector3 cartesian){
        Vector3 spherical;
        spherical.x = Mathf.Sqrt(cartesian.x * cartesian.x + cartesian.y * cartesian.y + cartesian.z * cartesian.z);
        spherical.y = Mathf.Acos(cartesian.y / spherical.x);
        spherical.z = Mathf.Atan2(cartesian.z, cartesian.x);
        return spherical;
    }
    public Vector3 SphericalToCartesian(Vector3 SphericalVector){
        float radius = SphericalVector.x;
        float polar = SphericalVector.y;
        float elevation = SphericalVector.z;

        float a = radius * Mathf.Cos(elevation);
        float x = a * Mathf.Cos(polar);
        float y = radius * Mathf.Sin(elevation);
        float z = a * Mathf.Sin(polar);
        return new Vector3(x, y, z);
    }
}
