namespace ReceiptCreator.Api.KavehNegar;

public class SendSms
{
    public static class SendSMS
    {

        public static async Task<String> SendSMSToUser(String token, String receptor)
        {

            try
            {

                String apiKey =
                    "31586633704961526C6966716F6365766C3151522F7873466D5054577261724B6A6930716B615876334C493D";
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var result = await api.VerifyLookup(receptor, token, "Jobs-receipt-creator-verify");
                foreach (var r in result.Message)
                {
                    Console.Write(r + "r.Messageid.ToString()");
                }

                return "ارسال موفق";
            }
            catch (Exception ex)
            {
                Console.Write("Message : " + ex.Message);
                return ex.Message;
            }
        }
    }

}