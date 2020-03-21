using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class Controller : MonoBehaviour
    {
        public Vector3Int[] Positions = new Vector3Int[]
        {
            new Vector3Int(0, 0, 1),
            new Vector3Int(0, 0, -1),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 0),
        };

        public string Init;
        public Transform Root;

        public Material M00_Up;
        public Material M01_Down;
        public Material M10_Left;
        public Material M11_Right;

        public Material Face_Yellow;
        public Material Face_White;
        public Material Face_Red;
        public Material Face_Blue;
        public Material Face_Orange;
        public Material Face_Green;

        public Material ButtonSelected;
        public Material ButtonUnselected;

        protected Cube[] Cubes;

        protected RotateButton[] RotateButtons;

        protected Face[] ForwardFaces = new Face[9];
        protected Face[] BackFaces = new Face[9];
        protected Face[] RightFaces = new Face[9];
        protected Face[] LeftFaces = new Face[9];
        protected Face[] UpFaces = new Face[9];
        protected Face[] DownFaces = new Face[9];

        protected Face[] AllFaces;
        protected Face[] Faces = new Face[54];

        protected bool Rotating = false;
        protected bool Transforming = false;

        protected Vector3 RotateAxis = Vector3.up;

        // 旋转一次所用时间（秒）
        protected static float RotateSpeed = 0.45f;

        protected float RotateAngleRemaining = 90.0f;

        // 旋转顺/逆时针
        protected float Clockwise = 1.0f;

        protected List<Cube> RotateComponents;

        public struct RotateInfo
        {
            public CubeFaceDirection Direction;
            public int Index;
            public float Clockwise;
        }

        protected List<RotateInfo> TransformSequence;

        protected void Start()
        {
            Cubes = FindObjectsOfType<Cube>();
            AllFaces = FindObjectsOfType<Face>();
            RotateButtons = FindObjectsOfType<RotateButton>();
            RotateComponents = new List<Cube>();
            TransformSequence = new List<RotateInfo>();

            FindFaces();
            for (int i = 0; i < 9; i++)
            {
                Faces[i + 0 * 9] = UpFaces[i];
                Faces[i + 1 * 9] = LeftFaces[i];
                Faces[i + 2 * 9] = ForwardFaces[i];
                Faces[i + 3 * 9] = RightFaces[i];
                Faces[i + 4 * 9] = DownFaces[i];
                Faces[i + 5 * 9] = BackFaces[i];
            }
            AllFaces = Faces;

            Initialize(Init);
        }

        protected void ResetCubes()
        {
            foreach (var cube in Cubes)
            {
                var t = cube.transform;
                t.position = cube.Position;
                t.rotation = Quaternion.identity;
                t.GetChild(0).localPosition = Vector3.zero;
            }
        }

        protected bool Hide = false;
        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Hide = !Hide;
                if (Hide)
                {
                    HideSomething();
                }
                else
                {
                    ShowSomething();
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                ColorFace();
            }

                if (Rotating)
            {
                float angle = Time.deltaTime / RotateSpeed * 90.0f;
                if (RotateAngleRemaining <= angle)
                {
                    angle = RotateAngleRemaining;
                    Rotating = false;
                    transform.Rotate(RotateAxis, angle * Clockwise, Space.World);
                    EndRotate();

                    if (Transforming)
                    {
                        TransformNext();
                    }
                }
                else
                {
                    RotateAngleRemaining -= angle;
                    transform.Rotate(RotateAxis, angle * Clockwise, Space.World);
                }
                return;
            }

            if (Transforming) return;
            // 检测按钮
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                ResetButtons();
                hit.transform.GetComponent<MeshRenderer>().sharedMaterial = ButtonSelected;
                RotateButton button = hit.transform.GetComponent<RotateButton>();
                button.Active = true;
            }
            else
            {
                ResetButtons();
            }

            // 检测点击
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                foreach (var button in RotateButtons)
                {
                    if (button.Active)
                    {
                        Clockwise = Input.GetMouseButton(0) ? 1.0f : -1.0f;
                        StartRotate(button.Direction, button.Index);
                        ResetButtons();
                        return;
                    }
                }
                ResetButtons();
            }
        }

        protected bool IsFastMode = false;
        public bool SwitchMode()
        {
            IsFastMode = !IsFastMode;
            RotateSpeed = IsFastMode ? 0.08f : 0.45f;
            return IsFastMode;
        }

        public void ColorFace()
        {
            ResetCubes();
            foreach (var face in ForwardFaces)
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = Face_White;
            }
            foreach (var face in BackFaces)
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = Face_Blue;
            }
            foreach (var face in LeftFaces)
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = Face_Yellow;
            }
            foreach (var face in RightFaces)
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = Face_Green;
            }
            foreach (var face in UpFaces)
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = Face_Red;
            }
            foreach (var face in DownFaces)
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = Face_Orange;
            }
        }

        public void Initialize(string input)
        {
            ResetCubes();
            int[,] directions = new int[6, 9];

            input = input.ToLower();
            switch (input)
            {
                case "random":
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            directions[i, j] = UnityEngine.Random.Range(0, 4);
                        }
                    }
                    break;
                case "u":
                case "up":
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            directions[i, j] = 0;
                        }
                    }
                    break;
                case "d":
                case "down":
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            directions[i, j] = 1;
                        }
                    }
                    break;
                case "l":
                case "left":
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            directions[i, j] = 2;
                        }
                    }
                    break;
                case "r":
                case "right":
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            directions[i, j] = 3;
                        }
                    }
                    break;
                default:
                    input = input.Replace("\n", "");
                    input = input.Replace(" ", "");
                    if (input.Length < 108) input = input.PadRight(108, '0');
                    for (int i = 0; i < 54; i++)
                    {
                        switch (input.Substring(i * 2, 2))
                        {
                            case "00":
                                directions[i / 9, i % 9] = 0;
                                break;
                            case "01":
                                directions[i / 9, i % 9] = 3;
                                break;
                            case "10":
                                directions[i / 9, i % 9] = 1;
                                break;
                            case "11":
                                directions[i / 9, i % 9] = 2;
                                break;
                            default:
                                directions[i / 9, i % 9] = 0;
                                break;
                        }
                    }
                    break;
            }

            FindFaces();

            for (int i = 0; i < 9; i++)
            {
                SetArrow(UpFaces[i], CubeInfo.UpArrowDirections[directions[0, i]], CubeFaceDirection.Up);
                SetArrow(LeftFaces[i], CubeInfo.LeftArrowDirections[directions[1, i]], CubeFaceDirection.Left);
                SetArrow(ForwardFaces[i], CubeInfo.ForwardArrowDirections[directions[2, i]], CubeFaceDirection.Forward);
                SetArrow(RightFaces[i], CubeInfo.RightArrowDirections[directions[3, i]], CubeFaceDirection.Right);
                SetArrow(DownFaces[i], CubeInfo.DownArrowDirections[directions[4, i]], CubeFaceDirection.Down);
                SetArrow(BackFaces[i], CubeInfo.BackArrowDirections[directions[5, i]], CubeFaceDirection.Back);
            }
        }

        protected void FindFaces()
        {
            for (int i = 0; i < 9; i++)
            {
                UpFaces[i] = FindFace(CubeInfo.UpFacePositions[i]);
                AssignIndex(UpFaces, 0);

                LeftFaces[i] = FindFace(CubeInfo.LeftFacePositions[i]);
                AssignIndex(LeftFaces, 9);

                ForwardFaces[i] = FindFace(CubeInfo.ForwardFacePositions[i]);
                AssignIndex(ForwardFaces, 18);

                RightFaces[i] = FindFace(CubeInfo.RightFacePositions[i]);
                AssignIndex(RightFaces, 27);

                DownFaces[i] = FindFace(CubeInfo.DownFacePositions[i]);
                AssignIndex(DownFaces, 36);

                BackFaces[i] = FindFace(CubeInfo.BackFacePositions[i]);
                AssignIndex(BackFaces, 45);
            }
        }

        public void AssignIndex(Face[] faces, int startIndex)
        {
            for (int i = 0; i < faces.Length; i++)
            {
                if (faces[i] == null) continue;
                faces[i].Index = startIndex + i;
            }
        }

        public void SetArrow(Face face, Vector3 target, CubeFaceDirection direction)
        {
            Transform t = face.transform;
            Vector3 up = new Vector3();
            Vector3 down = new Vector3();
            Vector3 left = new Vector3();
            Vector3 right = new Vector3();
            switch (direction)
            {
                case CubeFaceDirection.Forward:
                    up = t.up;
                    down = -up;
                    left = t.right;
                    right = -left;
                    break;
                case CubeFaceDirection.Back:
                    up = t.up;
                    down = -up;
                    right = t.right;
                    left = -right;
                    break;
                case CubeFaceDirection.Right:
                    up = t.up;
                    down = -up;
                    left = t.forward;
                    right = -left;
                    break;
                case CubeFaceDirection.Left:
                    up = t.up;
                    down = -up;
                    left = -t.forward;
                    right = -left;
                    break;
                case CubeFaceDirection.Up:
                    up = -t.forward;
                    down = -up;
                    left = t.right;
                    right = -left;
                    break;
                case CubeFaceDirection.Down:
                    up = -t.forward;
                    down = -up;
                    left = -t.right;
                    right = -left;
                    break;
            }
            if (Approximate(up, target))
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = M00_Up;
            }
            else if (Approximate(down, target))
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = M01_Down;
            }
            else if (Approximate(left, target))
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = M10_Left;
            }
            else if (Approximate(right, target))
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = M11_Right;
            }
            else
            {
                face.GetComponent<MeshRenderer>().sharedMaterial = Face_White;
            }
            face.Direction = target;
        }

        public float NonZeroNumber(Vector3 v)
        {
            if (Approximate(v.x, 1)) return v.x;
            if (Approximate(v.y, 1)) return v.y;
            return v.z;
        }

        public bool Approximate(Vector3 a, Vector3 b)
        {
            if (!Approximate(a.x, b.x)) return false;
            if (!Approximate(a.y, b.y)) return false;
            if (!Approximate(a.z, b.z)) return false;
            return true;
        }

        public bool Approximate(float a, float b)
        {
            return Mathf.Abs(a - b) < 0.12f;
        }

        public void Output(ref string result, ref string raw)
        {
            if (Transforming) return;
            FindFaces();

            result = "";
            raw = "";

            result += "U:";
            result += FacesOutput(UpFaces, CubeInfo.UpArrowDirections, ref raw);
            result += "\n";

            result += "L:";
            result += FacesOutput(LeftFaces, CubeInfo.LeftArrowDirections, ref raw);
            result += "\n";

            result += "F:";
            result += FacesOutput(ForwardFaces, CubeInfo.ForwardArrowDirections, ref raw);
            result += "\n";

            result += "R:";
            result += FacesOutput(RightFaces, CubeInfo.RightArrowDirections, ref raw);
            result += "\n";

            result += "D:";
            result += FacesOutput(DownFaces, CubeInfo.DownArrowDirections, ref raw);
            result += "\n";

            result += "B:";
            result += FacesOutput(BackFaces, CubeInfo.BackArrowDirections, ref raw);
            result += "\n";
        }
        
        // 0,1,2,3对应上下左右
        public string FacesOutput(Face[] faces, Vector3[] targetDirections, ref string raw)
        {
            string result = "";
            foreach (var face in faces)
            {
                Vector3 d = face.transform.localToWorldMatrix.MultiplyVector(face.Direction).normalized;
                if (Approximate(d, targetDirections[0]))
                {
                    face.Arrow = 0;
                    result += "↑";
                    raw += "00";
                }
                else if (Approximate(d, targetDirections[1]))
                {
                    face.Arrow = 2;
                    result += "↓";
                    raw += "10";
                }
                else if (Approximate(d, targetDirections[2]))
                {
                    face.Arrow = 3;
                    result += "←";
                    raw += "11";
                }
                else
                {
                    face.Arrow = 1;
                    result += "→";
                    raw += "01";
                }
            }
            return result;
        }


        public void Transform(string seq)
        {
            Transforming = true;
            TransformSequence.Clear();

            seq = seq.Replace("S", "RLUUFuDFFRRBBLUUfbURRDFFURRU");
            seq = seq.Replace("s", "urruffdrruBFuulbbrrffdUfuulr");
            seq = seq.Replace("M", "UUDDLFFuDRRBudRLFFRUdrLUfb");
            seq = seq.Replace("m", "BFulRDurfflrDUbrrdUffldduu");

            for (int i = 0; i < seq.Length; i++)
            {
                char c = seq[i];
                RotateInfo ri = new RotateInfo();
                switch (c)
                {
                    case 'F':
                        ri.Direction = CubeFaceDirection.Forward;
                        ri.Index = 1;
                        ri.Clockwise = 1.0f;
                        break;
                    case 'f':
                        ri.Direction = CubeFaceDirection.Forward;
                        ri.Index = 1;
                        ri.Clockwise = -1.0f;
                        break;
                    case 'B':
                        ri.Direction = CubeFaceDirection.Back;
                        ri.Index = -1;
                        ri.Clockwise = 1.0f;
                        break;
                    case 'b':
                        ri.Direction = CubeFaceDirection.Back;
                        ri.Index = -1;
                        ri.Clockwise = -1.0f;
                        break;
                    case 'R':
                        ri.Direction = CubeFaceDirection.Right;
                        ri.Index = -1;
                        ri.Clockwise = 1.0f;
                        break;
                    case 'r':
                        ri.Direction = CubeFaceDirection.Right;
                        ri.Index = -1;
                        ri.Clockwise = -1.0f;
                        break;
                    case 'L':
                        ri.Direction = CubeFaceDirection.Left;
                        ri.Index = 1;
                        ri.Clockwise = 1.0f;
                        break;
                    case 'l':
                        ri.Direction = CubeFaceDirection.Left;
                        ri.Index = 1;
                        ri.Clockwise = -1.0f;
                        break;
                    case 'U':
                        ri.Direction = CubeFaceDirection.Up;
                        ri.Index = 1;
                        ri.Clockwise = 1.0f;
                        break;
                    case 'u':
                        ri.Direction = CubeFaceDirection.Up;
                        ri.Index = 1;
                        ri.Clockwise = -1.0f;
                        break;
                    case 'D':
                        ri.Direction = CubeFaceDirection.Down;
                        ri.Index = -1;
                        ri.Clockwise = 1.0f;
                        break;
                    case 'd':
                        ri.Direction = CubeFaceDirection.Down;
                        ri.Index = -1;
                        ri.Clockwise = -1.0f;
                        break;
                    default:
                        continue;
                }
                TransformSequence.Add(ri);
            }
            TransformNext();
        }

        public void TransformNext()
        {
            if (TransformSequence.Count == 0)
            {
                Transforming = false;
                return;
            }
            RotateInfo ri = TransformSequence[0];
            TransformSequence.RemoveAt(0);
            Clockwise = ri.Clockwise;
            StartRotate(ri.Direction, ri.Index);
        }

        public Face FindFace(Vector3 v)
        {
            foreach (var face in AllFaces)
            {
                Vector3 position = face.transform.position;
                if (Approximate(position.x, v.x) && Approximate(position.y, v.y) &&
                    Approximate(position.z, v.z)) return face;
            }
            return null;
        }

        public void StartRotate(CubeFaceDirection direction, int index)
        {
            switch (direction)
            {
                case CubeFaceDirection.Forward:
                    RotateAxis = Vector3.forward;
                    break;
                case CubeFaceDirection.Back:
                    RotateAxis = Vector3.forward;
                    Clockwise *= -1.0f;
                    break;
                case CubeFaceDirection.Right:
                    RotateAxis = Vector3.left;
                    Clockwise *= 1.0f;
                    break;
                case CubeFaceDirection.Left:
                    RotateAxis = Vector3.left;
                    Clockwise *= -1.0f;
                    break;
                case CubeFaceDirection.Up:
                    RotateAxis = Vector3.up;
                    break;
                case CubeFaceDirection.Down:
                    RotateAxis = Vector3.up;
                    Clockwise *= -1.0f;
                    break;
            }
            Combine(direction, index);
            Rotating = true;
        }

        protected void EndRotate()
        {
            RotateAngleRemaining = 90.0f;
            ResetButtons();
            Separate();
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        protected void Combine(CubeFaceDirection direction, int index)
        {
            RotateComponents.Clear();

            foreach (var cube in Cubes)
            {
                float i = 0;
                switch (direction)
                {
                    case CubeFaceDirection.Up:
                    case CubeFaceDirection.Down:
                        i = cube.transform.position.y;
                        break;
                    case CubeFaceDirection.Left:
                    case CubeFaceDirection.Right:
                        i = cube.transform.position.x;
                        break;
                    case CubeFaceDirection.Forward:
                    case CubeFaceDirection.Back:
                        i = cube.transform.position.z;
                        break;
                }
                if (i < index + 0.01f && i > index - 0.01f)
                {
                    RotateComponents.Add(cube);
                }
            }
            foreach (var cube in RotateComponents)
            {
                cube.GetComponent<Transform>().SetParent(transform, true);
            }
        }

        protected void Separate()
        {
            foreach (var cube in Cubes)
            {
                cube.GetComponent<Transform>().SetParent(Root, true);
            }
        }

        protected void ResetButtons()
        {
            foreach (var button in RotateButtons)
            {
                button.transform.GetComponent<MeshRenderer>().sharedMaterial = ButtonUnselected;
                button.transform.localScale = Vector3.one;
                button.Active = false;
            }
        }

        public void HideSomething()
        {
            foreach (var button in RotateButtons)
            {
                button.transform.gameObject.SetActive(false);
            }
        }
        public void ShowSomething()
        {
            foreach (var button in RotateButtons)
            {
                button.transform.gameObject.SetActive(true);
            }
        }
    }
}