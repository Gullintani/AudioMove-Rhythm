using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera[] CameraList;
    public float TargetSize = 2f;
    public GameMainController GameMainController;
    public AudioSource Verbal;
    public AudioClip Clip1_ExerciseStart;
    public AudioClip Clip2_Calibration;
    public GameAudioController GameAudioController;
    public UIDocument UI;
    private VisualElement UIRoot;
    private Button UIButtonCalibration;
    private Button UIButtonBack;


    private void OnEnable() {
        
    }
    void Start()
    {   
        // Initialize variables
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButtonCalibration = UIRoot.Q<Button>("ButtonCalibration");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");

        // OnStart displays and verbal prompt
        Verbal.PlayOneShot(Clip1_ExerciseStart);
        Debug.Log(PlayerPrefsUtility.LoadVector3List()[0]);
        Debug.Log(PlayerPrefsUtility.LoadVector3List()[1]);
        Debug.Log(PlayerPrefsUtility.LoadVector3List()[2]);


        // Set Button Event
        UIButtonCalibration.clicked += delegate(){
            Debug.Log("Calibration");
        };
        UIButtonBack.clicked += delegate(){
            Debug.Log("Back to Setting Scene");
        };
    }

    // Update is called once per frame
    void Update(){
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log("Space pressed. Test function triggered.");
        // }
    }

    private Vector3 CartesianToSpherical(Vector3 cartesian){
        Vector3 spherical;
        spherical.x = Mathf.Sqrt(cartesian.x * cartesian.x + cartesian.y * cartesian.y + cartesian.z * cartesian.z);
        spherical.y = Mathf.Acos(cartesian.y / spherical.x);
        spherical.z = Mathf.Atan2(cartesian.z, cartesian.x);
        return spherical;
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
}
