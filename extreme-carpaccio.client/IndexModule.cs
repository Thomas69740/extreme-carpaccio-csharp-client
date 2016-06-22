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
                switch (order.Country)
                {
                    case "DE":
                    case "FR":
                    case "RO":
                    case "NL":
                    case "EL":
                    case "LV":
                    case "MT":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    case "UK":
                    case "PL":
                    case "BG":
                    case "DK":
                    case "IE":
                    case "CY":
                        totalWithTaxe = totalWithoutTax * 1.21m;
                        break;
                    case "IT":
                    case "LU":
                        totalWithTaxe = totalWithoutTax * 1.25m;
                        break;
                    case "ES":
                    case "CZ":
                        totalWithTaxe = totalWithoutTax * 1.19m;
                        break;
                    case "BE":
                    case "SI":
                        totalWithTaxe = totalWithoutTax * 1.24m;
                        break;
                    case "PT":
                    case "SE":
                    case "HR":
                    case "LT":
                        totalWithTaxe = totalWithoutTax * 1.23m;
                        break;
                    case "HU":
                        totalWithTaxe = totalWithoutTax * 1.27m;
                        break;
                    case "AT":
                    case "EE":
                        totalWithTaxe = totalWithoutTax * 1.22m;
                        break;
                    case "FI":
                        totalWithTaxe = totalWithoutTax * 1.17m;
                        break;
                    case "SK":
                        totalWithTaxe = totalWithoutTax * 1.18m;
                        break;
                    default:
                        return null;

                }
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