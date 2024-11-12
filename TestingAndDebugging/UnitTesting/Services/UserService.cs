namespace UnitTesting.Services
{
    public class UserService
    {
        public bool RegisterUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("username and password cannot be empty.");
            }

            if (password.Length < 8)
            {
                throw new ArgumentException("Password must be at lest 8 characters.");
            }

            return true;
        }

        public int GetUserAge(DateTime birthdate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthdate.Year;

            //today: 11/12/2024
            //input: 05/31/2003 

            //age: 21
            //if( 11/12/2003 < 05/31/2003 )
            //    age--

            // get exact age
            if (today.AddYears(-age) < birthdate)
            {
                age--;
            }

            return age;
        }
    }
}
