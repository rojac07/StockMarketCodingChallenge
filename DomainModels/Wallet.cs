namespace DomainModels
{
    public class Wallet
    {
        public Wallet(double balance)
        {
            this.Balance = balance;
            this.Stocks = 0;
        }

        /// <summary>
        /// Amount of stocks in my account.
        /// </summary>
        public double Stocks { get; set; }

        /// <summary>
        /// Remaining $ balance.
        /// </summary>
        public double Balance { get; set; }
    }
}
