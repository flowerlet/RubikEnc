using System;
using System.IO;
using Assets.Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class UIController : MonoBehaviour
    {
        public Transform CameraCenter;
        public PressButton ButtonLeft;
        public PressButton ButtonRight;

        public InputField TextFieldInit;
        public InputField TextFieldTransform;
        public InputField TextFieldOutput;
        public InputField TextFieldRaw;

        public Button ButtonInit;
        public Button ButtonTransform;
        public Button ButtonOutput;

        public Button ButtonSwitch;
        public Text ButtonSwitchText;
        public Button ButtonExport;

        protected Controller RubikController;

        protected void Start()
        {
            RubikController = GetComponent<Controller>();
            ButtonLeft.OnPress.AddListener(() =>
            {
                CameraCenter.Rotate(Vector3.up, -100.0f * Time.deltaTime, Space.World);
            });
            ButtonRight.OnPress.AddListener(() =>
            {
                CameraCenter.Rotate(Vector3.up, 100.0f * Time.deltaTime, Space.World);
            });

            ButtonInit.onClick.AddListener(()=>
            {
                RubikController.Initialize(TextFieldInit.text);
            });

            ButtonTransform.onClick.AddListener(() =>
            {
                RubikController.Transform(TextFieldTransform.text);
            });

            ButtonOutput.onClick.AddListener(() =>
            {
                string result = "";
                string raw = "";
                RubikController.Output(ref result, ref raw);
                TextFieldOutput.text = result;
                TextFieldRaw.text = raw;
            });

            ButtonSwitch.onClick.AddListener(() =>
            {
                ButtonSwitchText.text = RubikController.SwitchMode() ? "ToBasicMode" : "ToFastMode";
            });
            
            ButtonExport.onClick.AddListener(() =>
            {
                string result = "";
                string raw = "";
                RubikController.Output(ref result, ref raw);
                string time = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                string fileName = time + " " + raw + ".png";

                string dir = Environment.CurrentDirectory + "\\Capture";
                Debug.Log(dir);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                
                ScreenCapture.CaptureScreenshot(dir + "\\" + fileName);
            });
        }
    }
}
