using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class TitleButtonManager : MonoBehaviour
{
    public GameObject dim;
    public GameObject popupLogin;
    private void Start()
    {
        dim.SetActive(false);
        popupLogin.SetActive(false);

        //AuthManager.Instance.ObserveEveryValueChanged(x=>x.isLogin).Subscribe(
        //    x=> SceneManager.LoadScene("Lobby"));
    }
    public void OnClickTitle()
    {
        dim.SetActive(true);
        popupLogin.SetActive(true);
    }
}
