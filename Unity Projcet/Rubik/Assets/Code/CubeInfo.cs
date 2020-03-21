using UnityEngine;

namespace Assets.Code
{
    public static class CubeInfo
    {
        public enum CubeDirection
        {
            Forward,
            Back,
            Right,
            Left,
            Up,
            Down
        }

        public static Vector3[] ForwardFacePositions = new[]
        {
            new Vector3(1, 1, 1.5f),
            new Vector3(0, 1, 1.5f),
            new Vector3(-1, 1, 1.5f),
            new Vector3(1, 0, 1.5f),
            new Vector3(0, 0, 1.5f),
            new Vector3(-1, 0, 1.5f),
            new Vector3(1, -1, 1.5f),
            new Vector3(0, -1, 1.5f),
            new Vector3(-1, -1, 1.5f),
        };

        public static Vector3[] BackFacePositions = new[]
        {
            new Vector3(-1, 1, -1.5f),
            new Vector3(0, 1, -1.5f),
            new Vector3(1, 1, -1.5f),
            new Vector3(-1, 0, -1.5f),
            new Vector3(0, 0, -1.5f),
            new Vector3(1, 0, -1.5f),
            new Vector3(-1, -1, -1.5f),
            new Vector3(0, -1, -1.5f),
            new Vector3(1, -1, -1.5f),
        };

        public static Vector3[] RightFacePositions = new[]
        {
            new Vector3(-1.5f, 1, 1),
            new Vector3(-1.5f, 1, 0),
            new Vector3(-1.5f, 1, -1),
            new Vector3(-1.5f, 0, 1),
            new Vector3(-1.5f, 0, 0),
            new Vector3(-1.5f, 0, -1),
            new Vector3(-1.5f, -1, 1),
            new Vector3(-1.5f, -1, 0),
            new Vector3(-1.5f, -1, -1),
        };

        public static Vector3[] LeftFacePositions = new[]
        {
            new Vector3(1.5f, 1, -1),
            new Vector3(1.5f, 1, 0),
            new Vector3(1.5f, 1, 1),
            new Vector3(1.5f, 0, -1),
            new Vector3(1.5f, 0, 0),
            new Vector3(1.5f, 0, 1),
            new Vector3(1.5f, -1, -1),
            new Vector3(1.5f, -1, 0),
            new Vector3(1.5f, -1, 1),
        };

        public static Vector3[] UpFacePositions = new[]
        {
            new Vector3(1, 1.5f, -1),
            new Vector3(0, 1.5f, -1),
            new Vector3(-1, 1.5f, -1),
            new Vector3(1, 1.5f, 0),
            new Vector3(0, 1.5f, 0),
            new Vector3(-1, 1.5f, 0),
            new Vector3(1, 1.5f, 1),
            new Vector3(0, 1.5f, 1),
            new Vector3(-1, 1.5f, 1),
        };

        public static Vector3[] DownFacePositions = new[]
        {
            new Vector3(1, -1.5f, 1),
            new Vector3(0, -1.5f, 1),
            new Vector3(-1, -1.5f, 1),
            new Vector3(1, -1.5f, 0),
            new Vector3(0, -1.5f, 0),
            new Vector3(-1, -1.5f, 0),
            new Vector3(1, -1.5f, -1),
            new Vector3(0, -1.5f, -1),
            new Vector3(-1, -1.5f, -1),
        };
        
        // 上下左右
        public static Vector3[] ForwardArrowDirections = new[]
        {
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
        };

        public static Vector3[] BackArrowDirections = new[]
        {
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(-1, 0, 0),
            new Vector3(1, 0, 0),
        };

        public static Vector3[] RightArrowDirections = new[]
        {
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
        };
        public static Vector3[] LeftArrowDirections = new[]
        {
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, 1),
        };
        public static Vector3[] UpArrowDirections = new[]
        {
            new Vector3(0, 0, -1),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
        };
        public static Vector3[] DownArrowDirections = new[]
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
        };
    }
}