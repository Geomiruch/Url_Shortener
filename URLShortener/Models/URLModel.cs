using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortener.Data;

namespace URLShortener.Models
{
    public class URLModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public string OriginalURL { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Owner { get; set; }

        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        
    }


}




