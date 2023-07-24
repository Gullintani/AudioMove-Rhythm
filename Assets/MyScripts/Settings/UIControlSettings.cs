using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIControlBodyPosition : MonoBehaviour
{
    public AudioSource Verbal;
    public UIDocument UI;
    public CameraRotation CameraRotation;
    public TargetPositionManager TargetPositionManager;
    public AudioClip Clip1_SelectPositionPrompt;
    public AudioClip Clip2_TargetSettingsPrompt;
    public AudioClip Clip3_WearingPrompt;
    public float TargetSize = 2f;
    private VisualElement UIRoot;
    private Button UIButtonConfirm;
    private Button UIButtonStart;
    private Button UIButtonBack;
    private Button UIButtonExerciseStart;
    private Slider UISliderSize;
    private Slider UISliderHeight;
    public string CurrentView = "BodyPosition";

    void Start()
    {
        // Variables claim
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButtonConfirm = UIRoot.Q<Button>("ButtonConfirm");
        UIButtonStart = UIRoot.Q<Button>("ButtonStart");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");
        UIButtonExerciseStart = UIRoot.Q<Button>("ButtonExerciseStart");
        UISliderSize = UIRoot.Q<Slider>("SliderSize");
        UISliderHeight = UIRoot.Q<Slider>("SliderHeight");
        UISliderSize.RegisterValueChangedCallback(OnSliderSizeValueChanged);
        UISliderHeight.RegisterValueChangedCallback(OnSliderHeightValueChanged);

        // Visual and audio state
        DisplayBodyPositionView();

        // Button event registration
        UIButtonConfirm.clicked += delegate(){
            DisplayTargetSettingView();
        };
        UIButtonStart.clicked += delegate(){
            DisplayWearingView();
        };
        UIButtonBack.clicked += delegate(){
            DisplayBodyPositionView();
        };
        UIButtonExerciseStart.clicked += delegate(){
            // Critical here, switch scene
            // Pass data through scenes test
            PlayerPrefsUtility.SaveVector3List(TargetPositionManager.PositionList);
            SceneManager.LoadScene("AudioMoveSystem");
        };
    }

    void Update()
    {   
        // If have selected, display button
        if(CameraRotation.CurrentSelection != null && CurrentView == "BodyPosition"){
            UIButtonConfirm.style.display = DisplayStyle.Flex;
        }
    }

    private void DisplayTargetSettingView(){
        CurrentView = "TargetSeeting";
        Verbal.PlayOneShot(Clip2_TargetSettingsPrompt);
        UIButtonConfirm.style.display = DisplayStyle.None;
        UIButtonStart.style.display = DisplayStyle.Flex;
        UIButtonBack.style.display = DisplayStyle.Flex;
        UISliderSize.style.display = DisplayStyle.Flex;
        UISliderHeight.style.display = DisplayStyle.Flex;
        CameraRotation.MovingDestinationPosition = CameraRotation.TargetSettingCameraBasePosition;
        CameraRotation.isMovingCamera = true;
        TargetPositionManager.GeneratePositions();
    }
    private void DisplayBodyPositionView(){
        CurrentView = "BodyPosition";
        Verbal.PlayOneShot(Clip1_SelectPositionPrompt);
        UIButtonConfirm.style.display = DisplayStyle.None;
        UIButtonStart.style.display = DisplayStyle.None;
        UIButtonBack.style.display = DisplayStyle.None;
        UIButtonExerciseStart.style.display = DisplayStyle.None;
        UISliderSize.style.display = DisplayStyle.None;
        UISliderHeight.style.display = DisplayStyle.None;
        CameraRotation.MovingDestinationPosition = CameraRotation.BodyPositionCameraBasePosition;
        CameraRotation.isMovingCamera = true;
        // Destroy previews and clear storage
        foreach (GameObject PreviewSphere in TargetPositionManager.PreviewSphereList)
        {
            Destroy(PreviewSphere);
            TargetPositionManager.PositionList = new List<Vector3>();
            TargetPositionManager.PreviewSphereList = new List<GameObject>();
        }
    }

    private void DisplayWearingView(){
        CurrentView = "Wearing";
        Verbal.PlayOneShot(Clip3_WearingPrompt);
        UIButtonStart.style.display = DisplayStyle.None;
        UIButtonBack.style.display = DisplayStyle.None;
        UISliderSize.style.display = DisplayStyle.None;
        UISliderHeight.style.display = DisplayStyle.None;
        UIButtonExerciseStart.style.display = DisplayStyle.Flex;
        CameraRotation.MovingDestinationPosition = CameraRotation.WearingCameraBasePosition;
        CameraRotation.isMovingCamera = true;
        CameraRotation.isWearingView = true;
    }

    private void OnSliderSizeValueChanged(ChangeEvent<float> evt){   
        // Change size data
        TargetSize = Mathf.Lerp(2, 8, UISliderSize.value / 100f);
        foreach (GameObject PreviewSphere in TargetPositionManager.PreviewSphereList){
            PreviewSphere.transform.localScale = new Vector3(TargetSize, TargetSize, TargetSize);
        }
    }

    private void OnSliderHeightValueChanged(ChangeEvent<float> evt){
        // Change phi data
        float Phi = Mathf.Lerp(TargetPositionManager.SettingViewOffset.y-2, TargetPositionManager.SettingViewOffset.y+2, UISliderHeight.value / 100f);
        // for (int index = 0; index < MainController.PositionList.Count; index++){
        //     // Update position list
        //     Vector3 TempCartesian = MainController.PositionList[index];
        //     Vector3 TempSpherical = CartesianToSpherical(new Vector3(TempCartesian.x, TempCartesian.y, TempCartesian.z));
        //     TempSpherical = new Vector3(TempSpherical.x, TempSpherical.y, Phi);
        //     MainController.PositionList[index] = SphericalToCartesian(TempSpherical);
        // }
        for (int index = 0; index < TargetPositionManager.PreviewSphereList.Count; index++){
            Vector3 TempVector = TargetPositionManager.PreviewSphereList[index].transform.position;
            TargetPositionManager.PreviewSphereList[index].transform.position = new Vector3(TempVector.x, Phi, TempVector.z);
        }
    }
}
