using MediatR;
using NetDream.Modules.Auth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Events
{
    public record CancelAccount(UserEntity User, int Timestamp) : INotification
    {
    }
}
