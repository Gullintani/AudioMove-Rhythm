using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{   
    public MobileController phone;
    public AudioController AudioController;
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount == 120) {
            WorldCalibration();
        }

        if (Time.frameCount == 130) {
            AudioController.GetComponent<AudioSource>().Play();
        }

    }

    public void WorldCalibration(){
        transform.rotation = phone.transform.rotation;
        transform.rotation *= Quaternion.Euler(0,0,180f);
        phone.SetInitialDownDirection();
    }
}
