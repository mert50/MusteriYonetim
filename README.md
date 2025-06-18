# 🏢 Müşteri Yönetim Sistemi

**Görsel Programlama Dersi Ödevi**  
C# Windows Forms & MySQL CRUD Uygulaması

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-00000F?style=for-the-badge&logo=mysql&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)

## 📋 İçindekiler
- [Proje Hakkında](#-proje-hakkında)
- [Özellikler](#-özellikler)
- [Kurulum](#-kurulum)
- [Kullanım](#-kullanım)
- [Ekran Görüntüleri](#-ekran-görüntüleri)
- [Teknik Detaylar](#-teknik-detaylar)
- [Geliştirici](#-geliştirici)

## 🎯 Proje Hakkında

Bu proje, Görsel Programlama dersi kapsamında geliştirilmiş modern bir müşteri yönetim sistemidir. Windows Forms ve MySQL veritabanı kullanılarak CRUD (Create, Read, Update, Delete) işlemlerini gerçekleştiren, modern UI tasarımına sahip masaüstü uygulamasıdır.

### Ödev Gereksinimleri ✅
- [x] MySQL veritabanı bağlantısı
- [x] CRUD işlemleri (Ekleme, Okuma, Güncelleme, Silme)
- [x] Dosya işlemleri (Kaydet/Aç)
- [x] ProgressBar ve Timer kullanımı
- [x] RadioButton grupları (GroupBox)
- [x] Random sayı üretimi ve ListBox
- [x] MessageBox kullanımı
- [x] Modern UI tasarımı (Bonus)

## ✨ Özellikler

### 🗄️ Veritabanı İşlemleri
- **Kayıt Ekleme:** Yeni müşteri bilgileri ekleme
- **Kayıt Arama:** ID ile müşteri bilgilerini getirme
- **Kayıt Güncelleme:** Mevcut müşteri bilgilerini düzenleme
- **Kayıt Silme:** Müşteri kaydını sistemden kaldırma

### 📁 Dosya İşlemleri
- **Dosya Kaydetme:** RichTextBox içeriğini .txt dosyasına kaydetme
- **Dosya Açma:** .txt dosyalarını okuma ve görüntüleme

### 🎨 Modern UI Özellikleri
- Flat Design tasarım
- Özel renk paleti
- Emoji ikonlar
- Shadow efektleri
- Hover animasyonları
- Özel başlık çubuğu
- Responsive layout

### 📊 Ek Özellikler
- ProgressBar (3 saniyede bir güncellenir)
- Random sayı üreteci (50-3000 arası)
- Cinsiyet ve sınıf seçimi (RadioButton)
- MessageBox test aracı

## 🚀 Kurulum

### Gereksinimler
- Windows 10/11
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- [XAMPP](https://www.apachefriends.org/tr/index.html) (MySQL için)
- [Visual Studio Code](https://code.visualstudio.com/)
- C# Extension (VS Code için)

### Adım 1: Projeyi İndirin
```bash
git clone https://github.com/_____/musteri-yonetim-sistemi.git
cd musteri-yonetim-sistemi
```

### Adım 2: XAMPP Kurulumu
1. XAMPP'i indirin ve kurun
2. XAMPP Control Panel'i açın
3. Apache ve MySQL servislerini başlatın

### Adım 3: Veritabanı Oluşturma
1. Tarayıcıda `http://localhost/phpmyadmin` adresine gidin
2. Yeni veritabanı oluşturun: `db`
3. SQL sekmesinde şu kodu çalıştırın:

```sql
CREATE TABLE IF NOT EXISTS musteriler (
    id INT(11) NOT NULL AUTO_INCREMENT,
    isim VARCHAR(255) NOT NULL,
    konu INT(1) NOT NULL,
    mesaj TEXT,
    PRIMARY KEY (id)
);
```

### Adım 4: NuGet Paketini Yükleyin
```bash
dotnet add package MySql.Data
```

### Adım 5: Uygulamayı Çalıştırın
```bash
dotnet run
```

## 📖 Kullanım

### Müşteri Ekleme
1. İsim alanına müşteri adını girin
2. Konu seçin (Arıza/Talep/Yardım)
3. Mesaj alanına detayları yazın
4. "💾 Kaydet" butonuna tıklayın

### Müşteri Arama
1. ID alanına aranacak müşteri ID'sini girin
2. "🔍 Ara" butonuna tıklayın
3. Bulunan kayıt formdaki alanlara yüklenecektir

### Müşteri Güncelleme
1. Önce müşteriyi arayın
2. Bilgileri düzenleyin
3. "✏️ Güncelle" butonuna tıklayın

### Müşteri Silme
1. Silinecek müşterinin ID'sini girin
2. "🗑️ Sil" butonuna tıklayın
3. Onay mesajına "Evet" deyin

## 📸 Ekran Görüntüleri

### Ana Ekran
![Ana Ekran](screenshots/main.png)

### Kayıt Ekleme
![Kayıt Ekleme](screenshots/add.png)

### Modern UI
![Modern UI](screenshots/modern-ui.png)

## 🛠️ Teknik Detaylar

### Kullanılan Teknolojiler
- **Dil:** C# (.NET 6.0)
- **UI Framework:** Windows Forms
- **Veritabanı:** MySQL (MariaDB)
- **ORM:** ADO.NET (MySql.Data)
- **IDE:** Visual Studio Code

### Proje Yapısı
```
MusteriYonetim/
├── Form1.cs           # Ana form ve iş mantığı
├── Form1.Designer.cs  # Form tasarımı (otomatik)
├── Program.cs         # Uygulama giriş noktası
├── MusteriYonetim.csproj  # Proje dosyası
└── README.md          # Bu dosya
```

### Veritabanı Şeması
```sql
musteriler
├── id (INT, AUTO_INCREMENT, PRIMARY KEY)
├── isim (VARCHAR 255)
├── konu (INT 1) -- 0: Arıza, 1: Talep, 2: Yardım
└── mesaj (TEXT)
```

### Connection String
```csharp
string connectionString = "Server=localhost;Database=db;Uid=root;Pwd=;";
```

## 🤔 Sık Sorulan Sorular

**S: MySQL bağlantı hatası alıyorum?**  
C: XAMPP'te MySQL servisinin çalıştığından emin olun. Connection string'i kontrol edin.

**S: Port 80 meşgul hatası?**  
C: Apache'nin portunu 8080 olarak değiştirin. phpMyAdmin'e `localhost:8080/phpmyadmin` adresinden erişin.

**S: ID otomatik artmıyor?**  
C: ID alanı AUTO_INCREMENT olarak ayarlanmış. Manuel ID girmenize gerek yok.

## 👨‍💻 Geliştirici

**Mert Korkmaz**  
Görsel Programlama Dersi Öğrencisi

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---