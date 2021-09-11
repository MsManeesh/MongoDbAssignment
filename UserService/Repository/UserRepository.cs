using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
namespace UserService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserRepository by inheriting IUserRepository class 
    //which is used to implement all methods in the classs
    public class UserRepository:IUserRepository
    {
        UserContext _userContext;
        //define a private variable to represent Reminder Database Context
        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<bool> AddUser(UserProfile user)
        {
            await _userContext.Users.InsertOneAsync(user);
            return true;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            UserProfile userExist= await _userContext.Users.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            if (userExist != null)
            {
                await _userContext.Users.DeleteOneAsync(x => x.UserId == userId);
                return true;
            }
            else
                return false;
        }

        public async Task<UserProfile> GetUser(string userId)
        {
            return await _userContext.Users.Find(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(UserProfile user)
        {
            await _userContext.Users.ReplaceOneAsync(x => x.UserId == user.UserId, user);
            return true;
        }

        //Implement the methods of interface Asynchronously.

        // Implement AddUser method which should be used to add  a new user Profile.  

        // Implement DeleteUser method which should be used to delete an existing user by userId.


        // Implement GetUser method which should be used to get a user by userId.



        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
