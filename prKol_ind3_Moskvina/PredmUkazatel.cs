using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prKol_ind3_Moskvina
{
    internal class PredmUkazatel
    {
        public string Word { get; set; }
        public List<int> Pages { get; set; }
        private List<PredmUkazatel> entries = new List<PredmUkazatel>();
        public PredmUkazatel(string word, List<int> pages)
        {
            Word = word;
            Pages = pages.Take(10).ToList();
        }
        public void AddOrUpdate(string word, List<int> pages)
        {
            var existing = entries.FirstOrDefault(e => e.Word.Equals(word, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.Pages = pages.Take(10).ToList();
            }
            else
            {
                entries.Add(new PredmUkazatel(word, pages));
            }
        }
        public void Add(string word, string pagesInp)
        {
            var pages = pagesInp.Split(',').Select(p => int.Parse(p.Trim())).Distinct().OrderBy(p => p).Take(10).ToList();
            AddOrUpdate(word, pages);
        }
        public void LoadFromFile(string name)
        {
            if (!File.Exists(name))
            {
                throw new FileNotFoundException($"Файл {name} не найден");
            }
            var lines = File.ReadAllLines(name);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    var word = parts[0].Trim();
                    var pages = parts[1].Split(',').Select(p => int.Parse(p.Trim())).Distinct().OrderBy(p => p).Take(10).ToList();
                    AddOrUpdate(word, pages);
                }
            }
        }
        
    }
}
