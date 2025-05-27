namespace NetDream.Modules.Shop.Backend.Forms
{
    public class ShippingGroupForm
    {
        public int Id { get; set; }

        public int ShippingId { get; set; }

        public int IsAll { get; set; }

        public float FirstStep { get; set; }

        public float FirstFee { get; set; }
        public float Additional { get; set; }

        public float AdditionalFee { get; set; }

        public float FreeStep { get; set; }
        public RegionForm[]? Regions { get; set; }
    }
}