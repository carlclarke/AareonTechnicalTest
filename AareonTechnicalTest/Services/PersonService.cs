using AareonTechnicalTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace AareonTechnicalTest.Services
{
    public class PersonService : IPersonService
    {
        private ApplicationContext _dbContext { get; set; }

        public PersonService(ApplicationContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Person CreatePerson(Person person)
        {
            this._dbContext.Persons.Add(person);
            this._dbContext.SaveChanges();

            return person;
        }

        public IEnumerable<Person> GetAllPersons()
        {
            return this._dbContext.Persons;
        }

        public Person GetPersonById(int personId)
        {
            return this._dbContext.Persons.Where(p => p.Id == personId).FirstOrDefault();
        }

        public bool UpdatePerson(int personId, Person person)
        {
            Person tempPerson = GetPersonById(personId);

            if (tempPerson != null)
            {
                tempPerson.Forename = person.Forename;
                tempPerson.IsAdmin = person.IsAdmin;
                tempPerson.Surname = person.Surname;

                this._dbContext.Update<Person>(tempPerson);
                int changes = this._dbContext.SaveChanges();

                return changes > 0;
            }

            return false;
        }

        public bool DeletePerson(int personId)
        {
            Person person = GetPersonById(personId);

            if (person != null)
            {
                this._dbContext.Remove(person);
                int changes = this._dbContext.SaveChanges();

                return changes > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if a person exists
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public bool PersonExists(int ticketId)
        {
            return GetPersonById(ticketId) != null;
        }
    }
}
