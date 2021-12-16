using AareonTechnicalTest.Models;
using System.Collections.Generic;

namespace AareonTechnicalTest.Services
{
    public interface ITicketService
    {
        Ticket CreateTicket(Ticket ticket);
        bool DeleteTicket(int ticketId);
        IEnumerable<Ticket> GetAllTickets();
        Ticket GetTicketById(int ticketId);
        bool UpdateTicket(int ticketId, Ticket ticket);
        bool TicketExists(int ticketId);
        bool CanPersonDeleteNote(int personId);
        Ticket CreateTicketNote(int ticketId, int personId, Note note);
        Ticket UpdateTicketNote(int ticketId, int personId, Note note);
        Ticket DeleteTicketNote(int ticketId, int personId);
    }
}