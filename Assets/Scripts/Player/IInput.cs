using UnityEngine;

public interface IInput
{
    Vector2 direction { get; }

    Vector2 look { get; }
    bool jump { get; }
    bool dash { get; }
}