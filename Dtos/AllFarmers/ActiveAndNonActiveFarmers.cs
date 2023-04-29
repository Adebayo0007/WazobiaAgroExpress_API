using AgroExpressAPI.Dtos.Farmer;

namespace AgroExpressAPI.Dtos.AllFarmers;
    public class ActiveAndNonActiveFarmers
    {
        public IEnumerable<FarmerDto> ActiveFarmers{get; set; }
        public IEnumerable<FarmerDto> NonActiveFarmers{get; set; }
    }
