using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyHPSliderPrefab; //적 체력을 나타내는 Slider UI 프리팹
    [SerializeField]
    private Transform canvasTransform;  //UI를 표현하는 Canvas 오브젝트의 Transform
    //[SerializeField]
    //private float spawnTime;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    private Wave currentWave;   //현재 웨이브 정보
    private int currentEnemyCount;  //현재 웨이브에 남아있는 적 숫자(웨이브 시작시 max 로 설정, 적 사망 시 -1)
    private List<Enemy> enemyList;  //현재 맵에 존재하는 모든 적의 정보
    public List<Enemy> EnemyList => enemyList;
    //현재 웨이브의 남아있는 적, 최대 적 숫자
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;
    private void Awake()
    {
        enemyList = new List<Enemy>();
        //적 생성 코루틴 함수 호출
        //StartCoroutine("SpawnEnemy");
    }
    public void StartWave(Wave wave)
    {
        currentWave = wave;
        currentEnemyCount = currentWave.maxEnemyCount;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        //현재 웨이브에서 생성한 적 숫자
        int spawnEnemyCount = 0;
        while(spawnEnemyCount<currentWave.maxEnemyCount)
        {
            //웨이브에 등장하는 적의 종류가 여러 종류일 때 임의의 적이 등장하도록 설정하고, 적 오브젝트 생성
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemy.Setup(this, wayPoints);   //wayPoint 정보를 매개변수로 Setup() 호출
            enemyList.Add(enemy);   //리스트에 방금 생성된 적 정보 저장

            SpawnEnemyHPSlider(clone);  //적 체력을 나타내는 Slider UI 생성 및 설정

            //전체 웨이브에서 생선한 적의 숫자 +1
            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        //적이 목표지점까지 도착했을 때
        if(type==EnemyDestroyType.Arrive)
        {
            //플레이어의 체력 감소
            playerHP.TakeDamage(1);
        }
        else if(type==EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += gold;
        }
        //적이 사망할 때 마다 현재 웨이브의 생존 적 숫자 감소(UI 표시용)
        currentEnemyCount--;
        //리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
