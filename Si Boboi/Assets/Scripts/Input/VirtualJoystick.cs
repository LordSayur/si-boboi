using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler,IPointerDownHandler, IPointerUpHandler
{
	Image backgroundImage;
	Image joystickImage;

	Vector2 input;

	void Start()
	{
		backgroundImage = GetComponent<Image> ();
		joystickImage = transform.GetChild(0).GetComponent<Image> ();
		input = Vector2.zero;
	}

	public virtual void OnDrag(PointerEventData ped)
	{
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(backgroundImage.rectTransform,
				ped.position,
				ped.pressEventCamera,
				out pos))
		{
			pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
			pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

			float x = (backgroundImage.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
			float y = (backgroundImage.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

			input = new Vector2 (x, y);
			input = (input.magnitude > 1) ? input.normalized : input;

			joystickImage.rectTransform.anchoredPosition =
				new Vector2 (input.x * (backgroundImage.rectTransform.sizeDelta.x / 3),
					input.y * (backgroundImage.rectTransform.sizeDelta.y / 3));
		}
	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		OnDrag (ped);
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		input = Vector2.zero;
		joystickImage.rectTransform.anchoredPosition = Vector2.zero;
	}

	public float GetAxis (string axisName)
	{
		if (axisName == "Horizontal")
			return input.x;
		else if (axisName == "Vertical")
			return input.y;

		return 0;
	}
}
