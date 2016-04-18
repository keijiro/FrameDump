using UnityEngine;
using System.IO;

namespace FrameDump
{
    [AddComponentMenu("FrameDump/Dumper")]
    public class Dumper : MonoBehaviour
    {
        #region Editable properties

        [SerializeField] int _frameRate = 30;
        [SerializeField] float _recordLength = 5;

        #endregion

        #region Public functions

        public static bool Dump(string name, RenderTexture rt)
        {
            if (!_recording) return false;

            var path = "Capture/" + name.Replace(" ", "_") + "_";
            path += _frameCount.ToString("0000") + ".png";

            RenderTexture.active = rt;

            var temp = new Texture2D(rt.width, rt.height);
            temp.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            System.IO.File.WriteAllBytes(path, temp.EncodeToPNG());

            DestroyImmediate(temp);

            RenderTexture.active = null;

            Debug.Log("FrameDump: " + path);

            return true;
        }

        #endregion

        #region Private members

        static bool _recording;
        static int _frameCount;

        #endregion

        #region MonoBehavior functions

        void Start()
        {
            Directory.CreateDirectory("Capture");

            Time.captureFramerate = _frameRate;

            _recording = true;
        }

        void Update()
        {
            if (_recording)
            {
                if (++_frameCount > _frameRate * _recordLength)
                {
                    Time.captureFramerate = 0;
                    _recording = false;
                }
            }
        }

        #endregion
    }
}
