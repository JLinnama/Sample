using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListSnapper : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

    // Handles the menus aircraft list gestures and snaps the list to fixed position when the finger is lifted

    public ScrollRect scrollRect;
    public HorizontalLayoutGroup group;

    float wantedX;
    float currentX;
    float swipeDuration;

    public int currentSelectedIndex = 0;

    bool swiping;

    float startHorizontal;

    public Animation buttonRoll;

    private void Update()
    {
        swipeDuration += 1.0f * Time.deltaTime;

        wantedX = (float)currentSelectedIndex / (float)((float)group.transform.childCount - 1.0f);

        currentX = Mathf.Lerp(currentX, (float)wantedX, (Mathf.Sin(3f) / 2.0f));

        if (swiping == false)
        {
            scrollRect.horizontalNormalizedPosition = currentX;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        swipeDuration = 0.0f;

        swiping = true;

        startHorizontal = scrollRect.horizontalNormalizedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        swiping = false;

        float endHori = scrollRect.horizontalNormalizedPosition;

        currentX = scrollRect.horizontalNormalizedPosition;
        wantedX = currentX;

        float durationFixer = swipeDuration / 10.0f;

        if (durationFixer > 0.06f)
        {
            durationFixer = 0.06f;
        }

        float treshold = 0.01f + durationFixer;

        if (Mathf.Abs(startHorizontal - endHori) > treshold)
        {
            if (endHori > startHorizontal)
            {
                ChangePlane(1);
            }
            else
            {
                ChangePlane(-1);
            }
        }
    }

    public void ChangePlane(int direction)
    {
        int oldIndex = currentSelectedIndex;

        currentSelectedIndex += direction;

        if (currentSelectedIndex < 0)
        {
            currentSelectedIndex = 0;
        }
        else if (currentSelectedIndex > group.transform.childCount - 1)
        {
            currentSelectedIndex = group.transform.childCount - 1;
        }

        if (currentSelectedIndex != oldIndex)
        {
            CollectionManager.selectedPlane = currentSelectedIndex;

            buttonRoll.Stop();
            buttonRoll.Play();

            CollectionManager.instance.UpdatePlayButton();
        }
    }
}
