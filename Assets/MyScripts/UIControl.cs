using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera[] CameraList;
    private int currentCameraIndex = 0;
    public MainController MainController;
    public AudioSource Verbal;
    public AudioClip Clip1_calibration;
    public AudioClip Clip2_start;
    public AudioController AudioController;
    public UIDocument UI;
    private VisualElement UIRoot;
    private Slider UISlider;
    public float SliderMinValue = 2, SliderMaxValue = 8;
    private Button UIButton;

    private void OnEnable() {
        Debug.Log("===================== OnEnable of UIControl =======================");
    }
    void Start()
    {   
        // Initialization
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButton = UIRoot.Q<Button>("ButtonCalibration");
        UISlider = UIRoot.Q<Slider>("SliderSize");
        
        // Event Register
        UISlider.RegisterValueChangedCallback(OnSliderValueChanged);

        // Button Event
        UIButton.clicked += delegate(){
            // Check if the programe have started
            if(MainController.HaveStarted){
                // If started, calibration
                MainController.WorldCalibration();
                Verbal.PlayOneShot(Clip1_calibration);
                SwitchToCameraBack();
            }else{
                // If not started, restart the music
                AudioController.GetComponent<AudioSource>().Play();
                MainController.WorldCalibration();
                MainController.HaveStarted = true;
                Verbal.PlayOneShot(Clip2_start);
                SwitchToCameraBack();
            }
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSliderValueChanged(ChangeEvent<float> evt)
    {   
        // Switch camera
        SwitchToCameraAbove();
        // Get slider data
        float SliderValue = UISlider.value;
        SliderValue = Mathf.Lerp(SliderMinValue, SliderMaxValue, SliderValue / 100f);
        AudioController.transform.localScale = new Vector3(SliderValue, SliderValue, SliderValue);
        // SwitchToCameraBack();
    }
    private void SwitchToCameraBack()
    {
            CameraList[currentCameraIndex].enabled = false;
            currentCameraIndex = 0;
            CameraList[currentCameraIndex].enabled = true;
    }
    private void SwitchToCameraAbove()
    {
            CameraList[currentCameraIndex].enabled = false;
            currentCameraIndex = 1;
            CameraList[currentCameraIndex].enabled = true;
    }
    private void SwitchToCameraSide()
    {
            CameraList[currentCameraIndex].enabled = false;
            currentCameraIndex = 2;
            CameraList[currentCameraIndex].enabled = true;
    }
}
