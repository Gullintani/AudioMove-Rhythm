using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentGameMainControl : MonoBehaviour
{   
    public ExperimentGameMobileControl Phone;
    public GameObject HeadTracker;
    // public ExperimentGameAudioControl GameAudioControl;
    // public ExperimentGameUIControl GameUIControl;
    
    void Start()
    {
        // Debug PlayerPrefs Settings
        PlayerPrefs.SetString("ExperimentLimb", "LeftUpperArm");
        PlayerPrefs.SetInt("ExperimentTask", 1);
        PlayerPrefs.SetInt("ExperimentTrail", 1);
        PlayerPrefs.SetInt("ExperimentMotion", 1);
        PlayerPrefs.SetInt("ExperimentAdaptiveShrink", 1);

        // PlayerPrefs settings:
        if(PlayerPrefs.GetInt("ExperimentMotion") != 2){
            HeadTracker.SetActive(false);
        }
    }

    void Update()
    {   

    }

    public void WorldCalibration(){
        transform.rotation = Phone.transform.rotation;
        transform.rotation *= Quaternion.Euler(0,0,180f);
        Phone.SetInitialDownDirection();
    }

}
