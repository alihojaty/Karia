using System;
using System.Threading.Tasks;

namespace Karia.Api.Helpers
{
    public class SendSms
    {

        public static async Task<bool> Send(string phoneNumber, string code)
        {
            try {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("6B6A37386B6D32513162376732644863416A76645A5976776971676C45514A43613459427339376E4B6A6F3D");
                var result =await api.Send("10008663", phoneNumber, $"کد فعال سازی : {code}");

                return true;

            } catch (Exception ex) {
                
                return false;

            } 
        }
    }
}
