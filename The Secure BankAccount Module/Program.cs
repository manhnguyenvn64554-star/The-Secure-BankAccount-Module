using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Secure_BankAccount_Module
{
    public class BankAccount
    {
        // TODO 1: Declare private fields (_balance, _pin, _failedAttempts)
        private decimal _balance;
        private string _pin;
        private int _failedAttempts;

        // TODO 2: Declare public AccountHolder property (read-only)
        public string AccountHolder { get; }

        // TODO 3: Declare IsLocked property with a private setter. Returns true if _failedAttempts >= 3.
        private bool _isLocked;

        public bool IsLocked
        {
            get
            {
                if (_failedAttempts >= 3)
                {
                    return true;
                }
                return _isLocked;
            }
            private set => _isLocked = value;
        }

        // Constructor
        public BankAccount(string accountHolder, decimal initialBalance, string initialPin)
        {
            AccountHolder = accountHolder;
            _balance = initialBalance > 0 ? initialBalance : 0;
            _pin = initialPin;
            _failedAttempts = 0;
            IsLocked = false;
        }

        // TODO 4: Implement Deposit method
        public bool Deposit(decimal amount)
        {
            if (IsLocked)
            {
                Console.WriteLine("ERROR: Account is locked.");
                return false;
            }
            else if (amount <= 0)
            {
                Console.WriteLine("ERROR: Deposit amount must be positive.");
                return false;
            }
             _balance += amount;
             Console.WriteLine($"Successfully deposited: ${amount:F2}");
             return true;
        }

        // TODO 5: Implement Withdraw method
        public bool Withdraw(decimal amount, string inputPin)
        {
            if (IsLocked)
            {
                Console.WriteLine("ERROR: Account is locked for security.");
                return false;
            }
            
            if (amount <= 0)
            {
                Console.WriteLine("ERROR: Withdrawal amount must be positive.");
                return false;
            }

            if (inputPin != _pin)
            {               
                _failedAttempts++;
                if (_failedAttempts <= 2)
                {
                    Console.WriteLine("ERROR: Incorrect PIN. Attempt: " + _failedAttempts + "/3");
                }
                else
                {
                    Console.WriteLine("ERROR: Too many attempts! Account has been locked.");
                }
                IsLocked = _failedAttempts >= 3;
                return false;
            }

            if (amount > _balance)
            {
                Console.WriteLine("ERROR: Insufficient funds.");
                return false;
            }
            _failedAttempts = 0;
            _balance -= amount;
            Console.WriteLine($"Successfully withdrew: ${amount:F2}");
            return true;
        }

        // TODO 6: Implement GetBalance method (PIN required)
        public decimal GetBalance(string inputPin)
        {
            if (IsLocked)
            {
                Console.WriteLine("ERROR: Account is locked.");
                return -1;
            }
            if (inputPin != _pin)
            {
                Console.WriteLine("ERROR: Incorrect PIN.");
                _failedAttempts++;
                IsLocked = _failedAttempts >= 3;
                return -1;
            }
            _failedAttempts = 0; 
            return _balance;
        }        

        // TODO 7: Implement ChangePin method
        public bool ChangePin(string currentPin, string newPin)
        {
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BankAccount account = new BankAccount("John Doe", 500.00m, "1234");

            Console.WriteLine($"Account Holder: {account.AccountHolder}");

            // Direct field access is impossible! (Uncommenting below will cause compiler errors)
            // account._balance = 1000000m; 
            // account._pin = "0000";

            Console.WriteLine("\n--- 1. Testing Deposit ---");
            account.Deposit(-50m); // Should fail
            account.Deposit(200m); // Should succeed

            Console.WriteLine("\n--- 2. Testing Protected Balance View ---");
            account.GetBalance("9999"); // Wrong PIN
            decimal currentBalance = account.GetBalance("1234"); // Correct PIN
            Console.WriteLine($"Verified Balance: ${currentBalance:F2}");

            Console.WriteLine("\n--- 3. Testing Lockout Mechanism ---");
            account.Withdraw(100m, "0000"); // Attempt 1 (Wrong)
            account.Withdraw(100m, "1111"); // Attempt 2 (Wrong)
            account.Withdraw(100m, "2222"); // Attempt 3 (Wrong -> Locks Account)

            // Further attempts should fail immediately due to lock
            account.Withdraw(100m, "1234"); // Correct PIN, but account is now locked!

            Console.WriteLine("\n--- 4. Account Lock Status ---");
            Console.WriteLine($"Is account locked? {account.IsLocked}");
        }
    }
}