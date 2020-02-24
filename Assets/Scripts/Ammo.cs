using UnityEngine;

public class Ammo : MonoBehaviour
{

    // Controls ammo shot by the player

    float speed;
    float speedMultiplier = 1.0f;

    public SpriteRenderer ammoSr;

    float travelledDistance;
    float travelCap = 10.0f;

    bool alive;

    public void SpawnAmmo(Vector2 pos, Quaternion direction, string type)
    {
        speed = 10.0f;
        speedMultiplier = 1.0f;

        transform.position = pos;
        transform.rotation = direction;

        travelledDistance = 0.0f;

        alive = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (alive) {
            speed += (5.0f * speedMultiplier) * Time.deltaTime;
            speedMultiplier += 1.0f * Time.deltaTime;

            transform.Translate(Vector2.up * speed * Time.deltaTime);
            travelledDistance += speed * Time.deltaTime;

            if (travelledDistance > travelCap)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
