// Allways face main camera

using UnityEngine;
 
public class AllwaysFacing : MonoBehaviour
{
void Update()
    {
    	transform.LookAt(transform.position - Camera.main.transform.position, Vector3.up);
    }
}