using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] public string itemCode;
    [SerializeField] public string itemName;
    [SerializeField] public ItemStaticData.ItemType itemType;
    [SerializeField] public Sprite itemImage;

    public Range useRange;
    public Range useSplash;

    protected virtual void OnEnable()
    {
        SetRange();
    }

    /// <summary>
    /// basicAttackRange, basicAttackSplash 두 변수를 꼭 유닛별로 초기화해주세요.
    /// </summary>
    protected abstract void SetRange();

    public abstract void Use(Unit user, List<Cube> cubesToUseOn);

    /// <summary>
    /// Range안의 Cube들을 받아서 그 Cube들중 Item을 사용가능한 Cube를 반환합니다.
    /// 이는 Item마다 조건이 다를테니 따로 구현합니다.
    /// </summary>
    public abstract List<Cube> RangeExtraCondition(List<Cube> cubesInRange);
    
}
