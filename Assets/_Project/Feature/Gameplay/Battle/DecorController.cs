using UnityEngine;

public class DecorController : MonoBehaviour, IUpdateManager
{
    [SerializeField]
    protected GameObject bg1;

    [SerializeField]
    protected GameObject bg2;

    [SerializeField]
    protected GameObject bgCurrent;

    [SerializeField]
    protected GameObject bgNeedMove;

    [SerializeField]
    protected GameObject hero;

    [SerializeField]
    protected float bgHeight = 21f;

    protected void Start()
    {
        UpdateManager.instance.Register(this);
        hero = BattleController.instance.GetPlayer();
        bgCurrent = bg1;
        bgNeedMove = bg2;
    }

    protected void OnEnable()
    {
        if (UpdateManager.instance != null)
            UpdateManager.instance.Register(this);
    }

    protected void OnDisable()
    {
        if (UpdateManager.instance == null) return;
        UpdateManager.instance.UnRegister(this);
    }

    public void UpdateMe()
    {
        if (BattleController.instance == null) return;
        if (hero == null)
        {
            hero = BattleController.instance.GetPlayer();
            return;
        }

        float disCurrent = Mathf.Abs(hero.transform.position.y - bgCurrent.transform.position.y);
        if (disCurrent > 7)
        {
            // chuyển vị trí (chỉ 1 lần thôi)
            MoveBackground(bgNeedMove);
        }

        if (disCurrent > 22)
        {
            GameObject temp = bgCurrent;
            bgCurrent = bgNeedMove;
            bgNeedMove = temp;
        }
    }

    protected void MoveBackground(GameObject bgNeedMove)
    {
        float heroY = hero.transform.position.y;
        float targetY = bgCurrent.transform.position.y;

        // Hero đã sang nửa trên BG hiện tại
        if (heroY > targetY)
        {
            bgNeedMove.transform.position = new Vector3(
                bgCurrent.transform.position.x,
                targetY + (bgHeight),
                bgCurrent.transform.position.z);
        }

        // Hero đã sang nửa dưới BG hiện tại
        else if (heroY < targetY)
        {
            bgNeedMove.transform.position = new Vector3(
                bgCurrent.transform.position.x,
                targetY - (bgHeight),
                bgCurrent.transform.position.z);

        }
    }
}