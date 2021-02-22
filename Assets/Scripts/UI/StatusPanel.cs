using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI unitName;
    [SerializeField] public TextMeshProUGUI unitDescription;
    [SerializeField] public TextMeshProUGUI unitDamage;
    [SerializeField] public Image unitcurrHealth;
    [SerializeField] public Image unitImage;
    [SerializeField] public Image[] unitSkill = new Image[3];

    public void SetStatusForUnit(Unit unit)
    {
        gameObject.SetActive(true);

        unitName.text = unit.name;
        unitDescription.text = "Description For Unit.";
        unitDamage.text = $"{unit.basicAttackDamageMin.ToString()} ~ {unit.basicAttackDamageMax.ToString()}";
        unitcurrHealth.fillAmount = unit.Health / unit.maxHealth;
        unitImage.sprite = unit.icon;
        //for(int i=0; i < 3; i++)
        //{
         //   unitSkill[i].sprite = unit.skill.icon;
        //}

        // -> skill은 아직 이미지가 없어서~
    }

    public void UnsetStatus()
    {
        gameObject.SetActive(false);
    }
}
