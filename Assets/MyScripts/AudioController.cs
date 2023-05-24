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
    public float percussionRadius = 2.0f;
    void Start() {

        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.tapTempo();
		processor.onBeat.AddListener(onOnbeatDetected);
		processor.onSpectrum.AddListener(onSpectrum);
    }

    private void Update(){

    }
    void onOnbeatDetected (){
		Debug.Log ("Beat!!!");
        // TeleportRandomly();
	}

	//This event will be called every frame while music is playing
	void onSpectrum (float[] spectrum){
		//The spectrum is logarithmically averaged
		//to 12 bands
		for (int i = 0; i < spectrum.Length; ++i) {
			Vector3 start = new Vector3 (i, 0, 0);
			Vector3 end = new Vector3 (i*5, spectrum [i] * 50, 0);
			Debug.DrawLine (start, end);
		}
	}

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
