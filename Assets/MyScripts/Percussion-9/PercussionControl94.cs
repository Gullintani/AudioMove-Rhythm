using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercussionControl94 : MonoBehaviour
{
    private AudioSource percussion;
    private bool triggerFlag;
    public MobileController mobileController;
    // Start is called before the first frame update
    void Start()
    {
        percussion = GetComponent<AudioSource>();
        triggerFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        float angleBetween = Vector3.Angle(transform.position, mobileController.pointingDirection);
        // Debug.Log("Percussion1 - PointingDirection: " + angleBetween);
        if (angleBetween < 20.0f && triggerFlag){
            percussion.Play();
            triggerFlag = false;
            Debug.Log("Percussion Triggered.");
        }else if(angleBetween > 20.0f && !triggerFlag){
            triggerFlag = true;
        }
    }
}
