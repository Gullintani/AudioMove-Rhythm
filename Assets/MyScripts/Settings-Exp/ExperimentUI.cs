using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ExperimentUI : MonoBehaviour
{
    public ExperimentCamera ExperimentCamera;
    public UIDocument UI;
    private VisualElement UIRoot;
    private Button UIButtonNext;
    private Button UIButtonBack;
    private RadioButtonGroup UITaskSelection;
    private RadioButtonGroup UITrailSelection;
    private RadioButtonGroup UIMotionSelection;
    private RadioButtonGroup UIAdaptiveShrink;
    public string CurrentView;

    void Start()
    {   
        // Variable initialize
        UIRoot = UI.rootVisualElement;
        UIButtonNext = UIRoot.Q<Button>("ButtonNext");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");
        UITaskSelection = UIRoot.Q<RadioButtonGroup>("TaskSelection");
        UITrailSelection = UIRoot.Q<RadioButtonGroup>("TrailSelection");
        UIMotionSelection = UIRoot.Q<RadioButtonGroup>("MotionSelection");
        UIAdaptiveShrink = UIRoot.Q<RadioButtonGroup>("AdaptiveShrink");

        // Button event registration
        UIButtonNext.clicked += delegate(){
            if(CurrentView == "BodyPosition"){
                DisplayConfiguration();
            }else if(CurrentView == "Configuration"){
                SaveConfiguration();
                SceneManager.LoadScene("ExperimentSystem");
            }
        };
        UIButtonBack.clicked += delegate(){
            if(CurrentView == "BodyPosition"){
                SceneManager.LoadScene("AudioMoveHome");
            }else if(CurrentView == "Configuration"){
                DisplayBodyPositionView();
            }
        };
        
        // Radio button group registration
        // UITaskSelection.RegisterValueChangedCallback(evt => Debug.Log(evt.newValue));

        // Display first view
        DisplayBodyPositionView();
    
    }

    void Update()
    {   
        // Handle display of Next button in BodyPosition view
        if(ExperimentCamera.CurrentSelection != null && CurrentView == "BodyPosition"){
            UIButtonNext.style.display = DisplayStyle.Flex;
        }

        // Handle display of TrailSelection in Configuration view when selection Task 3
        if(UITaskSelection.value != -1 && CurrentView == "Configuration"){
            UIMotionSelection.style.display = DisplayStyle.Flex;
            if(UITaskSelection.value == 2 || UITaskSelection.value == 1){
                UITrailSelection.style.display = DisplayStyle.Flex;
            }else{
                UITrailSelection.style.display = DisplayStyle.None;
                UITrailSelection.value = -1;
            }
        }else{
            UITrailSelection.style.display = DisplayStyle.None;
            UIMotionSelection.style.display = DisplayStyle.None;
            UITrailSelection.value = -1;
            UIMotionSelection.value = -1;
        }

        if(CurrentView == "Configuration" && UITaskSelection.value != -1 && UIMotionSelection.value != -1 && UIAdaptiveShrink.value != -1){
            UIButtonNext.style.display = DisplayStyle.Flex;
        }
    }

    void DisplayBodyPositionView(){
        CurrentView = "BodyPosition";
        ExperimentCamera.isConfiguration = false;
        UIButtonNext.style.display = DisplayStyle.None;
        UITaskSelection.style.display = DisplayStyle.None;
        UITrailSelection.style.display = DisplayStyle.None;
        UIMotionSelection.style.display = DisplayStyle.None;
        UIAdaptiveShrink.style.display = DisplayStyle.None;
        UITaskSelection.value = -1;
        UITrailSelection.value = -1;
        UIMotionSelection.value = -1;
        UIAdaptiveShrink.value = -1;
    }

    void DisplayConfiguration(){
        CurrentView = "Configuration";
        ExperimentCamera.isConfiguration = true;
        UIButtonNext.style.display = DisplayStyle.None;
        UITaskSelection.style.display = DisplayStyle.Flex;
        UIAdaptiveShrink.style.display = DisplayStyle.Flex;
    }

    void SaveConfiguration(){
        PlayerPrefs.SetString("ExperimentLimb", ExperimentCamera.CurrentSelection.name);
        PlayerPrefs.SetInt("ExperimentTask", UITaskSelection.value);
        PlayerPrefs.SetInt("ExperimentTrail", UITrailSelection.value);
        PlayerPrefs.SetInt("ExperimentMotion", UIMotionSelection.value);
        PlayerPrefs.SetInt("ExperimentAdaptiveShrink", UIAdaptiveShrink.value);
    }
}