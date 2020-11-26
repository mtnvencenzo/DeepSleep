namespace DateTest
{
    using System;
    using System.Globalization;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine(System.Environment.OSVersion.ToString());
            Console.WriteLine("-------------------------");
            Console.WriteLine("");
            Console.WriteLine("Outputting DateTime-Local");
            Console.WriteLine("");

            DateTimeOffset now = DateTimeOffset.Now;
            DateTime utc = DateTime.UtcNow;


            Console.WriteLine($"{now.ToString("o", CultureInfo.InvariantCulture)} - ToString(o {nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{now.ToString("o", CultureInfo.GetCultureInfo("en"))} - ToString(o en)");
            Console.WriteLine($"{now.ToString("r", CultureInfo.InvariantCulture)} - ToString(r {nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{now.ToString("r", CultureInfo.GetCultureInfo("en"))} - ToString(r en)");
            Console.WriteLine($"{now.ToString("s", CultureInfo.InvariantCulture)} - ToString(s {nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{now.ToString("s", CultureInfo.GetCultureInfo("en"))} - ToString(s en)");
            Console.WriteLine($"{now.ToString(CultureInfo.InvariantCulture)} - ToString({nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{now.ToString(CultureInfo.GetCultureInfo("en"))} - ToString(en)");
            Console.WriteLine($"{now.ToString()} - ToString()");


            Console.WriteLine("");
            Console.WriteLine("Outputting DateTime-UTc");
            Console.WriteLine("");
            Console.WriteLine($"{utc.ToString("o", CultureInfo.InvariantCulture)} - ToString(o {nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{utc.ToString("r", CultureInfo.InvariantCulture)} - ToString(r {nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{utc.ToString("s", CultureInfo.InvariantCulture)} - ToString(s {nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{utc.ToString(CultureInfo.InvariantCulture)} - ToString({nameof(CultureInfo.InvariantCulture)})");
            Console.WriteLine($"{utc.ToString()} - ToString()");

            Console.WriteLine("");
            Console.WriteLine("");
            ////List<string> list = new List<string>();
            //foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(ci => ci.TwoLetterISOLanguageName))
            //{
            //    var culture = ci;

            //    try
            //    {
            //        culture = System.Globalization.CultureInfo.CreateSpecificCulture(ci.Name);
            //    }
            //    catch { }

            //    //Console.WriteLine(String.Format("{0,-12}{1}", ci.Name, ci.EnglishName));

            //    Console.WriteLine($"{now.ToString(culture)} - ToString({culture.Name})");
            //}

            Console.WriteLine("Finshed, press any key to exit");
            Console.ReadKey();
        }




        //var en = CultureInfo.GetCultureInfo("en");
        //    var gb = CultureInfo.GetCultureInfo("gb");
        //    var af = CultureInfo.GetCultureInfo("af");
        //    var ar = CultureInfo.GetCultureInfo("ar");
        //    var ba = CultureInfo.GetCultureInfo("ba");
        //    var da = CultureInfo.GetCultureInfo("da");
        //    var es = CultureInfo.GetCultureInfo("es");
        //    var fa = CultureInfo.GetCultureInfo("fa");
        //    var fr = CultureInfo.GetCultureInfo("fr");
        //    var it = CultureInfo.GetCultureInfo("it");
        //    var ja = CultureInfo.GetCultureInfo("ja");

        //    Console.WriteLine($"{now.ToString()} - ToString()");
        //    Console.WriteLine($"{now.ToString(CultureInfo.InvariantCulture)} - ToString(CultureInfo.InvariantCulture)");
        //    Console.WriteLine($"{now.ToString(CultureInfo.CurrentCulture)} - ToString(CultureInfo.CurrentCulture)");
        //    Console.WriteLine($"{now.ToString(en)} - ToString(en)");
        //    Console.WriteLine($"{now.ToString(gb)} - ToString(gb)");
        //    Console.WriteLine($"{now.ToString(af)} - ToString(af)");
        //    Console.WriteLine($"{now.ToString(ar)} - ToString(ar)");
        //    Console.WriteLine($"{now.ToString(ba)} - ToString(ba)");
        //    Console.WriteLine($"{now.ToString(da)} - ToString(da)");
        //    Console.WriteLine($"{now.ToString(es)} - ToString(es)");
        //    Console.WriteLine($"{now.ToString(fa)} - ToString(fa)");
        //    Console.WriteLine($"{now.ToString(fr)} - ToString(fr)");
        //    Console.WriteLine($"{now.ToString(it)} - ToString(it)");
        //    Console.WriteLine($"{now.ToString(ja)} - ToString(ja)");
    }
}
