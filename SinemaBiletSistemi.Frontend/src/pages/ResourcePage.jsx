import { RefreshCw } from 'lucide-react';
import { useEffect, useMemo, useState } from 'react';
import { apiClient } from '../api/client';
import { DataTable } from '../components/DataTable';
import { EntityForm, createInitialForm, pickFormValues } from '../components/EntityForm';

export function ResourcePage({ config }) {
  const createFields = config.createFields ?? config.fields;
  const editFields = config.editFields ?? createFields;
  const [rows, setRows] = useState([]);
  const [form, setForm] = useState(() => createInitialForm(createFields));
  const [editingRow, setEditingRow] = useState(null);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');

  const activeFields = useMemo(() => (editingRow ? editFields : createFields), [createFields, editFields, editingRow]);

  async function loadRows() {
    setLoading(true);
    setError('');

    try {
      const data = await apiClient.list(config.resource);
      setRows(Array.isArray(data) ? data : []);
    } catch (exception) {
      setError(exception.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    setEditingRow(null);
    setForm(createInitialForm(createFields));
    loadRows();
  }, [config.resource]);

  async function handleSubmit(event) {
    event.preventDefault();
    setSubmitting(true);
    setError('');
    setMessage('');

    try {
      if (editingRow) {
        await apiClient.update(config.resource, editingRow[config.primaryKey], form);
        setMessage(`${config.singleLabel} güncellendi.`);
      } else {
        await apiClient.create(config.resource, form);
        setMessage(`${config.singleLabel} eklendi.`);
      }

      setEditingRow(null);
      setForm(createInitialForm(createFields));
      await loadRows();
    } catch (exception) {
      setError(exception.message);
    } finally {
      setSubmitting(false);
    }
  }

  function handleEdit(row) {
    setEditingRow(row);
    setForm(pickFormValues(row, editFields));
    setMessage('');
    setError('');
  }

  async function handleDelete(row) {
    const approved = window.confirm(`${config.singleLabel} silinsin mi?`);

    if (!approved) {
      return;
    }

    setError('');
    setMessage('');

    try {
      await apiClient.remove(config.resource, row[config.primaryKey]);
      setMessage(`${config.singleLabel} silindi.`);
      await loadRows();
    } catch (exception) {
      setError(exception.message);
    }
  }

  function handleCancel() {
    setEditingRow(null);
    setForm(createInitialForm(createFields));
  }

  return (
    <div className="resource-page">
      <div className="section-toolbar">
        <div>
          <h2>{config.title}</h2>
          <p>{rows.length} kayıt</p>
        </div>
        <button className="secondary-button" type="button" onClick={loadRows} disabled={loading}>
          <RefreshCw size={17} />
          <span>{loading ? 'Yenileniyor' : 'Yenile'}</span>
        </button>
      </div>

      {error && <div className="alert error">{error}</div>}
      {message && <div className="alert success">{message}</div>}

      <div className="work-grid">
        <EntityForm
          title={editingRow ? `${config.singleLabel} Düzenle` : `${config.singleLabel} Ekle`}
          fields={activeFields}
          value={form}
          editing={Boolean(editingRow)}
          onChange={setForm}
          onSubmit={handleSubmit}
          onCancel={handleCancel}
          submitting={submitting}
        />

        <DataTable
          rows={rows}
          columns={config.columns}
          primaryKey={config.primaryKey}
          editable={config.editable}
          onEdit={handleEdit}
          onDelete={handleDelete}
        />
      </div>
    </div>
  );
}
