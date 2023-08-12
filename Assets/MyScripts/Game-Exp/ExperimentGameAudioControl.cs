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
    public ExperimentGameMobileControl Phone;
    public ExperimentGameUIControl UIControl;
    private int CurrentPositionIndex;
    private Material TargetMaterial = null;
    public bool isMoving = false;
    private float MovingSpeed = 10f;
    private Vector3 MovingDestination;
    private Vector3 PositionOffect;
    public List<Vector3> PositionList;
    private int NumberOfTargets, TryTime, MaxTryTime;
    private float InitialScale;
    private float Radius, Tolerance;
    public List<float> ErrorAngles;

    void Start() {
        // Initialize Variables
        MaxTryTime = 3;
        TryTime = MaxTryTime;
        InitialScale = PlayerPrefs.GetInt("ExperimentAdaptiveShrink")==0? 8f : 5f;
        Radius = 5f;
        Tolerance = 20f;
        CurrentPositionIndex = 0;

        // Load data from PlayerPref
        GetExperimentPosition();

        // Initialize sphere material, color and size
        TargetMaterial = GetComponent<Renderer>().material;
        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        ColorHit(false);
        
        // Initialize MovingDestination
        MovingDestination = PositionList[CurrentPositionIndex];

        
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

    public void OverhitColor(bool isOverhit){
        TargetMaterial.color = isOverhit ? Color.blue : Color.yellow;
    }
    
    private void MoveSmooth(){
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, MovingDestination, MovingSpeed * Time.deltaTime);
    }

    // Move to the next position in PositionList
    public void MoveToNextPosition(){
        Debug.Log("AudioMoved.");
        if(CurrentPositionIndex + 1 >= PositionList.Count){
            CurrentPositionIndex = -1;
        }
        CurrentPositionIndex += 1;
        MovingDestination = PositionList[CurrentPositionIndex];
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
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 20f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 50f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 80f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 110f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 140f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 170f, 0f)));            
        }else if(PlayerPrefs.GetInt("ExperimentTask") == 1){
            // Task 2
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 60f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 120f, 0f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 60f, 30f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 120f, 30f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 60f, -30f)));
            PositionList.Add(SphericalToCartesian(new Vector3(Radius, 120f, -30f)));
        }else if(PlayerPrefs.GetInt("ExperimentTask") == 2){
            // Task 3
            if(PlayerPrefs.GetInt("ExperimentTrail") == 0 ){
                // Task 3, Trail 1
                PositionList.Add(SphericalToCartesian(new Vector3(Radius, 45f, -30f)));
                PositionList.Add(SphericalToCartesian(new Vector3(Radius, 120f, 0f)));
                PositionList.Add(SphericalToCartesian(new Vector3(Radius, 60f, 30f)));
            }else if(PlayerPrefs.GetInt("ExperimentTrail") == 1){
                // Task 3, Trail 2
                PositionList.Add(SphericalToCartesian(new Vector3(Radius, 45f, -30f)));
                PositionList.Add(SphericalToCartesian(new Vector3(Radius, 120f, 0f)));
                PositionList.Add(SphericalToCartesian(new Vector3(Radius, 60f, 30f)));
            }
        }
        return;
    }

    public void AdpativeShrink(float minimumAngle, float maxAlphaAngel, float betaAngle){
        UIControl.Verbal.PlayOneShot(UIControl.Clip4_SuccessHit2);
        
        float ShrinkScale = Mathf.Abs(Mathf.Sin(minimumAngle * Mathf.Deg2Rad) * Radius);
        if (ShrinkScale > InitialScale || ShrinkScale <= 0){
            ShrinkScale = InitialScale;
        }
        // Debug
        Debug.Log("MinAngle: " + minimumAngle + "; MaxElevation: " + maxAlphaAngel + "; target_InitialDown: " + betaAngle + "; ShrinkScale: " + ShrinkScale + "; TryTime: " + TryTime);
        transform.localScale = new Vector3(ShrinkScale, ShrinkScale, ShrinkScale);
        
        if (minimumAngle <= Tolerance && maxAlphaAngel <= (betaAngle + 10.0f)){ // 20.0f is the tolerant angle here
            TryTime -= 1;
            transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
            Phone.minBetweenAngle = 90.0f;
            Phone.maxElevationAngel = 0.0f;
            UIControl.Verbal.PlayOneShot(UIControl.Clip3_SuccessHit);
            if (TryTime == 0){
                MoveToNextPosition();
                TryTime = MaxTryTime;
                UIControl.Verbal.PlayOneShot(UIControl.Clip7_AudioMove);
            }
            ErrorAngles.Add(minimumAngle);
            Debug.Log(ErrorAngles);
            return;
        }
        if (maxAlphaAngel > (betaAngle + 10.0f)){
            transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
            Phone.minBetweenAngle = 90.0f;
            Phone.maxElevationAngel = 0.0f;
            // TryTime = MaxTryTime;
            UIControl.Verbal.PlayOneShot(UIControl.Clip5_OverHit);
            return;
        }
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
