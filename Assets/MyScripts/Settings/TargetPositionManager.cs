using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPositionManager : MonoBehaviour
{
    public GameObject PreviewPrefab;
    public Vector3 SettingViewOffset;
    public List<Vector3> PositionList;
    public List<GameObject> PreviewSphereList;
    private int BPMLevel;
    public Material OriginalMaterial;
    void Start()
    {
        // Because generated preview are based on (0,0,0), I need offset for TargetSettingView
        SettingViewOffset = new Vector3(0, 0, 0);
        Vector3 initialCartesian = new Vector3(Mathf.Sqrt(3)/4, Mathf.Sqrt(3)/2, 0.25f);
        Debug.Log("initial cartesian: " + initialCartesian);
        Vector3 transformedSpherical = CartesianToSpherical(initialCartesian);
        Debug.Log("transformed spherical: " + transformedSpherical);
        Vector3 BackCartesian = SphericalToCartesian(transformedSpherical);
        Debug.Log("Return back to cartesian: " + BackCartesian);

    }

    void Update()
    {
        
    }
    public void GenerrateTarget(){

    }

    public void GeneratePositions(){
        // Generate positions
        PositionList = GenerateSphericalPositions(startAngle:30f, endAngle:150f, numberOfPosition:4, distance:5f);
        for (int index = 0; index < PositionList.Count; index++)
        {
            PositionList[index] = SphericalToCartesian(PositionList[index]);
            // Debug.Log("Position (Cartesian)" + index + ": " + PositionList[index]);
            
            // Generate Preview
            GameObject PreviewSphere = Instantiate(PreviewPrefab, PositionList[index] + SettingViewOffset, Quaternion.identity);
            PreviewSphere.name = "PreviewSphere " + index.ToString();
            Renderer renderer = PreviewSphere.GetComponent<Renderer>();
            renderer.material = OriginalMaterial;
            PreviewSphereList.Add(PreviewSphere);
        }
    }
    
    // Constrain the position of target when dragging
    public Vector3 PositionConstrain(GameObject SelectedTarget, float HorizontalInput, float VerticalInput, float StartThetaAngle, float EndThetaAngle, float StartPhiAngle, float EndPhiAngle, float Distance){
        // Get the current spherical position;
        Vector3 CurrrentSpherical = CartesianToSpherical(SelectedTarget.transform.position);
        // float Theta = CurrrentSpherical.y - HorizontalInput * 0.1f;
        // float Phi = CurrrentSpherical.z + VerticalInput * 0.1f;
        float Theta = Mathf.Clamp(CurrrentSpherical.y - HorizontalInput * 0.1f, StartThetaAngle, EndThetaAngle);
        float Phi = Mathf.Clamp(CurrrentSpherical.z + VerticalInput * 0.1f, StartPhiAngle, EndPhiAngle);
        return SphericalToCartesian(new Vector3(Distance, Theta, Phi));
    }

    private List<Vector3> GenerateSphericalPositions(float startAngle, float endAngle, int numberOfPosition, float distance)
    {
        float angleStep = (endAngle - startAngle) / (numberOfPosition - 1);

        List<Vector3> positions = new List<Vector3>();

        for (int index = 0; index < numberOfPosition; index++)
        {
            float theta = startAngle + angleStep * index;

            float phi = 0f;

            Vector3 sphericalPosition = new Vector3(distance, theta, phi);
            positions.Add(sphericalPosition);
        }

        return positions;
    }
    private Vector3 CartesianToSpherical(Vector3 cartesian){
        float radius = Mathf.Sqrt(cartesian.x * cartesian.x + cartesian.y * cartesian.y + cartesian.z * cartesian.z);
        float a = Mathf.Sqrt(cartesian.x * cartesian.x + cartesian.z * cartesian.z);
        float theta = Mathf.Acos(cartesian.x/ a) * Mathf.Rad2Deg;
        float phi = Mathf.Asin(cartesian.y/ radius) * Mathf.Rad2Deg;
        
        // spherical.z = (spherical.z + 360) % 360;
        return new Vector3(radius, theta, phi);
    }
    private Vector3 SphericalToCartesian(Vector3 SphericalVector){
        float radius = SphericalVector.x;
        float theta = SphericalVector.y;
        float phi = SphericalVector.z;

        float a = radius * Mathf.Cos(phi * Mathf.Deg2Rad);
        float x = a * Mathf.Cos(theta * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(phi * Mathf.Deg2Rad);
        float z = a * Mathf.Sin(theta * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }
}
