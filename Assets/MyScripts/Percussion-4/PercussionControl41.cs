using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercussionControl41 : MonoBehaviour
{
    private AudioSource percussion;
    private bool triggerFlag;
    public MobileController mobileController;
    public float tolerant = 20.0f;
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
        if (angleBetween <= tolerant && triggerFlag){
            percussion.Play();
            triggerFlag = false;
            Debug.Log("Percussion Triggered.");
        }else if(angleBetween > tolerant && !triggerFlag){
            triggerFlag = true;
        }
    }
}
