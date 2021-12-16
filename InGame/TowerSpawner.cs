using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;  //타워 건설 시 골드 감소
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;   //타워 건설 버튼을 눌렀는지 체크
    private GameObject followTowerClone = null; //임시 타워 사용 완료 시 삭제를 위해 저장하는 변수
    private int towerType;

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        //버튼을 중복해서 누르는 것을 방지하기 위해 필요
        if(isOnTowerButton)
        {
            return;
        }
        //타워 건설 가능 여부 확인
        //타워를 건설할 만큼 돈이 없으면 타워 건설 x
        if(towerTemplate[towerType].weapon[0].cost>playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        isOnTowerButton = true;
        //마우스를 따라다니는 임시 타워 생성
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        //타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnerTower(Transform tileTransform)
    {
        //타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if(isOnTowerButton==false)
        {
            return;
        }
        //if (towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        //{
        //    systemTextViewer.PrintText(SystemType.Money);
        //    return;
        //}
        Tile tile = tileTransform.GetComponent<Tile>();

        //타워 건설 가능 여부 확인
        if (tile.IsBuildTower)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        isOnTowerButton = false;
        tile.IsBuildTower = true;
        //  playerGold.CurrentGold -= towerBuildGold;
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // 선택한 타일 위치에 타워 건설(타일보다 z축 +1의 위치에 배치)
        Vector3 position = tileTransform.position + Vector3.back;
        //  GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        clone.GetComponent<TowerWeapon>().Setup(this, enemySpawner, playerGold, tile);

        //새로 배치되는 타워가 버프 타워 주변에 배치될 경우
        //버프 효과를 받을 수 있도록 모든 버프 타워의 버프 효과 갱신
        OnBuffAllBuffTowers();

        //타워를 배치했기 때문에 마우스를 따라다니는 임시 타워 삭제
        Destroy(followTowerClone);
        //타워 건설을 취소할 수 있는 코루틴 함수 중지
        StopCoroutine("OnTowerCancelSystem");
    }
    private IEnumerator OnTowerCancelSystem()
    {
        while(true)
        {
            //ESC키 또는 마우스 오른쪽 버튼을 눌렀을 때 타워 건설 취소
            if(Input.GetKeyDown(KeyCode.Escape)||Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;

                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }
    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i=0; i<towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if(weapon.WeaponType==WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
