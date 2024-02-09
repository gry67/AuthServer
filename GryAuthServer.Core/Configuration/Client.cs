using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryAuthServer.Core.Configuration
{
    public class Client
    {
        public string Id { get; set; } 
        public string Secret { get; set; }

        //Client'ın hangi domainlere erişebileceğini yazdığımız liste
        public List<string> Audiences { get; set; }

    }// Kullanıcı adı ve şifre OLMADAN token alan client'ı temsil eden sınıf. Poco yada dto değil. Client görmez
}
