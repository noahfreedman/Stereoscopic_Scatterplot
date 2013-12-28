// Allways face main camera

using UnityEngine;

 
public class AlwaysFacing : MonoBehaviour
{
    public Transform Target;
void Update()
    {
        if (Target)
        {
            transform.LookAt(transform.position - Target.transform.position, Vector3.up);
        }
        else
        {
            transform.LookAt(transform.position - Camera.main.transform.position, Vector3.up);
        }
    }
}