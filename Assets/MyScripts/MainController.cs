using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{   
    public MobileController Phone;
    public AudioController AudioController;
    public List<Vector3> PositionList;
    public bool HaveStarted = false;
    
    private void OnEnable() {
        Debug.Log("===================== OnEnable of MainController =======================");
    }
    void Start()
    {
        Debug.Log("===================== Start of MainController =======================");
        
        
        // Generate Basic PositionList
        GeneratePositions();

        // Customize Interface

    }

    void Update()
    {
        if (Time.frameCount == 120) {
            WorldCalibration();
        }

    }

    private void GeneratePositions(){
        // Get music clip
        AudioClip musicClip = AudioController.GetComponent<AudioSource>().clip;
        UniBpmAnalyzer bpmAnalyzer = new UniBpmAnalyzer();
        int BPM = UniBpmAnalyzer.AnalyzeBpm(musicClip);
        if (BPM < 0)
        {
            Debug.LogError("AudioClip is null.");
            return;
        } else{
            Debug.Log("BPM is " + BPM);
        }
        
        // For different BPM
        if(BPM <= 80){
            Debug.Log("BPM is smaller than 80");
        }else if(BPM <= 100){
            Debug.Log("BPM is smaller than 100");
        }else if(BPM <= 120){
            Debug.Log("BPM is smaller than 120");
        }else if(BPM <= 180){
            Debug.Log("BPM is smaller than 180");
        }else if(BPM > 180){
            Debug.Log("BPM is larger than 180");
        }
    }

    public Vector3 SphericalToCartesian(float radius, float polar, float elevation){
        float a = radius * Mathf.Cos(elevation);
        float x = a * Mathf.Cos(polar);
        float y = radius * Mathf.Sin(elevation);
        float z = a * Mathf.Sin(polar);
        return new Vector3(x, y, z);
    }
    public void WorldCalibration(){
        transform.rotation = Phone.transform.rotation;
        transform.rotation *= Quaternion.Euler(0,0,180f);
        Phone.SetInitialDownDirection();
    }
}
