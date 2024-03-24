using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D _change;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(_change, new Vector2(_change.width / 2f, _change.height / 2f), CursorMode.ForceSoftware);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
