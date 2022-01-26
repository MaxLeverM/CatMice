using System;
using Lever.UI.Windows.Behaviours;
using Lever.UI.Windows.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class LoginBehaviour : PopUpWindow
    {
        [SerializeField] private TMP_InputField nicknameInputField;
        [SerializeField] private Button selectNameButton;
        [SerializeField] private int minCharInNick = 3;

        private IUIManager uiManager;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        private void Start()
        {
            Show();
        }

        public void Show(string previousName = "")
        {
            selectNameButton.interactable = false;
            ToggleVisibleCanvasGroup(CanvasGroup, true);
            
            nicknameInputField.text = previousName;
            
            Animations.MoveToTargetPosition();
            
            nicknameInputField.onValueChanged.AddListener((value) => CheckNickLeght(value, minCharInNick, selectNameButton));
            selectNameButton.onClick.AddListener(ConfirmName);
        }

        public void Hide()
        {
            Animations.MoveToEndPosition(() => ToggleVisibleCanvasGroup(CanvasGroup, false));
            
            nicknameInputField.onValueChanged.RemoveListener((value) => CheckNickLeght(value, minCharInNick, selectNameButton));
            selectNameButton.onClick.RemoveListener(ConfirmName);
        }

        

        private void ConfirmName()
        {
            Debug.Log(nicknameInputField.text);
            uiManager.OpenRoomBrowser(nicknameInputField.text);
            Hide();
        }
        
    }