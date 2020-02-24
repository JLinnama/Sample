using UnityEngine;

public class MissilePool : MonoBehaviour
{

    // Handles the pooling of the enemy missiles

    public static MissilePool instance;

    public GameObject missilePrefab;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnMissile()
    {
        GameObject missile = null;

        foreach (Transform t in transform)
        {
            if (!t.gameObject.activeSelf)
            {
                if (t.gameObject.GetComponent<Missile>().alive == false)
                {
                    missile = t.gameObject;
                    break;
                }
            }
        }

        if (missile == null)
        {
            missile = Instantiate(missilePrefab, transform);
        }

        missile.GetComponent<Missile>().Spawn();
    }

    public void OnGameEnd()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                t.GetComponent<Missile>().Destroy(true);
            }
        }
    }
}
