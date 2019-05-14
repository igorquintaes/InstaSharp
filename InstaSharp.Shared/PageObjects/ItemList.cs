using InstaSharp.Shared.Enum;

namespace InstaSharp.Shared.PageObjects
{
    public class ItemList
    {
        public ItemList(
            string title, 
            string description, 
            string imageUrl, 
            FollowerStatus status)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            Status = status;
        }

        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public string ImageUrl { get; protected set; }
        public FollowerStatus Status { get; protected set; }
    }
}
