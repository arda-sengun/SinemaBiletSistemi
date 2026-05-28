import { CalendarClock, Film, Ticket, Users } from 'lucide-react';

const cards = [
  { key: 'filmler', label: 'Film', icon: Film },
  { key: 'seanslar', label: 'Seans', icon: CalendarClock },
  { key: 'musteriler', label: 'Müşteri', icon: Users },
  { key: 'biletler', label: 'Bilet', icon: Ticket }
];

export function Dashboard({ stats, loading, error }) {
  return (
    <div className="dashboard">
      {error && <div className="alert error">{error}</div>}

      <div className="stats-grid">
        {cards.map((card) => {
          const Icon = card.icon;
          return (
            <article className="stat-card" key={card.key}>
              <div className="stat-icon">
                <Icon size={21} />
              </div>
              <div>
                <span>{card.label}</span>
                <strong>{loading ? '-' : stats[card.key] ?? 0}</strong>
              </div>
            </article>
          );
        })}
      </div>

      <div className="dashboard-band">
        <div>
          <h2>Güncel Durum</h2>
          <p>API bağlantısı, kayıt listeleri ve satış akışı tek panelden yönetiliyor.</p>
        </div>
        <div className="status-pill">{loading ? 'Yükleniyor' : 'Hazır'}</div>
      </div>
    </div>
  );
}
