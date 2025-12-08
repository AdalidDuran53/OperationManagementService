using EntitiesCustom;

namespace OperationManagementServicesTest
{
    public class TransactionTest
    {
        #region insert
        [Fact]
        public void Transaction_ValidateInsert_OK()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), "TestTransaction", 100.00m, DateTime.Now, false, false);
            bool result = String.IsNullOrEmpty(transaction.Validate(operationExceptionCode));

            Assert.True(result);
        }

        [Fact]
        public void Transaction_ValidateInsert_TransactionName_Empty()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), String.Empty, 100.00m, DateTime.Now, false, false);
            bool result = transaction.Validate(operationExceptionCode).Equals("OMS-TRANSACTIONNAME-ERROR");

            Assert.True(result);
        }

        [Fact]
        public void Transaction_ValidateInsert_TransactionAmount_Empty()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), "TestTransaction", null, DateTime.Now, false, false);
            bool result = transaction.Validate(operationExceptionCode).Equals("OMS-TRANSACTIONAMOUNT-ERROR");

            Assert.True(result);
        }

        [Fact]
        public void Transaction_ValidateInsert_TransactionName_InvalidLength()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), "TestTransactionTestTransactionTestTransactionTestTransactionTestTransaction", 100.00m, DateTime.Now, false, false);
            bool result = transaction.Validate(operationExceptionCode).Equals("OMS-TRANSACTIONNAME-ERROR");

            Assert.True(result);
        }
        #endregion



        #region update
        [Fact]
        public void Transaction_ValidateUpdate_OK()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), "TestTransaction", 100.00m, DateTime.Now, false, true);
            bool result = String.IsNullOrEmpty(transaction.Validate(operationExceptionCode));

            Assert.True(result);
        }

        [Fact]
        public void Transaction_ValidateUpdate_TransactionName_Empty()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), String.Empty, 100.00m, DateTime.Now, false, true);
            bool result = String.IsNullOrEmpty(transaction.Validate(operationExceptionCode));

            Assert.True(result);
        }

        [Fact]
        public void Transaction_ValidateUpdate_TransactionAmount_Empty()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), "TestTransaction", null, DateTime.Now, false, true);
            bool result = String.IsNullOrEmpty(transaction.Validate(operationExceptionCode));

            Assert.True(result);
        }

        [Fact]
        public void Transaction_ValidateUpdate_TransactionName_InvalidLength()
        {
            string operationExceptionCode = null;
            Transaction transaction = new Transaction(1, Guid.NewGuid(), "TestTransactionTestTransactionTestTransactionTestTransactionTestTransaction", 100.00m, DateTime.Now, false, true);
            bool result = transaction.Validate(operationExceptionCode).Equals("OMS-TRANSACTIONNAME-ERROR");

            Assert.True(result);
        }
        #endregion
    }
}
