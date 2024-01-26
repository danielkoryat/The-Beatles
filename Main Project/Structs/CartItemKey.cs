using System.Text.Json.Serialization;

namespace Main_Project.Structs
{
    public struct CartItemKey
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }

        public CartItemKey(int itemId, string itemType)
        {
            ItemId = itemId;
            ItemType = itemType;
        }

        public bool Equals(CartItemKey other)
        {
            return ItemId == other.ItemId && ItemType == other.ItemType;
        }

        public override bool Equals(object obj)
        {
            return obj is CartItemKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ItemId * 397) ^ (ItemType != null ? ItemType.GetHashCode() : 0);
            }
        }
    }
}