using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;  //타워 생성을 위한 프리팹
    public GameObject followTowerPrefab;    //임시 타워 프리팹
    public Weapon[] weapon; //레벨업 타워(무기)정보

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;   //보여지는 타워 이미지
        public float damage;
        public float slow;  //감속 퍼센트(0.2=20%)
        public float buff; //공격력 증가율 (0.2 = +20%)
        public float rate;
        public float range;
        public int cost;    // 필요 골드(0레벨 : 건설, 1레벨 : 업그레이드)
        public int sell;    //타워 판매 시 획득 골드
    }
}
