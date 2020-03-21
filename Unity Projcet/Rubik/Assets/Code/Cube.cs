using UnityEngine;

namespace Assets.Code
{
    public enum CubeFaceDirection
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Back
    }

    public class Cube : MonoBehaviour
    {
        public Vector3Int Position;
    }
}
