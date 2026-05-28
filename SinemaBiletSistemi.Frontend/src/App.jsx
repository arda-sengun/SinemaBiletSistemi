import { useEffect, useMemo, useState } from 'react';
import { apiClient } from './api/client';
import { AppShell } from './components/AppShell';
import { BiletlerPage } from './pages/BiletlerPage';
import { Dashboard } from './pages/Dashboard';
import { ResourcePage } from './pages/ResourcePage';

const commonNumber = { type: 'number', inputType: 'number', min: 1 };

const resources = {
  filmler: {
    key: 'filmler',
    label: 'Filmler',
    eyebrow: 'Katalog',
    title: 'Film Listesi',
    singleLabel: 'Film',
    resource: 'Filmler',
    primaryKey: 'filmID',
    editable: false,
    columns: [
      { name: 'filmID', label: 'ID' },
      { name: 'filmAdi', label: 'Film Adı' },
      { name: 'yonetmen', label: 'Yönetmen' },
      { name: 'sure', label: 'Süre' },
      { name: 'tur', label: 'Tür' },
      { name: 'afisURL', label: 'Afiş URL' }
    ],
    fields: [
      { name: 'filmAdi', label: 'Film Adı' },
      { name: 'yonetmen', label: 'Yönetmen' },
      { name: 'sure', label: 'Süre', ...commonNumber },
      { name: 'tur', label: 'Tür' },
      { name: 'afisURL', label: 'Afiş URL', inputType: 'url' }
    ]
  },
  salonlar: {
    key: 'salonlar',
    label: 'Salonlar',
    eyebrow: 'Mekan',
    title: 'Salon Listesi',
    singleLabel: 'Salon',
    resource: 'Salonlar',
    primaryKey: 'salonID',
    editable: false,
    columns: [
      { name: 'salonID', label: 'ID' },
      { name: 'salonAdi', label: 'Salon Adı' },
      { name: 'kapasite', label: 'Kapasite' }
    ],
    fields: [
      { name: 'salonAdi', label: 'Salon Adı' },
      { name: 'kapasite', label: 'Kapasite', ...commonNumber }
    ]
  },
  koltuklar: {
    key: 'koltuklar',
    label: 'Koltuklar',
    eyebrow: 'Yerleşim',
    title: 'Koltuk Listesi',
    singleLabel: 'Koltuk',
    resource: 'Koltuklar',
    primaryKey: 'koltukID',
    editable: false,
    columns: [
      { name: 'koltukID', label: 'ID' },
      { name: 'salonID', label: 'Salon ID' },
      { name: 'koltukNo', label: 'Koltuk No' }
    ],
    fields: [
      { name: 'salonID', label: 'Salon ID', ...commonNumber },
      { name: 'koltukNo', label: 'Koltuk No' }
    ]
  },
  seanslar: {
    key: 'seanslar',
    label: 'Seanslar',
    eyebrow: 'Program',
    title: 'Seans Listesi',
    singleLabel: 'Seans',
    resource: 'Seanslar',
    primaryKey: 'seansID',
    editable: true,
    columns: [
      { name: 'seansID', label: 'ID' },
      { name: 'filmID', label: 'Film ID' },
      { name: 'salonID', label: 'Salon ID' },
      { name: 'tarihSaat', label: 'Tarih Saat', type: 'datetime' },
      { name: 'biletFiyati', label: 'Bilet Fiyatı', type: 'currency' },
      { name: 'bosKoltukSayisi', label: 'Boş Koltuk' }
    ],
    createFields: [
      { name: 'filmID', label: 'Film ID', ...commonNumber },
      { name: 'salonID', label: 'Salon ID', ...commonNumber },
      { name: 'tarihSaat', label: 'Tarih Saat', type: 'datetime' },
      { name: 'biletFiyati', label: 'Bilet Fiyatı', type: 'currency', inputType: 'number', min: 1, step: '0.01' }
    ],
    editFields: [
      { name: 'filmID', label: 'Film ID', ...commonNumber },
      { name: 'salonID', label: 'Salon ID', ...commonNumber },
      { name: 'tarihSaat', label: 'Tarih Saat', type: 'datetime' },
      { name: 'biletFiyati', label: 'Bilet Fiyatı', type: 'currency', inputType: 'number', min: 1, step: '0.01' },
      { name: 'bosKoltukSayisi', label: 'Boş Koltuk', type: 'number', inputType: 'number', min: 0 }
    ]
  },
  musteriler: {
    key: 'musteriler',
    label: 'Müşteriler',
    eyebrow: 'Kişiler',
    title: 'Müşteri Listesi',
    singleLabel: 'Müşteri',
    resource: 'Musteriler',
    primaryKey: 'musteriID',
    editable: true,
    columns: [
      { name: 'musteriID', label: 'ID' },
      { name: 'adSoyad', label: 'Ad Soyad' },
      { name: 'telefon', label: 'Telefon' },
      { name: 'email', label: 'Email' }
    ],
    fields: [
      { name: 'adSoyad', label: 'Ad Soyad' },
      { name: 'telefon', label: 'Telefon', inputType: 'tel' },
      { name: 'email', label: 'Email', inputType: 'email' }
    ]
  },
  biletler: {
    key: 'biletler',
    label: 'Biletler',
    eyebrow: 'Satış',
    title: 'Bilet Listesi',
    singleLabel: 'Bilet',
    resource: 'Biletler',
    primaryKey: 'biletID',
    editable: true,
    columns: [
      { name: 'biletID', label: 'ID' },
      { name: 'seansID', label: 'Seans ID' },
      { name: 'musteriID', label: 'Müşteri ID' },
      { name: 'koltukID', label: 'Koltuk ID' },
      { name: 'satisTarihi', label: 'Satış Tarihi', type: 'datetime' },
      { name: 'odemeTutari', label: 'Ödeme', type: 'currency' }
    ],
    createFields: [
      { name: 'seansID', label: 'Seans ID', ...commonNumber },
      { name: 'musteriID', label: 'Müşteri ID', ...commonNumber },
      { name: 'koltukID', label: 'Koltuk ID', ...commonNumber }
    ],
    editFields: [
      { name: 'seansID', label: 'Seans ID', ...commonNumber },
      { name: 'musteriID', label: 'Müşteri ID', ...commonNumber },
      { name: 'koltukID', label: 'Koltuk ID', ...commonNumber },
      { name: 'satisTarihi', label: 'Satış Tarihi', type: 'datetime' },
      { name: 'odemeTutari', label: 'Ödeme Tutarı', type: 'currency', inputType: 'number', min: 1, step: '0.01' }
    ]
  }
};

const pages = [
  { key: 'dashboard', label: 'Dashboard', eyebrow: 'Genel Bakış' },
  resources.filmler,
  resources.salonlar,
  resources.koltuklar,
  resources.seanslar,
  resources.musteriler,
  resources.biletler
];

function App() {
  const [activePage, setActivePage] = useState('dashboard');
  const [stats, setStats] = useState({});
  const [statsLoading, setStatsLoading] = useState(false);
  const [statsError, setStatsError] = useState('');

  const activeResource = useMemo(() => resources[activePage], [activePage]);

  useEffect(() => {
    async function loadStats() {
      setStatsLoading(true);
      setStatsError('');

      try {
        const entries = await Promise.all(
          Object.values(resources).map(async (resource) => {
            const data = await apiClient.list(resource.resource);
            return [resource.key, Array.isArray(data) ? data.length : 0];
          })
        );

        setStats(Object.fromEntries(entries));
      } catch (exception) {
        setStatsError(exception.message);
      } finally {
        setStatsLoading(false);
      }
    }

    loadStats();
  }, []);

  return (
    <AppShell activePage={activePage} pages={pages} onNavigate={setActivePage}>
      {activePage === 'dashboard' ? (
        <Dashboard stats={stats} loading={statsLoading} error={statsError} />
      ) : activePage === 'biletler' ? (
        <BiletlerPage config={activeResource} />
      ) : (
        <ResourcePage config={activeResource} />
      )}
    </AppShell>
  );
}

export default App;
