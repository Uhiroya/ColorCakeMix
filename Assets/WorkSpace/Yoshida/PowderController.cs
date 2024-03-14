using UnityEngine;
using UnityEngine.UI;

public class PowderController : Button, ICanvasRaycastFilter
{
    private const float Radius = 80;
    
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return Vector2.Distance(sp, transform.position) < Radius;
    }
}