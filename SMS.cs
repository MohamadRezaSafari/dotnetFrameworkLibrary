using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TarahiOnline.Providers
{
    public class SMS
    {
        private static readonly string Message;

        public static string SendSMS(string message, string mobile)
        {
            //if (message.Length > 80)
            //{
            //    throw new Exception();
            //}

            //try
            //{
            //    SmsWebService.Send objSend = new SmsWebService.Send();
            //    byte[] StatusArray = null;
            //    long[] RecIdArray = { };
            //    string userName = "qwew";
            //    string password = "qwewqe";
            //    string virtualNumber = "1321313";
            //    string[] recipientNumbersArray = new string[] { mobile };
            //    string MessageBody = message;
            //    int sendStatus;
            //    sendStatus = objSend.SendSms(userName, password, virtualNumber, recipientNumbersArray, MessageBody, false, ref StatusArray, ref RecIdArray);
            //    switch (sendStatus)
            //    {
            //        case -1:
            //            Message = "نام کاربری یا رمز عبور صحیح نیست";
            //            break;
            //        case 0:
            //            Message = "پیام با موفقیت ارسال شد" + "\n Status[0]=> " + StatusArray[0] + "\n RecId[0]=> " + RecIdArray[0];
            //            break;
            //        case 1:
            //            Message = "اعتبار شما برای ارسال کافی نیست";
            //            break;
            //        case 2:
            //            Message = "اکانت شما دارای محدودیت ارسال می باشد";
            //            break;
            //        case 3:
            //            Message = "شماره مجازی معتبر نمی باشد";
            //            break;
            //    }
            //}
            //catch (Exception)
            //{

            //    //throw;
            //}

            return Message;
        }
        public static string SendSMS(string message, string mobile, string delivery)
        {
            //if (message.Length > 80)
            //{
            //    throw new Exception();
            //}

            //try
            //{
            //    SmsWebService.Send objSend = new SmsWebService.Send();
            //    byte[] StatusArray = null;
            //    long[] RecIdArray = { };
            //    string userName = "qwew";
            //    string password = "qwewqe";
            //    string virtualNumber = "1321313";
            //    string[] recipientNumbersArray = new string[] { mobile };
            //    string MessageBody = message;
            //    int sendStatus;
            //    sendStatus = objSend.SendSms(userName, password, virtualNumber, recipientNumbersArray, MessageBody, false, ref StatusArray, ref RecIdArray);
            //    switch (sendStatus)
            //    {
            //        case -1:
            //            Message = "نام کاربری یا رمز عبور صحیح نیست";
            //            break;
            //        case 0:
            //            Message = "پیام با موفقیت ارسال شد" + "\n Status[0]=> " + StatusArray[0] + "\n RecId[0]=> " + RecIdArray[0];
            //            break;
            //        case 1:
            //            Message = "اعتبار شما برای ارسال کافی نیست";
            //            break;
            //        case 2:
            //            Message = "اکانت شما دارای محدودیت ارسال می باشد";
            //            break;
            //        case 3:
            //            Message = "شماره مجازی معتبر نمی باشد";
            //            break;
            //    }
            //}
            //catch (Exception)
            //{

            //    //throw;
            //}
            
            return Message;
        }
    }
}