using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{   
    public MobileController Phone;
    public AudioController AudioController;
    public GameObject PreviewPrefab;
    public List<Vector3> PositionList;
    public bool HaveStarted = false;
    private int BPMLevel;

    
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
            BPMLevel = 2;
            Debug.Log("BPM is smaller than 80");
        }else if(BPM <= 100){
            BPMLevel = 3;
            Debug.Log("BPM is smaller than 100");
        }else if(BPM <= 120){
            BPMLevel = 4;
            Debug.Log("BPM is smaller than 120");
        }else if(BPM <= 180){
            BPMLevel = 5;
            Debug.Log("BPM is smaller than 180");
        }else if(BPM > 180){
            BPMLevel = 6;
            Debug.Log("BPM is larger than 180");
        }

        // Generate positions
        PositionList = GeneratePositionsInFront(startAngle:30f, endAngle:150f, numberOfPosition:BPMLevel, distance:5f);
        for (int index = 0; index < PositionList.Count; index++)
        {
            PositionList[index] = SphericalToCartesian(PositionList[index]);
            Debug.Log("Position (Cartesian)" + index + ": " + PositionList[index]);
        }

        // Generate Preview

    }
    private List<Vector3> GeneratePositionsInFront(float startAngle, float endAngle, int numberOfPosition, float distance)
    {
        float angleStep = (endAngle - startAngle) / (numberOfPosition - 1);

        List<Vector3> positions = new List<Vector3>();

        for (int index = 0; index < numberOfPosition; index++)
        {
            float phi = startAngle + angleStep * index;

            float thetaRad = phi * Mathf.Deg2Rad;
            float phiRad = 0f;
            // float phiRad = elevation * Mathf.Deg2Rad;

            Vector3 sphericalPosition = new Vector3(distance, thetaRad, phiRad);
            positions.Add(sphericalPosition);
        }

        return positions;
    }

    public Vector3 SphericalToCartesian(Vector3 SphericalVector){
        float radius = SphericalVector.x;
        float polar = SphericalVector.y;
        float elevation = SphericalVector.z;

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
