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
                    totalWithoutTax += Price[i] * ((decimal)Quantitie[i]);
                }
                decimal totalWithTaxe = 0;
                switch (order.Country)
                {
                    case "DE":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    case "UK":
                        totalWithTaxe = totalWithoutTax * 1.21m;
                        break;
                    case "FR":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    case "IT":
                        totalWithTaxe = totalWithoutTax * 1.25m;
                        break;
                    case "ES":
                        totalWithTaxe = totalWithoutTax * 1.19m;
                        break;
                    case "PL":
                        totalWithTaxe = totalWithoutTax * 1.21m;
                        break;
                    case "RO":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    case "NL":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    case "BE":
                        totalWithTaxe = totalWithoutTax * 1.24m;
                        break;
                    case "EL":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    case "CZ":
                        totalWithTaxe = totalWithoutTax * 1.19m;
                        break;
                    case "PT":
                        totalWithTaxe = totalWithoutTax * 1.23m;
                        break;
                    case "HU":
                        totalWithTaxe = totalWithoutTax * 1.27m;
                        break;
                    case "SE":
                        totalWithTaxe = totalWithoutTax * 1.23m;
                        break;
                    case "AT":
                        totalWithTaxe = totalWithoutTax * 1.22m;
                        break;
                    case "BG":
                        totalWithTaxe = totalWithoutTax * 1.21m;
                        break;
                    case "DK":
                        totalWithTaxe = totalWithoutTax * 1.21m;
                        break;
                    case "FI":
                        totalWithTaxe = totalWithoutTax * 1.17m;
                        break;
                    case "SK":
                        totalWithTaxe = totalWithoutTax * 1.18m;
                        break;
                    case "IE":
                        totalWithTaxe = totalWithoutTax * 1.21m;
                        break;
                    case "HR":
                        totalWithTaxe = totalWithoutTax * 1.23m;
                        break;
                    case "LT":
                        totalWithTaxe = totalWithoutTax * 1.23m;
                        break;
                    case "SI":
                        totalWithTaxe = totalWithoutTax * 1.24m;
                        break;
                    case "LV":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    case "EE":
                        totalWithTaxe = totalWithoutTax * 1.22m;
                        break;
                    case "CY":
                        totalWithTaxe = totalWithoutTax * 1.21m;
                        break;
                    case "LU":
                        totalWithTaxe = totalWithoutTax * 1.25m;
                        break;
                    case "MT":
                        totalWithTaxe = totalWithoutTax * 1.2m;
                        break;
                    default:
                        return null;

                }
                bill = new Bill();
                bill.total = totalWithTaxe;
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