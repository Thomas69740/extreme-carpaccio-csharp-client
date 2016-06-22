using System.Collections.Generic;
using System.IO;
using System.Text;

namespace xCarpaccio.client
{
    using Nancy;
    using System;
    using Nancy.ModelBinding;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Dictionary<string, decimal> taxes = new Dictionary<string, decimal>()
            {
                { "DE", 1.2m},
                {"FR", 1.2m},
                {"RO", 1.2m},
                {"NL", 1.2m},
                {"EL", 1.2m},
                {"LV", 1.2m},
                {"MT", 1.2m},
                {"UK", 1.21m},
                {"PL", 1.21m},
                {"BG", 1.21m},
                {"DK", 1.21m},
                {"IE", 1.21m},
                {"CY", 1.21m},
                {"IT", 1.25m},
                {"LU", 1.25m},
                {"ES", 1.19m},
                {"CZ", 1.19m},
                {"BE", 1.24m},
                {"SI", 1.24m},
                {"PT", 1.23m},
                {"SE", 1.23m},
                {"HR", 1.23m},
                {"LT", 1.23m},
                {"HU", 1.27m},
                {"AT", 1.22m},
                {"EE", 1.22m},
                {"FI", 1.17m},
                {"SK", 1.18m},
            };

            Get["/"] = _ => "It works !!! You need to register your server on main server.";

            Post["/order"] = _ =>
            {
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    Console.WriteLine("Order received: {0}", reader.ReadToEnd());
                }

                var order = this.Bind<Order>();
                Bill bill = null;
                decimal[] Price = order.Prices;
                int[] Quantitie = order.Quantities;
                if (Price.Length != Quantitie.Length)
                {
                    return null;
                }
                decimal totalWithoutTax = 0;
                for (int i = 0; i < Price.Length; i++)
                {
                    totalWithoutTax += ((decimal)Price[i]) * ((decimal)Quantitie[i]);
                }
                decimal totalWithTaxe = 0;

                if (!taxes.ContainsKey(order.Country))
                    return null;
                totalWithTaxe = totalWithoutTax*taxes[order.Country];
                decimal totalWithReduction = 0;
                if (order.Reduction == "STANDARD")
                {
                    if (totalWithTaxe >= 50000)
                        totalWithReduction = totalWithTaxe*0.85m;
                    else if (totalWithTaxe >= 10000)
                        totalWithReduction = totalWithTaxe*0.9m;
                    else if (totalWithTaxe >= 7000)
                        totalWithReduction = totalWithTaxe*0.93m;
                    else if (totalWithTaxe >= 5000)
                        totalWithReduction = totalWithTaxe*0.95m;
                    else if (totalWithTaxe >= 1000)
                        totalWithReduction = totalWithTaxe*0.97m;
                    else
                        totalWithReduction = totalWithTaxe;
                }
                else if (order.Reduction == "PAY THE PRICE")
                {
                    totalWithReduction = totalWithTaxe;
                }
                else
                {
                    return null;
                }
                
                bill = new Bill();
                bill.total = totalWithReduction;
                //TODO: do something with order and return a bill if possible
                // If you manage to get the result, return a Bill object (JSON serialization is done automagically)
                // Else return a HTTP 404 error : return Negotiate.WithStatusCode(HttpStatusCode.NotFound);

                return bill;
            };

            Post["/feedback"] = _ =>
            {
                var feedback = this.Bind<Feedback>();
                Console.Write("Type: {0}: ", feedback.Type);
                Console.WriteLine(feedback.Content);
                return Negotiate.WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}