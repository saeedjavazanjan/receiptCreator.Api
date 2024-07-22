namespace ReceiptCreator.Api.KavehNegar;

public static class SendSms
{
        public static async Task<String> SendSMSToUser(String token, String receptor)
        {

            try
            {

                String apiKey =
                    "";
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var result = await api.VerifyLookup(receptor, token, "Jobs-receipt-creator-verify");
                foreach (var r in result.Message)
                {
                    Console.Write(r + "r.Messageid.ToString()");
                }

                return "Ok";
            }
            catch (Exception ex)
            {
                Console.Write("Message : " + ex.Message);
                return ex.Message;
            }
        }
    

}
