using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.MessageService.Repositories
{
    public class MessageProtocol
    {
        public const string SESSION_KEY = "ms_code";

        public const string OPTION_MAIL_KEY = "mail_protocol";
        public const string OPTION_SMS_KEY = "sms_protocol";
        public const int TYPE_TEXT = 1;
        public const int TYPE_HTML = 5;

        public const int RECEIVE_TYPE_MOBILE = 1;
        public const int RECEIVE_TYPE_EMAIL = 2;

        public const int STATUS_NONE = 0;
        public const int STATUS_SENDING = 1;

        public const int STATUS_SEND_FAILURE = 4;

        public const int STATUS_SENT = 6;
        public const int STATUS_SENT_USED = 7;
        public const int STATUS_SENT_EXPIRED = 9;

        public const string EVENT_LOGIN_CODE = "login_code";
        public const string EVENT_REGISTER_CODE = "register_code";
        public const string EVENT_REGISTER_EMAIL_CODE = "register_email";
        public const string EVENT_FIND_CODE = "find_code";
    }
}
