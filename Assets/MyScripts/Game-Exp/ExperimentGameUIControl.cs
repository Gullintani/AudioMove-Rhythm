using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using HearXR;
public class ExperimentGameUIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera[] CameraList;
    public float TargetSize = 2f;
    public ExperimentGameMainControl GameMainControl;
    public ExperimentGameAudioControl GameAudioControl;
    public HeadphoneMotionExample HeadphoneMotionExample;
    public AudioSource Verbal;
    public AudioClip Clip1_ExerciseStart;
    public AudioClip Clip2_Calibration;
    public AudioClip Clip3_SuccessHit;
    public AudioClip Clip4_SuccessHit2;
    public AudioClip Clip5_OverHit;
    public AudioClip Clip7_AudioMove;
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
            HeadphoneMotionExample.CalibrateStartingRotation();
            GameMainControl.WorldCalibration();
        };
        UIButtonBack.clicked += delegate(){
            SceneManager.LoadScene("AudioMoveSettings");
        };
    }

    void Update(){

    }

    void ShowResults(){
        
    }
    
}
