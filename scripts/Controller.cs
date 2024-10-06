using Godot;
using Godot.Collections;


public enum ControllerKey
{
  None,
  Right,
  Left,
  Up,
  Down
};

public class Controller
{
  private static ControllerKey key = ControllerKey.None;
  private static readonly Dictionary<ControllerKey, string> keyMaps = new()
    {
      { ControllerKey.None, "none" },
      { ControllerKey.Right, "move_right" },
      { ControllerKey.Left, "move_left" },
      { ControllerKey.Up, "move_up" },
      { ControllerKey.Down, "move_down" }
    };
  private static bool longDown = true;

  private static string GetKey(ControllerKey key)
  {
    return keyMaps[key];
  }

  private static bool MergeKeyDown(ControllerKey currentKey, bool down, bool up)
  {
    if (longDown)
    {
      if (down)
      {
        key = currentKey;
      }

      else if (up)
      {
        key = ControllerKey.None;
      }
    }
    else
    {
      key = ControllerKey.None;
    }

    return down || (key == currentKey && currentKey != ControllerKey.None);
  }

  private static (bool, bool) KeyDown(ControllerKey key)
  {
    string keyName = GetKey(key);
    bool keyUp = Input.IsActionJustReleased(keyName);
    bool keyDown = MergeKeyDown(key, Input.IsActionJustPressed(keyName), keyUp);

    return (keyDown, keyUp);
  }

  public static bool ToggleLongDown()
  {
    longDown = !longDown;
    return longDown;
  }

  public static (bool, bool) IsLeft()
  {
    return KeyDown(ControllerKey.Left);
  }

  public static (bool, bool) IsRight()
  {
    return KeyDown(ControllerKey.Right);
  }

  public static (bool, bool) IsUp()
  {
    return KeyDown(ControllerKey.Up);
  }

  public static (bool, bool) IsDown()
  {
    return KeyDown(ControllerKey.Down);
  }

  public static (bool, int) MoveX()
  {
    var (left, _) = IsLeft();
    var (right, _) = IsRight();

    if (left)
    {
      return (true, -1);
    }
    else if (right)
    {
      return (true, 1);
    }
    else
    {
      return (false, 0);
    }
  }

  public static (bool, int) MoveY()
  {
    var (up, _) = IsUp();
    var (down, _) = IsDown();

    if (up)
    {
      return (true, -1);
    }
    else if (down)
    {
      return (true, 1);
    }
    else
    {
      return (false, 0);
    }
  }
}
