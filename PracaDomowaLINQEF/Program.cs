using PracaDomowaLINQEF.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowaLINQEF
{
    class Program
    {
        static void Main()
        {
            // 1. Wyswietl podstawowe dane o pierwszym dostepny zleceniu
            // produkcyjnym (WorkOrder).
            //  ShowInfoAboutFirsttWorkOrder();

            // 2. Wyswietl informacje o produkcie, ktory byl produkowany na otrzymanym w zadaniu 
            // pierwszym zleceniu produkcyjnym
            // ShowInfoAboutProductFromFirstWorkOrder();

            //3. Wyswietl pierwsze 10 zlecen produkcyjnych.
            //4. Zmodyfikuj zadanie 3 tak, aby pominac pierwszych 10, a nastepnie wyswietlic nastepne 10 zlecen produkcyjnych oraz nazwe produktu jaki byl na tych zleceniach produkowany.
            // ShowTenFirstWorkOrders();

            //MIDJIM:

            //5. Wyswietl tylko te zlecenia produkcyjne, na ktorych byly jakieś odrzuty (moze Ci sie przydac 
            // kolumna ScrappedQty)
            // ShowWorkOrdersWithScraps();

            //6. Wyswietl pierwszych 10 zlecen z odrzutami - poinformuj jaki produkt byl produkowany, ile sie
            // mialo wyprodukowac, ile bylo odrzutow
            // ShowFirstTenWorkOrdersWithScraps();

            //7. Zmodyfikuj zadanie 6 tak, aby dodatkowo wyswietlic informacje o przyczynie odrzutu(ScrapReason)
            // Task6Extented();

            //8. Wyswietl informacje o najgorszych zleceniach produkcyjnych - tam, gdzie procent odrzutow do wszystkich 
            // wyprodukowanych byl najwiekszy - TOP 10 najgorszych zleceń produkcyjnych
            // ShowTop10OfWorstWorkOrders();

            //CZELENDŻING:

            //9. Uzywajac grupowania zlecen produkcyjnych wg produkowanego produktu, wyswietl ile 
            //danych produktow wyprodukowalismy
            //10. Posortuj dane orzymane w zadaniu 9 tak, zeby wiadomo bylo czego produkujemy najwiecej
            //ShowTotalProductionPerProduct();

            //11. Pogrupuj tym razem ilosc odrzutow w zleceniu produkcyjnym wg ScrapReason.Musimy wiedziec,
            // ktory ScrapReason powoduje najwiecej odrzutow.Wyswietl odpowiedni raport w konsoli.
            ShowOurProblemsInScrapReasons();

            Console.WriteLine();
            Console.WriteLine("finito");
            Console.ReadKey();
        }
        
        private static void ShowInfoAboutFirsttWorkOrder()
        {
            using (AW2012Context DbContext = new AW2012Context())
            {
                var workOrders = from wo in DbContext.WorkOrders
                                 orderby wo.WorkOrderID
                                 select new
                                 {
                                     wo.WorkOrderID,
                                     wo.OrderQty,
                                     wo.StartDate,
                                     wo.ScrappedQty,
                                 };
                var workOrder = workOrders.First();
                Console.WriteLine($"ID:{workOrder.WorkOrderID}, Qty:{workOrder.OrderQty}, StartDate:{workOrder.StartDate}");

            }
        }

        // 2. Wyswietl informacje o produkcie, ktory byl produkowany na otrzymanym w zadaniu 
        // pierwszym zleceniu produkcyjnym
        private static void ShowInfoAboutProductFromFirstWorkOrder()
        {
            using (AW2012Context DbContext = new AW2012Context())
            {
                var workOrders = from wo in DbContext.WorkOrders
                                 join pr in DbContext.Products
                                 on wo.ProductID equals pr.ProductID
                                 orderby wo.WorkOrderID
                                 select new
                                 {
                                     pr.Name,
                                     pr.Class,
                                     pr.ListPrice
                                 };
                var workOrder = workOrders.First();
                Console.WriteLine($"Product Class: {workOrder.Class}, Product Name: {workOrder.Name} Price: {workOrder.ListPrice}");

            }
        }

        //3. Wyswietl pierwsze 10 zlecen produkcyjnych.
        private static void ShowTenFirstWorkOrders()
        {
            using (AW2012Context DbContext = new AW2012Context())
            {
                var workOrders = from wo in DbContext.WorkOrders
                                 orderby wo.WorkOrderID
                                 select new
                                 {
                                     wo.WorkOrderID,
                                     wo.OrderQty,
                                     wo.StartDate,
                                     wo.ScrappedQty,
                                 };

                var tenWorkOrders = workOrders.ToList().Take(10);
                // Zad 4 
                //var workOrder = workOrders.ToList().Skip(10).Take(10);

                foreach (var w in tenWorkOrders)
                {
                    Console.WriteLine($"ID:{w.WorkOrderID}, Qty:{w.OrderQty}, StartDate:{w.StartDate}");
                }

            }
        }
        //MIDJIM:

        //5. Wyswietl tylko te zlecenia produkcyjne, na ktorych byly jakieś odrzuty (moze Ci sie przydac 
        // kolumna ScrappedQty)
        private static void ShowWorkOrdersWithScraps()
        {
            using (AW2012Context dbContext = new AW2012Context())
            {
                //var workOrdersWithScraps
                //from wo in dbContext.WorkOrders
                //             where wo.ScrappedQty > 0
                //             orderby wo.WorkOrderID
                //             select new
                //             {
                //                 wo.WorkOrderID,
                //                 wo.OrderQty,
                //                 wo.ScrappedQty,
                //                 wo.ScrapReasonID
                //             };

                // var workOrder = workOrdersWithScraps.ToList();

                var workOrdersWithScraps = dbContext.WorkOrders.Where(wo => wo.ScrappedQty > 0).OrderBy(wo => wo.WorkOrderID).ToList();

                foreach (var w in workOrdersWithScraps)
                {
                    Console.WriteLine($"ID: {w.WorkOrderID,-10} Ordered Qty: {w.OrderQty,-6}  Scrapped Qty: {w.ScrappedQty,-6}  Scrap Reason Id: {w.ScrapReasonID}");
                }
            }
        }

        //6. Wyswietl pierwszych 10 zlecen z odrzutami - poinformuj jaki produkt byl produkowany, ile sie
        // mialo wyprodukowac, ile bylo odrzutow
        private static void ShowFirstTenWorkOrdersWithScraps()
        {
            using (AW2012Context dbContext = new AW2012Context())
            {
                var workOrders = from wo in dbContext.WorkOrders
                                 join pr in dbContext.Products
                                 on wo.ProductID equals pr.ProductID
                                 where wo.ScrappedQty > 0
                                 orderby wo.WorkOrderID
                                 select new
                                 {
                                     pr.Name,
                                     wo.OrderQty,
                                     wo.ScrappedQty,
                                 };

                var workOrder = workOrders.ToList();
                foreach (var w in workOrder)
                {
                    Console.WriteLine($"Poduct Name: {w.Name,-35} Ordered Qty: {w.OrderQty,-4}  Scrapped Qty: {w.ScrappedQty,-4}");
                }
            }
        }

        //7. Zmodyfikuj zadanie 6 tak, aby dodatkowo wyswietlic informacje o przyczynie odrzutu(ScrapReason)
        private static void Task6Extented()
        {
            using (AW2012Context dbContext = new AW2012Context())
            {
                var workOrders = from wo in dbContext.WorkOrders
                                 join pr in dbContext.Products
                                 on wo.ProductID equals pr.ProductID
                                 join scr in dbContext.ScrapReasons
                                 on wo.ScrapReasonID equals scr.ScrapReasonID
                                 where wo.ScrappedQty > 0
                                 orderby wo.WorkOrderID
                                 select new
                                 {
                                     pr.Name,
                                     wo.OrderQty,
                                     wo.ScrappedQty,
                                     scrapReason = scr.Name
                                 };

                var workOrder = workOrders.ToList();
                foreach (var w in workOrder)
                {
                    Console.WriteLine($"Poduct Name: {w.Name,-35} Ordered Qty: {w.OrderQty,-4}  Scrapped Qty: {w.ScrappedQty,-4} Scrap Reason: {w.scrapReason}");
                }
            }
        }
        //8. Wyswietl informacje o najgorszych zleceniach produkcyjnych - tam, gdzie procent odrzutow do wszystkich 
        // wyprodukowanych byl najwiekszy - TOP 10 najgorszych zleceń produkcyjnych
        private static void ShowTop10OfWorstWorkOrders()
        {
            using (AW2012Context dbContext = new AW2012Context())
            {
                var workOrders = from wo in dbContext.WorkOrders
                                 join pr in dbContext.Products
                                 on wo.ProductID equals pr.ProductID
                                 join scr in dbContext.ScrapReasons
                                 on wo.ScrapReasonID equals scr.ScrapReasonID
                                 where wo.ScrappedQty > 0
                                 select new
                                 {
                                     pr.Name,
                                     wo.OrderQty,
                                     wo.ScrappedQty,
                                     percentOfScrapped = (float)wo.ScrappedQty * 100 / wo.OrderQty,
                                     scrapReason = scr.Name
                                 };

                var workOrder = workOrders.ToList().OrderByDescending(x => x.percentOfScrapped).Take(10);
                foreach (var w in workOrder)
                {
                    Console.WriteLine($"Product Name: {w.Name,-25} %: {w.percentOfScrapped,-10} Ordered Qty: " +
                        $"{w.OrderQty,-8} Scrapped Qty:{w.ScrappedQty}");
                }
            }
        }
        //CZELENDŻING:

        //9. Uzywajac grupowania zlecen produkcyjnych wg produkowanego produktu, wyswietl ile 
        //danych produktow wyprodukowalismy
        private static void ShowTotalProductionPerProduct()
        {
            using (AW2012Context dbContext = new AW2012Context())
            {
                var production = from wo in dbContext.WorkOrders
                                 join pr in dbContext.Products
                                 on wo.ProductID equals pr.ProductID
                                 group wo by pr.ProductID into grouppedproduction

                                 //10. Posortuj dane orzymane w zadaniu 9 tak, zeby wiadomo bylo czego produkujemy najwiecej
                                 orderby grouppedproduction.Sum(p => p.OrderQty) descending
                                 // koniec zadania 10

                                 select new
                                 {
                                     ProductID = grouppedproduction.Key,
                                     prPerProd = grouppedproduction.ToList()
                                 };
                foreach (var p in production)
                {
                    var productName = p.prPerProd.Select(pr => pr.Product.Name).First();
                    var prodQty = p.prPerProd.Sum(pr => pr.OrderQty);
                    Console.WriteLine($"{productName,-30} production Qty: {prodQty}");
                }
            }
        }

        //11. Pogrupuj tym razem ilosc odrzutow w zleceniu produkcyjnym wg ScrapReason.Musimy wiedziec,
        // ktory ScrapReason powoduje najwiecej odrzutow.Wyswietl odpowiedni raport w konsoli.
        private static void ShowOurProblemsInScrapReasons()
        {
            using (AW2012Context dbContext = new AW2012Context())
            {
                var workOrders = from wo in dbContext.WorkOrders                                
                                 join scr in dbContext.ScrapReasons
                                 on wo.ScrapReasonID equals scr.ScrapReasonID
                                 where wo.ScrappedQty > 0
                                 group wo by scr.ScrapReasonID into grouppedWorkOrders
                                 orderby grouppedWorkOrders.Sum(w=>w.ScrappedQty) descending
                                 select new
                                 {
                                     ScrapReasonID = grouppedWorkOrders.Key,
                                     groupWO = grouppedWorkOrders.ToList()
                                 };
                foreach (var wor in workOrders)
                {
                    Console.WriteLine( $"Scrap Reason: {wor.groupWO.Select(w=>w.ScrapReason.Name).First(),-35} occured {wor.groupWO.Sum(q=>q.ScrappedQty)} times.");
                }                
            }
        }
    }
}
