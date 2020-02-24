using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{

    // Controls an enemy missile, follows player and checks triggers with player ammo and player aircraft

    Quaternion wantedRotation;
    Vector2 moveDirection = new Vector2(0.0f, 1.0f);

    float speed;

    public TrailRenderer trail;
    public SpriteRenderer missileSr;

    public bool alive;
    public CircleCollider2D collider;

    public Animation explosion;

    private void Update()
    {
        if (alive)
        {
            speed += 0.05f * Time.deltaTime;

            Vector3 differenceStartEnd = transform.position - GameManager.instance.player.transform.position;

            float rotationArrow = Mathf.Atan2(differenceStartEnd.y, differenceStartEnd.x) * Mathf.Rad2Deg;

            wantedRotation = Quaternion.Euler(0.0f, 0.0f, rotationArrow + 90.0f);

            transform.Translate(moveDirection * speed * Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, wantedRotation, 75.0f * Time.deltaTime);
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        explosion.transform.GetChild(0).localScale = new Vector2(0.0f, 0.0f);
        trail.Clear();
        missileSr.enabled = true;
        alive = true;
        collider.enabled = true;

        speed = 5.0f;
        Vector2 spawnPos = new Vector2(0, 0);

        if (Random.Range(0, 2) == 0)
        {
            spawnPos = new Vector2(10.0f, Random.Range(-10.0f, 10.0f));

            if (Random.Range(0, 2) == 0)
            {
                spawnPos = new Vector2(-10.0f, spawnPos.y);
            }
        }
        else
        {
            spawnPos = new Vector2(Random.Range(-10.0f, 10.0f), 10.0f);

            if (Random.Range(0, 2) == 0)
            {
                spawnPos = new Vector2(spawnPos.x, -10.0f);
            }
        }

        transform.position = (Vector2)GameManager.instance.player.transform.position + spawnPos;
        trail.Clear();

        Vector3 difference = transform.position - GameManager.instance.player.transform.position;

        float rota = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        wantedRotation = Quaternion.Euler(0.0f, 0.0f, rota + 90.0f);
        transform.rotation = wantedRotation;
    }

    public void Destroy(bool forced)
    {
        explosion.Play();

        alive = false;
        missileSr.enabled = false;
        collider.enabled = false;

        StartCoroutine(DisableWithDelay());

        if (forced == false)
        {
            GameManager.score += 1;
            AudioManager.instance.PlayRequestedClip(AudioManager.instance.score, 0.5f);
        }

        GameManager.instance.UpdateScore();

        AudioManager.instance.PlayRequestedClip(AudioManager.instance.explosion, 1.0f);
    }

    IEnumerator DisableWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        trail.Clear();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;

        if (tag == "Ammo")
        {
            GameManager.missilesDestroyed += 1;

            if (GameManager.missilesDestroyed == GameManager.incomingMissiles)
            {
                GameManager.instance.FinishRound();
            }

            Destroy(false);
        }

        if (tag == "Player")
        {
            PlayerController.instance.OnPlayerDeath();
            StartCoroutine(GameManager.instance.EndGame());
        }
    }
}
