public class Status
{
    public StatusType statusType { get; private set; }
    public float duration { get; private set; }

    public Status(StatusType statusType, float duration)
    {
        this.statusType = statusType;
        this.duration = duration;
    }

    public void updateDuration(float time)
    {
        duration -= time;
    }
}