using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    private enum ESkillType
    {
        Passive,

        Active
    }

    protected Entity Owner;

    [SerializeField]
    private float cooldownTime;

    [SerializeField]
    private ESkillType skillType;

    private float _cooldownTimer;

    protected bool _canUseSkill;

    protected void SetEntity(Entity entity)
    {
        Owner = entity;
    }

    public abstract void OnEquip(Entity entity);

    public abstract void UseSkill();

    public abstract void OnUnequip();

    protected virtual void UpdateCooldown()
    {
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = 0f;
                _canUseSkill = true;
            }
        }
    }

    protected virtual void BeginCooldown()
    {
        _canUseSkill = false;
        _cooldownTimer = cooldownTime;
    }


    public abstract void OnSkillComplete();
}