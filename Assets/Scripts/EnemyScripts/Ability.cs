public class Ability {
    public AbilityType abilityType { get; private set; }
    public float cooldown { get; private set; }
    public float duration { get; private set; }
    private float maxDuration;
    private float maxCooldown;

    public Ability(AbilityType abilityType, float duration, float cooldown) {
        this.abilityType = abilityType;
        this.maxDuration = duration;
        this.maxCooldown = cooldown;
        this.duration = 0;
        this.cooldown = 0;
    }

    public void startAbility() {
        duration = maxDuration;
        cooldown = maxCooldown;
    }

    public void updateAbility(float time) {
        if (duration > 0) {
            duration -= time;
        } else if (cooldown > 0) {
            cooldown -= time;
        }
    }

    public bool isReady() {
        return cooldown <= 0;
    }

    // command functions to directly change the timers
    public void updateCooldown(float time) {
        cooldown -= time;
    }

    public void updateDuration(float time) {
        duration -= time;
    }
}