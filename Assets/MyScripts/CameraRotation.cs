using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public GameObject target;
    public GameObject PhoneLeftUpperArm;
    public GameObject PhoneLeftLowerArm;
    public GameObject PhoneRightUpperArm;
    public GameObject PhoneRightLowerArm;
    public GameObject PhoneLeftUpperLeg;
    public GameObject PhoneLeftLowerLeg;
    public GameObject PhoneRightUpperLeg;
    public GameObject PhoneRightLowerLeg;
    public float rotationSpeed = 50f;
    // private Material OriginalMaterial;
    // public Material SelectMaterial;
    private GameObject CurrentSelection = null;
    private void Start()
    {   
        HidePhone(PhoneLeftUpperArm);
        HidePhone(PhoneLeftLowerArm);
        HidePhone(PhoneRightUpperArm);
        HidePhone(PhoneRightLowerArm);
        HidePhone(PhoneLeftUpperLeg);
        HidePhone(PhoneLeftLowerLeg);
        HidePhone(PhoneRightUpperLeg);
        HidePhone(PhoneRightLowerLeg);

        // Pass data through scenes test
        // PlayerPrefs.SetInt("score", 100);
    }

    private void Update()
    {   
        // Camera Rotation (only keep horizontal axis)
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
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
                    }else{
                        HidePhone(CurrentSelection);
                        CurrentSelection = null;
                    }

                    // Material change (hard to implement)
                    // GameObject selectedObject = hit.collider.gameObject;
                    // if (selectedObject.name != "cubeRoomEnv"){
                    //     if (CurrentSelection != null){
                    //         DeSelect(CurrentSelection);
                    //     }
                    //     Select(selectedObject);
                    //     CurrentSelection = selectedObject;
                    // }else{
                    //     DeSelect(CurrentSelection);
                    //     CurrentSelection = null;
                    // }
                    Debug.Log("Selected object: " + CurrentSelection.name);
                }
            }

            if (touch.phase == TouchPhase.Moved){
                // Get moving offset from mouse or touch
                float horizontalInput = Input.GetAxis("Mouse X") + Input.touches[0].deltaPosition.x;
                // float verticalInput = Input.GetAxis("Mouse Y") + Input.touches[0].deltaPosition.y;
                
                // Calculate rotation angle
                float horizontalRotation = horizontalInput * rotationSpeed * Time.deltaTime;
                // float verticalRotation = verticalInput * rotationSpeed * Time.deltaTime;
                // Debug.Log(horizontalRotation);

                // Rotate around the target
                transform.RotateAround(target.transform.position, Vector3.up, horizontalRotation);
                // transform.RotateAround(target.transform.position, transform.right, -verticalRotation);
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

    // private void SelectHighlightMaterial(GameObject selection){
    //     Renderer renderer = selection.GetComponent<Renderer>();
    //     if (renderer != null)
    //     {
    //         OriginalMaterial = renderer.material;
    //         renderer.material = SelectMaterial;
    //     }
    // }

    // private void DeSelectHighlightMaterial(GameObject selection){
    //     Renderer renderer = selection.GetComponent<Renderer>();
    //     if (renderer != null)
    //     {
    //         renderer.material = OriginalMaterial;
    //     }
    // }
}

