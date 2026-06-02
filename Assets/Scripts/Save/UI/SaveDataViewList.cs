using System;
using System.Collections.Generic;
using Save.Data;
using UnityEngine;
using Zenject;

namespace Save.UI
{
    public class SaveDataViewList : MonoBehaviour
    {
        [SerializeField] private GameObject _viewPrefab;
        [SerializeField] private RectTransform _content;
        
        private List<SaveDataView> _views = new();
        
        public event Action<SaveData> LoadRequested; 

        [Inject] private SaveFileList SaveFileList { get; set; }
        [Inject] private ISaveFileWriteService SaveFileWriteService { get; set; }
        
        private void Awake()
        {
            SaveFileList.Refreshed += Refresh;
            SaveFileList.Refresh();
        }

        private void Refresh()
        {
            SaveData[] saves = SaveFileList.Saves;

            for (int i = 0; i < saves.Length || i < _views.Count; i++)
            {
                if (i >= saves.Length)
                {
                    _views[i].gameObject.SetActive(false);
                    continue;
                }

                while (i >= _views.Count)
                {
                    _views.Add(CreateView());
                }
                
                _views[i].gameObject.SetActive(true);
                _views[i].Set(saves[i]);
            }
            
            
        }

        private SaveDataView CreateView()
        {
            SaveDataView view = Instantiate(_viewPrefab, _content).GetComponent<SaveDataView>();
            view.LoadRequested += LoadRequested;
            view.DeletionRequested += Delete;
            return view;
        }

        private void Delete(SaveData saveData)
        {
            SaveFileWriteService.Delete(saveData.filename);
            SaveFileList.Refresh();
        }

        private void OnDestroy()
        {
            SaveFileList.Refreshed -= Refresh;
        }
    }
}