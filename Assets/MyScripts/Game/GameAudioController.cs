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
    private int CurrentPositionIndex=0;
    private Material TargetMaterial = null;
    public bool isMoving = false;
    public float MovingSpeed = 10.0f;
    private Vector3 MovingDestination;
    private List<Vector3> PositionList;
    private float TargetSize;
    void Start() {
        // Load data from PlayerPref in setting scene
        PositionList = PlayerPrefsUtility.LoadVector3List();
        TargetSize = PlayerPrefs.GetFloat("TargetSize");

        // Initialize sphere material, color and size
        TargetMaterial = GetComponent<Renderer>().material;
        ColorHit(false);
        this.transform.localScale = new Vector3(TargetSize, TargetSize, TargetSize);
        
        // Initialize MovingDestination
        MovingDestination = new Vector3(0f, 0f, 5f);
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
            MoveSmooth();
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
}
