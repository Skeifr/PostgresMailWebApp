using Microsoft.AspNetCore.Mvc;

namespace MailWebApp.Controllers
{
    [Route("/api/[controller]")]
    public class MessagesController : Controller
    {
        [HttpGet]
        public IEnumerable<MessageGet> Get()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var messages = db.MessagesDBSet.ToList();
                foreach (var message in messages) 
                {
                    message.Recipient = new ContactsGet(db.ContactsDBSet.Where(contact => contact.id == message.Recipient_id).FirstOrDefault());
                    message.Sender = new ContactsGet(db.ContactsDBSet.Where(contact => contact.id == message.Sender_id).FirstOrDefault());
                }
                return messages.Select(message=>new MessageGet(message)).ToList();
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    var messages = db.MessagesDBSet.ToList();
                    foreach (var message in messages)
                    {
                        message.Recipient = new ContactsGet(db.ContactsDBSet.Where(contact => contact.id == message.Recipient_id).FirstOrDefault());
                        message.Sender = new ContactsGet(db.ContactsDBSet.Where(contact => contact.id == message.Sender_id).FirstOrDefault());
                    }
                   var messageResult = messages.Select(message => new MessageGet(message)).SingleOrDefault(message => message.id == id);

                    if (messageResult == null)
                        return NotFound();
                    else
                        return Ok(messageResult);
                }
                catch (Exception ex)
                {
                    return NotFound(ex);
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    Message? deletingMessage = db.MessagesDBSet.SingleOrDefault(p => p.id == id);
                    if (deletingMessage != null)
                    {
                        db.MessagesDBSet.Remove(deletingMessage);
                        return Ok(new { Messsage = "deleting success" });
                    }
                    else
                        return Ok(new { Messsage = "id contact not found" });
                }
                catch (Exception ex)
                {
                    return NotFound(ex);
                }
            }
        }
    }
}
