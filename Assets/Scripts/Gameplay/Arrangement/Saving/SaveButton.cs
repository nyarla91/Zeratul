using System;
using _Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Arrangement.Saving
{
    public class SaveButton : MonoBehaviour
    {
        [SerializeField] private GameplaySavingFlow _savingFlow;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _invalidMessage;
        [SerializeField] private Button _button;

        
        private void Update()
        {
            bool isFilenameValid = _inputField.text.IsFilenameValid();
            _button.interactable = isFilenameValid;
            _invalidMessage.enabled = ! isFilenameValid;
        }

        public void Save()
        {
            if ( ! _inputField.text.IsFilenameValid())
                return;
            _savingFlow.Save(_inputField.text, false);
        }
    }
}