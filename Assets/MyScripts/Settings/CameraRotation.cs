using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public GameObject Dummy;
    public GameObject Headphone;
    public GameObject PhoneLeftUpperArm;
    public GameObject PhoneLeftLowerArm;
    public GameObject PhoneRightUpperArm;
    public GameObject PhoneRightLowerArm;
    public GameObject PhoneLeftUpperLeg;
    public GameObject PhoneLeftLowerLeg;
    public GameObject PhoneRightUpperLeg;
    public GameObject PhoneRightLowerLeg;
    public UIControlBodyPosition UISetting;
    public TargetPositionManager TargetPositionManager;
    public DigitalHumanControl DigitalHumanControl;
    private float RotationSpeed;
    private float SelfRotationSpeed;
    private float MovingSpeed = 15f;
    public Vector3 BodyPositionCameraBasePosition;
    public Vector3 TargetSettingCameraBasePosition;
    public Vector3 MovingDestinationPosition;
    private Vector3 HeadphonePosition;
    public bool isMovingCamera;
    public bool isRotatingCamera;
    public bool isSelectingMusic;
    public GameObject CurrentSelection = null;
    public GameObject CurrentTargetSelection;
    private Vector3 CurrentLookAt;
    private Quaternion TargetRotation;
    public Material OriginalMaterial;
    public Material HighlightMaterial;
    private float StartThetaAngle, EndThetaAngle, StartPhiAngle, EndPhiAngle;
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
        TargetSettingCameraBasePosition = new Vector3(0f, 1f, -6f);
        isMovingCamera = false;
        isRotatingCamera = false;
        isSelectingMusic = true;
        RotationSpeed = 10f;
        SelfRotationSpeed = 10f;
        MovingSpeed = 10f;
        HeadphonePosition = new Vector3(1.87f, 0f, 4f);
        // Limb motion limitation
        StartThetaAngle = 45f;
        EndThetaAngle = 135f;
        StartPhiAngle = -30f;
        EndPhiAngle = 30f;

    }

    private void Update()
    {   
        // Camera self rotation control
        if (isSelectingMusic == false){
            CurrentLookAt = Dummy.transform.position;
            TargetRotation = Quaternion.LookRotation(Dummy.transform.position - transform.position);
            Headphone.SetActive(false);
        }else{
            CurrentLookAt = HeadphonePosition;
            TargetRotation = Quaternion.LookRotation(HeadphonePosition - transform.position);
            Headphone.SetActive(true);
        }
        if(TargetRotation != transform.rotation){
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, SelfRotationSpeed * Time.deltaTime);
        } else {
                transform.LookAt(CurrentLookAt);
        }
        
        // Camera transform control
        if (isMovingCamera == true){
            transform.position = Vector3.MoveTowards(transform.position, MovingDestinationPosition, MovingSpeed * Time.deltaTime);
            if(transform.position == MovingDestinationPosition){
                isMovingCamera = false;
            }
        }

        // Camera rotation control, And control the click on remove button in target setting scene
        if (Input.touchCount > 0 && isMovingCamera == false && isSelectingMusic == false && Input.GetTouch(0).position.y > Screen.height * 0.10f && Input.GetTouch(0).position.y < Screen.height * 0.8f)
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
                    if(UISetting.CurrentView == "BodyPosition"){
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
                            DigitalHumanControl.AttachedLimb = CurrentSelection.name;
                            PlayerPrefs.SetString("BodyPosition", CurrentSelection.name);
                            // Update constrains depends on limb selection
                            UpdatePositionConstrain(CurrentSelection.name);
                        }
                    }

                    // In target setting view
                    if(UISetting.CurrentView == "TargetSetting"){
                        // Only hit the preview spheres
                        if(IsPreviewSphere(selectedObject)){
                            // Select preview sphere
                            if(CurrentTargetSelection != null && CurrentTargetSelection.name != "cubeRoomEnv"){
                                DeSelectHighlightMaterial(CurrentTargetSelection);
                            }
                            SelectHighlightMaterial(selectedObject);
                            CurrentTargetSelection = selectedObject;
                        }else if(CurrentTargetSelection!=null && IsPreviewSphere(CurrentTargetSelection) == true && selectedObject.name == "cubeRoomEnv"){
                            // Cancel selection
                            DeSelectHighlightMaterial(CurrentTargetSelection);
                            CurrentTargetSelection = selectedObject;
                        }
                    }
                }
            }

            // Camera Rotation (only keep horizontal axis)
            if (touch.phase == TouchPhase.Moved && isSelectingMusic == false){
                // Get moving offset from mouse or touch
                float horizontalInput = Input.GetAxis("Mouse X") + Input.touches[0].deltaPosition.x;
                float verticalInput = Input.GetAxis("Mouse Y") + Input.touches[0].deltaPosition.y;
                if (UISetting.CurrentView != "TargetSetting"){
                    // Calculate rotation angle
                    float horizontalRotation = horizontalInput * RotationSpeed * Time.deltaTime;
                    float verticalRotation = verticalInput * RotationSpeed * Time.deltaTime;

                    // Rotate around the target
                    transform.RotateAround(Dummy.transform.position, Vector3.up, horizontalRotation);
                    transform.RotateAround(Dummy.transform.position, transform.right, -verticalRotation);
                }else if(UISetting.CurrentView == "TargetSetting"){
                    // Dragging target
                    if(IsPreviewSphere(CurrentTargetSelection)){
                        // Move the target
                        // CurrentTargetSelection.transform.position += new Vector3(horizontalInput * 0.005f, verticalInput * 0.005f, 0);
                        CurrentTargetSelection.transform.position = TargetPositionManager.PositionConstrain(SelectedTarget:CurrentTargetSelection, HorizontalInput:horizontalInput, VerticalInput:verticalInput, StartThetaAngle:StartThetaAngle, EndThetaAngle:EndThetaAngle, StartPhiAngle:StartPhiAngle, EndPhiAngle:EndPhiAngle, Distance:5f);
                    }else{
                        // In target setting view, move camera in a plane
                        transform.position += new Vector3(horizontalInput * 0.01f, 0f, 0f);
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
    public bool IsPreviewSphere(GameObject InputObject){
        return InputObject.name.ToLower().Contains("Preview".ToLower());
    }
    private void SelectHighlightMaterial(GameObject selection){
        Renderer renderer = selection.GetComponent<Renderer>();
        renderer.material = HighlightMaterial;
    }

    private void DeSelectHighlightMaterial(GameObject selection){
        Renderer renderer = selection.GetComponent<Renderer>();
        renderer.material = OriginalMaterial;
    }
    private void UpdatePositionConstrain(string AttachedLimb){
        if(AttachedLimb.Contains("LeftUpperArm")||AttachedLimb.Contains("RightUpperArm")){
                StartThetaAngle = 45f;
                EndThetaAngle = 135f;
                StartPhiAngle = -30f;
                EndPhiAngle = 30f;
                TargetPositionManager.SettingViewOffset = new Vector3(0f, 0f, 0f);
            }else if(AttachedLimb.Contains("LeftLowerArm")||AttachedLimb.Contains("RightLowerArm")){
                StartThetaAngle = 60;
                EndThetaAngle = 120f;
                StartPhiAngle = -30f;
                EndPhiAngle = 30f;
                TargetPositionManager.SettingViewOffset = new Vector3(0f, -0.5f, 0f);
            }else if(AttachedLimb.Contains("LeftUpperLeg")||AttachedLimb.Contains("RightUpperLeg")){
                StartThetaAngle = 60;
                EndThetaAngle = 120f;
                StartPhiAngle = -80f;
                EndPhiAngle = -30f;
                TargetPositionManager.SettingViewOffset = new Vector3(0f, -1f, 0f);
            }else if(AttachedLimb.Contains("LeftLowerLeg")||AttachedLimb.Contains("RightLowerLeg")){
                StartThetaAngle = 70;
                EndThetaAngle = 110f;
                StartPhiAngle = -80f;
                EndPhiAngle = -45f;
                TargetPositionManager.SettingViewOffset = new Vector3(0f, -1.5f, 0f);
            }
        TargetPositionManager.GenerateThetaStart = StartThetaAngle;
        TargetPositionManager.GenerateThetaEnd = EndThetaAngle;
    }
}

