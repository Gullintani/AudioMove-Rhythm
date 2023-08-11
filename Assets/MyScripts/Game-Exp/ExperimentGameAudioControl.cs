using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class ExperimentGameAudioControl : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Music;
    public Vector3 BodyPositionOffSet = new Vector3(-1.33f, 0.0f, 0.0f);
    public ExperimentGameMainControl GameMainControl;
    private int CurrentPositionIndex=0;
    private Material TargetMaterial = null;
    public bool isMoving = false;
    private float MovingSpeed = 10f;
    private Vector3 MovingDestination;
    private Vector3 PositionOffect;
    public List<Vector3> PositionList;
    private int NumberOfTargets;
    void Start() {
        // Debug PlayerPrefs Settings
        PlayerPrefs.SetString("ExperimentLimb", "LeftUpperArm");
        PlayerPrefs.SetInt("ExperimentTask", 1);
        PlayerPrefs.SetInt("ExperimentTrail", 1);
        PlayerPrefs.SetInt("ExperimentMotion", 0);
        PlayerPrefs.SetInt("ExperimentAdaptiveShrink", 0);

        // Load data from PlayerPref
        GetExperimentPosition();

        // Initialize sphere material, color and size
        TargetMaterial = GetComponent<Renderer>().material;
        ColorHit(false);
        
        // Initialize MovingDestination
        MovingDestination = PositionList[0];
    }

    private void Update(){
        // Testing function.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed. AudioController.cs test triggered.");
            MoveToNextPosition();
            // MoveToNextRandomPosition();
        }

        // Smooth moving
        if(transform.localPosition != MovingDestination){
            isMoving = true;
            MoveSmooth();
        }else{
            isMoving = false;
        }
    }

	public void ColorHit(bool isHit) {
        TargetMaterial.color = isHit ? Color.green : Color.yellow;
    }
    
    private void MoveSmooth(){
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, MovingDestination, MovingSpeed * Time.deltaTime);
    }

    // Move to the next position in PositionList
    public void MoveToNextPosition(){
        if(CurrentPositionIndex >= PositionList.Count){
            CurrentPositionIndex = 0;
        }
        MovingDestination = PositionList[CurrentPositionIndex];
        CurrentPositionIndex += 1;
    }

    // Move to random next position in PositionList
    public void MoveToNextRandomPosition(){
        int randomIndex = Random.Range(0, PositionList.Count);
        while (randomIndex == CurrentPositionIndex)
        {
            randomIndex = Random.Range(0, PositionList.Count);
        }
        CurrentPositionIndex = randomIndex;
        MovingDestination = PositionList[CurrentPositionIndex];
    }

    private void GetExperimentPosition(){
        if (PlayerPrefs.GetInt("ExperimentTask") == 0){
            // Task 1
            PositionList.Add(SphericalToCartesian(new Vector3(5, 20f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 50f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 80f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 110f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 140f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 170f, 0f)));            
        }else if(PlayerPrefs.GetInt("ExperimentTask") == 1){
            // Task 2
            PositionList.Add(SphericalToCartesian(new Vector3(5, 60f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 120f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 60f, 30f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 120f, 30f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 60f, -30f)));
            PositionList.Add(SphericalToCartesian(new Vector3(5, 120f, -30f)));
        }else if(PlayerPrefs.GetInt("ExperimentTask") == 2){
            // Task 3
            if(PlayerPrefs.GetInt("ExperimentTrail") == 0 ){
                // Task 3, Trail 1
                PositionList.Add(SphericalToCartesian(new Vector3(5, 45f, -30f)));
                PositionList.Add(SphericalToCartesian(new Vector3(5, 120f, 0f)));
                PositionList.Add(SphericalToCartesian(new Vector3(5, 60f, 30f)));
            }else if(PlayerPrefs.GetInt("ExperimentTrail") == 1){
                // Task 3, Trail 2
                PositionList.Add(SphericalToCartesian(new Vector3(5, 45f, -30f)));
                PositionList.Add(SphericalToCartesian(new Vector3(5, 120f, 0f)));
                PositionList.Add(SphericalToCartesian(new Vector3(5, 60f, 30f)));
            }
        }
        return;
    }

    public void AdaptiveShrink(){
        
    }

    private Vector3 SphericalToCartesian(Vector3 SphericalVector){
        float radius = SphericalVector.x;
        float theta = SphericalVector.y;
        float phi = SphericalVector.z;

        float a = radius * Mathf.Cos(phi * Mathf.Deg2Rad);
        float x = a * Mathf.Cos(theta * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(phi * Mathf.Deg2Rad);
        float z = a * Mathf.Sin(theta * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }
}
