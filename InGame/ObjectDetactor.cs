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
    private Transform hitTransform = null;  //���콺 Ŭ������ ������ ������Ʈ �ӽ� ���� 

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        //���콺�� UI�� �ӹ��� ���� ���� �Ʒ� �ڵ尡 ������� �ʵ��� ��
        if(EventSystem.current.IsPointerOverGameObject()==true)
        {
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            //ray.origin : ������ ������ġ(ī�޶� ��ġ)
            //ray.direction : ������ �������
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                if(hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnerTower(hit.transform);
                }
                //Ÿ���� �����ϸ� �ش� Ÿ�� ������ ����ϴ� Ÿ�� ����â On
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
