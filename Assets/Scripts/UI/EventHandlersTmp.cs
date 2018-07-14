using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventHandlersTmp : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public RectTransform canvas;
	public Transform pigPosition;

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 mousePosition;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, eventData.position, eventData.pressEventCamera, out mousePosition))
		{
			transform.position = mousePosition;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		
		Vector2 mousePosition;
		
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.gameObject.transform as RectTransform, eventData.position, eventData.pressEventCamera, out mousePosition))
		{
			mousePosition.x = mousePosition.x/266 + .5f;
			mousePosition.y = mousePosition.y/266;
			this.gameObject.GetComponent<RectTransform>().pivot = mousePosition;
		}

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
		iTween.MoveTo(this.gameObject, iTween.Hash("x", pigPosition.position.x, "y", pigPosition.position.y, "time", 0.3f, "easetype", "spring"));
	}
}
