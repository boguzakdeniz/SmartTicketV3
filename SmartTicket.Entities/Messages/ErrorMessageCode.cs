using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTicket.Entities.Messages
{
    public enum ErrorMessageCode
    {
        //Todo hata mesajları 
        UserNotFound=100,
        UserAlreadyDefined=101,
        BadPassword = 102,
        RegisterNull = 103,
        UsernameIsUsed = 104,
        MailIsUsed = 105,
        PhoneIsUsed = 106,
        MailNull = 107,
        UserAlreadyActive = 108,
        ActivateIdDoesNotExists = 109,
    }
}
