using UnityEngine;

public class Aircraft : MonoBehaviour
{

    // Controls the scale of a single aircraft in the aircraft menu list, creating a cool effect for scrolling the list

    public Transform comparePoint;
    public Transform trails;

    public SpriteRenderer aircraftSr;

    void Update()
    {
        float distanceToMiddle = Vector2.Distance(transform.position, comparePoint.position);

        float scale = 1.0f - (distanceToMiddle / 3.0f);

        if (scale < 0.0f)
        {
            scale = 0.0f;
        }

        transform.localScale = new Vector2(scale, scale);
    }
}
