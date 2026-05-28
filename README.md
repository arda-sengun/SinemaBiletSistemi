# 🎬 Sinema Bilet Satış Sistemi

Bu proje, **N-Katmanlı (N-Tier) mimari** standartlarına uygun olarak geliştirilmiş, tam kapsamlı bir **sinema bileti satış ve salon yönetim sistemidir**.

Projenin temel amacı; veri bütünlüğünü ve ağır iş kurallarını (kapasite hesaplama, bilet fiyatlandırma vb.) istemci veya API tarafında değil, doğrudan **veritabanı seviyesinde (MSSQL)** çözerek yüksek performanslı ve güvenli bir backend mimarisi sunmaktır.

Frontend tarafı tamamen bağımsız bir istemci (client) olarak tasarlanmıştır.

---

# 🚀 Kullanılan Teknolojiler

## Backend

* C#
* .NET Web API

## Mimari

* N-Tier Architecture

  * DataAccess
  * Business
  * Entities
  * WebAPI

## Veritabanı

* Microsoft SQL Server
* T-SQL
* Stored Procedures
* Trigger
* User Defined Functions (UDF)

## Frontend

* React.js
* Vite

---

# ⚙️ Veritabanı ve İş Kuralları (Core Logic)

Uygulamanın temel iş mantığı veritabanı katmanında çalışmaktadır.

Hiçbir CRUD işleminde doğrudan SQL sorgusu (`SELECT`, `INSERT`, `UPDATE`, `DELETE`) kullanılmamıştır. Tüm veri erişim işlemleri **Stored Procedure** yapıları üzerinden yönetilmektedir.

## 🔹 Trigger Kullanımı

Bilet satıldığında veya iptal edildiğinde seansın boş koltuk kapasitesini otomatik güncelleyen trigger yapıları bulunmaktadır.

Örnek:

* `BosKoltukSayisi` alanı anlık olarak güncellenir.
* Veri tutarlılığı korunur.
* Manuel kapasite kontrolü ihtiyacı ortadan kalkar.

## 🔹 UDF (User Defined Function) Kullanımı

Sistemde aşağıdaki hesaplamalar için kullanıcı tanımlı fonksiyonlar kullanılmaktadır:

* Seans bazlı anlık boş koltuk sayısı
* Toplam gelir hesaplama
* Salon doluluk oranları

---

# 🛠️ Kurulum ve Çalıştırma Rehberi

Projeyi yerel bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz.

---

# 1️⃣ Veritabanının Kurulumu

Uygulamanın çalışabilmesi için öncelikle veritabanı şemasının oluşturulması gerekir.

## Adımlar

1. Proje dizininde bulunan `SinemaBiletDB.sql` dosyasını açın.
2. Dosyayı **SQL Server Management Studio (SSMS)** üzerinden çalıştırın.
3. Script çalıştırıldığında:

   * `SinemaBiletSistemi` isimli veritabanı oluşturulur.
   * Tablolar oluşturulur.
   * Stored Procedure’ler eklenir.
   * Trigger ve Function yapıları oluşturulur.
   * Örnek veriler sisteme yüklenir.

---

# 2️⃣ Backend (Web API) Kurulumu

## Adımlar

1. Backend projesindeki `appsettings.json` dosyasını açın.

2. `ConnectionStrings` bölümünü kendi SQL Server bilginize göre güncelleyin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SinemaBiletSistemi;Trusted_Connection=True;"
}
```

3. Terminal üzerinden proje dizinine gidin ve aşağıdaki komutu çalıştırın:

```bash
dotnet run
```

4. API başarıyla çalıştığında:

* Swagger arayüzü açılır
* Endpoint’ler erişilebilir hale gelir

---

# 3️⃣ Frontend (React) Kurulumu

## Adımlar

1. Yeni bir terminal açın.
2. Frontend klasörüne girin:

```bash
cd SinemaBiletSistemi.Frontend
```

3. Gerekli paketleri yükleyin:

```bash
npm install
```

4. React uygulamasını başlatın:

```bash
npm run dev
```

5. Tarayıcı üzerinden verilen localhost adresine giderek uygulamayı görüntüleyebilirsiniz.

---

# 📌 Proje Özellikleri

* 🎟️ Bilet satış sistemi
* 🪑 Salon ve koltuk yönetimi
* 🎬 Film ve seans yönetimi
* 💰 Dinamik gelir hesaplama
* ⚡ Trigger destekli kapasite yönetimi
* 🔒 Stored Procedure tabanlı güvenli veri erişimi
* 🧩 Katmanlı mimari yapısı
* 🌐 RESTful API desteği

---

# 📂 Proje Yapısı

```text
SinemaBiletSistemi
│
├── Business
├── DataAccess
├── Entities
└── SinemaBiletSistemi.Frontend
```

---

# 📖 Gelecekte Eklenebilecek Özellikler

* JWT Authentication
* Online ödeme sistemi
* QR kodlu bilet sistemi
* Redis cache desteği
* Docker desteği
* CI/CD entegrasyonu

---

# 👨‍💻 Geliştirici Notu

Bu proje özellikle:

* Veritabanı odaklı backend mimarisi,
* MSSQL iş kuralları yönetimi,
* Stored Procedure mimarisi,
* Trigger/UDF kullanımı,
* N-Tier Architecture

konularında güçlü bir örnek proje olarak geliştirilmiştir.

---
