// Rotate camera smoothly around a target
// Set range from target
// rotate left or right
// pause
// stop
// speed

using UnityEngine;

public class DemoOrbitCamera : MonoBehaviour
{
    public Transform target;
    public float RotationSpeed = 5.0f;
    void LateUpdate()
    {
        transform.RotateAround(target.position, Vector3.up, RotationSpeed * Time.deltaTime);

    }
}