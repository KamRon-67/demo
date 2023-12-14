using System;
using LegacyApp.Models;
using LegacyApp.Repositories;
using LegacyApp.Services;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;
        
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = _dateTimeProvider.DateTimeNow;
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            if (age < 21)
            {
                return false;
            }
            
            var client = _clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            if (client.Name == "VeryImportantClient")
            {
                // Skip credit chek
                user.HasCreditLimit = false;
            }
            else if (client.Name == "ImportantClient")
            {
                // Do credit check and double credit limit
                user.HasCreditLimit = true;
                
                    var creditLimit = _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                
            }
            else
            {
                // Do credit check
                user.HasCreditLimit = true;

                    var creditLimit = _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }
            
            UserDataAccess.AddUser(user);

            return true;
        }
    }
}