using System;


namespace Providers
{
    public class Payment
    {
        //private static readonly string PgwSite = "https://bpm.shaparak.ir/pgwchannel/startpay.mellat";
        private static readonly string callBackUrl = "http://localhost:49679/BankCallback";
        private static readonly long terminalId = long.Parse("111111");
        private static readonly string userName = "ABLY.IR";
        private static readonly string password = "111111";
        string localDate = string.Empty;
        string localTime = string.Empty;


        public Payment()
        {
            try
            {
                localDate = DateTime.Now.ToString("yyyyMMdd");
                localTime = DateTime.Now.ToString("HHMMSS");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        // *** Pay ***
        public string[] Pay(long orderID, long priceAmount, string additionalText = null)
        {
            try
            {
                string resultRequest = bpPayRequest(orderID, priceAmount, additionalText);
                string[] StatusSendRequest = resultRequest.Split(',');
                if (int.Parse(StatusSendRequest[0]) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                {
                    return new string[] { "Success", StatusSendRequest[1] };
                }
                return new string[] { null, MellatResult(StatusSendRequest[0]) };
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        // bpPayRequest
        private string bpPayRequest(long orderId, long priceAmount, string additionalText)
        {
            try
            {
                Health.MellatWebService.PaymentGatewayImplService WebService = new Health.MellatWebService.PaymentGatewayImplService();
                return WebService.bpPayRequest(terminalId, userName, password, orderId, priceAmount, localDate, localTime,
                                 additionalText, callBackUrl, 0);

            }
            catch (Exception error)
            {
                throw new Exception(error.Message); ;
            }
        }


        // VerifyRequest
        private string VerifyRequest(long orderId, long saleOrderId, long saleReferenceId)
        {
            try
            {
                Health.MellatWebService.PaymentGatewayImplService WebService = new Health.MellatWebService.PaymentGatewayImplService();
                return WebService.bpVerifyRequest(terminalId, userName, password, orderId, saleOrderId, saleReferenceId);

            }
            catch (Exception Error)
            {
                throw new Exception(Error.Message);
            }
        }


        // SettleRequest
        private string SettleRequest(long orderId, long saleOrderId, long saleReferenceId)
        {
            try
            {
                Health.MellatWebService.PaymentGatewayImplService WebService = new Health.MellatWebService.PaymentGatewayImplService();
                return WebService.bpSettleRequest(terminalId, userName, password, orderId, saleOrderId, saleReferenceId);
            }
            catch (Exception Error)
            {
                throw new Exception(Error.Message);
            }
        }


        // InquiryRequest
        private string InquiryRequest(long orderId, long saleOrderId, long saleReferenceId)
        {
            try
            {
                Health.MellatWebService.PaymentGatewayImplService WebService = new Health.MellatWebService.PaymentGatewayImplService();
                return WebService.bpInquiryRequest(terminalId, userName, password, orderId, saleOrderId, saleReferenceId);

            }
            catch (Exception Error)
            {
                throw new Exception(Error.Message);
            }
        }


        // bpReversalRequest
        private string bpReversalRequest(long orderId, long saleOrderId, long saleReferenceId)
        {
            try
            {
                Health.MellatWebService.PaymentGatewayImplService WebService = new Health.MellatWebService.PaymentGatewayImplService();
                return WebService.bpReversalRequest(terminalId, userName, password, orderId, saleOrderId, saleReferenceId);

            }
            catch (Exception error)
            {
                throw new Exception(error.Message); ;
            }
        }


        // BankCallback
        public string CallBack(string ResCode, long saleOrderId = -999, long saleReferenceId = -999)
        {
            bool Run_bpReversalRequest = false;
            string resultCode_bpPayRequest = ResCode;            

            try
            {
                string resultCode_bpinquiryRequest = "-9999";
                string resultCode_bpSettleRequest = "-9999";
                string resultCode_bpVerifyRequest = "-9999";

                if (int.Parse(resultCode_bpPayRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                {
                    #region Success

                    resultCode_bpVerifyRequest = VerifyRequest(saleOrderId, saleOrderId, saleReferenceId);

                    if (string.IsNullOrEmpty(resultCode_bpVerifyRequest))
                    {
                        #region Inquiry Request

                        resultCode_bpinquiryRequest = InquiryRequest(saleOrderId, saleOrderId, saleReferenceId);
                        if (int.Parse(resultCode_bpinquiryRequest) != (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                        {
                            //the transactrion faild
                            Run_bpReversalRequest = true;
                            return DesribtionStatusCode(int.Parse(resultCode_bpinquiryRequest.Replace("_", " ")));
                        }

                        #endregion
                    }

                    if ((int.Parse(resultCode_bpVerifyRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                        ||
                        (int.Parse(resultCode_bpinquiryRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ))
                    {

                        #region SettleRequest

                        resultCode_bpSettleRequest = SettleRequest(saleOrderId, saleOrderId, saleReferenceId);
                        if ((int.Parse(resultCode_bpSettleRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ)
                            || (int.Parse(resultCode_bpSettleRequest) == (int)MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ))
                        {
                            return "تراکنش شما با موفقیت انجام شد " + " لطفا شماره پیگیری را یادداشت نمایید" + saleReferenceId;
                        }
                        else
                        {
                            Run_bpReversalRequest = true;
                            return DesribtionStatusCode(int.Parse(resultCode_bpSettleRequest.Replace("_", " ")));                            
                        }

                        // Save information to Database...

                        #endregion
                    }
                    else
                    {
                        Run_bpReversalRequest = true;
                        return DesribtionStatusCode(int.Parse(resultCode_bpVerifyRequest.Replace("_", " ")));                        
                    }

                    #endregion
                }
                else
                {
                    Run_bpReversalRequest = true;
                    return MellatResult(resultCode_bpPayRequest);
                }
            }
            catch (Exception)
            {
                Run_bpReversalRequest = true;
                throw new Exception("متاسفانه خطایی رخ داده است، لطفا مجددا عملیات خود را انجام دهید در صورت تکرار این مشکل را به بخش پشتیبانی اطلاع دهید");
                //return "متاسفانه خطایی رخ داده است، لطفا مجددا عملیات خود را انجام دهید در صورت تکرار این مشکل را به بخش پشتیبانی اطلاع دهید";
                // Save and send Error for admin user
            }
            finally
            {
                if (Run_bpReversalRequest) //ReversalRequest
                {
                    if (saleOrderId != -999 && saleReferenceId != -999)
                        bpReversalRequest(saleOrderId, saleOrderId, saleReferenceId);
                    // Save information to Database...
                }
            }
        }


        // MellatBankReturnCode
        private enum MellatBankReturnCode
        {
            ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ = 0,
            ﺷﻤﺎره_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 11,
            ﻣﻮﺟﻮدی_ﻛﺎﻓﻲ_ﻧﻴﺴﺖ = 12,
            رﻣﺰ_ﻧﺎدرﺳﺖ_اﺳﺖ = 13,
            ﺗﻌﺪاد_دﻓﻌﺎت_وارد_ﻛﺮدن_رﻣﺰ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ = 14,
            ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 15,
            دﻓﻌﺎت_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ = 16,
            ﻛﺎرﺑﺮ_از_اﻧﺠﺎم_ﺗﺮاﻛﻨﺶ_ﻣﻨﺼﺮف_ﺷﺪه_اﺳﺖ = 17,
            ﺗﺎرﻳﺦ_اﻧﻘﻀﺎی_ﻛﺎرت_ﮔﺬﺷﺘﻪ_اﺳﺖ = 18,
            ﻣﺒﻠﻎ_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ = 19,


            ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 111,
            ﺧﻄﺎی_ﺳﻮﻳﻴﭻ_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت = 112,
            ﭘﺎﺳﺨﻲ_از_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_درﻳﺎﻓﺖ_ﻧﺸﺪ = 113,
            دارﻧﺪه_ﻛﺎرت_ﻣﺠﺎز_ﺑﻪ_اﻧﺠﺎم_اﻳﻦ_ﺗﺮاﻛﻨﺶ_ﻧﻴﺴﺖ = 114,


            ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 21,
            ﺧﻄﺎی_اﻣﻨﻴﺘﻲ_رخ_داده_اﺳﺖ = 23,
            اﻃﻼﻋﺎت_ﻛﺎرﺑﺮی_ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 24,
            ﻣﺒﻠﻎ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 25,
            ﭘﺎﺳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 31,
            ﻓﺮﻣﺖ_اﻃﻼﻋﺎت_وارد_ﺷﺪه_ﺻﺤﻴﺢ_ﻧﻤﻲ_ﺑﺎﺷﺪ = 32,
            ﺣﺴﺎب_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 33,
            ﺧﻄﺎی_ﺳﻴﺴﺘﻤﻲ = 34,
            ﺗﺎرﻳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 35,
            ﺷﻤﺎره_درﺧﻮاﺳﺖ_ﺗﻜﺮاری_اﺳﺖ = 41,
            ﺗﺮاﻛﻨﺶ_Sale_یافت_نشد_ = 42,
            ﻗﺒﻼ_Verify_درﺧﻮاﺳﺖ_داده_ﺷﺪه_اﺳﺖ = 43,
            درخواست_verify_یافت_نشد = 44,
            ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ = 45,
            ﺗﺮاﻛﻨﺶ_Settle_نشده_اﺳﺖ = 46,
            ﺗﺮاﻛﻨﺶ_Settle_یافت_نشد = 47,
            تراکنش_Reverse_شده_است = 48,
            تراکنش_Refund_یافت_نشد = 49,


            شناسه_قبض_نادرست_است = 412,
            ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻧﺎدرﺳﺖ_اﺳﺖ = 413,
            سازﻣﺎن_ﺻﺎدر_ﻛﻨﻨﺪه_ﻗﺒﺾ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 414,
            زﻣﺎن_ﺟﻠﺴﻪ_ﻛﺎری_ﺑﻪ_ﭘﺎﻳﺎن_رسیده_است = 415,
            ﺧﻄﺎ_در_ﺛﺒﺖ_اﻃﻼﻋﺎت = 416,
            ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻛﻨﻨﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 417,
            اﺷﻜﺎل_در_ﺗﻌﺮﻳﻒ_اﻃﻼﻋﺎت_ﻣﺸﺘﺮی = 418,
            ﺗﻌﺪاد_دﻓﻌﺎت_ورود_اﻃﻼﻋﺎت_از_ﺣﺪ_ﻣﺠﺎز_ﮔﺬﺷﺘﻪ_اﺳﺖ = 419,
            IP_نامعتبر_است = 421,

            ﺗﺮاﻛﻨﺶ_ﺗﻜﺮاری_اﺳﺖ = 51,
            ﺗﺮاﻛﻨﺶ_ﻣﺮﺟﻊ_ﻣﻮﺟﻮد_ﻧﻴﺴﺖ = 54,
            ﺗﺮاﻛﻨﺶ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 55,
            ﺧﻄﺎ_در_واریز = 61
        }

        // DesribtionStatusCode
        private String DesribtionStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 0:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ.ToString();
                case 11:
                    return MellatBankReturnCode.ﺷﻤﺎره_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 12:
                    return MellatBankReturnCode.ﻣﻮﺟﻮدی_ﻛﺎﻓﻲ_ﻧﻴﺴﺖ.ToString();
                case 13:
                    return MellatBankReturnCode.رﻣﺰ_ﻧﺎدرﺳﺖ_اﺳﺖ.ToString();
                case 14:
                    return MellatBankReturnCode.ﺗﻌﺪاد_دﻓﻌﺎت_وارد_ﻛﺮدن_رﻣﺰ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ.ToString();
                case 15:
                    return MellatBankReturnCode.ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 16:
                    return MellatBankReturnCode.دﻓﻌﺎت_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ.ToString();
                case 17:
                    return MellatBankReturnCode.ﻛﺎرﺑﺮ_از_اﻧﺠﺎم_ﺗﺮاﻛﻨﺶ_ﻣﻨﺼﺮف_ﺷﺪه_اﺳﺖ.ToString();
                case 18:
                    return MellatBankReturnCode.ﺗﺎرﻳﺦ_اﻧﻘﻀﺎی_ﻛﺎرت_ﮔﺬﺷﺘﻪ_اﺳﺖ.ToString();
                case 19:
                    return MellatBankReturnCode.ﻣﺒﻠﻎ_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ.ToString();
                case 111:
                    return MellatBankReturnCode.ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 112:
                    return MellatBankReturnCode.ﺧﻄﺎی_ﺳﻮﻳﻴﭻ_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت.ToString();
                case 113:
                    return MellatBankReturnCode.ﭘﺎﺳﺨﻲ_از_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_درﻳﺎﻓﺖ_ﻧﺸﺪ.ToString();
                case 114:
                    return MellatBankReturnCode.دارﻧﺪه_ﻛﺎرت_ﻣﺠﺎز_ﺑﻪ_اﻧﺠﺎم_اﻳﻦ_ﺗﺮاﻛﻨﺶ_ﻧﻴﺴﺖ.ToString();
                case 21:
                    return MellatBankReturnCode.ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 23:
                    return MellatBankReturnCode.ﺧﻄﺎی_اﻣﻨﻴﺘﻲ_رخ_داده_اﺳﺖ.ToString();
                case 24:
                    return MellatBankReturnCode.اﻃﻼﻋﺎت_ﻛﺎرﺑﺮی_ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 25:
                    return MellatBankReturnCode.ﻣﺒﻠﻎ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 31:
                    return MellatBankReturnCode.ﭘﺎﺳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 32:
                    return MellatBankReturnCode.ﻓﺮﻣﺖ_اﻃﻼﻋﺎت_وارد_ﺷﺪه_ﺻﺤﻴﺢ_ﻧﻤﻲ_ﺑﺎﺷﺪ.ToString();
                case 33:
                    return MellatBankReturnCode.ﺣﺴﺎب_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 34:
                    return MellatBankReturnCode.ﺧﻄﺎی_ﺳﻴﺴﺘﻤﻲ.ToString();
                case 35:
                    return MellatBankReturnCode.ﺗﺎرﻳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 41:
                    return MellatBankReturnCode.ﺷﻤﺎره_درﺧﻮاﺳﺖ_ﺗﻜﺮاری_اﺳﺖ.ToString();
                case 42:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Sale_یافت_نشد_.ToString();
                case 43:
                    return MellatBankReturnCode.ﻗﺒﻼ_Verify_درﺧﻮاﺳﺖ_داده_ﺷﺪه_اﺳﺖ.ToString();



                case 44:
                    return MellatBankReturnCode.درخواست_verify_یافت_نشد.ToString();
                case 45:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ.ToString();
                case 46:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_نشده_اﺳﺖ.ToString();

                case 47:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_یافت_نشد.ToString();
                case 48:
                    return MellatBankReturnCode.تراکنش_Reverse_شده_است.ToString();
                case 49:
                    return MellatBankReturnCode.تراکنش_Refund_یافت_نشد.ToString();
                case 412:
                    return MellatBankReturnCode.شناسه_قبض_نادرست_است.ToString();
                case 413:
                    return MellatBankReturnCode.ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻧﺎدرﺳﺖ_اﺳﺖ.ToString();
                case 414:
                    return MellatBankReturnCode.سازﻣﺎن_ﺻﺎدر_ﻛﻨﻨﺪه_ﻗﺒﺾ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 415:
                    return MellatBankReturnCode.زﻣﺎن_ﺟﻠﺴﻪ_ﻛﺎری_ﺑﻪ_ﭘﺎﻳﺎن_رسیده_است.ToString();
                case 416:
                    return MellatBankReturnCode.ﺧﻄﺎ_در_ﺛﺒﺖ_اﻃﻼﻋﺎت.ToString();
                case 417:
                    return MellatBankReturnCode.ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻛﻨﻨﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 418:
                    return MellatBankReturnCode.اﺷﻜﺎل_در_ﺗﻌﺮﻳﻒ_اﻃﻼﻋﺎت_ﻣﺸﺘﺮی.ToString();
                case 419:
                    return MellatBankReturnCode.ﺗﻌﺪاد_دﻓﻌﺎت_ورود_اﻃﻼﻋﺎت_از_ﺣﺪ_ﻣﺠﺎز_ﮔﺬﺷﺘﻪ_اﺳﺖ.ToString();
                case 421:
                    return MellatBankReturnCode.IP_نامعتبر_است.ToString();

                case 51:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺗﻜﺮاری_اﺳﺖ.ToString();
                case 54:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﻣﺮﺟﻊ_ﻣﻮﺟﻮد_ﻧﻴﺴﺖ.ToString();
                case 55:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 61:
                    return MellatBankReturnCode.ﺧﻄﺎ_در_واریز.ToString();

            }
            return "";
        }

        // MellatResult
        private string MellatResult(string ID)
        {
            string result = "";
            switch (ID)
            {
                case "-100":
                    result = "پرداخت لغو شده";
                    break;
                case "0":
                    result = "تراكنش با موفقيت انجام شد";
                    break;
                case "11":
                    result = "شماره كارت نامعتبر است ";
                    break;
                case "12":
                    result = "موجودي كافي نيست ";
                    break;
                case "13":
                    result = "رمز نادرست است ";
                    break;
                case "14":
                    result = "تعداد دفعات وارد كردن رمز بيش از حد مجاز است ";
                    break;
                case "15":
                    result = "كارت نامعتبر است ";
                    break;
                case "16":
                    result = "دفعات برداشت وجه بيش از حد مجاز است ";
                    break;
                case "17":
                    result = "كاربر از انجام تراكنش منصرف شده است ";
                    break;
                case "18":
                    result = "تاريخ انقضاي كارت گذشته است ";
                    break;
                case "19":
                    result = "مبلغ برداشت وجه بيش از حد مجاز است ";
                    break;
                case "111":
                    result = "صادر كننده كارت نامعتبر است ";
                    break;
                case "112":
                    result = "خطاي سوييچ صادر كننده كارت ";
                    break;
                case "113":
                    result = "پاسخي از صادر كننده كارت دريافت نشد ";
                    break;
                case "114":
                    result = "دارنده كارت مجاز به انجام اين تراكنش نيست";
                    break;
                case "21":
                    result = "پذيرنده نامعتبر است ";
                    break;
                case "23":
                    result = "خطاي امنيتي رخ داده است ";
                    break;
                case "24":
                    result = "اطلاعات كاربري پذيرنده نامعتبر است ";
                    break;
                case "25":
                    result = "مبلغ نامعتبر است ";
                    break;
                case "31":
                    result = "پاسخ نامعتبر است ";
                    break;
                case "32":
                    result = "فرمت اطلاعات وارد شده صحيح نمي باشد ";
                    break;
                case "33":
                    result = "حساب نامعتبر است ";
                    break;
                case "34":
                    result = "خطاي سيستمي ";
                    break;
                case "35":
                    result = "تاريخ نامعتبر است ";
                    break;
                case "41":
                    result = "شماره درخواست تكراري است ، دوباره تلاش کنید";
                    break;
                case "42":
                    result = "يافت نشد  Sale تراكنش";
                    break;
                case "43":
                    result = "داده شده است  Verify قبلا درخواست";
                    break;
                case "44":
                    result = "يافت نشد  Verfiy درخواست";
                    break;
                case "45":
                    result = "شده است  Settle تراكنش";
                    break;
                case "46":
                    result = "نشده است  Settle تراكنش";
                    break;
                case "47":
                    result = "يافت نشد  Settle تراكنش";
                    break;
                case "48":
                    result = "شده است  Reverse تراكنش";
                    break;
                case "49":
                    result = "يافت نشد  Refund تراكنش";
                    break;
                case "412":
                    result = "شناسه قبض نادرست است ";
                    break;
                case "413":
                    result = "شناسه پرداخت نادرست است ";
                    break;
                case "414":
                    result = "سازمان صادر كننده قبض نامعتبر است ";
                    break;
                case "415":
                    result = "زمان جلسه كاري به پايان رسيده است ";
                    break;
                case "416":
                    result = "خطا در ثبت اطلاعات ";
                    break;
                case "417":
                    result = "شناسه پرداخت كننده نامعتبر است ";
                    break;
                case "418":
                    result = "اشكال در تعريف اطلاعات مشتري ";
                    break;
                case "419":
                    result = "تعداد دفعات ورود اطلاعات از حد مجاز گذشته است ";
                    break;
                case "421":
                    result = "نامعتبر است  IP";
                    break;
                case "51":
                    result = "تراكنش تكراري است ";
                    break;
                case "54":
                    result = "تراكنش مرجع موجود نيست ";
                    break;
                case "55":
                    result = "تراكنش نامعتبر است ";
                    break;
                case "61":
                    result = "خطا در واريز ";
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }
    }
}