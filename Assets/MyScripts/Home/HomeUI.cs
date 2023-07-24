using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HomeUI : MonoBehaviour
{
    public AudioSource Verbal;
    public UIDocument UI;
    public HomeCamera HomeCamera;
    public AudioClip Clip1_;
    public AudioClip Clip2_;
    private VisualElement UIRoot;
    private Button UIButtonRelax;
    private Button UIButtonExercise;
    private Button UIButtonRehablitation;
    private Button UIButtonAchievements;
    private Button UIButtonBack;
    public string CurrentModeSelection = "Home";
    void Start()
    {
        // Variables claim
        Verbal = GetComponent<AudioSource>();
        UIRoot = UI.rootVisualElement;
        UIButtonRelax = UIRoot.Q<Button>("ButtonRelax");
        UIButtonExercise = UIRoot.Q<Button>("ButtonExercise");
        UIButtonRehablitation = UIRoot.Q<Button>("ButtonRehablitation");
        UIButtonAchievements = UIRoot.Q<Button>("ButtonAchievements");
        UIButtonBack = UIRoot.Q<Button>("ButtonBack");

        // Initial visual state
        DisplayHomeView();

        // Button event registration
        UIButtonRelax.clicked += delegate(){
            CurrentModeSelectionChange("Relax");
            SceneManager.LoadScene("AudioMoveSettings");
        };
        UIButtonExercise.clicked += delegate(){
            CurrentModeSelectionChange("Exercise");
            SceneManager.LoadScene("AudioMoveSettings");
        };
        UIButtonRehablitation.clicked += delegate(){
            CurrentModeSelectionChange("Rehablitation");
            SceneManager.LoadScene("AudioMoveSettings");
        };
        UIButtonAchievements.clicked += delegate(){
            CurrentModeSelectionChange("Achievements");
            DisplayAchievementsView();
        };
        UIButtonBack.clicked += delegate(){
            CurrentModeSelectionChange("Home");
            DisplayHomeView();
        };
    }

    void Update()
    {
        
    }

    private void DisplayAchievementsView(){
        UIButtonRelax.style.display = DisplayStyle.None;
        UIButtonExercise.style.display = DisplayStyle.None;
        UIButtonRehablitation.style.display = DisplayStyle.None;
        UIButtonAchievements.style.display = DisplayStyle.None;
        UIButtonBack.style.display = DisplayStyle.Flex;
    }
    private void DisplayHomeView(){
        UIButtonRelax.style.display = DisplayStyle.Flex;
        UIButtonExercise.style.display = DisplayStyle.Flex;
        UIButtonRehablitation.style.display = DisplayStyle.Flex;
        UIButtonAchievements.style.display = DisplayStyle.Flex;
        UIButtonBack.style.display = DisplayStyle.None;

    }
    private void CurrentModeSelectionChange(string Mode){
        CurrentModeSelection = Mode;
        PlayerPrefs.SetString("CurrentModeSelection", Mode);
    }
}
