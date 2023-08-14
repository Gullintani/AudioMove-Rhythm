using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class GameAudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Music;
    public Vector3 BodyPositionOffSet = new Vector3(-1.33f, 0.0f, 0.0f);
    public GameMainController GameMainController;
    public GameMobileController Phone;
    public GameUIControl UIControl;
    private int CurrentPositionIndex=0;
    private Material TargetMaterial = null;
    public bool isMoving = false;
    private float MovingSpeed = 10f;
    private float ScalingSpeed = 10f;
    private Vector3 MovingDestination;
    private Vector3 TargetSize;
    private Vector3 PositionOffect;
    public List<Vector3> PositionList;
    public List<Vector3> TargetSizeList;
    private int NumberOfTargets, MaxTryTime, TryTime;
    private float Tolerance, InitialScale;
    void Start() {
        // Load data from PlayerPref in setting scene
        NumberOfTargets = PlayerPrefs.GetInt("NumberOfTargets");
        PositionOffect = new Vector3(0f, PlayerPrefs.GetFloat("VerticalOffest"), 0f);
        PositionList = PlayerPrefsUtility.LoadVector3List("Position");
        TargetSizeList = PlayerPrefsUtility.LoadVector3List("TargetSize");
        PositionList = PositionList.GetRange(0, NumberOfTargets);
        TargetSizeList = TargetSizeList.GetRange(0, NumberOfTargets);

        // Initialize sphere material, color and size
        TargetMaterial = GetComponent<Renderer>().material;
        ColorHit(false);
        this.transform.localScale = TargetSizeList[0];
        
        // Initialize MovingDestination
        MovingDestination = PositionList[0];
        TargetSize = TargetSizeList[0];

        // Initialize setting parameters
        Tolerance = 20.0f;
        InitialScale = 8.0f;
        MaxTryTime = 1;
        TryTime = 1;
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
            ChangeSizeSmooth();
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
    private void ChangeSizeSmooth(){
        transform.localScale = Vector3.Lerp(transform.localScale, TargetSize, ScalingSpeed * Time.deltaTime);
    }

    // Move to the next position in PositionList
    public void MoveToNextPosition(){
        if(CurrentPositionIndex >= PositionList.Count){
            CurrentPositionIndex = 0;
        }
        MovingDestination = PositionList[CurrentPositionIndex];
        TargetSize = TargetSizeList[CurrentPositionIndex];
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
        TargetSize = TargetSizeList[CurrentPositionIndex];
    }

    public void AdpativeShrink(float minimumAngle, float maxAlphaAngel, float betaAngle){
        float ShrinkScale = Mathf.Abs(Mathf.Sin(minimumAngle * Mathf.Deg2Rad) * 5f); // 5f is the radius here
        if (ShrinkScale > InitialScale || ShrinkScale <= 0){
            ShrinkScale = InitialScale;
        }else if(ShrinkScale < TargetSize.x){
            ShrinkScale = TargetSize.x;
        }
        // Debug
        // Debug.Log("MinAngle: " + minimumAngle + "; MaxElevation: " + maxAlphaAngel + "; target_InitialDown: " + betaAngle + "; ShrinkScale: " + ShrinkScale + "; TryTime: " + TryTime);
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
            return;
        }else if(maxAlphaAngel > (betaAngle + 10.0f)){
            transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
            Phone.minBetweenAngle = 90.0f;
            Phone.maxElevationAngel = 0.0f;
            UIControl.Verbal.PlayOneShot(UIControl.Clip5_OverHit);
            return;
        }else if(minimumAngle > Tolerance){
            UIControl.Verbal.PlayOneShot(UIControl.Clip4_SuccessHit2);
        }
    }
}
