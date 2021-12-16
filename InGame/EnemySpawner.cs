using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab;
    [SerializeField]
    private GameObject enemyHPSliderPrefab; //�� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;  //UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField]
    //private float spawnTime;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    private Wave currentWave;   //���� ���̺� ����
    private int currentEnemyCount;  //���� ���̺꿡 �����ִ� �� ����(���̺� ���۽� max �� ����, �� ��� �� -1)
    private List<Enemy> enemyList;  //���� �ʿ� �����ϴ� ��� ���� ����
    public List<Enemy> EnemyList => enemyList;
    //���� ���̺��� �����ִ� ��, �ִ� �� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;
    private void Awake()
    {
        enemyList = new List<Enemy>();
        //�� ���� �ڷ�ƾ �Լ� ȣ��
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
        //���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;
        while(spawnEnemyCount<currentWave.maxEnemyCount)
        {
            //���̺꿡 �����ϴ� ���� ������ ���� ������ �� ������ ���� �����ϵ��� �����ϰ�, �� ������Ʈ ����
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemy.Setup(this, wayPoints);   //wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy);   //����Ʈ�� ��� ������ �� ���� ����

            SpawnEnemyHPSlider(clone);  //�� ü���� ��Ÿ���� Slider UI ���� �� ����

            //��ü ���̺꿡�� ������ ���� ���� +1
            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        //���� ��ǥ�������� �������� ��
        if(type==EnemyDestroyType.Arrive)
        {
            //�÷��̾��� ü�� ����
            playerHP.TakeDamage(1);
        }
        else if(type==EnemyDestroyType.Kill)
        {
            playerGold.CurrentGold += gold;
        }
        //���� ����� �� ���� ���� ���̺��� ���� �� ���� ����(UI ǥ�ÿ�)
        currentEnemyCount--;
        //����Ʈ���� ����ϴ� �� ���� ����
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
