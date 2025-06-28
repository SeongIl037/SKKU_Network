using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform Target;
    
    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);
    }
}
