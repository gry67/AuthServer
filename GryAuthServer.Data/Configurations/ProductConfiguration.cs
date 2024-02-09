using GryAuthServer.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryAuthServer.Data.Configurations
{
    //IEntityConfiguration arayüzü,Contexteki onModelCreating metodunun dışında,başka sınıflarda entity
    //konfigürasyonu yapmamızı sağlar. Product için Ayrı bir class açıp bu arayüzü implemente ettik
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);// Id Key olarak işaretlendi
            
            //Name doldurulması zorunlu oldu. max uzunluğu 200 karakter
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Stock).IsRequired();

            //Price propunun kolon biçimi decimal oldu. Max 18 karakter alabilir ve virgülden sonra 2 karakter alır
            builder.Property(x=>x.Price).HasColumnType("decimal(18,2)");

            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
