﻿using Hattrick.ServiceLayer.Models;

namespace Hattrick.ServiceLayer.Service
{
    public interface ITopOffers
    {
        List<TopOfferModel> GetAll();
    }

    public class TopOffersService : ITopOffers
    {
        private readonly HattrickDbContext _context;

        public TopOffersService(HattrickDbContext context)
        {
            _context = context;
        }
        public List<TopOfferModel> GetAll()
        {
            return _context.TopOffers.ToList();
        }
    }
}
