using AareonTechnicalTest.Models;
using System.Collections.Generic;

namespace AareonTechnicalTest.Services
{
    public interface IPersonService
    {
        Person CreatePerson(Person person);
        bool DeletePerson(int personId);
        IEnumerable<Person> GetAllPersons();
        Person GetPersonById(int personId);
        bool UpdatePerson(int personId, Person person);
        bool PersonExists(int ticketId);
    }
}