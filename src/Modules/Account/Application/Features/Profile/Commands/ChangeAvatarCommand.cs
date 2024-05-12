using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Application.Features.Profile.Commands
{
    public class ChangeAvatarCommand : Command<Result<ChangeAvatarResponse>>
    {
        public ChangeAvatarCommand(string username, IFormFile image) 
        {
            Username = username;
            AvatarImage = image;
        }

        public string Username { get; set; }
        public IFormFile AvatarImage { get; set; }
    }
}
