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
    private Button UIButtonBack;
    public bool HaveStarted = false;


    private void OnEnable() {
        // Initialization
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButton = UIRoot.Q<Button>("ButtonCalibration");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");
        UISliderSize = UIRoot.Q<Slider>("SliderSize");
        UISliderHeight = UIRoot.Q<Slider>("SliderHeight");
    }
    void Start()
    {   
        
        
        // Event Register
        UISliderSize.RegisterValueChangedCallback(OnSliderSizeValueChanged);
        UISliderHeight.RegisterValueChangedCallback(OnSliderHeightValueChanged);

        // Calibration Button Event
        UIButton.clicked += delegate(){
            // Check if the programe have started
            if(HaveStarted){
                // If started, calibration
                MainController.WorldCalibration();
                Verbal.PlayOneShot(Clip1_calibration);
                SwitchToCameraBack();
            }else{
                // If not started,
                ActivateGameScene();
            }
        };
        // Back Button Event
        UIButtonBack.clicked += delegate(){
            ActivateSettingScene();
            SwitchToCameraBack();
        };
    }

    // Update is called once per frame
    void Update(){
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log("Space pressed. Test function triggered.");
        // }
    }

    public void ActivateSettingScene(){
        // 1. generate preview shperes
        MainController.GeneratePositions();
        // 2. hide audio move sphere
        AudioController.transform.localPosition = new Vector3(0f, 10f, 5f);
        AudioController.transform.localScale = new Vector3(0f, 0f, 0f);
        // 3. display ui
        UISliderSize.style.display = DisplayStyle.Flex;
        UISliderHeight.style.display = DisplayStyle.Flex;
        UIButtonBack.style.display = DisplayStyle.None;
        HaveStarted = false;

    }

    public void ActivateGameScene(){
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
        HaveStarted = true;
        Verbal.PlayOneShot(Clip2_start);
        SwitchToCameraBack();
        // 4. hide setting ui
        UISliderSize.style.display = DisplayStyle.None;
        UISliderHeight.style.display = DisplayStyle.None;
        UIButtonBack.style.display = DisplayStyle.Flex;
        // 5. restore size
        AudioController.transform.localPosition = new Vector3(0f, 0f, 5f);
        AudioController.transform.localScale = new Vector3(TargetSize, TargetSize, TargetSize);

        HaveStarted = true;

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
        float Phi = Mathf.Lerp(-2, 2, UISliderHeight.value / 100f);
        // for (int index = 0; index < MainController.PositionList.Count; index++){
        //     // Update position list
        //     Vector3 TempCartesian = MainController.PositionList[index];
        //     Vector3 TempSpherical = CartesianToSpherical(new Vector3(TempCartesian.x, TempCartesian.y, TempCartesian.z));
        //     TempSpherical = new Vector3(TempSpherical.x, TempSpherical.y, Phi);
        //     MainController.PositionList[index] = SphericalToCartesian(TempSpherical);
        // }
        for (int index = 0; index < MainController.PreviewSphereList.Count; index++){
            Vector3 TempVector = MainController.PreviewSphereList[index].transform.position;
            MainController.PreviewSphereList[index].transform.position = new Vector3(TempVector.x, Phi, TempVector.z);
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
