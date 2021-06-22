namespace Karia.Api.Models
{
    public class ConfirmVerificationCodeDto
    {
        public string PhoneNumber { get; set; }
        public int Code { get; set; }
    }
}