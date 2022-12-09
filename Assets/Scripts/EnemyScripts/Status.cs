public class Status
{
    public StatusType statusType { get; private set; }
    public float duration { get; private set; }
    public float damage { get; private set; }

    public Status(StatusType statusType, float duration)
    {
        this.statusType = statusType;
        this.duration = duration;
        this.damage = 0;
    }

    public Status(StatusType statusType, float duration, float damage)
    {
        this.statusType = statusType;
        this.duration = duration;
        this.damage = damage;
    }

    public void updateDuration(float time)
    {
        duration -= time;
    }
}