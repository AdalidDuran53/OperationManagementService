using EntitiesCustom;

namespace OperationManagementServicesTest
{
    public class UserTest
    {
        [Fact]
        public void User_Validate_OK()
        {
            string operationExceptionCode = null;
            User user = new User(Guid.NewGuid(), "TestUser", "TestPassword","TestSalst");
            bool result = String.IsNullOrEmpty(user.Validate(operationExceptionCode));

            Assert.True(result);
        }

        [Fact]
        public void User_Validate_UserName_Empty()
        {
            string operationExceptionCode = null;
            User user = new User(Guid.NewGuid(), String.Empty, "TestPassword", "TestSalst");
            bool result = user.Validate(operationExceptionCode).Equals("OMS-USERNAME-ERROR");

            Assert.True(result);
        }

        [Fact]
        public void User_Validate_UserName_InvalidLength()
        {
            string operationExceptionCode = null;
            User user = new User(Guid.NewGuid(), "TestUserTestUserTestUserTestUserTestUserTestUserTestUserTestUserTestUser", "TestPassword", "TestSalst");
            bool result = user.Validate(operationExceptionCode).Equals("OMS-USERNAME-ERROR");

            Assert.True(result);
        }

        [Fact]
        public void User_Validate_Password_Empty()
        {
            string operationExceptionCode = null;
            User user = new User(Guid.NewGuid(), "TestUser", String.Empty, "TestSalst");
            bool result = user.Validate(operationExceptionCode).Equals("OMS-PASSWORD-ERROR");

            Assert.True(result);
        }

        [Fact]
        public void User_Validate_Password_InvalidLength()
        {
            string operationExceptionCode = null;
            User user = new User(Guid.NewGuid(), "TestUser", "TestPasswordTestPasswordTestPasswordTestPasswordTestPassword", "TestSalst");
            bool result = user.Validate(operationExceptionCode).Equals("OMS-PASSWORD-ERROR");

            Assert.True(result);
        }
    }
}
