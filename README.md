# Deepfake MVC Web Uygulaması

Bu proje, ASP.NET Core MVC ile geliştirilmiş bir web arayüzüdür. Kullanıcılar sisteme giriş yaparak `.mp4` formatında video yükleyebilir ve videonun deepfake (sahte) olup olmadığını analiz ettirebilir.

## 🔍 Özellikler

- 👤 Kullanıcı kayıt ve giriş sistemi
- 📤 Video yükleme arayüzü
- 🔗 C# Web API üzerinden Flask API'ye video gönderme
- ✅ Gerçek/sahte (real/fake) analizi sonucunun görsel olarak sunulması

## 📦 Bağımlılıklar

- ASP.NET Core MVC (.NET 8.0)
- C# Web API (IHttpClientFactory ile Flask API’ye bağlanır)
- Bootstrap (arayüz düzenlemeleri için)
- Newtonsoft.Json (API dönüşünü ayrıştırmak için)

## 🔗 İlgili Projeler

- [deepfake-api_flask-api](https://github.com/SerhatITO/deepfake-api_flask-api) — PyTorch modeliyle videoları analiz eden Flask tabanlı API.

## 🧪 Nasıl Çalışır?

1. MVC uygulaması açılır.
2. Giriş yaptıktan sonra video yükleme sayfası görünür.
3. Yüklenen video C# Web API’ye gönderilir.
4. Web API bu videoyu Flask API’ye iletir.
5. Flask API modelle analiz yapar ve `real/fake` sonucu JSON olarak döner.
6. MVC sayfasında sonuç kullanıcıya gösterilir.

## 📁 Proje Yapısı

DeepfakeWebApp/
├── Controllers/
│ └── AccountController.cs
│ └── VideoController.cs
├── Models/
│ └── UploadModel.cs
├── Views/
│ └── Account/
│ └── Video/
├── wwwroot/
│ └── css, js, bootstrap
├── appsettings.json
├── Program.cs
├── Startup.cs

## 📝 Notlar

- Videolar `IFormFile` olarak alınıp Flask API’ye yönlendirilir.
- API yanıtı alınamazsa hata mesajı gösterilir.
- Şu anda yalnızca `.mp4` formatı desteklenmektedir.
