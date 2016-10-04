using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class InputAdapter : UInput 
{
    public Button shotButton;
    public Button moveButton;
    public float MoveCoeficient;

    public override bool IsShooting()
    {
        return IsKeyboardShooting() || IsFireButtonPressed();
    }

    public override float GetHorizontalMovment()
    {
        return CustomButtonHorizontalMovment() + BuiltInHorizontalMovment();
    }

    public float BuiltInHorizontalMovment()
    {
        return Input.GetAxis("Horizontal");
    }

    public float CustomButtonHorizontalMovment()
    {
        var rect = moveButton.GetComponent<RectTransform>();
        var xMove = GetClickInside(rect, Vector2.zero).Value.x;
        if (xMove > 0)
            return MoveCoeficient;
        if (xMove < 0)
            return -MoveCoeficient;
        return 0;
    }

    private Vector2? GetClickInside(RectTransform rect, Vector2? defaultVal)
    {
        Vector3 buttonSize = rect.TransformVector(rect.rect.size);
        Vector3 butttonBotLeft = rect.position - (buttonSize / 2);
        Rect area = new Rect(butttonBotLeft, buttonSize);

        var pointPressed = GetPointAreaPressed(area);
        if (pointPressed.HasValue == false)
            return defaultVal;

        var distanceFromCenterX = pointPressed.Value.x - (area.x + buttonSize.x / 2);
        var distanceFromCenterY = pointPressed.Value.y - (area.y + buttonSize.y / 2);
        return new Vector2(distanceFromCenterX / area.width, distanceFromCenterY / area.height);
    }

    bool IsButtonPressed(Button btn)
    {
        return GetClickInside(btn.GetComponent<RectTransform>(), null).HasValue;
    }
        //var rect = moveButton.GetComponent<RectTransform>();
        //Vector3 fullSize = rect.TransformVector(rect.rect.size);
        //Vector3 halfwidthSize = new Vector3(fullSize.x / 2, fullSize.y, fullSize.z);

        //Vector3 posBotLeft = rect.position - (fullSize / 2);

        //Rect leftHalfButton = new Rect(posBotLeft, halfwidthSize);

        //return IsAreaPressed(leftHalfButton);
    Vector2[] GetTouches()
    {
        Vector2[] touches;
#if UNITY_ANDROID
        touches = Array.ConvertAll(Input.touches,
            new Converter<Touch, Vector2>((touch) => touch.position));

        // For unity editor, delete later
        if (Input.GetMouseButton(0) && touches.Length == 0)
            touches = new Vector2[1] { Input.mousePosition };
#else
        if (Input.GetMouseButton(0))
            touches = new Vector2[1] { Input.mousePosition };
        else
            touches = new Vector2[0];
#endif
        return touches;
    }

    Vector2? GetPointAreaPressed(Rect area)
    {
        //Debug.Log("Checking for area " + area);
        foreach (var touch in GetTouches())
        {
            Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(touch);

            if (area.Contains(worldTouchPos))
                return worldTouchPos;
        }

        return null;
    }

    bool IsFireButtonPressed()
    {
        return IsButtonPressed(shotButton);
    }

    bool IsKeyboardShooting()
    {
        return Input.GetKey(KeyCode.Space);
    }
}