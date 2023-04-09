using System.ComponentModel.DataAnnotations;

namespace Onigns.HelpDesk.UI.ViewModels
{
    public class HelpDeskTicket
    {
        public int Id { get; set; }
        [Required]
        public string TicketStatus { get; set; }
        [Required]
        public DateTime TicketDate { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2,
        ErrorMessage =
        "Description must be a minimum of 2 and maximum of 50 characters.")]
        public string TicketDescription { get; set; }
        [Required]

        [EmailAddress]
        public string TicketRequesterEmail { get; set; }
        public string TicketGuid { get; set; }
    }

    public class HelpDeskStatus
    {
        public string ID { get; set; }
        public string Text { get; set; }
        public static List<HelpDeskStatus> Statuses =
            new List<HelpDeskStatus>() {
                new HelpDeskStatus(){ ID= "New", Text= "New" },
                new HelpDeskStatus(){ ID= "Open", Text= "Open" },
                new HelpDeskStatus(){ ID= "Urgent", Text= "Urgent" },
                new HelpDeskStatus(){ ID= "Closed", Text= "Closed" },
            };
    }
}
