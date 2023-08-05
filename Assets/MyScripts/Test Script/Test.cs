using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject LeftHandEffector;
    public GameObject Target;    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        LeftHandEffector.transform.position = new Vector3(Target.transform.position.x/2, Target.transform.position.y, Target.transform.position.z/2);
        LeftHandEffector.transform.LookAt(Target.transform);
        LeftHandEffector.transform.RotateAround(LeftHandEffector.transform.position, LeftHandEffector.transform.right, 90);
    }
}
