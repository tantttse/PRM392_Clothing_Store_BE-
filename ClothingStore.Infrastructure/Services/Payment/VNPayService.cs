using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Payment;
using ClothingStore.Application.Features.Carts.Dtos;
using ClothingStore.Application.Features.Payments.Dtos;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Configs.Payment;
using Shared.Infrastructure.Configs.Payments;

namespace Infrastructure.Payments
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPayConfig _vnPay;

        public VnPayService(IOptions<VnPayConfig> options)
        {
            _vnPay = options.Value;
        }

        public string CreatePaymentUrl(CreatePaymentDto dto)
        {
            var vnp_Url = _vnPay.Url;
            var vnp_TmnCode = _vnPay.TmnCode;
            var vnp_HashSecret = _vnPay.HashSecret;
            var vnp_ReturnUrl = _vnPay.ReturnUrl; 

            var query = new SortedDictionary<string, string>
            {
                ["vnp_Version"] = "2.1.0",
                ["vnp_Command"] = "pay",
                ["vnp_TmnCode"] = vnp_TmnCode,
                ["vnp_Amount"] = ((int)(dto.Amount * 100)).ToString(),
                ["vnp_CreateDate"] = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                ["vnp_CurrCode"] = "VND",
                ["vnp_IpAddr"] = dto.IpAddress,
                ["vnp_Locale"] = "vn",
                ["vnp_OrderInfo"] = $"Thanh toan don hang {dto.CartId}",
                ["vnp_OrderType"] = "other",
                ["vnp_ReturnUrl"] = vnp_ReturnUrl,
                ["vnp_TxnRef"] = Guid.NewGuid().ToString()
            };

            var rawData = string.Join("&", query.Select(x => $"{x.Key}={x.Value}"));
            var secureHash = HmacSHA512(vnp_HashSecret, rawData);
            return $"{vnp_Url}?{rawData}&vnp_SecureHash={secureHash}";
        }

        public VnPayResponseDto ValidateResponse(IDictionary<string, string> queryParams)
        {
            var vnp_HashSecret = _vnPay.HashSecret;
            var receivedHash = queryParams["vnp_SecureHash"];
            queryParams.Remove("vnp_SecureHash");
            queryParams.Remove("vnp_SecureHashType");

            var rawData = string.Join("&", queryParams.OrderBy(x => x.Key)
                .Select(x => $"{x.Key}={x.Value}"));

            var computedHash = HmacSHA512(vnp_HashSecret, rawData);
            var isValid = computedHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase);
            var responseCode = queryParams.ContainsKey("vnp_ResponseCode") ? queryParams["vnp_ResponseCode"] : "";

            return new VnPayResponseDto
            {
                Success = isValid && responseCode == "00",
                ResponseCode = responseCode,
                TransactionNo = queryParams.ContainsKey("vnp_TransactionNo") ? queryParams["vnp_TransactionNo"] : null,
                Message = isValid ? "Payment verified" : "Invalid hash"
            };
        }

        private static string HmacSHA512(string key, string input)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }
    }
}
