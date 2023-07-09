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
    void Start() {
        AudioClip musicClip = GetComponent<AudioSource>().clip;

        UniBpmAnalyzer bpmAnalyzer = new UniBpmAnalyzer();
        int bpm = UniBpmAnalyzer.AnalyzeBpm(musicClip);
        if (bpm < 0)
        {
            Debug.LogError("AudioClip is null.");
            return;
        } else{
            Debug.Log("BPM is " + bpm);
        }
    }

    private void Update(){

    }

	//This event will be called every frame while music is playing

    public Vector3 SphericalToCartesian(float radius, float polar, float elevation){
        float a = radius * Mathf.Cos(elevation);
        float x = a * Mathf.Cos(polar);
        float y = radius * Mathf.Sin(elevation);
        float z = a * Mathf.Sin(polar);
        return new Vector3(x, y, z);
    }

    public void TeleportRandomly() {
        Vector3 direction = Random.onUnitSphere;
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1.0f);
        float distance = 2.0f * Random.value + 1.5f;
        transform.localPosition = distance * direction;
    }
}
