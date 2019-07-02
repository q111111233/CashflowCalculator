using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace CashflowCalculator.Controllers
{
    [Route("api/[controller]")]
    public class LoanController : Controller
    {
        public static List<Loan> loans = new List<Loan>();
        public List<CashFlow> CashFlows = new List<CashFlow>();
        public CashFlow Pool;

        [HttpGet]
        public List<CashFlow> CashFlowCalculate()
        {
            foreach (var loan in loans) {
                CashFlows.Add(new CashFlow(loan));
            }

            return CashFlows;
        }

        [HttpGet("[action]")]
        public CashFlow getPool()
        {
            CashFlow Pool = new CashFlow();
            if (loans.Count != 0)
            {
                foreach (var loan in loans)
                {
                    CashFlows.Add(new CashFlow(loan));
                }
                for (var i = 0; i <= loans.Max(t => t.Term); i++)
                {
                    Pool.MonthPays.Add(new MonthPay());

                    foreach (var cashFlow in CashFlows)
                    {
                        if (cashFlow.MonthPays.Count <= loans.Max(t => t.Term))
                            cashFlow.MonthPays.Add(new MonthPay());
                        Pool.MonthPays[i].Month = i;
                        Pool.MonthPays[i].Interest += cashFlow.MonthPays[i].Interest;
                        Pool.MonthPays[i].Principal += cashFlow.MonthPays[i].Principal;
                        Pool.MonthPays[i].RemBalance += cashFlow.MonthPays[i].RemBalance;
                    }
                }
            }
            return Pool;
        }

        [HttpPost]
        public ActionResult<Loan> PostLoan([FromBody]Loan loan)
        {
            loans.Add(loan);

            return loan;
        }

        public class Loan
        {
            public double Balance { get; set; }
            public int Term { get; set; }
            public double Rate { get; set; }
        }

        public class CashFlow
        {
            public List<MonthPay> MonthPays { get; set; }
            private readonly double monthlyPayment;

            public CashFlow()
            {
                MonthPays = new List<MonthPay>();
            }

            public CashFlow(Loan loan) {
                MonthPays = new List<MonthPay>();
                monthlyPayment = loan.Balance * (loan.Rate / 1200) / (1 - Math.Pow((1 + loan.Rate / 1200) , (-loan.Term)));
                MonthPays.Add(new MonthPay
                {
                    Month = 0,
                    Interest = loan.Balance * (loan.Rate / 1200),
                    Principal = monthlyPayment - loan.Balance * (loan.Rate / 1200),
                    RemBalance = loan.Balance,
                });
                
                for (var i = 1; i <= loan.Term; i++)
                {
                    MonthPays.Add(new MonthPay
                    {
                        Month = i,
                        Interest = MonthPays.Last().RemBalance * loan.Rate / 1200,
                        Principal = monthlyPayment - MonthPays.Last().RemBalance * loan.Rate / 1200,
                        RemBalance = MonthPays.Last().RemBalance - (monthlyPayment - MonthPays.Last().RemBalance * loan.Rate / 1200),
                    });
                }
            }
        }

        public class MonthPay
        {
            public int Month { get; set; }
            public double Interest { get; set; }
            public double Principal { get; set; }
            public double RemBalance { get; set; }
        }
    }
}
