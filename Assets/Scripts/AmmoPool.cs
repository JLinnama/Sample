using UnityEngine;

public class AmmoPool : MonoBehaviour
{

    // Manages the pooling of players ammo, attached to the pools parent

    public static AmmoPool instance;

    public GameObject ammoPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void Shoot(Vector2 pos, Quaternion direction, string type)
    {
        GameObject ammo = null;

        foreach (Transform t in transform)
        {
            if (!t.gameObject.activeSelf)
            {
                ammo = t.gameObject;
                break;
            }
        }

        if (ammo == null)
        {
            ammo = Instantiate(ammoPrefab, transform);
        }

        ammo.GetComponent<Ammo>().SpawnAmmo(pos, direction, type);
    }
}
