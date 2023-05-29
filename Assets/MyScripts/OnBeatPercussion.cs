using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OnBeatPercussion : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource Percussion;
    void Start()
    {
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.tapTempo();
		processor.onBeat.AddListener(onOnbeatDetected);
		processor.onSpectrum.AddListener(onSpectrum);

        Percussion = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onOnbeatDetected (){
        // TeleportRandomly();
        Percussion.Play();
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
}
