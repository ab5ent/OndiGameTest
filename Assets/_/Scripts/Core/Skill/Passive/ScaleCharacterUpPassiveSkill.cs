using UnityEngine;

public class ScaleCharacterUpPassiveSkill : Skill
{
    [SerializeField] private float _scale;

    [SerializeField] private float _duration;

    private float timer;

    private bool isScaled = false;

    public override void OnEquip(Entity entity)
    {
        SetEntity(entity);
        isScaled = false;
    }

    public override void UseSkill()
    {
    }

    public override void OnUnequip()
    {
        SetEntity(null);
        Destroy(gameObject);
    }

    protected void Update()
    {
        if (!Owner)
        {
            return;
        }

        if (!isScaled)
        {
            Owner.transform.localScale = Vector3.Lerp(Owner.transform.localScale, Vector3.one * _scale, timer / 2f);
            timer += Time.deltaTime;

            if (timer >= 0.5f)
            {
                isScaled = true;
                timer = 0;
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= _duration)
            {
                OnSkillComplete();
            }
        }
    }

    public override void OnSkillComplete()
    {
        isScaled = false;
        timer = 0;

        Owner.transform.localScale = Vector3.one;
        OnUnequip();
    }
}