using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{   
    public MobileController phone;
    public AudioSource SEStart;
    public CubeController CubeController;
    // Start is called before the first frame update
    void Start()
    {   
        // Camera Body Position OffSet Setting
        // transform.Find("Main Camera").localPosition = new Vector3(1.38f, 2.23f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount == 120) {
            WorldCalibration();
            // Debug.Log("World Calibration Complete.");
            // SEStart.Play();
        }

        if (Time.frameCount == 130) {
            CubeController.GetComponent<AudioSource>().Play();
        }

        // Delayed Voice Prompt
        // if (Time.frameCount == 240) {
        //     SEStart.Play();
        // }
    }

    public void WorldCalibration(){
        // Debug.Log("At frame 60, phone rotation is: " + phone.transform.rotation);
        // ResonanceRoom.transform.rotation = phone.transform.rotation;
        // Camera.transform.rotation = phone.transform.rotation;
        transform.rotation = phone.transform.rotation;
        
        // Debug for the initial mobile rotation
        // Debug.Log("phone rotation is:"+phone.transform.rotation);
        transform.rotation *= Quaternion.Euler(0,0,180f);
        // phone.initialDownDirection = transform.up;
        phone.SetInitialDownDirection();
        phone.ResetGyro();
        SEStart.Play();

    }
}
