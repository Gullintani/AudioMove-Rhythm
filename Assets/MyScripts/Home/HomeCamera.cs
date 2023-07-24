using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCamera : MonoBehaviour
{
    private float RotationSpeed = 10f;
    private float MovingSpeed = 15f;
    public HomeUI HomeUI;
    private Quaternion InitialRotation;
    void Start()
    {
        // Initial visual state
        InitialRotation = transform.rotation;
    }
    void Update()
    {   
        if(HomeUI.CurrentModeSelection == "Achievements"){
            RotateAchievements();
        }
        if(HomeUI.CurrentModeSelection == "Home"){
            RotateHome();
        }
    }
    public void RotateAchievements(){
        Vector3 TargetDirection = new Vector3(transform.rotation.x + 90f, transform.rotation.y, transform.rotation.z);
        Quaternion TargetRotation = Quaternion.LookRotation(TargetDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, RotationSpeed * Time.deltaTime);
    }

    public void RotateHome(){
        transform.rotation = Quaternion.Slerp(transform.rotation, InitialRotation, RotationSpeed * Time.deltaTime);
    }
}
