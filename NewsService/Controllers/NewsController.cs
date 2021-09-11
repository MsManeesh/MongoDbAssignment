using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsService.Exceptions;
using NewsService.Models;
using NewsService.Services;
namespace NewsService.Controllers
{
    /*
    * As in this assignment, we are working with creating RESTful web service, hence annotate
    * the class with [ApiController] annotation and define the controller level route as per REST Api standard.
    */
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        /*
        * NewsService should  be injected through constructor injection. 
        * Please note that we should not create service object using the new keyword
        */
        INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        /* Implement HttpVerbs and Functionalities asynchronously*/

        /*
         * Define a handler method which will get us the news by a userId.
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news found successfully.
         * This handler method should map to the URL "/api/news/{userId}" using HTTP GET method
         */
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                List<News> newsList = await _newsService.FindAllNewsByUserId(userId);
                return Ok(newsList);
            }
            catch (NoNewsFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        /*
        * Define a handler method which will create a specific news by reading the
        * Serialized object from request body and save the news details in a News table
        * in the database.
        * 
        * Please note that CreateNews method should add a news and also handle the exception.
        * This handler method should return any one of the status messages basis on different situations: 
        * 1. 201(CREATED) - If the news created successfully. 
        * 2. 409(CONFLICT) - If the userId conflicts with any existing newsid
        * 
        * This handler method should map to the URL "/api/news" using HTTP POST method
        */
        [HttpPost]
        [Route("{userId}")]
        public async Task<IActionResult> Post(string userId, [FromBody] News news)
        {
            try
            {
                int output= await _newsService.CreateNews(userId,news);
                return Created("", output);
            }
            catch (NewsAlreadyExistsException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        /*
         * Define a handler method which will delete a news from a database.
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news deleted successfully from database. 
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should map to the URL "/api/news/userId/newsId" using HTTP Delete
         * method" where "id" should be replaced by a valid newsId without {}
         */
        [HttpDelete]
        [Route("{userId}/{newsId:int}")]
        public async Task<IActionResult> Delete(string userId, int newsId)
        {
            try
            {
                bool flag = await _newsService.DeleteNews(userId,newsId);
                return Ok(flag);
            }
            catch (NoNewsFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        /*
         * Define a handler method (DeleteReminder) which will delete a news from a database.
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news deleted successfully from database using userId with newsId
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should map to the URL "/api/news/userId/newsId/reminder" using HTTP Delete
         * method" where "id" should be replaced by a valid newsId without {}
         */
        [HttpDelete]
        [Route("{userId}/{newsId:int}/reminder")]
        public async Task<IActionResult> DeleteReminder(string userId, int newsId)
        {
            try
            {
                bool flag = await _newsService.DeleteReminder(userId, newsId);
                return Ok(flag);
            }
            catch (NoReminderFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        /*
         * Define a handler method (Put) which will update a news by userId,newsId and with Reminder Details
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news updated successfully to the database using userId with newsId
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should map to the URL "/api/news/userId/newsId/reminder" using HTTP PUT
         * method" where "id" should be replaced by a valid newsId without {}
         */
        [HttpPut]
        [Route("{userId}/{newsId:int}/reminder")]
        public async Task<IActionResult> Put(string userId, int newsId,Reminder reminder)
        {
            try
            {
                bool flag = await _newsService.AddOrUpdateReminder(userId, newsId,reminder);
                return Ok(flag);
            }
            catch (NoNewsFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    }
}
