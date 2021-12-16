using AareonTechnicalTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace AareonTechnicalTest.Services
{
    public class TicketService : ITicketService
    {
        private ApplicationContext _dbContext { get; set; }

        public TicketService(ApplicationContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Ticket CreateTicket(Ticket ticket)
        {
            this._dbContext.Tickets.Add(ticket);
            this._dbContext.SaveChanges();

            return ticket;
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            return this._dbContext.Tickets;
        }

        public Ticket GetTicketById(int ticketId)
        {
            return this._dbContext.Tickets.Where(t => t.Id == ticketId).FirstOrDefault();
        }

        public bool UpdateTicket(int ticketId, Ticket ticket)
        {
            Ticket tempTicket = GetTicketById(ticketId);

            if (tempTicket != null)
            {
                tempTicket.Content = ticket.Content;
                tempTicket.PersonId = ticket.PersonId;

                this._dbContext.Update<Ticket>(tempTicket);
                int changes = this._dbContext.SaveChanges();

                return changes > 0;
            }

            return false;
        }

        public bool DeleteTicket(int ticketId)
        {
            Ticket ticket = GetTicketById(ticketId);

            if (ticket != null)
            {
                this._dbContext.Remove(ticket);
                int changes = this._dbContext.SaveChanges();

                return changes > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if a ticket exists
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public bool TicketExists(int ticketId)
        {
            return GetTicketById(ticketId) != null;
        }

        /// <summary>
        /// Determine is person can delete a ticket note (person has to be admin)
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public bool CanPersonDeleteNote(int personId)
        {
            Person person = this._dbContext.Persons.Where(p => p.Id == personId).FirstOrDefault();

            if(person != null)
            {
                return person.IsAdmin;
            }

            return false;
        }

        /// <summary>
        /// Create (add) a note to a ticket
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Ticket CreateTicketNote(int ticketId, int personId, Note note)
        {
            return UpdateTicketNote(ticketId, personId, note);
        }

        /// <summary>
        /// Update a note on a ticket
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Ticket UpdateTicketNote(int ticketId, int personId, Note note)
        {
            Ticket ticket = GetTicketById(ticketId);

            ticket.Note = note.NoteContent;
            ticket.PersonId = personId;

            if (UpdateTicket(ticketId, ticket))
            {
                return ticket;
            }

            return null;
        }

        /// <summary>
        /// Delete a note on a ticket
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Ticket DeleteTicketNote(int ticketId, int personId)
        {
            if (CanPersonDeleteNote(personId))
            {
                Ticket ticket = GetTicketById(ticketId);

                ticket.Note = null;
                ticket.PersonId = personId;

                if (UpdateTicket(ticketId, ticket))
                {
                    return ticket;
                }
            }

            return null;
        }


    }
}
