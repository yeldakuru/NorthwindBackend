
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Product : IEntity//class çıplak kalmasın inheritence (kalıtım)
                                  //yada  implementasyon verilmeli.implementasyon imza tekniği kullanıldı 
    {
                        //kodlamayı ileride kısıtlamak ve önümüzü görmek için
    
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
    }  //product nesnesi hazırlandı

}











