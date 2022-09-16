using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;
    public static event Action<string> OnRegisterFailed;
    public static event Action<AccountInfo> OnRegisterSuccess;

    public static event Action<string> OnLoginFailed;
    public static event Action<AccountInfo> OnLoginSuccess;

    private void Awake()
    {
        Instance = this;
    }

    #region Registers
    public void RegisterAccount(AccountInfo _accountInfo)
    {
        StartCoroutine(RegisterAsync(_accountInfo));
    }

    private IEnumerator RegisterAsync(AccountInfo _accountInfo)
    {
        DatabaseReference _DBReference = FirebaseDatabase.DefaultInstance.RootReference;
        var _accountTask = _DBReference.Child("accounts").Child(_accountInfo.username.ToUpper()).GetValueAsync();
        yield return new WaitUntil(predicate: () => _accountTask.IsCompleted);
        if(_accountTask.Exception != null)
        {
            OnRegisterFailed?.Invoke("Internal server error, please try again later.");
            yield break;
        }
        if(_accountTask.Result.Value != null)
        {
            OnRegisterFailed?.Invoke("This username has been taken!");
            yield break;
        }
        var _registerTask = _DBReference.Child("accounts").Child(_accountInfo.username.ToUpper())
            .SetRawJsonValueAsync(JsonUtility.ToJson(new AccountInfo() { idName = _accountInfo.idName, password = _accountInfo.password, username = _accountInfo.username } ));
        yield return new WaitUntil(() => _registerTask.IsCompleted);
        OnRegisterSuccess?.Invoke(_accountInfo);
    }
    #endregion

    #region Logins
    public void LoginAccount(AccountInfo _accountInfo)
    {
        StartCoroutine(LoginAsync(_accountInfo));
    }

    private IEnumerator LoginAsync(AccountInfo _accountInfo)
    {
        DatabaseReference _DBReference = FirebaseDatabase.DefaultInstance.RootReference;
        var _accountTask = _DBReference.Child("accounts").Child(_accountInfo.username.ToUpper()).GetValueAsync();
        yield return new WaitUntil(() => _accountTask.IsCompleted);
        if (_accountTask.Exception != null)
        {
            OnLoginFailed?.Invoke("Internal server error, please try again later.");
            yield break;
        }
        if (_accountTask.Result.Value == null)
        {
            OnLoginFailed?.Invoke("This account has not been registered yet");
            yield break;
        }
        OnLoginSuccess?.Invoke(_accountInfo);
    }
    #endregion
}
