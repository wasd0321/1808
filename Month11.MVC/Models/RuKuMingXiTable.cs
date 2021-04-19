using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Month11.MVC.Models
{
    public class RuKuMingXiTable
    {
        public int Mid        { get; set; }
        public int Ruid       { get; set; }
        public string MingName   { get; set; }
        public string MingBian   { get; set; }
        public string MingXian   { get; set; }
        public string MingCailIa { get; set; }
        public int Num        { get; set; }
        public int HanPrice   { get; set; }
        public int BuPrice    { get; set; }
        public int HanCount   { get; set; }
        public int BuCount    { get; set; }
        public int JianBiLi   { get; set; }
        public decimal JiaPrice   { get; set; }
        public decimal BuJiaPrice { get; set; }
    }
}
