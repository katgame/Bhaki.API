using Dice.API.Data.Models;
using Dice.API.Data.Dto;
using System.Collections.Generic;
using System;

namespace Dice.API.Interfaces
{
    public interface IProfileService
    {
        bool CreateProfile(Guid userID);
     
    }
}
