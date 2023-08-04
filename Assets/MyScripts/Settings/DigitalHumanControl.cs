using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalHumanControl : MonoBehaviour

{
    public GameObject IKLeftHandEffector;
    public GameObject IKRightHandEffector;
    public CameraRotation CameraRotation;
    public UIControlBodyPosition UIControlBodyPosition;
    public TargetPositionManager TargetPositionManager;
    private Vector3 InitialIKLeftHandEffectorPosition, IKLeftHandEffectorDownPosition;
    private Vector3 InitialIKRightHandEffectorPosition, IKRightHandEffectorDownPosition;
    private float MovingSpeed = 5.0f;
    private int currentTargetIndex = 0;
    
    private void Start()
    {
        // Initial variable settings
        InitialIKLeftHandEffectorPosition = IKLeftHandEffector.transform.position;
        InitialIKRightHandEffectorPosition = IKRightHandEffector.transform.position;
        IKLeftHandEffectorDownPosition = new Vector3(-0.209000006f, 1.00600004f, 0.0320000015f);
        IKRightHandEffectorDownPosition = new Vector3(0.208000004f, 0.994000018f, 0.0120000001f);

        // Control the animation in Preview view
    }

    private void Update() {
        if(UIControlBodyPosition.CurrentView == "TargetSetting"){
            IKLeftHandEffector.transform.localPosition = IKLeftHandEffectorDownPosition;
            IKRightHandEffector.transform.localPosition = IKRightHandEffectorDownPosition;
            if(CameraRotation.IsPreviewSphere(CameraRotation.CurrentTargetSelection) == true){
                IKLeftHandEffector.transform.position = TargetPositionManager.LimbEffectorCartesian + new Vector3(0f, 0f, 0f);
            }
        }
        else if(UIControlBodyPosition.CurrentView == "Preview"){
            if(Vector3.Distance(IKLeftHandEffector.transform.position, TargetPositionManager.PositionListPlay[currentTargetIndex]) < 0.1f){
                currentTargetIndex = (currentTargetIndex + 1) % TargetPositionManager.PositionListPlay.Count;
                // Debug.Log("Digital Human Limb Arrive at Destination" + TargetPositionManager.PositionListPlay[currentTargetIndex]);
            }
            MoveToTarget(IKLeftHandEffector);
        }
        else{
            IKLeftHandEffector.transform.position = InitialIKLeftHandEffectorPosition;
            IKRightHandEffector.transform.position = InitialIKRightHandEffectorPosition;
        }
    }

    void MoveToTarget(GameObject MovingGameObject)
    {
        MovingGameObject.transform.position = Vector3.Lerp(MovingGameObject.transform.position, TargetPositionManager.PositionListPlay[currentTargetIndex], MovingSpeed * Time.deltaTime);
    }

}