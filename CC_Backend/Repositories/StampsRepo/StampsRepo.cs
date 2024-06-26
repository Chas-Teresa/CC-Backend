﻿using CC_Backend.Data;
using CC_Backend.Models.Viewmodels;
using CC_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace CC_Backend.Repositories.Stamps
{
    public class StampsRepo : IStampsRepo
    {
        private readonly NatureAIContext _context;

        public StampsRepo(NatureAIContext context)
        {
            _context = context;
        }





        // Get all stamps from user

        public async Task<ICollection<StampViewModel>> GetStampsFromUserAsync(string userId)
        {
            var result = await _context.Users
                .Include(u => u.StampsCollected)
                    .ThenInclude(u => u.Stamp)
                .Where(u => u.Id == userId)
                .SelectMany(u => u.StampsCollected)
                .ToListAsync();


            // Create a list of viewmodels that mirrors the list of stamps that a user has collected.
            var stampsList = new List<StampViewModel>();

            foreach (var stamp in result)
            {
                var stampViewModel = new StampViewModel
                {
                    Name = stamp.Stamp.Name,

                };
                stampsList.Add(stampViewModel);
            }

            return stampsList;
        }




        // Save a StampCollected-object to the database it and connect a user to it.
        public async Task AwardStampToUserAsync(string userId, StampCollected stamp)
        {
            try
            {
                _context.StampsCollected.Add(stamp);
                await _context.SaveChangesAsync();

                var user = await _context.Users
                    .Include(u => u.StampsCollected)
                    .SingleOrDefaultAsync(u => u.Id == userId);

                // Add the new StampCollected object to the StampsCollected collection.
                if (user.StampsCollected == null)
                {
                    // Initialize the collection if necessary.
                    user.StampsCollected = new List<StampCollected>();
                }

                user.StampsCollected.Add(stamp);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to add stamp.", ex);
            }
        }
    }
}
