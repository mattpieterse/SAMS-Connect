using Connect.Data.Models;

namespace Connect.UI.Models;

public sealed class ForumBroadcastWrapper(
    IForumBroadcast instance,
    bool isRecommended
)
{
    public IForumBroadcast Instance => instance;
    public bool IsRecommended => isRecommended;

    public string CategoryFormatted => Instance.CategoryFormatted;
    public string CreatedAtFormatted => Instance.CreatedAtFormatted;
}
