using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEMovement : MonoBehaviour
{
    private float timeGap = 0.2f;
    // Start is called before the first frame update
    void Start()
    {   
        // StartCoroutine(DirectionTest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DirectionTest(){
        
        transform.localPosition = SphericalToCartesian(3.0f, 0.0f, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);
        
        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 2, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);
        
        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 3, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);
        
        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 4, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 5, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 6, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 7, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 8, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 9, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 10, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 11, 0.0f);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(timeGap);

        // transform.localPosition = SphericalToCartesian(3.0f, Mathf.PI/6 * 12, 0.0f);
        // this.GetComponent<AudioSource>().Play();
        // yield return new WaitForSeconds(timeGap);

        transform.localPosition = new Vector3(0, 3, 0);
    }

    public Vector3 SphericalToCartesian(float radius, float polar, float elevation){
        float a = radius * Mathf.Cos(elevation);
        float x = a * Mathf.Cos(polar);
        float y = radius * Mathf.Sin(elevation);
        float z = a * Mathf.Sin(polar);
        return new Vector3(x, y, z);
    }
}
