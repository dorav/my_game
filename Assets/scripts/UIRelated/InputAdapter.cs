using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class InputAdapter : IInput 
{
    public Button shotButton;
    public Button moveButton;

    public override bool IsShooting()
    {
        return IsKeyboardShooting() || IsFireButtonPressed();
    }

    public override bool IsMovingRight()
    {
        return IsClickMovingRight() || IsKeyboardMovingRight();
    }

    public override bool IsMovingLeft()
    {
        return IsClickMovingLeft() || IsKeyboardMovingLeft();
    }

    public bool IsKeyboardMovingLeft()
    {
        return Input.GetAxis("Horizontal") < 0;
    }

    public bool IsKeyboardMovingRight()
    {
        return Input.GetAxis("Horizontal") > 0;
    }

    public bool IsClickMovingRight()
    {
        var rect = moveButton.GetComponent<RectTransform>();
        Vector3 fullSize = rect.TransformVector(rect.rect.size);
        Vector3 halfwidthSize = new Vector3(fullSize.x / 2, fullSize.y, fullSize.z);

        Vector3 posBotMid = rect.position - (fullSize / 2) + new Vector3(halfwidthSize.x, 0);

        Rect leftHalfButton = new Rect(posBotMid, halfwidthSize);

        return IsAreaPressed(leftHalfButton);
    }

    public bool IsClickMovingLeft()
    {
        var rect = moveButton.GetComponent<RectTransform>();
        Vector3 fullSize = rect.TransformVector(rect.rect.size);
        Vector3 halfwidthSize = new Vector3(fullSize.x / 2, fullSize.y, fullSize.z);

        Vector3 posBotLeft = rect.position - (fullSize / 2);

        Rect leftHalfButton = new Rect(posBotLeft, halfwidthSize);

        return IsAreaPressed(leftHalfButton);
    }

    bool IsButtonPressed(Button btn)
    {
        var rect = btn.GetComponent<RectTransform>();
        Vector3 buttonSize = rect.TransformVector(rect.rect.size);
        Vector3 butttonBotLeft = rect.position - (buttonSize / 2);
        Rect area = new Rect(butttonBotLeft, buttonSize);

        return IsAreaPressed(area);
    }

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

    bool IsAreaPressed(Rect area)
    {
        //Debug.Log("Checking for area " + area);
        foreach (var touch in GetTouches())
        {
            Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(touch);

            if (area.Contains(worldTouchPos))
                return true;
        }

        return false;
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