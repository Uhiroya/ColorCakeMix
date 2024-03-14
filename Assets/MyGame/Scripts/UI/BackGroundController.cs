using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BackGroundController : SingletonMonoBehavior<BackGroundController>
{
    [SerializeField] private RawImage _image;
    private Vector2 _uvSpeed;
    private CanvasGroup _canvasGroup;
    protected override void OnAwake()
    {
        base.OnAwake();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Display(bool flag)
    {
        if (flag)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
    public void ManualUpdate(float deltaTime)
    {
        UVScroll(deltaTime);
    }

    public void SetUVSpeed(Vector2 uvSpeed)
    {
        _uvSpeed = uvSpeed;
    }

    private void UVScroll(float deltaTime)
    {
        var rect = _image.uvRect;
        rect.y += _uvSpeed.y * deltaTime;
        rect.y %= 1f;
        rect.x += _uvSpeed.x * deltaTime;
        rect.x %= 1f;
        _image.uvRect = rect;
    }
}
