using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace ChatApplication.Bot
{
    public class Api
    {
        private const string Url = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";

        private static readonly HttpClient client = new HttpClient();

        public async Task<Csv> Consume(string code)
        {
            var respuesta = await client.GetAsync(string.Format(Url, code));
            return Pros(await respuesta.Content.ReadAsStreamAsync());
        }

        public Csv Pros(Stream contenido)
        {
            using (var csv = new CsvReader(new StreamReader(contenido), CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Csv>().FirstOrDefault();
            }
        }
    }
}
