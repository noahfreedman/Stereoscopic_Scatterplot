// Allways face main camera

using UnityEngine;


public class AlwaysFacing : MonoBehaviour
{
    public Transform Target;

    void Update()
    {
        if (Target)
        {
            transform.LookAt(Target);
            transform.Rotate(Vector3.up, 180);
        }
        else
        {
            transform.LookAt(Camera.main.transform.position);

        }

    }
}