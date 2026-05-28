import { useEffect, useMemo, useState } from 'react';
import { MdEventSeat } from 'react-icons/md';
import { apiClient } from '../api/client';

function getRowLabel(koltukNo) {
  const match = String(koltukNo ?? '').trim().match(/^[A-Za-zÇĞİÖŞÜçğıöşü]+/);
  return match ? match[0].toUpperCase('tr-TR') : 'SIRA';
}

function getSeatNumber(koltukNo) {
  const match = String(koltukNo ?? '').trim().match(/\d+/);
  return match ? Number(match[0]) : Number.MAX_SAFE_INTEGER;
}

function getSeatStatus(seat, selectedSeatId, soldSeatIds) {
  if (soldSeatIds.has(Number(seat.koltukID))) {
    return 'sold';
  }

  if (Number(seat.koltukID) === Number(selectedSeatId)) {
    return 'selected';
  }

  return 'empty';
}

export function CinemaximumSeatMap({ seansID, salonID, selectedSeatId, onSeatSelect }) {
  const [seats, setSeats] = useState([]);
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const soldSeatIds = useMemo(() => {
    return new Set(
      tickets
        .filter((ticket) => Number(ticket.seansID) === Number(seansID))
        .map((ticket) => Number(ticket.koltukID))
    );
  }, [tickets, seansID]);

  const groupedSeats = useMemo(() => {
    const salonSeats = seats
      .filter((seat) => Number(seat.salonID) === Number(salonID))
      .sort((a, b) => {
        const rowCompare = getRowLabel(a.koltukNo).localeCompare(getRowLabel(b.koltukNo), 'tr-TR');
        return rowCompare || getSeatNumber(a.koltukNo) - getSeatNumber(b.koltukNo);
      });

    return salonSeats.reduce((groups, seat) => {
      const row = getRowLabel(seat.koltukNo);
      groups[row] ??= [];
      groups[row].push(seat);
      return groups;
    }, {});
  }, [seats, salonID]);

  useEffect(() => {
    if (!seansID || !salonID) {
      setSeats([]);
      setTickets([]);
      return;
    }

    async function loadSeatData() {
      setLoading(true);
      setError('');

      try {
        const [seatData, ticketData] = await Promise.all([
          apiClient.list('Koltuklar'),
          apiClient.list('Biletler')
        ]);

        setSeats(Array.isArray(seatData) ? seatData : []);
        setTickets(Array.isArray(ticketData) ? ticketData : []);
      } catch (exception) {
        setError(exception.message);
      } finally {
        setLoading(false);
      }
    }

    loadSeatData();
  }, [seansID, salonID]);

  function handleSeatClick(seat) {
    if (soldSeatIds.has(Number(seat.koltukID))) {
      return;
    }

    onSeatSelect(seat.koltukID);
  }

  if (!seansID || !salonID) {
    return (
      <div className="seat-map-empty">
        Seans seçildiğinde koltuk haritası burada görünecek.
      </div>
    );
  }

  return (
    <section className="seat-map" aria-label="Koltuk seçimi">
      <div className="screen-wrap">
        <div className="cinema-screen">SİNEMA PERDESİ</div>
      </div>

      {loading && <div className="seat-map-message">Koltuklar yükleniyor...</div>}
      {error && <div className="seat-map-message error">{error}</div>}

      {!loading && !error && (
        <div className="seat-grid-wrap">
          {Object.entries(groupedSeats).length === 0 ? (
            <div className="seat-map-message">Bu salon için koltuk bulunamadı.</div>
          ) : (
            Object.entries(groupedSeats).map(([rowName, rowSeats]) => (
              <div className="seat-row" key={rowName}>
                <span className="row-label">{rowName}</span>
                <div className="seat-row-grid">
                  {rowSeats.map((seat, index) => {
                    const status = getSeatStatus(seat, selectedSeatId, soldSeatIds);
                    const hasAisle = rowSeats.length > 6 && index === Math.ceil(rowSeats.length / 2);

                    return (
                      <button
                        className={`seat-button ${status} ${hasAisle ? 'with-aisle' : ''}`}
                        disabled={status === 'sold'}
                        key={seat.koltukID}
                        onClick={() => handleSeatClick(seat)}
                        title={seat.koltukNo}
                        type="button"
                      >
                        <MdEventSeat aria-hidden="true" />
                        <span>{seat.koltukNo}</span>
                      </button>
                    );
                  })}
                </div>
              </div>
            ))
          )}
        </div>
      )}

      <div className="seat-legend">
        <span><MdEventSeat className="legend-empty" /> Boş</span>
        <span><MdEventSeat className="legend-selected" /> Seçili</span>
        <span><MdEventSeat className="legend-sold" /> Dolu</span>
      </div>
    </section>
  );
}
