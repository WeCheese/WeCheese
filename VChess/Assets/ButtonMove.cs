using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Location;

public class ButtonMove : MonoBehaviour
{
    private float original_movement_magnitude = EditorLocationProviderLocationLog.movement_magnitude;

    public void ButtonMoveUp()
    {
        EditorLocationProviderLocationLog.movement_magnitude *= 10;
        EditorLocationProviderLocationLog.MoveUp();
        EditorLocationProviderLocationLog.movement_magnitude = original_movement_magnitude;
    }

    public void ButtonMoveDown()
    {
        EditorLocationProviderLocationLog.movement_magnitude *= 10;
        EditorLocationProviderLocationLog.MoveDown();
        EditorLocationProviderLocationLog.movement_magnitude = original_movement_magnitude;
    }

    public void ButtonMoveLeft()
    {
        EditorLocationProviderLocationLog.movement_magnitude *= 10;
        EditorLocationProviderLocationLog.MoveLeft();
        EditorLocationProviderLocationLog.movement_magnitude = original_movement_magnitude;
    }

    public void ButtonMoveRight()
    {
        EditorLocationProviderLocationLog.movement_magnitude *= 10;
        EditorLocationProviderLocationLog.MoveRight();
        EditorLocationProviderLocationLog.movement_magnitude = original_movement_magnitude;
    }
}
