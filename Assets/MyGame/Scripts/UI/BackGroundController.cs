using UnityEngine;
using UnityEngine.UI;

public class BackGroundController : SingletonMonoBehavior<BackGroundController>
{
    [SerializeField] private RawImage _image;
    private Vector2 _uvSpeed;

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
