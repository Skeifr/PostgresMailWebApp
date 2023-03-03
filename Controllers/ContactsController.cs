using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace MailWebApp
{
    [Route("/api/[controller]")]
    public class ContactsController : Controller
    {
        Regex mailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        [HttpGet]
        public IEnumerable<ContactsGet> Get()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.ContactsDBSet.Select(contact => new ContactsGet(contact)).ToList();
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    var result = db.ContactsDBSet.SingleOrDefault(p => p.id == id);

                    if (result == null)
                        return NotFound();
                    else
                        return Ok(new ContactsGet(result));
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
                    Contact? deletingContact = db.ContactsDBSet.SingleOrDefault(p => p.id == id);
                    if (deletingContact != null)
                    {
                        db.ContactsDBSet.Remove(deletingContact);
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

        [HttpPost]
        public IActionResult Post(ContactPost contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    if (!mailRegex.Match(contact.MailAdress).Success)
                        return BadRequest("email adress incorrect!");

                    db.ContactsDBSet.Add(new Contact(contact));
                    db.SaveChanges();
                    return CreatedAtAction(nameof(Get), contact);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
        }

        [HttpPost("AddContact")]
        public IActionResult PostBody([FromBody] ContactPost contact) => Post(contact);

        [HttpPut]
        public IActionResult Put(ContactPut contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    var storedContact = db.ContactsDBSet.SingleOrDefault(c => c.id == contact.id);

                    if (storedContact == null)
                        return NotFound();

                    if (!mailRegex.Match(contact.MailAdress).Success)
                        return BadRequest("email adress incorrect!");

                    storedContact.MailAdress = contact.MailAdress;
                    storedContact.Name = contact.Name;
                    storedContact.Surname = contact.Surname;
                    storedContact.Patronymic = contact.Patronymic;
                    storedContact.Password = contact.Password;

                    db.ContactsDBSet.Update(storedContact);
                    db.SaveChanges();

                    return Ok(storedContact);
                }
                catch (Exception ex)
                {
                    return NotFound(ex);
                }
            }
        }

        [HttpPut("UpdateContact")]
        public IActionResult PutBody([FromBody] ContactPut contact) => Put(contact);
    }
}
