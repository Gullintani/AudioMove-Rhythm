using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentGameMainControl : MonoBehaviour
{   
    public ExperimentGameMobileControl Phone;
    // public ExperimentGameAudioControl GameAudioControl;
    // public ExperimentGameUIControl GameUIControl;
    
    private void OnEnable() {

    }
    void Start()
    {

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
