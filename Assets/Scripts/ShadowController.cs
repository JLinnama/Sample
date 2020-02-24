using UnityEngine;

public class ShadowController : MonoBehaviour
{

    // Simply mimics the assigned transform

    public Transform transformToMimic;
    float shadowOffsetY = 1.5f;

    private void Update()
    {
        transform.position = new Vector2(transformToMimic.position.x, transformToMimic.position.y - shadowOffsetY);
        transform.rotation = transformToMimic.rotation;
        transform.localScale = transformToMimic.localScale;
    }
}
