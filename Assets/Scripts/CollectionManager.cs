using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{

    // Handles the "collections" and things related to them, in this case the owned aircrafts

    public static CollectionManager instance;

    public ScrollRect planeScroller;
    public Transform planeWorldScroller;
    public List<int> aircraftScoreRequirements;

    public static int selectedPlane;

    public Image playButton;
    public Sprite playSprite;
    public Sprite lockedSprite;

    public Color unlockedColor;
    public Color lockedColor;

    private void Awake()
    {
        instance = this;
    }

    public void UpdatePlayButton()
    {
        if (DataManager.instance.IsItemUnlocked(DataManager.aircraftSave, selectedPlane))
        {
            playButton.sprite = playSprite;
        }
        else
        {
            playButton.sprite = lockedSprite;
        }
    }

    public void UpdatePlaneSelection()
    {
        foreach (Transform t in planeWorldScroller)
        {
            int index = t.GetSiblingIndex();

            Aircraft aircraft = t.GetComponent<Aircraft>();

            if (DataManager.instance.IsItemUnlocked(DataManager.aircraftSave, index))
            {
                aircraft.aircraftSr.color = unlockedColor;
                aircraft.trails.gameObject.SetActive(true);
            }
            else
            {
                aircraft.aircraftSr.color = lockedColor;
                aircraft.trails.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        planeWorldScroller.transform.localPosition = new Vector2(planeScroller.horizontalNormalizedPosition * -10.0f, 0.0f);
    }
}
