﻿using Microsoft.EntityFrameworkCore;
using CC_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using CC_Backend.Models.Viewmodels;
using CC_Backend.Data;

namespace CC_Backend.Repositories.Stamps
{
    public interface IStampsRepo
    {

        Task<ICollection<StampViewModel>> GetStampsFromUserAsync(string userId);

        Task AwardStampToUserAsync(string userId, StampCollected stamp);
    }

}
