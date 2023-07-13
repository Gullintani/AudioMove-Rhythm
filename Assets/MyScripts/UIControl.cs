using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera[] CameraList;
    private int currentCameraIndex = 0;
    public float TargetSize = 2f;
    public MainController MainController;
    public AudioSource Verbal;
    public AudioClip Clip1_calibration;
    public AudioClip Clip2_start;
    public AudioController AudioController;
    public UIDocument UI;
    private VisualElement UIRoot;
    private Slider UISliderSize;
    private Slider UISliderHeight;
    private Button UIButton;

    private void OnEnable() {
        Debug.Log("===================== OnEnable of UIControl =======================");
    }
    void Start()
    {   
        // Initialization
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButton = UIRoot.Q<Button>("ButtonCalibration");
        UISliderSize = UIRoot.Q<Slider>("SliderSize");
        UISliderHeight = UIRoot.Q<Slider>("SliderHeight");
        
        // Event Register
        UISliderSize.RegisterValueChangedCallback(OnSliderSizeValueChanged);
        UISliderHeight.RegisterValueChangedCallback(OnSliderHeightValueChanged);

        // Button Event
        UIButton.clicked += delegate(){
            // Check if the programe have started
            if(MainController.HaveStarted){
                // If started, calibration
                MainController.WorldCalibration();
                Verbal.PlayOneShot(Clip1_calibration);
                SwitchToCameraBack();
            }else{
                // If not started,
                // 1. restart the music
                AudioController.GetComponent<AudioSource>().Play();
                // 2. destroy preview spheres
                foreach (GameObject PreviewSphere in MainController.PreviewSphereList)
                {
                    Destroy(PreviewSphere);
                }
                MainController.PreviewSphereList.Clear();
                // 3. make target visiable
                AudioController.transform.localScale = new Vector3(TargetSize, TargetSize, TargetSize);

                MainController.WorldCalibration();
                MainController.HaveStarted = true;
                Verbal.PlayOneShot(Clip2_start);
                SwitchToCameraBack();
            }
            
        };
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnSliderSizeValueChanged(ChangeEvent<float> evt){   
        // Switch camera
        SwitchToCameraAbove();
        // Change size data
        TargetSize = Mathf.Lerp(2, 8, UISliderSize.value / 100f);
        foreach (GameObject PreviewSphere in MainController.PreviewSphereList){
            PreviewSphere.transform.localScale = new Vector3(TargetSize, TargetSize, TargetSize);
        }
    }

    private void OnSliderHeightValueChanged(ChangeEvent<float> evt){
        // Switch camera
        SwitchToCameraSide();
        // Change phi data
        float Phi = Mathf.Lerp(-45, 45, UISliderHeight.value / 100f);
        for (int index = 0; index < MainController.PositionList.Count; index++){
            // Update position list
            Vector3 TempCartesian = MainController.PositionList[index];
            Vector3 TempSpherical = CartesianToSpherical(new Vector3(TempCartesian.x, TempCartesian.y, TempCartesian.z));
            TempSpherical = new Vector3(TempSpherical.x, TempSpherical.y, Phi);
            MainController.PositionList[index] = SphericalToCartesian(TempSpherical);
            
            // Update preview sphere
            MainController.PreviewSphereList[index].transform.localPosition = MainController.PositionList[index];
        }
        
    }

    private void SwitchToCameraBack(){
            CameraList[currentCameraIndex].enabled = false;
            currentCameraIndex = 0;
            CameraList[currentCameraIndex].enabled = true;
    }
    private void SwitchToCameraAbove(){
            CameraList[currentCameraIndex].enabled = false;
            currentCameraIndex = 1;
            CameraList[currentCameraIndex].enabled = true;
    }
    private void SwitchToCameraSide(){
            CameraList[currentCameraIndex].enabled = false;
            currentCameraIndex = 2;
            CameraList[currentCameraIndex].enabled = true;
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
