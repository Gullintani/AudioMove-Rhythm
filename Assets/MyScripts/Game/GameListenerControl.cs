using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListenerControl : MonoBehaviour
{
    public string AttachedLimb;
    void Start()
    {
        AttachedLimb = PlayerPrefs.GetString("BodyPosition");
        UpdateListenerOffset();
    }

    void Update()
    {
        
    }
    void UpdateListenerOffset(){
        if(AttachedLimb.Contains("LeftUpperArm")){
            this.transform.localPosition = new Vector3(1f, 1f, 0f);
        }else if(AttachedLimb.Contains("RightUpperArm")){
            this.transform.localPosition = new Vector3(-1f, 1f, 0f);
        }else if(AttachedLimb.Contains("LeftLowerArm")){
            this.transform.localPosition = new Vector3(1f, 2f, 0f);
        }else if(AttachedLimb.Contains("RightLowerArm")){
            this.transform.localPosition = new Vector3(-1f, 2f, 0f);
        }else if(AttachedLimb.Contains("LeftUpperLeg")){
            this.transform.localPosition= new Vector3(0.5f, 3.5f, 0f);
        }else if(AttachedLimb.Contains("RightUpperLeg")){
            this.transform.localPosition= new Vector3(-0.5f, 3.5f, 0f);
        }else if(AttachedLimb.Contains("LeftLowerLeg")){
            this.transform.localPosition= new Vector3(0.5f, 2.5f, 0f); // Assume sitting
        }else if(AttachedLimb.Contains("RightLowerLeg")){
            this.transform.localPosition= new Vector3(-0.5f, 2.5f, 0f); // Assume sitting
        }else{
            this.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}
