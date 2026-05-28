import { Pencil, Trash2 } from 'lucide-react';

function formatValue(value, field) {
  if (value === null || value === undefined || value === '') {
    return '-';
  }

  if (field.type === 'datetime') {
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? value : date.toLocaleString('tr-TR');
  }

  if (field.type === 'currency') {
    return Number(value).toLocaleString('tr-TR', {
      style: 'currency',
      currency: 'TRY'
    });
  }

  return value;
}

export function DataTable({ rows, columns, primaryKey, editable, onEdit, onDelete }) {
  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            {columns.map((column) => (
              <th className={column.compact ? 'compact-col' : undefined} key={column.name}>
                {column.label}
              </th>
            ))}
            {editable && (
              <th key="actions" className="actions-col">
                İşlem
              </th>
            )}
          </tr>
        </thead>
        <tbody>
          {rows.length === 0 ? (
            <tr>
              <td className="empty-cell" colSpan={columns.length + (editable ? 1 : 0)}>
                Kayıt bulunamadı.
              </td>
            </tr>
          ) : (
            rows.map((row, index) => (
              <tr key={row[primaryKey] ?? `${primaryKey}-${index}`}>
                {columns.map((column) => (
                  <td className={column.compact ? 'compact-col' : undefined} key={column.name}>
                    {formatValue(row[column.name], column)}
                  </td>
                ))}
                {editable && (
                  <td key="actions" className="row-actions">
                    <button className="icon-button" type="button" onClick={() => onEdit(row)} aria-label="Düzenle">
                      <Pencil size={17} />
                    </button>
                    <button className="icon-button danger" type="button" onClick={() => onDelete(row)} aria-label="Sil">
                      <Trash2 size={17} />
                    </button>
                  </td>
                )}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}
