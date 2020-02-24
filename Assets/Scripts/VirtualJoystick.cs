using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    // Moves the input values from the joystick to PlayerControllers movement speeds

    public Image backgroundImage;
    public Image joystickImage;

    Vector3 inputVector;

    Vector3 stickTargetPos;

    public GameObject joystickHolder;
    public Vector2 wantedJoystickPosition;

    bool movementStick = true;

    public virtual void OnDrag(PointerEventData data)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, data.position, data.pressEventCamera, out pos))
        {
            PlayerController.turning = true;

            pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2f, pos.y * 2f, 0f);

            if (inputVector.magnitude > 1.0f)
            {
                inputVector = inputVector.normalized;
            }

            stickTargetPos = new Vector3(inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 3), inputVector.y * (backgroundImage.rectTransform.sizeDelta.y / 3));

            Vector3 difference = new Vector3(0, 0,0) - inputVector;

            float rota = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            PlayerController.wantedRotation = Quaternion.Euler(0.0f, 0.0f, rota + 90.0f);
        }
    }

    void Update()
    {
        if (movementStick)
        {
            PlayerController.movementSpeedX = Horizontal();
            PlayerController.movementSpeedY = Vertical();

            joystickImage.rectTransform.anchoredPosition = Vector3.Lerp(joystickImage.rectTransform.anchoredPosition, stickTargetPos, 20f * Time.deltaTime);
        }
    }

    public void SetStickPos(Vector3 input)
    {
        float scaledX = Camera.main.ScreenToViewportPoint(input).x - 0.5f;
        float scaledY = Camera.main.ScreenToViewportPoint(input).y - 0.5f;
        scaledX *= 2;
        scaledY *= 2;

        Vector2 scaledPos = new Vector2(scaledX, scaledY);

        if (scaledPos.x > 0.5f && scaledPos.y < 0.0f)
        {
            gameObject.transform.parent.GetComponent<RectTransform>().position = input;
        }
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        AudioManager.instance.turningSource.Play();
        OnDrag(data);
    }


    public virtual void OnPointerUp(PointerEventData data)
    {
        if (movementStick)
        {
            PlayerController.turning = false;

            inputVector = Vector3.zero;
            stickTargetPos = Vector3.zero;
        }
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        if (inputVector.x != 0)
        {
            return inputVector.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }

    public void ResetStick()
    {
        inputVector = Vector3.zero;
        stickTargetPos = Vector3.zero;
    }
}
