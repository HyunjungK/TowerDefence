using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ObjectDetactor : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;  //마우스 클릭으로 선택한 오브젝트 임시 저장 

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        //마우스가 UI에 머물러 있을 때는 아래 코드가 실행되지 않도록 함
        if(EventSystem.current.IsPointerOverGameObject()==true)
        {
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            //ray.origin : 광선의 시작위치(카메라 위치)
            //ray.direction : 광선의 진행방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                if(hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnerTower(hit.transform);
                }
                //타워를 선택하면 해당 타워 정보를 출력하는 타워 정보창 On
                else if(hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(hitTransform==null || hitTransform.CompareTag("Tower") ==false)
            {
                towerDataViewer.OffPanel();
            }
            hitTransform = null;
        }
    }

}
