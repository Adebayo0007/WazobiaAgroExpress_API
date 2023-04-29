namespace AgroExpressAPI.Dtos.RequestedProduct;
    public class OrderedRequestAndPendingRequest
    {
        public IEnumerable<RequestedProductDto> OrderedProduct{get; set; }
        public IEnumerable<RequestedProductDto> PendingProduct{get; set; }
    }
