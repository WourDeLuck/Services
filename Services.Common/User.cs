using System;

namespace Services.Common
{
    /// <summary>
    /// Class with necessary fields for User.
    /// </summary>
    public class User
    {
        // Login field
        public string Login { get; set; }

        // First name field
        public string FirstName { get; set; }

        // Last name field
        public string LastName { get; set; }

        // Birthday date field
        public DateTime BirthDate { get; set; }

        // Password field
        public string Password { get; set; }

        // Id field
        public int Id { get; set; }

        // Date of account registration field
        public DateTime CreatedDate { get; set; }

        // Date of modify field
        public DateTime ModifiedDate { get; set; }

        // Additional field
        // E-mail field
        public string EMail { get; set; }
    }
}
