using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Firebase;

public class AuthManager : MonoBehaviour
{
    private static AuthManager instance = null;

    [SerializeField]
    InputField emailField;
    [SerializeField]
    InputField passField;

    public bool isLogin = false;

    Firebase.Auth.FirebaseAuth auth;    //������ ������ ��ü

    public static FirebaseUser user;
    private void Awake()
    {
        //�ʱ�ȭ
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static AuthManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    public void Login()
    {
        // �����Ǵ� �Լ� : �̸��ϰ� ��й�ȣ�� �α���
        auth.SignInWithEmailAndPasswordAsync(emailField.text, passField.text).ContinueWith(
            (task) =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(emailField.text + " �� �α��� �ϼ̽��ϴ�.");
                    isLogin = true;
                    //SceneManager.LoadScene("Lobby");
                }
                else
                {
                    isLogin = false;
                    Debug.Log("�α��ο� �����ϼ̽��ϴ�.");
                }
            });
        this.ObserveEveryValueChanged(x => x.isLogin).Subscribe(x =>
            {
                SceneManager.LoadScene("Lobby");
                isLogin = false;
            });
    }
    public void Register()
    {
        //�����Ǵ� �Լ� :  �̸��ϰ� ��й�ȣ�� ȸ������ 
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passField.text).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log(emailField.text + "�� ȸ������");
                }
                else
                {
                    Debug.Log("ȸ������ ����");
                }
            });
    }
}
