# PHONE_BOOK
Rise Technology firmasının assessment projesidir. Uygulama microservis mimarisiyle tasarlanmış bir telefon rehberi uygulamasıdır. Çalışmadan beklenen genel yapı :

- Minimum, Report ve PersonProcesses olmak üzere 2 micro servis olmalıdır.
- PersonProcesses micro servisinde kişilere ve kişilere ait iletişim bilgilerinin (Kişinin 1'e N ilişkili iletişim bilgisi olabilir) temel CRUD işlemleri yapılmaktadır. 
- Report micro servisinde rapor oluşturma, raporun detaylarını görüntüleme ve tüm raporları listeleme özellikleri bulunmaktadır. Ek olarak rapor oluşturma talebinden sonra rapor oluşturma süreci mesaj kuyruğu kullanan bir yapı ile arkaplan işlemi olarak devam edecektir. Diğer micro servis ile iletişimi HTTP veya AMQ üzerinden yapacaktır.

# Kullanılan Teknolojiler

- [.NET 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)

- [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)

- [PostGreSQL](https://www.postgresql.org/)

- [RabbitMQ](https://www.rabbitmq.com/)

- [Docker](https://www.docker.com/)

- [xUnit](https://xunit.net/)

- [Moq](https://github.com/moq)

- [Coverlet](https://github.com/coverlet-coverage/coverlet)

# Başlarken

1) [**Docker**](https://www.docker.com/)'ın bilgisayarınızda yüklü olduğundan emin olunuz. Ardından aşağıdaki komut ile RabbitMq'u Docker üzerinden çalıştırınız.

```bash
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
```

2. [**PostgreSQL**](https://www.postgresql.org/)'nin bilgisayarınızda yüklü olduğundan emin olunuz. Ardından aşağıdaki adımları uygulayınız.
   1. **PersonProcesses.API** içerisinde bulunan **appsettings.json** dosyasındaki **"ConnectionStrings"** içerisinde bulunan **User ID**, **Password** ve **Host** bilgilerini kendinize uygun şekilde düzenleyeniz.
   2. **Report.API** içerisinde bulunan **appsettings.json** dosyasındaki **"ConnectionStrings"** içerisinde bulunan **User ID**, **Password** ve **Host** bilgilerini kendinize uygun şekilde düzenleyeniz.

3. **RabbitMQ** bağlantı bilgisini kendinize göre düzenlemek isterseniz eğer aşağıdaki adımları uygulayınız. 

   1. **PersonProcesses.API** içerisinde bulunan **appsettings.json** dosyasındaki **"Options"** içerisinde bulunan **RabbitMqCon** bilgisini kendinize uygun şekilde düzenleyeniz.

      **NOT:** Default ayarlarla kullanmak isterseniz eğer değişiklik yapmanıza gerek yoktur.
   
   2. **Report.API** içerisinde bulunan **appsettings.json** dosyasındaki **"Options"** içerisinde bulunan **RabbitMqCon** bilgisini kendinize uygun şekilde düzenleyeniz.
   
         **NOT:** Default ayarlarla kullanmak isterseniz eğer değişiklik yapmanıza gerek yoktur.
   
4. **PersonProcesses.API** için **Report.API** içerisinde bulunan **appsettings.json** dosyasındaki **ApiUrl** bilgisini kendinize uygun şekilde düzenleyiniz.

   > **NOT:** Projeler **IIS** üzerinden ayağa kaldırılacaksa eğer **4**. ve **5.** maddelerde değişiklik yapmanıza gerek yoktur.
5.  **PersonProcesses.API** klasörü içerisinde bir **terminal** açıp aşağıdaki komut ile **PersonProcesses.API** projesini çalıştırabilirsiniz.

      ```bash
      dotnet run
      ```

6. **Report.API** klasörü içerisinde bir **terminal** açıp aşağıdaki komut ile **Report.API** projesini çalıştırabilirsiniz.

      ```bash
      dotnet run
      ```

7. Projeler varsayılan ayarlar ile derlenip, çalıştırıldığında aşağıdaki url'ler üzerinden **swagger** arayüzüne ulaşabilirsiniz.

      ```
      PersonProcesses.API Url: https://localhost:7218/swagger/index.html
      Report.API Url   : https://localhost:7075/swagger/index.html
      ```
8. **Coverlot** için **terminal** açıp aşağıdaki komutları izleyebilirsiniz.

      ```
       cd test_klasor_yolu
       dotnet add package coverlet.msbuild
       dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
      ```
      
# Unit Test Code Coverage Sonuçları

![PaplikaStudioCoverlot](https://user-images.githubusercontent.com/112504947/188114256-5b9dd499-ecbb-4ce8-91e9-7bc124ca40aa.JPG)
