using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class Audioontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public MobileController MobileController;
    public List<Vector3> PositionList;
    private Material material = null;
    public int MaxTrialTime, NumberOfHorizontalPositions, NumberOfVerticalPositions, Radius = 5;
    public int TrialTime, lastPositionIndex = 0, newRandomPositionIndex;
    public float InitialScale, speed, tolerantAngle;
    public bool isMoving = false;
    List<int> randomPositionIndexList;
    public Vector3 BodyPositionOffSet = new Vector3(-1.33f, 0.0f, 0.0f);

    void Start() {
        // Random.InitState(419);

        material = GetComponent<Renderer>().material;
        SetGazedAt(false);
        // Teleport Position Settings
        // PositionList = new List<Vector3>();
        // Generate Position
        // PositionList = GeneratePositions();
        
        // Old Position List
        // Vector3 BodyPositionOffSet = new Vector3(-1.33f, 0.0f, 0.0f);
        Vector3 VerticalOffSet = new Vector3(0.0f, 0.0f, 0.0f);
        PositionList = new List<Vector3>();

        // Try Drawing Curve
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/6, Mathf.PI/-6) + BodyPositionOffSet + VerticalOffSet);
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/6*5, Mathf.PI/6) + BodyPositionOffSet + VerticalOffSet);


        // Basic, Horizontal 4 Direction Positions
        // PositionList.Add(SphericalToCartesian(Radius, 0.0f, 0.0f) + BodyPositionOffSet + VerticalOffSet);
        PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/5, 0.0f) + BodyPositionOffSet + VerticalOffSet);
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/3, 0.0f) + BodyPositionOffSet + VerticalOffSet);
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/3*2, 0.0f) + BodyPositionOffSet + VerticalOffSet);
        PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/5*4, 0.0f) + BodyPositionOffSet + VerticalOffSet);
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI, 0.0f) + BodyPositionOffSet + VerticalOffSet);


        // Additional Higer Positions
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/6, Mathf.PI/7) + BodyPositionOffSet);
        PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/5, Mathf.PI/7) + BodyPositionOffSet);
        PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/5*4, Mathf.PI/7) + BodyPositionOffSet);
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/6*5, Mathf.PI/6) + BodyPositionOffSet);
        
        // Additional Lower Positions
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/2, Mathf.PI/-8) + BodyPositionOffSet);
        PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/5, Mathf.PI/-8) + BodyPositionOffSet);
        PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/5*4, Mathf.PI/-8) + BodyPositionOffSet);
        // PositionList.Add(SphericalToCartesian(Radius, Mathf.PI/6*5, Mathf.PI/-6) + BodyPositionOffSet);

        // Additional Extreme Positions

        // Initialize Shrink
        TrialTime = MaxTrialTime;
        transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        transform.localPosition = new Vector3(0, 0, Radius);

        randomPositionIndexList = Enumerable.Range(0, PositionList.Count).ToList();
    }

    private void Update() {
        // Smooith Transition
        if (isMoving){
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, PositionList[newRandomPositionIndex], speed * Time.deltaTime);
        }
        if (transform.localPosition == PositionList[newRandomPositionIndex]){
            isMoving = false;
        }
    }

    /// Sets the gaze state.
    public void SetGazedAt(bool gazedAt) {
        material.color = gazedAt ? Color.green : Color.red;
    }

    public void OverhitColor(bool isOverhit){
        material.color = isOverhit ? Color.blue : Color.green;
    }
    /// Teleports the cube to a random location.
    // public void TeleportRandomly() {
    //     Vector3 direction = Random.onUnitSphere;
    //     direction.y = Mathf.Clamp(direction.y, 0.5f, 1.0f);
    //     float distance = 2.0f * Random.value + 1.5f;
    //     transform.localPosition = distance * direction;
    // }

    public void TeleportRandomly() {
        if(randomPositionIndexList.Count == 0){
            Debug.Break();
        }
        // Prevent Duplicated Position
        newRandomPositionIndex = randomPositionIndexList[Random.Range(0, randomPositionIndexList.Count)];

        // Teleportation        
        // transform.localPosition = PositionList[newRandomPositionIndex];

        // Smooth Transition
        isMoving = true;

        // Set lastPositionIndex
        lastPositionIndex = newRandomPositionIndex;
        randomPositionIndexList.Remove(lastPositionIndex);
    }

    public void CubeShrink(float minimumAngle, float maxAlphaAngel, float betaAngle){
        float ShrinkScale = Mathf.Abs(Mathf.Sin(minimumAngle*Mathf.PI/180) * Radius);
        if (ShrinkScale > InitialScale || ShrinkScale <= 0){
            ShrinkScale = InitialScale;
        }
        transform.localScale = new Vector3(ShrinkScale, ShrinkScale, ShrinkScale);

        // Debug.Log("ShrinkScale is: " + ShrinkScale + ", TrialTime is: " + TrialTime);

        // Bullseye Hit and TrialTime == 0
        // if (TrialTime == 0 || minimumAngle <= tolerantAngle){
        if (minimumAngle <= tolerantAngle && maxAlphaAngel <= (betaAngle + 10.0f)){
            TrialTime -= 1;
            transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
            SE.Play();
            MobileController.minimumAngle = 90.0f;
            MobileController.maxAlphaAngel = 0.0f;
            if (TrialTime == 0){
                TeleportRandomly();
                TrialTime = MaxTrialTime;
            }
            return;
        }
        if (maxAlphaAngel > (betaAngle + 10.0f)){
            transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
            MobileController.minimumAngle = 90.0f;
            MobileController.maxAlphaAngel = 0.0f;
            TrialTime = MaxTrialTime;
            return;
        }
    }

    public List<Vector3> GeneratePositions(){
        // Debug.Log("Number of Positions (Horizontal * Vertical) is: " + NumberOfHorizontalPositions + " * " + NumberOfVerticalPositions);
        float HorizontalSplit = Mathf.PI/NumberOfHorizontalPositions;
        float VerticalSplit = Mathf.PI/NumberOfVerticalPositions;
        PositionList = new List<Vector3>();
        for(int h = 1; h < NumberOfHorizontalPositions-1; h++){
            for (int v = 1; v < NumberOfVerticalPositions-1; v++){
                Vector3 ShperePosition = SphericalToCartesian(Radius, h*HorizontalSplit, v*VerticalSplit) + new Vector3(0, 0, 3);
                PositionList.Add(ShperePosition);
            }
        }
        // Debug.Log(PositionList.Count + " Positions Generated");
        return PositionList;
    }
    public Vector3 SphericalToCartesian(float radius, float polar, float elevation){
        float a = radius * Mathf.Cos(elevation);
        float x = a * Mathf.Cos(polar);
        float y = radius * Mathf.Sin(elevation);
        float z = a * Mathf.Sin(polar);
        return new Vector3(x, y, z);
    }
}
