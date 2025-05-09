//using TechXpress.Web.Services.Implementations;

//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using TechXpress.Data.Models;
//using TechXpress.Data.Repositories.GenericRepository;

//namespace TechXpress.Web.Areas.Admin.Services
//{
//    public interface IMarketingService
//    {
//        Task<List<MarketingCampaign>> GetAllCampaignsAsync();
//        Task<MarketingCampaign> GetCampaignByIdAsync(int id);
//        Task<MarketingCampaign> CreateCampaignAsync(MarketingCampaign campaign);
//        Task<MarketingCampaign> UpdateCampaignAsync(MarketingCampaign campaign);
//        Task<bool> DeleteCampaignAsync(int id);
//        Task<List<MarketingCampaign>> GetCampaignsByDateRangeAsync(DateTime startDate, DateTime endDate);
//        Task<decimal> CalculateROIAsync(int campaignId);
//        Task<List<MarketingCampaign>> GetActiveCampaignsAsync();
//    }

//    public class MarketingService : IMarketingService
//    {
//        private readonly IGenericRepository<MarketingCampaign> _campaignRepository;

//        public MarketingService(IGenericRepository<MarketingCampaign> campaignRepository)
//        {
//            _campaignRepository = campaignRepository;
//        }

//        public async Task<List<MarketingCampaign>> GetAllCampaignsAsync()
//        {
//            return (List<MarketingCampaign>)await _campaignRepository.GetAllAsync();
//        }

//        public async Task<MarketingCampaign> GetCampaignByIdAsync(int id)
//        {
//            return await _campaignRepository.GetByIdAsync(id);
//        }

//        public async Task<MarketingCampaign> CreateCampaignAsync(MarketingCampaign campaign)
//        {
//            campaign.CreatedDate = DateTime.UtcNow;
//            return await _campaignRepository.AddAsync(campaign);
//        }

//        public async Task<MarketingCampaign> UpdateCampaignAsync(MarketingCampaign campaign)
//        {
//            campaign.ModifiedDate = DateTime.UtcNow;
//            return await _campaignRepository.Update(campaign);
//        }

//        public async Task<bool> DeleteCampaignAsync(int id)
//        {
//            return await _campaignRepository.DeleteAsync(id);
//        }

//        public async Task<List<MarketingCampaign>> GetCampaignsByDateRangeAsync(DateTime startDate, DateTime endDate)
//        {
//            return await _campaignRepository.FindAsync(c =>
//                c.StartDate >= startDate && c.EndDate <= endDate);
//        }

//        public async Task<decimal> CalculateROIAsync(int campaignId)
//        {
//            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
//            if (campaign == null || campaign.Budget == 0)
//                return 0;

//            return ((campaign.RevenueGenerated - campaign.Budget) / campaign.Budget) * 100;
//        }

//        public async Task<List<MarketingCampaign>> GetActiveCampaignsAsync()
//        {
//            var now = DateTime.UtcNow;
//            return await _campaignRepository.FindAsync(c =>
//                c.StartDate <= now && c.EndDate >= now);
//        }
//    }
//}