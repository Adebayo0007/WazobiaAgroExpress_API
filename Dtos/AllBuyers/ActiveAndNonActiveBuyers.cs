using AgroExpressAPI.Dtos.Buyer;

namespace AgroExpressAPI.Dtos.AllBuyers;
    public class ActiveAndNonActiveBuyers
    {
        public IEnumerable<BuyerDto> ActiveBuyers{get; set; }
        public IEnumerable<BuyerDto> NonActiveBuyers{get; set; }
    }
