namespace DomainModels
{
    public class Wallet
    {
        //private double stocks;
        //private double balance;
        public Wallet(double balance)
        {
            //this.balance = balance;
            Balance = balance;
        }

        //public double WithdrawMoney(double amount)
        //{
        //    if (Balance >= amount)
        //    {
        //        return this.balance = balance - amount;
        //    }
        //    return 0;
        //}

        //public double WithdrawStocks(double amount)
        //{
        //    if(Stocks >= amount)
        //    {
        //        return this.stocks = stocks - amount;
        //    }
        //    return 0;
        //}


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
