using NewsService.Models;
using NewsService.Repository;
using NewsService.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NewsService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e NewsService by inheriting INewsService

    public class NewsService : INewsService
    {
        INewsRepository _newsRepo;
        public NewsService(INewsRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }
        /*
        * NewsRepository should  be injected through constructor injection. 
        * Please note that we should not create NewsRepository object using the new keyword
        */
        /* Implement all the methods of respective interface asynchronously*/

        /* Implement CreateNews method to add the new news details*/
        public async Task<int> CreateNews(string userId, News news)
        {
            bool flag=await _newsRepo.IsNewsExist(userId, news.Title);
            if (flag == false)
            {
                return await _newsRepo.CreateNews(userId, news);
            }
            else
                throw new NewsAlreadyExistsException($"{userId} have already added this news");
            
        }
        /* Implement AddOrUpdateReminder using userId and newsId*/
        public async Task<bool> AddOrUpdateReminder(string userId, int newsId, Reminder reminder)
        {
            News news = await _newsRepo.GetNewsById(userId, newsId);
            if (news != null)
            {
                return await _newsRepo.AddOrUpdateReminder(userId,newsId,reminder);
            }
            else
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
        }
        /* Implement DeleteNews method to remove the existing news*/
        public async Task<bool> DeleteNews(string userId, int newsId)
        {
            bool flag= await _newsRepo.DeleteNews(userId, newsId);
            if (flag)
            {
                return flag;
            }
            else
                throw new NoNewsFoundException($"NewsId {newsId} for {userId} doesn't exist");
        }
        /* Implement DeleteReminder method to delte the Reminder using userId*/
        public async Task<bool> DeleteReminder(string userId, int newsId)
        {
            bool flag = await _newsRepo.IsReminderExists(userId, newsId);
            if (flag)
            {
                return await _newsRepo.DeleteReminder(userId, newsId);
            }
            else
                throw new NoReminderFoundException("No reminder found for this news");
        }
        /* Implement FindAllNewsByUserId to get the News Details by userId*/
        public async Task<List<News>> FindAllNewsByUserId(string userId)
        {
            List<News> newsList =await  _newsRepo.FindAllNewsByUserId(userId);
            if (newsList != null)
                return newsList;
            else
                throw new NoNewsFoundException($"No news found for {userId}");
        }

    }
}
