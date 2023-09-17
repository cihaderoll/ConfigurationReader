# ConfigurationReader

Bu solution, normalde uygulama içerisinde tutulan ve bir güncelleme ya da ekleme gerektiğinde uygulamanın yeniden build ve deploy edilmesini gerektiren config bilgilerini dinamik olarak görüntülemek, silmek ya da güncellemek için kullanılacak bir proje grubunu içermektedir. 

.Net 5.0 ile yazılmış bir MVC projesidir. 

Solution içerisinde, MVC uygulamasının uzantıları olan Common, Core ve Domain class librarylerini yanında, ConfigurationReader adında, config değerlerinin istenilen tipte elde edilmesine olanak tanıyan bir adet class library bulunmaktadır.

Katmanlı mimari ile yazılmıştır.

Storage kısmında, kalıcı veri depolama için MSSQL Server, caching mechanizm olarak Redis kullanılmıştır.

MVC uygulamasında, veritabanı bağlantıları oluşturma aşamasında Repository ve UnitOfWork pattern kullanıldı. Bu sayede, uygulamaya ileriki zamanlardaki farklı geliştirmelere de açık hale getirilmiş ve kod tekrarından kaçınılmış oldu.

Hem MVC uygulamasında, hem class library içerisidne cachelene yapısı kullanılmıştır. MVC tarafında, Vewriler veritabanından çekildiği sırada cache datası da setlenerek, daha sonraki requestlerde çok daha hızlı response dönülmesi amaçlanmıştır. Ayrıca eğer bir config değeri gücellenmişse, yeni bir tane eklenmişse ya da varolan kayıt silinmişse, database operasyonları tamamlandıktan hemen sonra cache datası güncellenerek, olası data concurrency problemleri aşılmış oldu.

Class library içerisinde ise, MVC uygulaması tarafında belirlenen milisaniye cinsinden süre aralığında, düzenli olarak cache datası veritabanındaki verilerle senkronize edilmiş, yukarıda belirtilen problemlerin önüne geçilmiştir.

Class librray üzerinden configlere erişimin kesintiye uğramaması adına, düzenli aralıklarla, proje dizininde bulunan appconfig.json dosyası içerisine güncel config değerleri aktarılmaktadır. Bu sayede eğer sql veritabanına erişilemez ve cache de boşalmış ise, veriler bu appconfig.json dosyası üzerinden okunarak MVC uyghulamasına iletilecek, uygulama son güncel config bilgileri ile çalışmaya devam edecektir.

ConfigurationReader Class Library dışarıya 2 adet metot açmaktadır

![image](https://github.com/cihaderoll/ConfigurationReader/assets/54106470/7e78c4ff-a097-44ca-8cff-791ba4925f60)

GetValue<T> metodu, generic olarak, parametre olarak geçilen key ile eşleşen config kaydının 'Value' değerini döner. Örnek kullanım aşağıdaki gibidir.

![image](https://github.com/cihaderoll/ConfigurationReader/assets/54106470/d3e4873c-bbf3-425e-bc60-1076a9f7a398)

Burada, 'UseCache' ismine sahip config bilgisinin 'Value' değerinin 'bool' tipinde dönmesi istenmiştir.

GetAllConfigurations<T> metodu ise, yine aynı şekilde generic olarak, veri tabanında tutulan aktif kayıtların bilgi listesini istenen tipte döner.

![image](https://github.com/cihaderoll/ConfigurationReader/assets/54106470/644ca05f-cc91-43af-84b2-6e5ee7c9c4be)

Üst görseldeki arayüz üzerinden, config değerlerini listeleme, ekleme, güncelleme ve silme işlemleri yapılabilir. Ayrıca arama kısmı üzerinden istenen config değerine ulaşılabilir.

