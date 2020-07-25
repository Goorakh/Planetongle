using System;
using UnityEngine;

[Serializable]
public class InputData
{
    public ActionType Action;
    public InputType Type;

    public KeyCode Key = KeyCode.None;

    public int MouseButton = -1;

    public bool IsActive()
    {
        if (Key != KeyCode.None)
        {
            switch (Type)
            {
                case InputType.Down:
                    return Input.GetKeyDown(Key);
                case InputType.Held:
                    return Input.GetKey(Key);
                case InputType.Up:
                    return Input.GetKeyUp(Key);
                default:
                    throw new NotImplementedException("Input type \"" + Type + "\" not implemented!");
            }
        }
        else if (MouseButton >= 0)
        {
            switch (Type)
            {
                case InputType.Down:
                    return Input.GetMouseButtonDown(MouseButton);
                case InputType.Held:
                    return Input.GetMouseButton(MouseButton);
                case InputType.Up:
                    return Input.GetMouseButtonUp(MouseButton);
                default:
                    throw new NotImplementedException("Input type \"" + Type + "\" not implemented!");
            }
        }
        else
        {
            throw new Exception("Neither a KeyCode nor a mouse button index present for action \"" + Action + "\"");
        }
    }
}
