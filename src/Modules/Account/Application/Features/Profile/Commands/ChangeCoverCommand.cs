using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Astrum.Account.Application.Features.Profile.Commands
{
    public class ChangeCoverCommand : Command<Result<ChangeCoverResponse>>
    {
        public ChangeCoverCommand(string username, IFormFile image) 
        {
            Username = username;
            CoverImage = image;
        }

        public string Username { get; set; }
        public IFormFile CoverImage { get; set; }
    }
}
