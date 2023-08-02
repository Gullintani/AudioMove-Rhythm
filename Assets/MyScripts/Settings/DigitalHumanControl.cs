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
    
    private void Start()
    {
        InitialIKLeftHandEffectorPosition = IKLeftHandEffector.transform.position;
        InitialIKRightHandEffectorPosition = IKRightHandEffector.transform.position;
        IKLeftHandEffectorDownPosition = new Vector3(-0.209000006f, 1.00600004f, 0.0320000015f);
        IKRightHandEffectorDownPosition = new Vector3(0.208000004f, 0.994000018f, 0.0120000001f);
    }

    private void Update() {
        if(UIControlBodyPosition.CurrentView == "TargetSetting"){
            IKLeftHandEffector.transform.localPosition = IKLeftHandEffectorDownPosition;
            IKRightHandEffector.transform.localPosition = IKRightHandEffectorDownPosition;
            if(CameraRotation.IsPreviewSphere(CameraRotation.CurrentTargetSelection) == true){
                IKLeftHandEffector.transform.position = TargetPositionManager.LimbEffectorCartesian;
            }
        }else{
            IKLeftHandEffector.transform.position = InitialIKLeftHandEffectorPosition;
            IKRightHandEffector.transform.position = InitialIKRightHandEffectorPosition;
        }
    }
}