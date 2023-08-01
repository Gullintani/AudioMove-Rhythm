using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalHumanControl : MonoBehaviour

{
    public GameObject IKLeftHandEffector;
    public CameraRotation CameraRotation;
    public UIControlBodyPosition UIControlBodyPosition;
    public TargetPositionManager TargetPositionManager;
    private Vector3 InitialIKLimbEffectorPosition;
    private void Start()
    {
        InitialIKLimbEffectorPosition = IKLeftHandEffector.transform.position;
    }

    private void Update() {
        if(UIControlBodyPosition.CurrentView == "TargetSetting" && CameraRotation.IsPreviewSphere(CameraRotation.CurrentTargetSelection) == true){
            IKLeftHandEffector.transform.position = TargetPositionManager.LimbEffectorCartesian;
        }else{
            IKLeftHandEffector.transform.position = InitialIKLimbEffectorPosition;
        }
    }
}