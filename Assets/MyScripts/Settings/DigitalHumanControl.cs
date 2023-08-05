using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalHumanControl : MonoBehaviour

{
    public GameObject IKLeftHandEffector;
    public GameObject IKRightHandEffector;
    public GameObject IKLeftLegEffector;
    public GameObject IKRightLegEffector;
    public GameObject CurrentMovingEffector;
    public string AttachedLimb;
    public CameraRotation CameraRotation;
    public UIControlBodyPosition UIControlBodyPosition;
    public TargetPositionManager TargetPositionManager;
    private Vector3 InitialIKLeftHandEffectorPosition, IKLeftHandEffectorDownPosition;
    private Vector3 InitialIKRightHandEffectorPosition, IKRightHandEffectorDownPosition;
    private Vector3 InitialIKLeftLegEffectorPosition, InitialIKRightLegEffectorPosition;
    private float MovingSpeed = 5.0f;
    private int currentTargetIndex = 0;
    private void Start()
    {
        // Initial variable settings
        InitialIKLeftHandEffectorPosition = IKLeftHandEffector.transform.position;
        InitialIKRightHandEffectorPosition = IKRightHandEffector.transform.position;
        InitialIKLeftLegEffectorPosition = IKLeftLegEffector.transform.position;
        InitialIKRightLegEffectorPosition = IKRightLegEffector.transform.position;
        IKLeftHandEffectorDownPosition = new Vector3(-0.209000006f, 1.00600004f, 0.0320000015f);
        IKRightHandEffectorDownPosition = new Vector3(0.208000004f, 0.994000018f, 0.0120000001f);

        // Control the animation in Preview view
    }

    private void Update() {
        // Update the CurrentMovingEffector
        if(AttachedLimb != null){
            if(AttachedLimb.Contains("LeftUpperArm")||AttachedLimb.Contains("LeftLowerArm")){
                CurrentMovingEffector = IKLeftHandEffector;
            }else if(AttachedLimb.Contains("RightUpperArm")||AttachedLimb.Contains("RightLowerArm")){
                CurrentMovingEffector = IKRightHandEffector;
            }else if(AttachedLimb.Contains("LeftUpperLeg")||AttachedLimb.Contains("LeftLowerLeg")){
                CurrentMovingEffector = IKLeftLegEffector;
            }else if(AttachedLimb.Contains("RightUpperLeg")||AttachedLimb.Contains("RightLowerLeg")){
                CurrentMovingEffector = IKRightLegEffector;
            }
        }
        
        // Following target in animation view
        if(UIControlBodyPosition.CurrentView == "TargetSetting"){
            IKLeftHandEffector.transform.localPosition = IKLeftHandEffectorDownPosition;
            IKRightHandEffector.transform.localPosition = IKRightHandEffectorDownPosition;
            IKLeftLegEffector.transform.position = InitialIKLeftLegEffectorPosition;
            IKRightLegEffector.transform.position = InitialIKRightLegEffectorPosition;
            if(CameraRotation.CurrentTargetSelection!=null && CameraRotation.IsPreviewSphere(CameraRotation.CurrentTargetSelection) == true){
                // Position control of effector
                CurrentMovingEffector.transform.position = TargetPositionManager.LimbEffectorCartesian + new Vector3(0f, 0f, 0f);
                // Rotation control of effector
                // CurrentMovingEffector.transform.LookAt(new Vector3(TargetPositionManager.LimbEffectorCartesian.x, TargetPositionManager.LimbEffectorCartesian.y, TargetPositionManager.LimbEffectorCartesian.z));
                // CurrentMovingEffector.transform.LookAt(CameraRotation.CurrentTargetSelection.transform, Vector3.up);
            }
        }
        // Animation in preview view
        else if(UIControlBodyPosition.CurrentView == "Preview"){
            if(Vector3.Distance(CurrentMovingEffector.transform.position, TargetPositionManager.PositionListPlay[currentTargetIndex]) < 0.1f){
                currentTargetIndex = (currentTargetIndex + 1) % TargetPositionManager.PositionListPlay.Count;
                // Debug.Log("Digital Human Limb Arrive at Destination" + TargetPositionManager.PositionListPlay[currentTargetIndex]);
            }
            MoveToTarget(CurrentMovingEffector);
        }
        else{
            IKLeftHandEffector.transform.position = InitialIKLeftHandEffectorPosition;
            IKRightHandEffector.transform.position = InitialIKRightHandEffectorPosition;
            IKLeftLegEffector.transform.position = InitialIKLeftLegEffectorPosition;
            IKRightLegEffector.transform.position = InitialIKRightLegEffectorPosition;
        }
    }

    private void MoveToTarget(GameObject MovingGameObject)
    {
        MovingGameObject.transform.position = Vector3.Lerp(MovingGameObject.transform.position, TargetPositionManager.PositionListPlay[currentTargetIndex], MovingSpeed * Time.deltaTime);
    }

}