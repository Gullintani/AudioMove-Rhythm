using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Renderer))]
public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Music;
    public Vector3 BodyPositionOffSet = new Vector3(-1.33f, 0.0f, 0.0f);
    public MainController MainController;
    private int CurrentPositionIndex=0;
    private Material TargetMaterial = null;
    public bool isMoving = false;

    void Start() {
        // Initialize sphere material and color
        TargetMaterial = GetComponent<Renderer>().material;
        ColorHit(false);
        
    }

    private void Update(){
        // Testing function.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed. Test function triggered.");
            MoveToNextPosition();
            // MoveToNextRandomPosition();
        }
    }

	public void ColorHit(bool isHit) {
        TargetMaterial.color = isHit ? Color.green : Color.cyan;
    }
    
    
    // Move to the next position in PositionList
    public void MoveToNextPosition(){
        if(CurrentPositionIndex >= MainController.PositionList.Count){
            CurrentPositionIndex = 0;
        }
        this.transform.localPosition = MainController.PositionList[CurrentPositionIndex];
        CurrentPositionIndex += 1;
    }

    // Move to random next position in PositionList
    public void MoveToNextRandomPosition(){
        int randomIndex = Random.Range(0, MainController.PositionList.Count);
        while (randomIndex == CurrentPositionIndex)
        {
            randomIndex = Random.Range(0, MainController.PositionList.Count);
        }
        CurrentPositionIndex = randomIndex;
        this.transform.localPosition = MainController.PositionList[CurrentPositionIndex];
    }

    public void MoveRandomly() {
        Vector3 direction = Random.onUnitSphere;
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1.0f);
        float distance = 2.0f * Random.value + 1.5f;
        transform.localPosition = distance * direction;
    }
}
