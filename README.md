# ⚡ Energy Billing System

Bu proje Sayax Teknoloji A.Ş adına yapılmış bir tasktır. </br></br>
Enerji sektöründe faaliyet gösteren firmaların müşterilerine kesilecek **fatura tutarlarını** ve **belediyelere ödenecek vergileri** hesaplamak amacıyla geliştirilmiştir. Proje, hem backend hem de frontend bileşenleri içeren modern ve dockerize edilmiş bir mimariye sahiptir.

---

## 🔧 Teknolojiler

- ✅ **.NET 9** – Backend REST API
- ✅ **Angular 17+** – Frontend kullanıcı arayüzü
- ✅ **Docker & Docker Compose** – Uygulamaları kapsülleyip çalıştırmak için
- ✅ **PrimeNG** – UI bileşenleri (tablo, filtre vb.)

--- 

## 🚀 Başlatmak için

### Gerekli Kurulumlar

- [Docker](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/install/) (Docker ile birlikte gelir)

---

### 🔨 Kurulum Adımları

```bash
git clone https://github.com/kullaniciadi/energy-billing.git
cd energy-billing
docker compose up --build
```


### 🌐 Erişim

Backend (Swagger): http://localhost:5217/swagger</br>
Frontend (Angular Uygulaması): http://localhost:4200


## 🧾 API Bilgisi

### Enerji, dağıtım, BTV, KDV ve genel toplam gibi kalemleri hesaplar
```http
  POST /api/invoice/calculate
```
# Request
| Parametre   | Tip    | Açıklama                            |
|-------------|--------|----------------------------------------|
| customerId  | Guid   | 3fa85f64-5717-4562-b3fc-2c963f66afa6            |

# Response

| Parametre   | Tip  | Açıklama                            |
|-------------|------|----------------------------------------|
| customerId  | Guid | 3fa85f64-5717-4562-b3fc-2c963f66afa6            |
| energyTotal  | decimal  | 3fa85f64-5717-4562-b3fc-2c963f66afa6            |
| distributionTotal  | decimal | 3fa85f64-5717-4562-b3fc-2c963f66afa6            |
| btvTotal  | decimal | 3fa85f64-5717-4562-b3fc-2c963f66afa6            |
| vatTotal  | decimal | 3fa85f64-5717-4562-b3fc-2c963f66afa6            |
| grandTotal  | decimal | 3fa85f64-5717-4562-b3fc-2c963f66afa6            |



## 🖥️ Frontend Özellikleri
Fatura kalemlerini tablo halinde gösterir

Toplam tutarları listeler

Arama / filtreleme

Excel'e aktarma (Export to Excel)


## 📦 Docker Compose Servisleri
Servis	Açıklama	Port
backend	.NET REST API	localhost:5217
frontend	Angular UI	localhost:4200

| Servis            | Açıklama      | Port                            |
|-------------------|---------------|----------------------------------------|
| Backend           | .NET REST API | localhost:5217         |
| Frontend          | decimal       | localhost:4200         |

## 💡 Geliştirme Notları
.dockerignore ve .gitignore dosyaları eklendi

Angular için production build otomatik yapılır

API istekleri frontend tarafından yapılabilir
