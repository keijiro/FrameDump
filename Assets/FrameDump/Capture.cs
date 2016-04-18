using UnityEngine;

namespace FrameDump
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("FrameDump/Capture")]
    public class Capture : MonoBehaviour
    {
        #region Editable properties

        [SerializeField] int _width = 1280;
        [SerializeField] int _height = 720;

        public enum SuperSampling { None, X2, X4, X8 }
        [SerializeField] SuperSampling _superSampling;

        #endregion

        #region Private members

        RenderTexture _renderTexture;

        #endregion

        #region MonoBehavior functions

        void OnEnable()
        {
            _renderTexture = new RenderTexture(
                Mathf.Clamp(_width, 1, 1024 * 8),
                Mathf.Clamp(_height, 1, 1024 * 8),
                24
            );

            _renderTexture.antiAliasing = (int)Mathf.Pow(2, (int)_superSampling);

            GetComponent<Camera>().targetTexture = _renderTexture;
        }

        void OnDisable()
        {
            GetComponent<Camera>().targetTexture = null;
            _renderTexture.Release();
            DestroyImmediate(_renderTexture);
            _renderTexture = null;
        }

        void Update()
        {
            if (!Dumper.Dump(gameObject.name, _renderTexture))
                enabled = false;
        }

        #endregion
    }
}
