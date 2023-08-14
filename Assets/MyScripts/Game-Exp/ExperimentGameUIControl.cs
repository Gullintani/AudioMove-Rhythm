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
    private Button UIButtonNextPosition;
    private Label UITextTarget1;
    private Label UITextTarget2;
    private Label UITextTarget3;
    private Label UITextTarget4;
    private Label UITextTarget5;
    private Label UITextTarget6;
    private Label UITextTime;
    private Label UICurrentState;
    private Label UIHeadAngleBetween;
    private int AllClearNumber;



    private void OnEnable() {
        
    }
    void Start()
    {   
        // Initialize variables
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButtonCalibration = UIRoot.Q<Button>("ButtonCalibration");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");
        UIButtonNextPosition = UIRoot.Q<Button>("NextPosition");
        UITextTime = UIRoot.Q<Label>("Time");
        UITextTarget1 = UIRoot.Q<Label>("Target1");
        UITextTarget2 = UIRoot.Q<Label>("Target2");
        UITextTarget3 = UIRoot.Q<Label>("Target3");
        UITextTarget4 = UIRoot.Q<Label>("Target4");
        UITextTarget5 = UIRoot.Q<Label>("Target5");
        UITextTarget6 = UIRoot.Q<Label>("Target6");
        UICurrentState = UIRoot.Q<Label>("CurrentState");
        UIHeadAngleBetween = UIRoot.Q<Label>("HeadAngleBetween");

        if(PlayerPrefs.GetInt("ExperimentTask")==1){
            AllClearNumber = 18;
        }else if(PlayerPrefs.GetInt("ExperimentTask")==2){
            AllClearNumber = 3;
        }else if(PlayerPrefs.GetInt("ExperimentTask")==0){
            AllClearNumber = 8;
        }

        // OnStart displays and verbal prompt
        Verbal.PlayOneShot(Clip1_ExerciseStart);
        UITextTime.style.display = DisplayStyle.None;
        UITextTarget1.style.display = DisplayStyle.None;
        UITextTarget2.style.display = DisplayStyle.None;
        UITextTarget3.style.display = DisplayStyle.None;
        UITextTarget4.style.display = DisplayStyle.None;
        UITextTarget5.style.display = DisplayStyle.None;
        UITextTarget6.style.display = DisplayStyle.None;
        DisplayCurrentState();
        if(PlayerPrefs.GetInt("ExperimentTask")==0){
            UIButtonNextPosition.style.display = DisplayStyle.Flex;
            UIHeadAngleBetween.style.display = DisplayStyle.Flex;
        }else{
            UIButtonNextPosition.style.display = DisplayStyle.None;
            UIHeadAngleBetween.style.display = DisplayStyle.None;
        }
        

        // Set Button Event
        UIButtonCalibration.clicked += delegate(){
            Verbal.PlayOneShot(Clip2_Calibration);
            // Calibrate headphone and smartphone at the same time
            HeadphoneMotionExample.CalibrateStartingRotation();
            GameMainControl.WorldCalibration();
        };
        UIButtonBack.clicked += delegate(){
            SceneManager.LoadScene("ExperimentSettings");
        };
        UIButtonNextPosition.clicked += delegate(){
            GameAudioControl.MoveToNextPosition();
        };
    }

    void Update(){
        if(GameAudioControl.ErrorAngles.Count == AllClearNumber){
            GameMainControl.IsTiming = false;
            ShowResults();
            GameAudioControl.gameObject.SetActive(false);
        }
        if(PlayerPrefs.GetInt("ExperimentTask")==0){
            UIHeadAngleBetween.text = GameAudioControl.HeadAngleBetween.ToString();
        }
    }

    void DisplayCurrentState(){
        UICurrentState.text = PlayerPrefs.GetString("ExperimentLimb") + "-Task" + PlayerPrefs.GetInt("ExperimentTask") + "-Trace" + PlayerPrefs.GetInt("ExperimentTrail", 1) +"-Motio"+ PlayerPrefs.GetInt("ExperimentMotion", 1) + "-Shrink" + PlayerPrefs.GetInt("ExperimentAdaptiveShrink", 0);
    }

    void ShowResults(){
        UITextTime.text = "Time: " + GameMainControl.Timer.ToString();
        if(PlayerPrefs.GetInt("ExperimentTask")==1){
            UITextTarget1.text = "1: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(0, 3));
            UITextTarget2.text = "2: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(3, 3));
            UITextTarget3.text = "3: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(6, 3));
            UITextTarget4.text = "4: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(9, 3));
            UITextTarget5.text = "5: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(12, 3));
            UITextTarget6.text = "6: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(15, 3));
            UITextTime.style.display = DisplayStyle.Flex;
            UITextTarget1.style.display = DisplayStyle.Flex;
            UITextTarget2.style.display = DisplayStyle.Flex;
            UITextTarget3.style.display = DisplayStyle.Flex;
            UITextTarget4.style.display = DisplayStyle.Flex;
            UITextTarget5.style.display = DisplayStyle.Flex;
            UITextTarget6.style.display = DisplayStyle.Flex;
        }else if(PlayerPrefs.GetInt("ExperimentTask")==2){
            UITextTarget1.text = "1: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(0, 1));
            UITextTarget2.text = "2: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(1, 1));
            UITextTarget3.text = "3: " + string.Join(", ", GameAudioControl.ErrorAngles.GetRange(2, 1));
            UITextTime.style.display = DisplayStyle.Flex;
            UITextTarget1.style.display = DisplayStyle.Flex;
            UITextTarget2.style.display = DisplayStyle.Flex;
            UITextTarget3.style.display = DisplayStyle.Flex;
        }        
    }
    
}
