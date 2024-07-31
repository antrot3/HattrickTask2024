using Hattrick.ServiceLayer.Models;

namespace Hattrick.ServerTests
{
    public class TestDataHelper
    {
        public static List<UserModel> GetFakeUserList()
        {
            return new List<UserModel>()
            {
                new UserModel
                {
                    Id = 1,
                    Name = "John Doe",
                    WalletBalance = 5000,
                }
            };
        }

        public static List<UserModel> GetFakeMatchesList()
        {
            return new List<UserModel>()
            {
                new UserModel
                {
                    Id = 1,
                    Name = "John Doe",
                    WalletBalance = 5000,
                }
            };
        }
    }
}