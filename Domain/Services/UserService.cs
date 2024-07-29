using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class UserService : IUserRepository
{
    private readonly IUserRepository _userRepository;

    public UserService (IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> CreateUser(User user)
    {
        User response = await _userRepository.CreateUser(user);
        return response;
    }

    // public async Task<User> GetUserById(string id)
    // {
    //     User user = await _userRepository.SearchUser(id);
    //     return user;
    // }

    // public async Task UpdateCustomer(string engineCustomerId, string paymentMethodId)
    // {
    //     await _customerRepository.UpdateCustomer(engineCustomerId, paymentMethodId);
    // }
    //
    // public async Task<Customer> SearchCustomerByYoursId(string yoursId)
    // {
    //     Customer response = await _customerRepository.SearchCustomerByYoursId(yoursId);
    //     return response;
    // }
}