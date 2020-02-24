using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Controls the players turning, movement, shooting cooldowns etc

    public static PlayerController instance;

    int health;
    int maxHealth = 100;

    Vector2 moveDirection = new Vector2(0.0f, 1.0f);

    float speed = 4.0f;

    public static Quaternion wantedRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    public static float movementSpeedY;
    public static float movementSpeedX;

    public Transform planeGraphics;
    public Animation explosion;
    SpriteRenderer planeSr;

    float planeTurningScaleVisual = 1.0f;

    public static bool turning;

    float firingCooldown;

    public Transform barrels;
    int currentBarrelIndex;

    public Transform clouds;
    Vector2 posInTens = new Vector2(0, 0);

    private void Awake()
    {
        instance = this;
    }

    public void OnStartGame()
    {
        planeGraphics = CollectionManager.instance.planeWorldScroller.GetChild(CollectionManager.selectedPlane).GetChild(0);
        planeSr = CollectionManager.instance.planeWorldScroller.GetChild(CollectionManager.selectedPlane).GetChild(0).GetComponent<SpriteRenderer>();

        planeSr.enabled = true;

        speed = 4.0f + (0.25f * CollectionManager.selectedPlane);
    }

    public void OnReturnToMainMenu()
    {
        wantedRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        planeSr.enabled = true;
    }

    public void OnPlayerDeath()
    {
        explosion.Play();
        planeSr.enabled = false;
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, wantedRotation, 180.0f * Time.deltaTime);

        var angle = Quaternion.Angle(transform.rotation, wantedRotation);

        float turningScale = 1.0f - (angle / 270f);

        planeTurningScaleVisual = Mathf.Lerp(planeTurningScaleVisual, turningScale, (Mathf.Sin(3f) / 2.0f));

        planeGraphics.localScale = new Vector2(planeTurningScaleVisual, 1.0f);

        AudioManager.instance.turningSource.pitch = 1.0f + (1.0f - planeTurningScaleVisual);

        if (!turning && GameManager.gameOn)
        {
            // Not turning, shoot

            firingCooldown -= 1.0f * Time.deltaTime;
            
            if (firingCooldown < 0.0f)
            {
                firingCooldown = 0.25f - (0.05f * CollectionManager.selectedPlane);

                AmmoPool.instance.Shoot(barrels.GetChild(0).position, transform.rotation, "Player");
                AmmoPool.instance.Shoot(barrels.GetChild(1).position, transform.rotation, "Player");

                currentBarrelIndex += 1;

                if (currentBarrelIndex == barrels.childCount)
                {
                    currentBarrelIndex = 0;
                }

                AudioManager.instance.PlayRequestedClip(AudioManager.instance.shoot, 0.5f);
            }
        }

        Vector2 oldPosInTens = posInTens;

        posInTens = new Vector2(Mathf.RoundToInt(transform.position.x/10.0f), Mathf.RoundToInt(transform.position.y/10.0f));

        if (oldPosInTens != posInTens)
        {
            clouds.transform.position = new Vector2(posInTens.x * 10.0f, posInTens.y * 10.0f);
        }
    }
}
