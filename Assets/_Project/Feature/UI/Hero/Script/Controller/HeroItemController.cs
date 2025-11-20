using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemController : MonoBehaviour
{
    public static event Action<HeroItemController> OnAnyClicked;
    public static event Action<HeroModel> fieldHero;

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

    [SerializeField]
    protected Button btnClick;

    [SerializeField]
    protected GameObject spriteClick;

    public HeroModel heroModel;

    private void Start()
    {
        btnClick.onClick.AddListener(OnClick);
    }

    protected void OnClick()
    {
        if (OnAnyClicked != null) OnAnyClicked(this);
        if (fieldHero != null) fieldHero(heroModel);
    }

    private void OnEnable()
    {
        OnAnyClicked += HandleAnyClicked;
    }

    private void OnDisable()
    {
        OnAnyClicked -= HandleAnyClicked;
    }

    protected void HandleAnyClicked(HeroItemController clicked)
    {
        bool active = (clicked == this);
        if (active)
        {
            spriteClick.SetActive(true);
        } else
        {
            spriteClick.SetActive(false);
        }
    }

    public void InitData(HeroModel hero)
    {
        heroModel = hero;
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
