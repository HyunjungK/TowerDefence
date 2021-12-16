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

    Firebase.Auth.FirebaseAuth auth;    //인증을 관리할 객체

    public static FirebaseUser user;
    private void Awake()
    {
        //초기화
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
        // 제공되는 함수 : 이메일과 비밀번호로 로그인
        auth.SignInWithEmailAndPasswordAsync(emailField.text, passField.text).ContinueWith(
            (task) =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log(emailField.text + " 로 로그인 하셨습니다.");
                    isLogin = true;
                    //SceneManager.LoadScene("Lobby");
                }
                else
                {
                    isLogin = false;
                    Debug.Log("로그인에 실패하셨습니다.");
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
        //제공되는 함수 :  이메일과 비밀번호로 회원가입 
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passField.text).ContinueWith(
            task =>
            {
                if (!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log(emailField.text + "로 회원가입");
                }
                else
                {
                    Debug.Log("회원가입 실패");
                }
            });
    }
}
