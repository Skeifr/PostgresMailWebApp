using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MailWebApp
{
    [Table("messages", Schema = "public")]
    public class Message
    {
        [Key]
        public int? id { get; set; }
        [Column("body")]
        public string? Body { get; set; }
        [Column("recipient_id")]
        public int? Recipient_id { get; set; }
        [Column("sender_id")]
        public int? Sender_id { get; set; }
        [NotMapped]
        public ContactsGet? Recipient { get; set; }
        [NotMapped]
        public ContactsGet? Sender { get; set; }
        [Column("title")]
        public string? Title { get; set; }
        [Column("sendeddate")]
        public DateTime SendedDate { get; set; }
    }

    public class MessageGet
    {
        public int? id { get; set; }
        public DateTime SendedDate { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public ContactsGet? Sender { get; set; }
        public ContactsGet? Recipient { get; set; }
        public MessageGet(Message message)
        {
            id = message.id;
            Body = message.Body;
            Recipient = message.Recipient;
            Sender = message.Sender;
            Title = message.Title;
            SendedDate = message.SendedDate;
        }
    }
}
