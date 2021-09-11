using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReminderService.Exceptions;
using ReminderService.Models;
using ReminderService.Services;
namespace ReminderService.Controllers
{
    /*
    * As in this assignment, we are working with creating RESTful web service, hence annotate
    * the class with [ApiController] annotation and define the controller level route as per REST Api standard.   
    */
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        /*
        * ReminderService should  be injected through constructor injection. 
        * Please note that we should not create Reminderservice object using the new keyword
        */
        IReminderService _reminderService;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }
        /* Implement HttpVerbs and its Functionalities asynchronously*/

        /*
        * Define a handler method which will get us the reminders by a userId.
        * 
        * This handler method should return any one of the status messages basis on
        * different situations: 
        * 1. 200(OK) - If the reminder found successfully.
        * 
        * This handler method should map to the URL "/api/reminder/{userId}" using HTTP GET method
        * and also handle the custom exception for the same
        */
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                List<ReminderSchedule> reminderSchedules = await _reminderService.GetReminders(userId);
                return Ok(reminderSchedules);
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
        * Define a handler method which will create a reminder by reading the
        * Serialized reminder object from request body and save the reminder in
        * reminder table in database. 
        * This handler method should return any one of the status messages
        * basis on different situations: 
        * 1. 201(CREATED - In case of successful creation of the reminder 
        * 2. 409(CONFLICT) - In case of duplicate reminder ID
        * This handler method should map to the URL "/api/reminder" using HTTP POST
        * method".
        */
        [HttpPost]
        public async Task<IActionResult> Post(Reminder reminder)
        {
            try
            {
                bool flag = await _reminderService.CreateReminder(reminder.UserId,reminder.Email,reminder.NewsReminders.FirstOrDefault());
                return Created("", flag);
            }
            catch (ReminderAlreadyExistsException e)
            {
                return Conflict(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        /*
        * Define a handler method which will delete a reminder from a database.
        * This handler method should return any one of the status messages basis on
        * different situations: 
        * 1. 200(OK) - If the reminder deleted successfully from database. 
        * 2. 404(NOT FOUND) - If the reminder with specified userId is  not found. 
        * This handler method should map to the URL "/api/reminder/{userId}/{newsId}" using HTTP Delete
        * method" where "id" should be replaced by a valid reminderId without {}
        */
        [HttpDelete]
        [Route("{userId}/{newsId:int}")]
        public async Task<IActionResult> Delete(string userId,int newsId)
        {
            try
            {
                bool flag = await _reminderService.DeleteReminder(userId, newsId);
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
         * Define a handler method (Put) which will update a reminder by userId,newsId and with Reminder Details
         * 
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news updated successfully to the database using userId with newsId
         * 2. 404(NOT FOUND) - If the news with specified newsId is not found.
         * 
         * This handler method should map to the URL "/api/news/userId" using HTTP PUT
         * method" where "id" should be replaced by a valid userId without {}
         */
        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> Put(string userId,ReminderSchedule reminderSchedule)
        {
            try
            {
                bool flag = await _reminderService.UpdateReminder(userId, reminderSchedule);
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
    }
}
