using System;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            _text.text = Application.version;
        }
    }
}