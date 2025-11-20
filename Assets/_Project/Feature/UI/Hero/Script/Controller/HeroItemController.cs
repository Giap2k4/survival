using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemController : MonoBehaviour
{
    [SerializeField]
    protected Image spriteHero;

    [SerializeField]
    protected TextMeshProUGUI txtNameHero;

    [SerializeField]
    protected GameObject isSelected;

    [SerializeField]
    protected GameObject isLock;

    [SerializeField]
    protected TextMeshProUGUI txtPriceHero;

    // type resources
    [SerializeField]
    protected Image resourceType;

    public void InitData(HeroModel hero)
    {
        spriteHero.sprite = Resources.Load<Sprite>("Hero/" + hero.id);
        txtNameHero.text = hero.nameHero;

        if (HeroManager.ListHeroOwned().TryGetValue(hero.id, out var value))
        {
            isLock.SetActive(false);
        }
        else
        {
            txtPriceHero.text = hero.resource.resQuantity.ToString();
        }

        resourceType.sprite = Resources.Load<Sprite>("Money/" + hero.resource.resId);
        txtPriceHero.text = hero.resource.resQuantity.ToString();

        if (hero.id == HeroManager.SelectedHero()) isSelected.SetActive(true);
    }
}
