using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroManager : DataPlayer<HeroData>
{
    public static void SelectHero(int idHero)
    {
        PlayerDataManager.Hero.database.idHeroSelected = idHero;
        PlayerDataManager.Hero.Save();
    }

    public static int SelectedHero() => PlayerDataManager.Hero.database.idHeroSelected;

    public static void AddHero(int idHero)
    {
        if (!PlayerDataManager.Hero.database.idHeroOwned.ContainsKey(idHero))
        {
            PlayerDataManager.Hero.database.idHeroOwned.Add(idHero, 1);
            PlayerDataManager.Hero.Save();
        }
    }

    public static Dictionary<int, int> ListHeroOwned()
    {
        return PlayerDataManager.Hero.database.idHeroOwned;
    }

    public override void SetDataDefault()
    {
        base.SetDataDefault();
        foreach (var item in DataManager.Resource.GetAll())
        {
            if (item.package.resType != EnumBase.ResourcesType.Hero) continue;

            var data = PlayerDataManager.Hero.database;

            if (!data.idHeroOwned.TryGetValue(item.package.resId, out var value))
            {
                data.idHeroOwned.Add(item.package.resId, 1);
                data.idHeroSelected = item.package.resId;
            }
        }
        PlayerDataManager.Hero.Save();
    }
}
