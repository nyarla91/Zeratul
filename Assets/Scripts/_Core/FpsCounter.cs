using System.Linq;
using TMPro;
using UnityEngine;

namespace _Core
{
    [ExecuteAlways]
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private int _bufferSize;

        private float[] _deltaTimeBuffer;

        private void Awake()
        {
            _deltaTimeBuffer = new float[_bufferSize];
        }

        private void Update()
        {
            int bufferIndex = Time.frameCount % _bufferSize;
            _deltaTimeBuffer[bufferIndex] = Time.deltaTime;
            float averageDelta = _deltaTimeBuffer.Average();
            int fps = Mathf.RoundToInt(1 / averageDelta);
            _text.text = fps.ToString();
        }
    }
}