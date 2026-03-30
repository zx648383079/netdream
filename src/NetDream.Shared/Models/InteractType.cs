namespace NetDream.Shared.Models
{
    public enum InteractType: byte
    {
        None = 0,
        Agree,
        Disagree,
        Collect,
        Like,
        Dislike,
        Bought
    }

    public enum RecordToggleType : byte
    {
        Deleted,
        Updated,
        Added,
    }
}
