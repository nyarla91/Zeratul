using UnityEngine;
using UnityEngine.InputSystem;

namespace UIUtility
{
    public class TabSystemMenuWindow : MenuWindow
    {
        [SerializeField] private Menu _subMenu;
        [SerializeField] private Tab[] _tabs;
        [SerializeField] private Tab _firstTab;
        [SerializeField] private bool _subTabs;

        public Tab CurrentTab { get; private set; }

        public override void Open()
        {
            base.Open();
            _subMenu.Open();

            foreach (Tab tab in _tabs)
            {
                tab.Close();
            }
            _firstTab.Open();
            CurrentTab = _firstTab;
        }

        public override void Close()
        {
            base.Close();
            _subMenu.Close();
            foreach (Tab tab in _tabs)
            {
                tab.Close();
            }
        }

        public void OpenTab(Tab openedTab)
        {
            foreach (Tab tab in _tabs)
            {
                if (tab == openedTab)
                    tab.Open();
                else
                    tab.Close();
            }
            CurrentTab = openedTab;
        }

        private void SwitchTabRight(InputAction.CallbackContext _) => OpenTab(CurrentTab.RightTab);
        private void SwitchTabLeft(InputAction.CallbackContext _) => OpenTab(CurrentTab.LeftTab);
    }
}