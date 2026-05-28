import { Filter, RefreshCw, Search, ShoppingCart, X } from 'lucide-react';
import { useEffect, useMemo, useState } from 'react';
import { apiClient } from '../api/client';
import { CinemaximumSeatMap } from '../components/CinemaximumSeatMap';
import { ConfirmDialog } from '../components/ConfirmDialog';
import { DataTable } from '../components/DataTable';

const columns = [
  { name: 'biletNo', label: 'Bilet No', compact: true },
  { name: 'filmAdi', label: 'Film' },
  { name: 'salonAdi', label: 'Salon' },
  { name: 'seansTarihi', label: 'Seans', type: 'datetime' },
  { name: 'musteriAdi', label: 'Müşteri' },
  { name: 'telefon', label: 'Telefon' },
  { name: 'koltukNo', label: 'Koltuk', compact: true },
  { name: 'odemeTutari', label: 'Ödeme', type: 'currency', compact: true },
  { name: 'satisTarihi', label: 'Satış Tarihi', type: 'datetime' }
];

function formatDateTime(value) {
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? value : date.toLocaleString('tr-TR');
}

function createMap(items, key) {
  return new Map(items.map((item) => [Number(item[key]), item]));
}

function getSeatLabel(koltuklar, koltukID) {
  const seat = koltuklar.find((koltuk) => Number(koltuk.koltukID) === Number(koltukID));
  return seat?.koltukNo ?? 'Seçilmedi';
}

export function BiletlerPage({ config }) {
  const [biletler, setBiletler] = useState([]);
  const [seanslar, setSeanslar] = useState([]);
  const [filmler, setFilmler] = useState([]);
  const [salonlar, setSalonlar] = useState([]);
  const [musteriler, setMusteriler] = useState([]);
  const [koltuklar, setKoltuklar] = useState([]);
  const [form, setForm] = useState({ seansID: '', musteriID: '', koltukID: '' });
  const [filters, setFilters] = useState({ search: '', filmID: '', salonID: '', seansID: '', musteriID: '' });
  const [editingRow, setEditingRow] = useState(null);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [ticketToDelete, setTicketToDelete] = useState(null);

  const filmMap = useMemo(() => createMap(filmler, 'filmID'), [filmler]);
  const salonMap = useMemo(() => createMap(salonlar, 'salonID'), [salonlar]);
  const seansMap = useMemo(() => createMap(seanslar, 'seansID'), [seanslar]);
  const musteriMap = useMemo(() => createMap(musteriler, 'musteriID'), [musteriler]);
  const koltukMap = useMemo(() => createMap(koltuklar, 'koltukID'), [koltuklar]);

  const selectedSeans = useMemo(() => {
    return seanslar.find((seans) => Number(seans.seansID) === Number(form.seansID));
  }, [form.seansID, seanslar]);

  const enrichedTickets = useMemo(() => {
    return biletler
      .map((bilet) => {
        const seans = seansMap.get(Number(bilet.seansID));
        const film = filmMap.get(Number(seans?.filmID));
        const salon = salonMap.get(Number(seans?.salonID));
        const musteri = musteriMap.get(Number(bilet.musteriID));
        const koltuk = koltukMap.get(Number(bilet.koltukID));

        return {
          ...bilet,
          biletNo: `#${bilet.biletID}`,
          filmID: seans?.filmID ?? '',
          salonID: seans?.salonID ?? '',
          filmAdi: film?.filmAdi ?? `Film ${seans?.filmID ?? '-'}`,
          salonAdi: salon?.salonAdi ?? `Salon ${seans?.salonID ?? '-'}`,
          seansTarihi: seans?.tarihSaat ?? '',
          musteriAdi: musteri?.adSoyad ?? `Müşteri ${bilet.musteriID}`,
          telefon: musteri?.telefon ?? '-',
          email: musteri?.email ?? '',
          koltukNo: koltuk?.koltukNo ?? `Koltuk ${bilet.koltukID}`
        };
      })
      .sort((a, b) => Number(b.biletID) - Number(a.biletID));
  }, [biletler, filmMap, koltukMap, musteriMap, salonMap, seansMap]);

  const filteredTickets = useMemo(() => {
    const search = filters.search.trim().toLocaleLowerCase('tr-TR');

    return enrichedTickets.filter((ticket) => {
      const matchesFilm = !filters.filmID || Number(ticket.filmID) === Number(filters.filmID);
      const matchesSalon = !filters.salonID || Number(ticket.salonID) === Number(filters.salonID);
      const matchesSeans = !filters.seansID || Number(ticket.seansID) === Number(filters.seansID);
      const matchesMusteri = !filters.musteriID || Number(ticket.musteriID) === Number(filters.musteriID);
      const haystack = [
        ticket.biletNo,
        ticket.filmAdi,
        ticket.salonAdi,
        ticket.musteriAdi,
        ticket.telefon,
        ticket.email,
        ticket.koltukNo,
        formatDateTime(ticket.seansTarihi),
        formatDateTime(ticket.satisTarihi)
      ]
        .join(' ')
        .toLocaleLowerCase('tr-TR');

      return matchesFilm && matchesSalon && matchesSeans && matchesMusteri && (!search || haystack.includes(search));
    });
  }, [enrichedTickets, filters]);

  function getSeansOptionLabel(seans) {
    const film = filmMap.get(Number(seans.filmID));
    const salon = salonMap.get(Number(seans.salonID));
    return `#${seans.seansID} - ${film?.filmAdi ?? `Film ${seans.filmID}`} / ${salon?.salonAdi ?? `Salon ${seans.salonID}`} - ${formatDateTime(seans.tarihSaat)}`;
  }

  async function loadData() {
    setLoading(true);
    setError('');

    try {
      const [biletData, seansData, filmData, salonData, musteriData, koltukData] = await Promise.all([
        apiClient.list('Biletler'),
        apiClient.list('Seanslar'),
        apiClient.list('Filmler'),
        apiClient.list('Salonlar'),
        apiClient.list('Musteriler'),
        apiClient.list('Koltuklar')
      ]);

      setBiletler(Array.isArray(biletData) ? biletData : []);
      setSeanslar(Array.isArray(seansData) ? seansData : []);
      setFilmler(Array.isArray(filmData) ? filmData : []);
      setSalonlar(Array.isArray(salonData) ? salonData : []);
      setMusteriler(Array.isArray(musteriData) ? musteriData : []);
      setKoltuklar(Array.isArray(koltukData) ? koltukData : []);
    } catch (exception) {
      setError(exception.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadData();
  }, []);

  function updateForm(field, value) {
    setForm((current) => ({
      ...current,
      [field]: value
    }));
  }

  function updateFilter(field, value) {
    setFilters((current) => ({
      ...current,
      [field]: value
    }));
  }

  function clearFilters() {
    setFilters({ search: '', filmID: '', salonID: '', seansID: '', musteriID: '' });
  }

  function handleSeansChange(value) {
    setEditingRow(null);
    setForm((current) => ({
      ...current,
      seansID: value,
      koltukID: ''
    }));
  }

  function resetForm() {
    setEditingRow(null);
    setForm({ seansID: '', musteriID: '', koltukID: '' });
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setSubmitting(true);
    setError('');
    setMessage('');

    try {
      const payload = {
        seansID: Number(form.seansID),
        musteriID: Number(form.musteriID),
        koltukID: Number(form.koltukID)
      };

      if (editingRow) {
        await apiClient.update('Biletler', editingRow.biletID, {
          biletID: editingRow.biletID,
          ...payload,
          satisTarihi: editingRow.satisTarihi,
          odemeTutari: editingRow.odemeTutari
        });
        setMessage('Bilet güncellendi.');
      } else {
        await apiClient.create('Biletler', payload);
        setMessage('Bilet satın alma işlemi tamamlandı.');
      }

      resetForm();
      await loadData();
    } catch (exception) {
      setError(exception.message);
    } finally {
      setSubmitting(false);
    }
  }

  function handleEdit(row) {
    setEditingRow(row);
    setForm({
      seansID: row.seansID,
      musteriID: row.musteriID,
      koltukID: row.koltukID
    });
    setMessage('');
    setError('');
  }

  function handleDelete(row) {
    setTicketToDelete(row);
  }

  async function confirmDelete() {
    if (!ticketToDelete) {
      return;
    }

    setMessage('');
    setError('');

    try {
      await apiClient.remove('Biletler', ticketToDelete.biletID);
      setMessage('Bilet silindi.');
      setTicketToDelete(null);
      await loadData();
    } catch (exception) {
      setError(exception.message);
    }
  }

  return (
    <div className="resource-page bilet-sales-page">
      <div className="section-toolbar">
        <div>
          <h2>{config.title}</h2>
          <p>{filteredTickets.length} / {biletler.length} kayıt gösteriliyor</p>
        </div>
        <button className="secondary-button" type="button" onClick={loadData} disabled={loading}>
          <RefreshCw size={17} />
          <span>{loading ? 'Yenileniyor' : 'Yenile'}</span>
        </button>
      </div>

      {error && <div className="alert error">{error}</div>}
      {message && <div className="alert success">{message}</div>}

      <div className="ticket-workspace">
        <form className="ticket-panel" onSubmit={handleSubmit}>
          <div className="form-head">
            <h2>{editingRow ? 'Bilet Düzenle' : 'Bilet Satışı'}</h2>
            {editingRow && (
              <button className="icon-button" type="button" onClick={resetForm} aria-label="Vazgeç">
                <X size={18} />
              </button>
            )}
          </div>

          <label className="field">
            <span>Seans</span>
            <select value={form.seansID} onChange={(event) => handleSeansChange(event.target.value)} required>
              <option value="">Seans seçin</option>
              {seanslar.map((seans) => (
                <option key={seans.seansID} value={seans.seansID}>
                  {getSeansOptionLabel(seans)}
                </option>
              ))}
            </select>
          </label>

          <label className="field">
            <span>Müşteri</span>
            <select value={form.musteriID} onChange={(event) => updateForm('musteriID', event.target.value)} required>
              <option value="">Müşteri seçin</option>
              {musteriler.map((musteri) => (
                <option key={musteri.musteriID} value={musteri.musteriID}>
                  {musteri.adSoyad} - {musteri.telefon}
                </option>
              ))}
            </select>
          </label>

          <div className="selected-seat-summary">
            <span>Seçilen Koltuk</span>
            <strong>{getSeatLabel(koltuklar, form.koltukID)}</strong>
          </div>

          <button className="primary-button" type="submit" disabled={submitting || !form.seansID || !form.musteriID || !form.koltukID}>
            <ShoppingCart size={18} />
            <span>{submitting ? 'İşleniyor' : editingRow ? 'Bileti Güncelle' : 'Bilet Satın Al'}</span>
          </button>
        </form>

        <CinemaximumSeatMap
          seansID={form.seansID}
          salonID={selectedSeans?.salonID}
          selectedSeatId={form.koltukID}
          onSeatSelect={(koltukID) => updateForm('koltukID', koltukID ?? '')}
        />
      </div>

      <section className="ticket-list-panel">
        <div className="ticket-list-head">
          <div>
            <h2>Bilet Listesi</h2>
            <p>Film, salon, seans, müşteri ve koltuk bilgileriyle detaylı satış kayıtları.</p>
          </div>
          <span className="status-pill">{filteredTickets.length} kayıt</span>
        </div>

        <div className="ticket-filters">
          <label className="field filter-search">
            <span>Arama</span>
            <div className="input-with-icon">
              <Search size={17} />
              <input
                type="search"
                value={filters.search}
                onChange={(event) => updateFilter('search', event.target.value)}
                placeholder="Müşteri, telefon, film, koltuk..."
              />
            </div>
          </label>

          <label className="field">
            <span>Film</span>
            <select value={filters.filmID} onChange={(event) => updateFilter('filmID', event.target.value)}>
              <option value="">Tüm filmler</option>
              {filmler.map((film) => (
                <option key={film.filmID} value={film.filmID}>
                  {film.filmAdi}
                </option>
              ))}
            </select>
          </label>

          <label className="field">
            <span>Salon</span>
            <select value={filters.salonID} onChange={(event) => updateFilter('salonID', event.target.value)}>
              <option value="">Tüm salonlar</option>
              {salonlar.map((salon) => (
                <option key={salon.salonID} value={salon.salonID}>
                  {salon.salonAdi}
                </option>
              ))}
            </select>
          </label>

          <label className="field">
            <span>Seans</span>
            <select value={filters.seansID} onChange={(event) => updateFilter('seansID', event.target.value)}>
              <option value="">Tüm seanslar</option>
              {seanslar.map((seans) => (
                <option key={seans.seansID} value={seans.seansID}>
                  {getSeansOptionLabel(seans)}
                </option>
              ))}
            </select>
          </label>

          <label className="field">
            <span>Müşteri</span>
            <select value={filters.musteriID} onChange={(event) => updateFilter('musteriID', event.target.value)}>
              <option value="">Tüm müşteriler</option>
              {musteriler.map((musteri) => (
                <option key={musteri.musteriID} value={musteri.musteriID}>
                  {musteri.adSoyad}
                </option>
              ))}
            </select>
          </label>

          <button className="secondary-button filter-clear" type="button" onClick={clearFilters}>
            <Filter size={17} />
            <span>Temizle</span>
          </button>
        </div>
      </section>

      <DataTable
        rows={filteredTickets}
        columns={columns}
        primaryKey="biletID"
        editable
        onEdit={handleEdit}
        onDelete={handleDelete}
      />

      <ConfirmDialog
        open={Boolean(ticketToDelete)}
        title="Bilet silinsin mi?"
        description={
          ticketToDelete
            ? `${ticketToDelete.biletNo} numaralı ${ticketToDelete.musteriAdi} bilet kaydı silinecek.`
            : ''
        }
        confirmText="Sil"
        cancelText="Vazgeç"
        onConfirm={confirmDelete}
        onCancel={() => setTicketToDelete(null)}
      />
    </div>
  );
}
