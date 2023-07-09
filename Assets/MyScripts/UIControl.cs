using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update
    public MainController MainController;
    public AudioSource Verbal;
    void Start()
    {
        Verbal = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button buttonCalibration = root.Q<Button>("ButtonCalibration");

        buttonCalibration.clicked += () => MainController.WorldCalibration();
        buttonCalibration.clicked += () => Verbal.Play();        
    }
}
