using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Default")]
    public GameObject loadingPanel;
    [Header("Logins")]
    public GameObject loginPanel;
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public GameObject loginUsernameEmptyError;
    public GameObject loginPasswordEmptyError;
    public TMP_Text loginErrorText;

    [Header("Main")]
    public GameObject mainPanel;

    [Header("Registers")]
    public GameObject registerPanel;
    public TMP_InputField registerIdNameInput;
    public TMP_InputField registerUsernameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_Text registerErrorText;

    private void Awake()
    {
        Instance = this;
    }

    public void BtnLoginClick()
    {
        ClearError();
        bool _hasError = false;
        if(string.IsNullOrEmpty(loginUsernameInput.text))
        {
            loginUsernameEmptyError.SetActive(true);
            _hasError = true;
        }
        if (string.IsNullOrEmpty(loginPasswordInput.text))
        {
            loginUsernameEmptyError.SetActive(true);
            _hasError = true;
        }
        if (_hasError)
            return;
        loginErrorText.text = "";
        loadingPanel.SetActive(true);
        loginPanel.SetActive(false);
        AccountInfo _accountInfo = new() { username = loginUsernameInput.text, password = loginPasswordInput.text };
        LoginManager.Instance.LoginAccount(_accountInfo);
    }

    public void BtnRegisterClick()
    {
        if (string.IsNullOrEmpty(registerUsernameInput.text) || string.IsNullOrEmpty(registerIdNameInput.text) || string.IsNullOrEmpty(registerPasswordInput.text))
            return;
        registerErrorText.text = "";
        loadingPanel.SetActive(true);
        registerPanel.SetActive(false);
        AccountInfo _accountInfo = new() { idName = registerIdNameInput.text, username = registerUsernameInput.text, password = registerPasswordInput.text };
        LoginManager.Instance.RegisterAccount(_accountInfo);
    }

    private void OnEnable()
    {
        LoginManager.OnRegisterSuccess += LoginManager_OnRegisterSuccess;
        LoginManager.OnRegisterFailed += LoginManager_OnRegisterFailed;
        LoginManager.OnLoginSuccess += LoginManager_OnLoginSuccess;
        LoginManager.OnLoginFailed += LoginManager_OnLoginFailed;
    }

    private void LoginManager_OnLoginFailed(string _errorMessage)
    {
        loginErrorText.text = _errorMessage;
        loginPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    private void LoginManager_OnLoginSuccess(AccountInfo _accountInfo)
    {
        GameManager.Instance.username = _accountInfo.username;
        mainPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    private void OnDisable()
    {
        LoginManager.OnRegisterSuccess -= LoginManager_OnRegisterSuccess;
        LoginManager.OnRegisterFailed -= LoginManager_OnRegisterFailed;
    }

    private void LoginManager_OnRegisterFailed(string _errorMessage)
    {
        registerErrorText.text = _errorMessage;
        registerPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    private void LoginManager_OnRegisterSuccess(AccountInfo _accountInfo)
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
        loginUsernameInput.text = _accountInfo.username;
        loginPasswordInput.text = _accountInfo.password;
        loadingPanel.SetActive(false);
    }

    public void InitOnce()
    {
        loginPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    private void ClearError()
    {
        loginUsernameEmptyError.SetActive(false);
        loginPasswordEmptyError.SetActive(false);
    }
}
