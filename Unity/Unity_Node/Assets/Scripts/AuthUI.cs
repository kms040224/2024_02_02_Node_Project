using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;

    public Button registerButton;
    public Button loginButton;

    public Text statusText;

    public AuthManager authManager;

    void Start()
    {
        authManager = GetComponent<AuthManager>();
        registerButton.onClick.AddListener(OnRegisterClick);
        loginButton.onClick.AddListener(OnLoginClick);

    }

    private void OnLoginClick()
    {
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        statusText.text = "�α��� ��...";
        yield return StartCoroutine(authManager.Login(usernameInput.text, passwordInput.text));
    }
    private void OnRegisterClick()
    {
        StartCoroutine(RegisterCoroutine());
    }

    private IEnumerator RegisterCoroutine()
    {
        statusText.text = "ȸ��������...";
        //yield return StartCoroutine(authManager.Re)
        yield return StartCoroutine(authManager.Register(usernameInput.text, passwordInput.text));
    }

    //����� ��� �ڷ�ƾ
    

    
}
