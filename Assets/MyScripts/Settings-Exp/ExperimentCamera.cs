using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentCamera : MonoBehaviour
{
    public GameObject Dummy;
    public GameObject CongifurationLookAt;
    public GameObject PhoneLeftUpperArm;
    public GameObject PhoneLeftLowerArm;
    public GameObject PhoneRightUpperArm;
    public GameObject PhoneRightLowerArm;
    public GameObject PhoneLeftUpperLeg;
    public GameObject PhoneLeftLowerLeg;
    public GameObject PhoneRightUpperLeg;
    public GameObject PhoneRightLowerLeg;
    public ExperimentUI ExperimentUI;
    public ExperimentTargetManager ExperimentTargetManager;
    private float SelfRotationSpeed;
    public Vector3 BodyPositionCameraBasePosition, ConfigurationPosition;
    public Vector3 MovingDestinationPosition;
    public bool isRotatingCamera;
    public GameObject CurrentSelection = null;
    private Vector3 CurrentLookAt;
    private Quaternion TargetRotation;
    public bool isConfiguration;
    private void Start()
    {   
        // Initial phone states
        HidePhone(PhoneLeftUpperArm);
        HidePhone(PhoneLeftLowerArm);
        HidePhone(PhoneRightUpperArm);
        HidePhone(PhoneRightLowerArm);
        HidePhone(PhoneLeftUpperLeg);
        HidePhone(PhoneLeftLowerLeg);
        HidePhone(PhoneRightUpperLeg);
        HidePhone(PhoneRightLowerLeg);

        // Initial variable setting
        BodyPositionCameraBasePosition = transform.position;
        ConfigurationPosition = CongifurationLookAt.transform.position;
        isRotatingCamera = false;
        isConfiguration = false;
        SelfRotationSpeed = 10f;

    }

    private void Update()
    {   
        // Camera self rotation control
        if (isConfiguration == false){
            CurrentLookAt = Dummy.transform.position;
            TargetRotation = Quaternion.LookRotation(Dummy.transform.position - transform.position);
        }else{
            CurrentLookAt = ConfigurationPosition;
            TargetRotation = Quaternion.LookRotation(ConfigurationPosition - transform.position);
        }
        if(TargetRotation != transform.rotation){
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, SelfRotationSpeed * Time.deltaTime);
        } else {
                transform.LookAt(CurrentLookAt);
        }
        
        // Camera rotation control, And control the click on remove button in target setting scene
        if (Input.touchCount > 0 && isConfiguration == false && Input.GetTouch(0).position.y > Screen.height * 0.10f && Input.GetTouch(0).position.y < Screen.height * 0.8f)
        // if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {   
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began){
                // Create ray from camera view
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {   
                    GameObject selectedObject = hit.collider.gameObject;
                    // In body position setting view
                    if(ExperimentUI.CurrentView == "BodyPosition"){
                        if (selectedObject.name != "cubeRoomEnv"){
                            // Hide previous selection phone
                            if(CurrentSelection != null){
                                HidePhone(CurrentSelection);
                            }
                            // Select current
                            if(selectedObject.name == "Left Arm"){
                                ShowPhone(PhoneLeftUpperArm);
                                CurrentSelection = PhoneLeftUpperArm;
                            }else if(selectedObject.name == "Left Forearm"){
                                ShowPhone(PhoneLeftLowerArm);
                                CurrentSelection = PhoneLeftLowerArm;
                            }else if(selectedObject.name == "Right Arm"){
                                ShowPhone(PhoneRightUpperArm);
                                CurrentSelection = PhoneRightUpperArm;
                            }else if(selectedObject.name == "Right Forearm"){
                                ShowPhone(PhoneRightLowerArm);
                                CurrentSelection = PhoneRightLowerArm;
                            }else if(selectedObject.name == "Left Thigh"){
                                ShowPhone(PhoneLeftUpperLeg);
                                CurrentSelection = PhoneLeftUpperLeg;
                            }else if(selectedObject.name == "Left Leg"){
                                ShowPhone(PhoneLeftLowerLeg);
                                CurrentSelection = PhoneLeftLowerLeg;
                            }else if(selectedObject.name == "Right Thigh"){
                                ShowPhone(PhoneRightUpperLeg);
                                CurrentSelection = PhoneRightUpperLeg;
                            }else if(selectedObject.name == "Right Leg"){
                                ShowPhone(PhoneRightLowerLeg);
                                CurrentSelection = PhoneRightLowerLeg;
                            }
                            // Save to PlayerPrefs
                            PlayerPrefs.SetString("ExperimentLimb", CurrentSelection.name);
                        }
                    }                    
                }
            }
        }
    }


    public void HidePhone(GameObject phone)
    {
        phone.SetActive(false);
    }

    public void ShowPhone(GameObject phone)
    {
        phone.SetActive(true);
    }
}

