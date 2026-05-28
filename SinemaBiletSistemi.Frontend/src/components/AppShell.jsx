import {
  Armchair,
  Building2,
  CalendarClock,
  Clapperboard,
  Film,
  LayoutDashboard,
  Menu,
  Ticket,
  Users,
  X
} from 'lucide-react';

const icons = {
  dashboard: LayoutDashboard,
  filmler: Film,
  salonlar: Building2,
  koltuklar: Armchair,
  seanslar: CalendarClock,
  musteriler: Users,
  biletler: Ticket
};

export function AppShell({ activePage, pages, onNavigate, children }) {
  const active = pages.find((page) => page.key === activePage);
  const ActiveIcon = icons[activePage] ?? Clapperboard;

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="brand">
          <span className="brand-mark">
            <Clapperboard size={22} />
          </span>
          <span className="brand-text">Sinema Bilet</span>
        </div>

        <nav className="sidebar-nav" aria-label="Ana menü">
          {pages.map((page) => {
            const Icon = icons[page.key] ?? Menu;
            return (
              <button
                key={page.key}
                className={`nav-item ${activePage === page.key ? 'active' : ''}`}
                onClick={() => onNavigate(page.key)}
                type="button"
              >
                <Icon size={18} />
                <span>{page.label}</span>
              </button>
            );
          })}
        </nav>
      </aside>

      <main className="main-area">
        <header className="topbar">
          <button className="icon-button mobile-menu" type="button" aria-label="Menüyü aç">
            <Menu size={20} />
          </button>
          <div className="topbar-title">
            <span className="topbar-icon">
              <ActiveIcon size={21} />
            </span>
            <div>
              <p>{active?.eyebrow ?? 'Yönetim Paneli'}</p>
              <h1>{active?.label ?? 'Dashboard'}</h1>
            </div>
          </div>
          <a className="swagger-link" href="http://localhost:5227/swagger" target="_blank" rel="noreferrer">
            Swagger
          </a>
        </header>

        <section className="content-area">{children}</section>
      </main>
    </div>
  );
}
