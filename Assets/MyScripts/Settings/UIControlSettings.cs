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
    public float TargetSize = 3f;
    private VisualElement UIRoot;
    private Button UIButtonNext;
    private Button UIButtonBack;
    private Button UIButtonAdd;
    private Button UIButtonRemove;
    private Slider UISliderSize;
    private Label UITextCoordinate;
    public string CurrentView = "BodyPosition";

    void Start()
    {
        // Variables claim
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButtonNext = UIRoot.Q<Button>("ButtonNext");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");
        UIButtonAdd = UIRoot.Q<Button>("ButtonAdd");
        UIButtonRemove = UIRoot.Q<Button>("ButtonRemove");
        UITextCoordinate = UIRoot.Q<Label>("TextCoordinate");

        UISliderSize = UIRoot.Q<Slider>("SliderSize");
        UISliderSize.RegisterValueChangedCallback(OnSliderSizeValueChanged);

        // Visual and audio state
        DisplayMusicSelectingView();

        // Button event registration
        UIButtonNext.clicked += delegate(){
            if(CurrentView == "MusicSelecting"){
                DisplayBodyPositionView();
            }else if(CurrentView == "BodyPosition"){
                DisplayTargetSettingView();
            }else if(CurrentView == "TargetSetting"){
                TargetPositionManager.SaveSettingsToPlay();

                DisplayPreview();
            }else if(CurrentView == "Preview"){
                SceneManager.LoadScene("AudioMoveSystem");
            }
        };
        UIButtonBack.clicked += delegate(){
            if(CurrentView == "MusicSelecting"){
                SceneManager.LoadScene("AudioMoveHome");
            }else if(CurrentView == "BodyPosition"){
                DisplayMusicSelectingView();
            }else if(CurrentView == "TargetSetting"){
                DisplayBodyPositionView();
            }else if(CurrentView == "Preview"){
                DisplayTargetSettingView();
            }
        };
        
        UIButtonAdd.clicked += delegate(){
            TargetPositionManager.GenerateTarget(new Vector3(0f, 0f, 5f));
        };

        UIButtonRemove.clicked += delegate(){
            Debug.Log("UI Remove Button Clicked");
            if(CameraRotation.CurrentTargetSelection.name != "cubeRoomEnv"){
                TargetPositionManager.RemoveTarget(CameraRotation.CurrentTargetSelection);
            }
        };
    }

    void Update()
    {   
        // If have selected, display button
        if(CameraRotation.CurrentSelection != null && CurrentView == "BodyPosition"){
            UIButtonNext.style.display = DisplayStyle.Flex;
        }

        if(CameraRotation.CurrentTargetSelection!=null && CameraRotation.IsPreviewSphere(CameraRotation.CurrentTargetSelection) == true && CurrentView == "TargetSetting"){
            Vector3 CurrentSphericalCoordinate = TargetPositionManager.CartesianToSpherical(CameraRotation.CurrentTargetSelection.transform.position);
            UITextCoordinate.text = "Azimuth: " + CurrentSphericalCoordinate.y.ToString("#0.00") + "; Elevation: " + CurrentSphericalCoordinate.z.ToString("#0.00") ;
        }else{
            UITextCoordinate.text = "";
        }

        // if(CameraRotation.CurrentTargetSelection.name.ToLower().Contains("Preview".ToLower()) == true && CurrentView == "TargetSetting"){
        //     UIButtonAdd.style.display = DisplayStyle.None;
        //     UIButtonRemove.style.display = DisplayStyle.Flex;
        // }else if(CameraRotation.CurrentTargetSelection.name.ToLower().Contains("Preview".ToLower()) == false && CurrentView == "TargetSetting"){
        //     UIButtonAdd.style.display = DisplayStyle.Flex;
        //     UIButtonRemove.style.display = DisplayStyle.None;
        // }
    }

    private void DisplayPreview(){
        CurrentView = "Preview";
        Verbal.PlayOneShot(Clip3_WearingPrompt);
        UIButtonNext.style.display = DisplayStyle.Flex;
        UIButtonBack.style.display = DisplayStyle.Flex;
        UIButtonAdd.style.display = DisplayStyle.None;
        UIButtonRemove.style.display = DisplayStyle.None;
        UISliderSize.style.display = DisplayStyle.None;
        CameraRotation.MovingDestinationPosition = CameraRotation.TargetSettingCameraBasePosition;
        CameraRotation.isMovingCamera = true;
    }

    private void DisplayMusicSelectingView(){
        CameraRotation.isSelectingMusic = true;
        CurrentView = "MusicSelecting";
        UIButtonNext.style.display = DisplayStyle.Flex;
        UIButtonBack.style.display = DisplayStyle.Flex;
        UIButtonAdd.style.display = DisplayStyle.None;
        UIButtonRemove.style.display = DisplayStyle.None;
        UISliderSize.style.display = DisplayStyle.None;
    }

    private void DisplayTargetSettingView(){
        // clear previous elements
        if(TargetPositionManager.PreviewSphereList.Count != 0){
            foreach (GameObject PreviewSphere in TargetPositionManager.PreviewSphereList)
                {
                    Destroy(PreviewSphere);
                    TargetPositionManager.PositionList = new List<Vector3>();
                    TargetPositionManager.PreviewSphereList = new List<GameObject>();
                }
        }

        // settings
        CurrentView = "TargetSetting";
        Verbal.PlayOneShot(Clip2_TargetSettingsPrompt);
        UIButtonNext.style.display = DisplayStyle.Flex;
        UIButtonBack.style.display = DisplayStyle.Flex;
        UIButtonAdd.style.display = DisplayStyle.Flex;
        UIButtonRemove.style.display = DisplayStyle.Flex;
        UISliderSize.style.display = DisplayStyle.Flex;
        CameraRotation.MovingDestinationPosition = CameraRotation.TargetSettingCameraBasePosition;
        CameraRotation.isMovingCamera = true;
        TargetPositionManager.GeneratePositions();
    }
    private void DisplayBodyPositionView(){
        CameraRotation.isSelectingMusic = false;
        CurrentView = "BodyPosition";
        Verbal.PlayOneShot(Clip1_SelectPositionPrompt);
        UIButtonNext.style.display = DisplayStyle.None;
        UIButtonBack.style.display = DisplayStyle.Flex;
        UIButtonAdd.style.display = DisplayStyle.None;
        UIButtonRemove.style.display = DisplayStyle.None;
        UISliderSize.style.display = DisplayStyle.None;
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

    private void OnSliderSizeValueChanged(ChangeEvent<float> evt){   
        // Change size data
        TargetSize = Mathf.Lerp(2, 8, UISliderSize.value / 100f);
        if(CameraRotation.IsPreviewSphere(CameraRotation.CurrentTargetSelection) == true){
            CameraRotation.CurrentTargetSelection.transform.localScale = new Vector3(TargetSize, TargetSize, TargetSize);
        }else{
            foreach (GameObject PreviewSphere in TargetPositionManager.PreviewSphereList){
                PreviewSphere.transform.localScale = new Vector3(TargetSize, TargetSize, TargetSize);
            }
            // Save target size
            PlayerPrefs.SetFloat("TargetSize", TargetSize);
        }
        
    }

}
