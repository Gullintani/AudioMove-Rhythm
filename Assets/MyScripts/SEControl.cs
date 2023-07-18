using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEControl : MonoBehaviour
{
    public AudioSource SE;
    public AudioClip ClipSuccess;
    void Start()
    {
        
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log("Space pressed. Test function triggered.");
        //     PlaySuccessClip();            
        // }
    }

    public void PlaySuccessClip(){
        SE.PlayOneShot(ClipSuccess);
    }
}
