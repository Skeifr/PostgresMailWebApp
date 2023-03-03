using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MailWebApp
{
    [Table("users", Schema = "public")]
    public class Contact
    {
        [Key]
        public int? id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("surname")]
        public string? Surname { get; set; }
        [Column("patronymic")]
        public string? Patronymic { get; set; }
        [Required]
        [Column("mailadress")]
        public string MailAdress { get; set; }
        [Column("passhash")]
        public byte[]? CorrectPasswordHash { get; set; }
        [Column("hashmodifier")]
        public int? Salt { get; set; }
        [NotMapped]
        public string Password
        {
            set
            {
                if (value != null)
                {
                    Salt = null;
                    CorrectPasswordHash = ComputePasswordHash(value);
                }
            }
        }
        public Contact() { }
        public Contact(ContactPost contact)
        {
            Name = contact.Name;
            Surname = contact.Surname;
            Patronymic = contact.Patronymic;
            MailAdress = contact.MailAdress;

            CorrectPasswordHash = ComputePasswordHash(contact.Password);
        }
        private int GenerateSaltForPassword()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[4];
            rng.GetNonZeroBytes(saltBytes);
            return (((int)saltBytes[0]) << 24) + (((int)saltBytes[1]) << 16) + (((int)saltBytes[2]) << 8) + ((int)saltBytes[3]);
        }
        internal byte[] ComputePasswordHash(string password)
        {
            if (Salt == null)
                Salt = GenerateSaltForPassword();

            byte[] saltBytes = new byte[4];
            saltBytes[0] = (byte)(Salt >> 24);
            saltBytes[1] = (byte)(Salt >> 16);
            saltBytes[2] = (byte)(Salt >> 8);
            saltBytes[3] = (byte)(Salt);

            byte[] passwordBytes = UTF8Encoding.UTF8.GetBytes(password);

            byte[] preHashed = new byte[saltBytes.Length + passwordBytes.Length];
            System.Buffer.BlockCopy(passwordBytes, 0, preHashed, 0, passwordBytes.Length);
            System.Buffer.BlockCopy(saltBytes, 0, preHashed, passwordBytes.Length, saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(preHashed);
        }
        public bool IsPasswordValid(string passwordToValidate)
        {
            byte[] hashedPassword = ComputePasswordHash(passwordToValidate);

            return hashedPassword.SequenceEqual(CorrectPasswordHash);
        }
    }

    public class ContactsGet
    {
        public int? id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        [Required]
        public string MailAdress { get; set; }
        public bool HavePassword { get; set; }
        public ContactsGet(Contact? contact)
        {
            id = contact.id;
            Name = contact.Name;
            Surname = contact.Surname;
            Patronymic = contact.Patronymic;
            MailAdress = contact.MailAdress;
            HavePassword = (contact.CorrectPasswordHash != null && contact.Salt != null);
        }
    }
    public class ContactPost
    {
        [Required]
        public string MailAdress { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
    }
    public class ContactPut
    {
        public int? id { get; set; }
        [Required]
        public string MailAdress { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
    }
}
