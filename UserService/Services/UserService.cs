using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
namespace UserService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserService by inheriting IUserService
    public class UserService:IUserService
    {
        /*
         * UserRepository should  be injected through constructor injection. 
         * Please note that we should not create USerRepository object using the new keyword
         */
        IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddUser(UserProfile user)
        {
            UserProfile userExist = await _userRepository.GetUser(user.UserId);
            if (userExist == null)
                return await _userRepository.AddUser(user);
            else
                throw new UserAlreadyExistsException($"{user.UserId} is already in use");
        }

        public async Task<bool> DeleteUser(string userId)
        {
            UserProfile userExist = await _userRepository.GetUser(userId);
            if (userExist != null)
                return await _userRepository.DeleteUser(userId);
            else
                throw new UserNotFoundException($"This user id doesn't exist");
        }

        public async Task<UserProfile> GetUser(string userId)
        {
            UserProfile user = await _userRepository.GetUser(userId);
            if (user != null)
                return user;
            else
                throw new UserNotFoundException($"This user id doesn't exist");
        }

        public async Task<bool> UpdateUser(string userId, UserProfile user)
        {
            UserProfile userExist = await _userRepository.GetUser(userId);
            if (userExist != null)
                return await _userRepository.UpdateUser(user);
            else
                throw new UserNotFoundException($"This user id doesn't exist");
        }
        //Implement the methods of interface Asynchronously.

        // Implement AddUser method which should be used to add  a new user Profile.  

        // Implement DeleteUser method which should be used to delete an existing user by userId.


        // Implement GetUser method which should be used to get a user by userId.

        // Implement UpdateUser method which should be used to update an existing user by using
        // UserProfile details.
    }
}
