using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine.UI;
public class LobbyButtonManager : MonoBehaviour
{
    public List<TextMeshProUGUI> texts;
    public CanvasGroup towerManagerCanvas;

    private void Start()
    {
        towerManagerCanvas.alpha = 0;
        towerManagerCanvas.interactable = false;
        towerManagerCanvas.blocksRaycasts = false;

        texts.Where(_ => _.name == "LevelText").SingleOrDefault().text = GameManager.Instance.level.ToString();
        texts.Where(_ => _.name == "GoldText").SingleOrDefault().text = GameManager.Instance.money.ToString();

        GameManager.Instance.ObserveEveryValueChanged(x => x.level).Subscribe(
            x => texts.Where(_ => _.name == "LevelText").SingleOrDefault().text
            = GameManager.Instance.level.ToString());
        GameManager.Instance.ObserveEveryValueChanged(x => x.money).Subscribe(
            x => texts.Where(_ => _.name == "GoldText").SingleOrDefault().text
            = GameManager.Instance.money.ToString());
    }

    public void OnClickStartBattle()
    {
        SceneManager.LoadScene("InGame");
    }
    public void OnClickTowerManager()
    {
        towerManagerCanvas.alpha = 1;
        towerManagerCanvas.interactable = true;
        towerManagerCanvas.blocksRaycasts = true;
    }
    public void OnClickCannonUpgrade()
    {

    }
    public void OnClickLaserUpgrade()
    {
        
    }
    public void OnClickDebuffUpgrade()
    {

    }
    public void OnClickBuffUpgrade()
    {

    }
    public void OnClickBackButton()
    {
        towerManagerCanvas.alpha = 0;
        towerManagerCanvas.interactable = false;
        towerManagerCanvas.blocksRaycasts = false;
    }
}
